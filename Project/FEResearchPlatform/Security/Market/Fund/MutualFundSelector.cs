
namespace Security
{
    public class MutualFundSelector
    {
        public MutualFundGroup GetAllFund()
        {
            return DataManager.GetDataLoader().GetMutualFunds(null);
        }

        public MutualFundGroup GetActiveEquityOpenFund()
        {
            AFundCategory c = AFundCategory.GetFundCategory(FundCategoryType.GalaxySecurity);
            c.AssetCategory = FundAssetCategory.Equity;
            c.OperationCategory = FundOperationCategory.OpenEnd;
            c.InvestmentCategory = FundInvestmentCategory.Active;
            c.StructureCategory = FundStructureCategory.Parent;

            return DataManager.GetDataLoader().GetMutualFunds(c);
        }

        public MutualFundGroup GetActiveHybridOpenFund()
        {
            AFundCategory c = AFundCategory.GetFundCategory(FundCategoryType.GalaxySecurity);
            c.AssetCategory = FundAssetCategory.Hybrid;
            c.OperationCategory = FundOperationCategory.OpenEnd;
            c.InvestmentCategory = FundInvestmentCategory.Active;
            c.StructureCategory = FundStructureCategory.Parent;

            return DataManager.GetDataLoader().GetMutualFunds(c);
        }
    }
}
