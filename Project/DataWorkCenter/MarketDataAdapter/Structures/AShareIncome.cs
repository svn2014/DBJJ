using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarketDataAdapter
{
    public class AShareIncome : AShareBase
    {
        public readonly string PrimaryKey = "WindCode, ReportPeriod, StatementType";
        public string WindCode = "";
        public DateTime? ReportPeriod = null;
        public FinancialStatementType StatementType = FinancialStatementType.Merged;

        public CompanyType CorpType = CompanyType.NonFinancial;
        public DateTime? AnnouncementDate = null;

        public double? TotalOperatingRevenue = null;
        public double? OperatingRevenue = null;
        public double? TotalProfit = null;
        public double? NetProfitInclMinInt = null;
        //public double? NetProfitAfterDedNRLP = null;

        public double? TotalOperationCost = null;
        public double? OperationCost = null;
        public double? SellingExpence = null;
        public double? AdminExpence = null;
        public double? FinanceExpence = null;
        public double? NetInvestmentIncome = null;
        public double? NetProfitExclMinInt = null;  //净利润(不含少数股东损益)
    }
}
