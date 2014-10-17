using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Security
{
    public abstract class ATimeSeries : ASampleDatePeriod
    {
        //================================
        //  定义：时间序列全部倒叙排列
        //================================        
        public abstract void Load();

        public string DataSource;
        public string Code = "";
        public ATimeSeries() { }
        public ATimeSeries(string code, DateTime start, DateTime end)
        {
            this.Code = code;
            base.SetDatePeriod(start, end);
        }
        public override void SetDatePeriod(DateTime start, DateTime end)
        {
            base.SetDatePeriod(start, end);
            this.TradingDates.Clear();
        }

        #region 时间序列
        public List<DateTime> TradingDates = new List<DateTime>();
        public int InsideSampleLength = 0;
        //原始数据，复权数据，校验过的数据
        public List<ATimeItem> OriginalTimeSeries = new List<ATimeItem>();
        public List<ATimeItem> AdjustedTimeSeries;
        //持有期收益率
        public string HoldingPeriodInfo = "";
        public Nullable<double> HoldingPeriodReturn = null;

        public virtual void Adjust()
        {
            //========================================
            //1）按交易日补齐数据并作复权调整
            //========================================
            #region 按交易日补齐数据并作复权调整
            if (this.TradingDates.Count == 0)
                DataManager.GetDataLoader().LoadTradingDate(this);

            if (TradingDates == null || TradingDates.Count == 0 || this.OriginalTimeSeries == null || this.OriginalTimeSeries.Count == 0)
            {
                MessageManager.GetInstance().AddMessage(MessageType.Warning, Message.C_Msg_GE2, "");
                return;
            }

            if (this.AdjustedTimeSeries == null)
                this.AdjustedTimeSeries = new List<ATimeItem>();
            else
                this.AdjustedTimeSeries.Clear();

            //找到缺失的交易日并补齐数据
            int iOriginalCnt = 0;
            for (int iTradeDayCount = 0; iTradeDayCount < this.TradingDates.Count; )
            {
                DateTime tradeday = this.TradingDates[iTradeDayCount];

                if (iOriginalCnt < this.OriginalTimeSeries.Count)
                {
                    ATimeItem item = this.OriginalTimeSeries[iOriginalCnt];

                    //判断是否是样本外数据
                    if (item.TradeDate < this.TimeSeriesStart)
                        item.IsOutsideSamplePeriod = true;
                    else
                        InsideSampleLength++;

                    if (tradeday >= item.TradeDate)
                    {
                        //交易日不在净值数据中则加入前一交易日的数据
                        ATimeItem newItem = (ATimeItem)item.Clone();
                        newItem.TradeDate = tradeday;

                        if (tradeday == item.TradeDate)
                            newItem.IsTrading = true;
                        else
                            newItem.IsTrading = false;

                        //复权调整
                        newItem.Adjust();

                        this.AdjustedTimeSeries.Add(newItem);

                        if (tradeday == item.TradeDate)
                            iOriginalCnt++;

                        //计数器加1
                        iTradeDayCount++;
                    }
                    else
                    {
                        iOriginalCnt++;
                    }
                }
                else
                    //OriginalTimeSeries更早交易日的数据
                    break;
            }
            #endregion

            //========================================
            //2）计算涨跌幅数据
            //========================================
            this.Calculate();
        }
        public virtual void Calculate()
        {
            //====================
            //计算涨跌幅: 时间序列全部降序排列
            //====================
        }
        #endregion        
    }
}
