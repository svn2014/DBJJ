using System;
using System.Diagnostics;

namespace Security
{
    public enum SecurityType
    {
        Equity,
        Bond,
        Index,
        Fund,
        Group
    }

    public enum ExchangeType
    {
        SSE,    //上交所
        SZSE,   //深交所
        IFE,    //中金所
        IBM,    //银行间
        OTC     //场外市场
    }

    public enum EquityBoardType
    {
        Main,           //主板
        SmallMedium,    //中小板
        StartUp         //创业板
    }

    public enum DataInfoType
    {
        SecurityInfo,
        SecurityReport,
        TradingPrice,
        FundNetAssetValue
    }

    public abstract class ASecurity : ASampleDatePeriod
    {
        public string DataSource;
        public string Code;
        public string Name;
        public SecurityType Type;
        public ExchangeType Exchange;
        public ATimeSeries TradingPrice;

        //设置时间区间
        public override void SetDatePeriod(DateTime start, DateTime end)
        {
            base.SetDatePeriod(start, end);
            this.BuildTimeSeries(this.Code, this.TimeSeriesStart, this.TimeSeriesEnd);
        }

        public abstract void LoadData(DataInfoType type);
        protected abstract void BuildTimeSeries(string code, DateTime start, DateTime end);
        
        public ASecurity(string code)
        {
            this.Code = code;
            this.BuildSecurity();
        }
        public ASecurity(string code, DateTime start, DateTime end)
        {
            this.Code = code;
            this.BuildSecurity();
            this.SetDatePeriod(start, end);
        }
        protected abstract void BuildSecurity();

        public virtual void DebugPrint(DataInfoType type, bool showheader){}
    }
}
