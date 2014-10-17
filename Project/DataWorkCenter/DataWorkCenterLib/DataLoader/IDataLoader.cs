using System;
using System.Collections.Generic;
using MarketDataAdapter;

namespace DataWorkCenterLib
{
    public interface IDataLoader
    {
        List<AShareCalendar> GetAShareCalendar();
        List<AShareDescription> GetAShareDescription();
        List<AShareEODPrices> GetAShareEODPrices();
        List<AShareBalanceSheet> GetAShareBalanceSheet();
        List<AShareIncome> GetAShareIncome();
    }
}
