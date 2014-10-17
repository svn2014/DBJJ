using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace PortfolioEvaluation
{
    public interface IFundDataAdapter
    {
        List<GZBItem> BuildGZBList(DataTable dtGZB);
    }

    public class FundDataAdaptorFactory
    {
        public enum AdapterType
        {
            YSS //赢时胜
        }

        public static IFundDataAdapter GetAdaptor(AdapterType type)
        {
            switch (type)
            {
                case AdapterType.YSS:
                    return new FundDataAdapterYSS();
                default:
                    return null;
            }
        }
    }
}
