using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarketDataAdapter
{
    public class AShareCalendar : AShareBase, IComparable
    {
        public readonly string PrimaryKey = "TradeDay";
        public DateTime? TradeDay = null;
        public ExchangeType Exchange = ExchangeType.SSE;

        public int CompareTo(object obj)
        {
            try
            {
                //按交易日降序
                AShareCalendar c = (AShareCalendar)obj;
                if (this.TradeDay > c.TradeDay)
                    return -1;
                else
                    return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
