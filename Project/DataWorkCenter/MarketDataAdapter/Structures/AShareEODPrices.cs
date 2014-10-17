using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarketDataAdapter
{
    public class AShareEODPrices : AShareBase, IComparable
    {
        public readonly string PrimaryKey = "WindCode,TradeDate";
        public DateTime? TradeDate = null;
        public string WindCode = "";
        public TradingStatus? Status = null;
        public AShareDescription Description = new AShareDescription();

        //原始价
        public double? PreClose = null;
        public double? Open = null;
        public double? High = null;
        public double? Low = null;
        public double? Close = null;
        public double? Average = null;

        ////复权系数
        //public double? AdjustedFactor = null;

        ////复权价
        //public double? AdjustedPreClose = null;
        //public double? AdjustedOpen = null;
        //public double? AdjustedHigh = null;
        //public double? AdjustedLow = null;
        //public double? AdjustedClose = null;
        //public double? AdjustedAverage = null;
        
        //量
        public double? Volume = null;   //单位：股
        public double? Amount = null;   //单位：元

        //变动%
        public double? PctChange = null;

        public int CompareTo(object obj)
        {
            try
            {
                //按交易日降序
                AShareEODPrices p = (AShareEODPrices)obj;
                if (this.TradeDate > p.TradeDate)
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
