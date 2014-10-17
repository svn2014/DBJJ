using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarketDataAdapter
{
    public class AShareBalanceSheet : AShareBase
    {
        public readonly string PrimaryKey = "WindCode, ReportPeriod, StatementType";
        public string WindCode = "";
        public DateTime? ReportPeriod = null;
        public FinancialStatementType StatementType = FinancialStatementType.Merged;

        public CompanyType CorpType = CompanyType.NonFinancial;
        public DateTime? AnnouncementDate = null;

        public double? Inventories = null;
        public double? TotalCurrentAssets = null;
        public double? TotalAssets = null;
        public double? TotalCurrentLiability = null;
        public double? TotalLiability = null;
        public double? TotalEquityInclMinInt = null;

        public double? MonetoryCap = null;
        public double? AccountRecievable = null;
        public double? Prepay = null;
        public double? FixAssets = null;
        public double? ConstructionInProgress = null;
    }
}
