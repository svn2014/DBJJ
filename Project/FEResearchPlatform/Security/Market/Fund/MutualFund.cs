using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Security
{
    public enum FundOperationCategory
    {
        Undefined,
        OpenEnd,
        CloseEnd
    }

    public enum FundAssetCategory
    {
        Undefined,
        Equity,
        Hybrid,
        Bond,
        Monetory,
        QDII,
        Other
    }

    public enum FundInvestmentCategory
    {
        Undefined,
        Active,
        Passive
    }

    public enum FundStructureCategory
    {
        Undefined,
        Parent,
        Child
    }

    public class MutualFund : ASecurity
    {
        #region 基础方法
        public MutualFund(string code) : base(code) { }
        public MutualFund(string code, DateTime start, DateTime end) : base(code, start, end) { }
        protected override void BuildSecurity()
        {
            base.Exchange = ExchangeType.OTC;
            base.Type = SecurityType.Fund;            
            this.Category = AFundCategory.GetFundCategory(FundCategoryType.GalaxySecurity);
        }
        protected override void BuildTimeSeries(string code, DateTime start, DateTime end)
        {
            if (this.TradingNAV == null)
                this.TradingNAV = new SeriesNetAssetValue(code, start, end);
            this.TradingNAV.SetDatePeriod(start, end);

            if (base.TradingPrice == null)
                base.TradingPrice = new SeriesFundPrice(code, start, end);
            base.TradingPrice.SetDatePeriod(start, end);

            if (this.FundReport == null)
                this.FundReport = new SeriesFundReport(code, start, end);
            this.FundReport.SetDatePeriod(start, end);
        }
        public override void LoadData(DataInfoType type)
        {
            try
            {
                switch (type)
                {
                    case DataInfoType.SecurityInfo:
                        DataManager.GetDataLoader().LoadMutualFundInfo(this);
                        break;
                    case DataInfoType.FundNetAssetValue:
                        if (this.TradingNAV == null)
                            this.TradingNAV = new SeriesNetAssetValue(base.Code, base.TimeSeriesStart, base.TimeSeriesEnd);
                        //仅对非货币基金
                        if (this.Category.AssetCategory != FundAssetCategory.Monetory)
                            this.TradingNAV.Load();
                        break;
                    case DataInfoType.TradingPrice:
                        if (base.TradingPrice == null)
                            base.TradingPrice = new SeriesFundPrice(base.Code, base.TimeSeriesStart, base.TimeSeriesEnd);
                        base.TradingPrice.Load();
                        break;
                    case DataInfoType.SecurityReport:
                        if (this.FundReport == null)
                        {
                            this.FundReport = new SeriesFundReport(base.Code, base.TimeSeriesStart, base.TimeSeriesEnd);
                        }
                        this.FundReport.Load();
                        break;
                    default:
                        MessageManager.GetInstance().AddMessage(MessageType.Warning, Message.C_Msg_GE1, type.ToString());
                        return;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("读取基金数据时出错！", ex); ;
            }
        }
        //public override void DebugPrintInfo()
        //{
        //    base.DebugPrintInfo();

        //    Debug.Write(this.Category.AssetCategory.ToString() + "\t");
        //    Debug.Write(this.Category.InvestmentCategory.ToString() + "\t");
        //    Debug.Write(this.Category.OperationCategory.ToString() + "\t");
        //    Debug.Write(this.Category.StructureCategory.ToString() + "\t");
        //    Debug.Write(this.ListedDate.ToString("yyyy-MM-dd") + "\t");
        //    Debug.Write(this.DelistedDate.ToString("yyyy-MM-dd") + "\t");
        //    Debug.WriteLine("");            
        //}
        //public override void DebugPrintTimeSeries()
        //{
        //    base.DebugPrintTimeSeries();

        //    Debug.Indent();
        //    if (this.TradingNAV != null)
        //        this.TradingNAV.DebugPrint();
        //    Debug.Unindent();
        //    Debug.Indent();
        //    if (this.FundReport != null)
        //        this.FundReport.DebugPrint();
        //    Debug.Unindent();
        //}
        #endregion

        #region 扩展属性
        public AFundCategory Category;
        public DateTime ListedDate;
        public DateTime DelistedDate;
        public bool IsStructured = false;
        public string ParentFundCode = "";
        public List<string> SubFundCodes;
        public SeriesNetAssetValue TradingNAV;
        public SeriesFundReport FundReport;
        #endregion
    }
}
