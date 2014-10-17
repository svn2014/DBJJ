using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Security
{
    public class TradingDay: ATimeSeries
    {
        public TradingDay()
        {
            this.Load();
        }
        public TradingDay(DateTime start, DateTime end) : base("", start, end)
        {
            base.SetDatePeriod(start, end);
            this.Load();
        }
        public override void Load()
        {
            if (this.TradingDates.Count == 0)
                DataManager.GetDataLoader().LoadTradingDate(this);
        }
        public override void Adjust() { }
    }
}
