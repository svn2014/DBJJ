using System;
using System.Diagnostics;

namespace Security
{
    public class Index: ASecurity
    {
        #region 基础方法
        public Index(string code) : base(code) { }
        public Index(string code, DateTime start, DateTime end) : base(code, start, end) { }
        protected override void BuildSecurity()
        {
            base.Exchange = ExchangeType.OTC;
            base.Type = SecurityType.Index;
        }
        protected override void BuildTimeSeries(string code, DateTime start, DateTime end)
        {
            if (base.TradingPrice == null)
                base.TradingPrice = new SeriesIndexPrice(code, start, end);
            base.TradingPrice.SetDatePeriod(start, end);
        }
        public override void LoadData(DataInfoType type)
        {
            switch (type)
            {
                case DataInfoType.SecurityInfo:
                    DataManager.GetDataLoader().LoadIndexInfo(this);
                    break;
                case DataInfoType.TradingPrice:
                    if (base.TradingPrice == null)
                        base.TradingPrice = new SeriesIndexPrice(base.Code, base.TimeSeriesStart, base.TimeSeriesEnd);
                    base.TradingPrice.Load();
                    break;
                default:
                    MessageManager.GetInstance().AddMessage(MessageType.Warning, Message.C_Msg_GE1, type.ToString());
                    return;
            }
        }
        //public override void DebugPrintInfo()
        //{
        //    base.DebugPrintInfo();

        //    Debug.Write(this.Category + "\t");
        //    Debug.Write(this.ListedDate.ToString("yyyy-MM-dd") + "\t");
        //    Debug.Write(this.DelistedDate.ToString("yyyy-MM-dd") + "\t");
        //    Debug.WriteLine("");
        //}
        #endregion

        #region 扩展属性
        public DateTime ListedDate;
        public DateTime DelistedDate;
        public string Category;
        #endregion 
    }
}
