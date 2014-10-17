using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarketDataAdapter;

namespace DataWorkCenterLib
{
    public interface IDataSaver
    {
        void SaveAShareDescription(List<AShareDescription> lstSrc);
        void SaveAShareEODPrices(List<AShareEODPrices> lstSrc);
        void SaveAShareCalendar(List<AShareCalendar> lstSrc);
        void SaveAShareBalanceSheet(List<AShareBalanceSheet> lstSrc);
        void SaveAShareIncome(List<AShareIncome> lstSrc);
    }
}
