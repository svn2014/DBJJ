using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace Security
{
    public class MutualFundGroup: ASecurityGroup
    {
        #region 基础方法
        public MutualFundGroup() : base(typeof(MutualFund)) { }
        public override void LoadData(DataInfoType type)
        {
            switch (type)
            {
                case DataInfoType.SecurityInfo:
                    base.LoadSecurityInfo();
                    break;
                case DataInfoType.TradingPrice:
                    DataManager.GetDataLoader().LoadMutualFundPrice(this);
                    break;
                case DataInfoType.FundNetAssetValue:
                    DataManager.GetDataLoader().LoadMutualFundNAV(this);
                    break;
                case DataInfoType.SecurityReport:
                    DataManager.GetDataLoader().LoadMutualFundReport(this);
                    break;
                default:
                    MessageManager.GetInstance().AddMessage(MessageType.Warning, Message.C_Msg_GE1, type.ToString());
                    break;
            }
        }
        #endregion

        #region 扩展属性
        private MutualFund WholeFund;
        #endregion

        #region 扩展方法
        public MutualFund GetWholeFund()
        {
            if (base.SecurityHoldings == null || base.SecurityHoldings.Count == 0)
                return null;

            WholeFund = new MutualFund("");
            WholeFund.Name = "基金组中包含的所有基金组成的整体";
            WholeFund.SetDatePeriod(base.TimeSeriesStart, base.TimeSeriesEnd);

            #region 构造整体基金的累计净值序列
            //==========================
            //计算公式：
            //  NAVw = Sum(NAVi * SHAREi)/Sum(SHAREi)
            //==========================
            try
            {
                WholeFund.TradingNAV.TradingDates = ((MutualFund)base.SecurityHoldings[0]).TradingNAV.TradingDates;
                WholeFund.TradingNAV.InsideSampleLength = ((MutualFund)base.SecurityHoldings[0]).TradingNAV.InsideSampleLength;
                WholeFund.TradingNAV.AdjustedTimeSeries = new List<ATimeItem>();
                WholeFund.FundReport.AdjustedTimeSeries = new List<ATimeItem>();

                //计算净值
                for (int i = 0; i < WholeFund.TradingNAV.TradingDates.Count; i++)
                {
                    NetAssetValue NAVw = new NetAssetValue();
                    NAVw.TradeDate = WholeFund.TradingNAV.TradingDates[i];
                    NAVw.IsTrading = true;

                    MutualFundReport RPTw = new MutualFundReport();
                    RPTw.TradeDate = NAVw.TradeDate;
                    RPTw.ReportDate = new DateTime(1900, 1, 1);
                    RPTw.IsTrading = true;

                    //SecurityHoldings[0]的时间序列可能不够长,不能在此处使用
                    //NAVw.IsOutsideSamplePeriod = ((MutualFund)base.SecurityHoldings[0]).TradingNAV.AdjustedTimeSeries[i].IsOutsideSamplePeriod;
                                        
                    foreach (MutualFund f in base.SecurityHoldings)
                    {
                        if (f.TradingNAV.AdjustedTimeSeries == null || f.FundReport.AdjustedTimeSeries == null
                            || i >= f.TradingNAV.AdjustedTimeSeries.Count || i >= f.FundReport.AdjustedTimeSeries.Count)
                        {
                            continue;
                        }

                        NAVw.IsOutsideSamplePeriod = f.TradingNAV.AdjustedTimeSeries[i].IsOutsideSamplePeriod;
                        RPTw.IsOutsideSamplePeriod = NAVw.IsOutsideSamplePeriod;

                        //成立不足30天的去除
                        if (f.ListedDate.AddDays(30) > WholeFund.TradingNAV.TradingDates[i])
                            continue;

                        //计算净值 和 资产配置
                        NetAssetValue NAVi = (NetAssetValue)f.TradingNAV.AdjustedTimeSeries[i];
                        MutualFundReport RPTi = (MutualFundReport)f.FundReport.AdjustedTimeSeries[i];
                        NAVw.UnitNAV += NAVi.UnitNAV * RPTi.TotalShare;
                        RPTw.TotalShare += RPTi.TotalShare;

                        if (RPTw.ReportDate < RPTi.ReportDate)
                            RPTw.ReportDate = RPTi.ReportDate;

                        RPTw.TotalEquityAsset += RPTi.TotalEquityAsset;
                        RPTw.TotalBondAsset += RPTi.TotalBondAsset;
                        RPTw.TotalNetAsset += RPTi.TotalNetAsset;
                        RPTw.PureBondAsset += RPTi.PureBondAsset;
                        RPTw.ConvertableBondAsset += RPTi.ConvertableBondAsset;
                    }

                    NAVw.UnitNAV = NAVw.UnitNAV / RPTw.TotalShare;
                    NAVw.AccumUnitNAV = NAVw.UnitNAV;
                    WholeFund.TradingNAV.AdjustedTimeSeries.Add(NAVw);
                    WholeFund.FundReport.AdjustedTimeSeries.Add(RPTw);
                }

                //计算净值收益率
                WholeFund.TradingNAV.Calculate();
                WholeFund.FundReport.Calculate();
            }
            catch (Exception ex)
            {
                throw new Exception(Message.C_Msg_MF11, ex);
            }
            #endregion

            return WholeFund;
        }
        #endregion
    }
}
