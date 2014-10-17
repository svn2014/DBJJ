using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace PortfolioEvaluation
{
    public interface IMarketDataAdapter
    {
        List<EquityPrice> GetPriceList(DataTable dtPriceInfo);
        List<AssetPosition> GetPositionList(DataTable dtPositionInfo);
        List<EquityIndustry> GetIndustryList(DataTable dtIndustryInfo);
        List<TradingDays> GetTradeingDays(DataTable dtTradingDays);
    }

    public class MarketDataAdaptorFactory
    {
        public enum AdapterType
        {
            Wind
        }

        public static IMarketDataAdapter GetAdaptor(AdapterType type)
        {
            switch (type)
            {
                case AdapterType.Wind:
                    return new MarketDataAdapterWind();
                default:
                    return null;
            }
        }
    }
}
