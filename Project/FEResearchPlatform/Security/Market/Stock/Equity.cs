using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Security
{
    public class Equity : ASecurity
    {
        #region 基础方法
        public Equity(string code) : base(code) { }
        public Equity(string code, DateTime start, DateTime end): base(code, start, end) { }
        protected override void BuildSecurity()
        {
            base.Exchange = GetEquityExchange(base.Code, ref this.BoardType);
            base.Type = SecurityType.Equity;
        }
        protected override void BuildTimeSeries(string code, DateTime start, DateTime end)
        {
            if (base.TradingPrice == null)
                base.TradingPrice = new SeriesEquityPrice(code, start, end);
            base.TradingPrice.SetDatePeriod(start, end);
        }
        public override void LoadData(DataInfoType type)
        {
            switch (type)
            {
                case DataInfoType.SecurityInfo:
                    DataManager.GetDataLoader().LoadEquityInfo(this);
                    break;
                case DataInfoType.TradingPrice:
                    if (base.TradingPrice == null)
                        base.TradingPrice = new SeriesEquityPrice(base.Code, base.TimeSeriesStart, base.TimeSeriesEnd);
                    base.TradingPrice.Load();
                    break;
                case DataInfoType.SecurityReport:
                    throw new NotImplementedException();
                default:
                    MessageManager.GetInstance().AddMessage(MessageType.Warning, Message.C_Msg_GE1, type.ToString());
                    return;
            }
        }
        //public override void DebugPrintInfo()
        //{
        //    base.DebugPrintInfo();

        //    Debug.Write(this.Exchange.ToString() + "\t");
        //    Debug.Write(this.BoardType.ToString() + "\t");
        //    Debug.Write(this.ListedDate.ToString("yyyy-MM-dd") + "\t");
        //    Debug.Write(this.DelistedDate.ToString("yyyy-MM-dd") + "\t");
        //    Debug.WriteLine("");
        //}
        #endregion

        #region 扩展属性
        public EquityBoardType BoardType;
        public DateTime ListedDate;
        public DateTime DelistedDate;
        #endregion

        #region 辅助函数
        public static ExchangeType GetEquityExchange(string code, ref EquityBoardType boardtype)
        {
            if (code == null || code.Length == 0)
            {
                MessageManager.GetInstance().AddMessage(MessageType.Warning, Message.C_Msg_EQ3, "(null)");
                return ExchangeType.OTC;
            }

            string code3 = code.Substring(0, 3);
            string code2 = code.Substring(0, 2);

            switch (code2)
            {
                case "60":  //沪市
                    boardtype = EquityBoardType.Main;
                    return ExchangeType.SSE;

                default:    //深市
                    switch (code3)
                    {
                        case "000":
                        case "001":
                            boardtype = EquityBoardType.Main;
                            break;
                        case "002":
                            boardtype = EquityBoardType.SmallMedium;
                            break;
                        case "300":
                            boardtype = EquityBoardType.StartUp;
                            break;
                        default:
                            break;
                    }
                    return ExchangeType.SZSE;
            }
        }
        #endregion
    }
}
