using System;
using System.Diagnostics;

namespace Security
{
    public class SeriesFundReport: ATimeSeries
    {
        #region 基础方法
        public SeriesFundReport(string code, DateTime start, DateTime end) : base(code, start, end) { }
        public override void Load()
        {
            try
            {
                DataManager.GetDataLoader().LoadMutualFundReport(this);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public override void DebugPrint()
        //{
        //    //if (base.TradingDates != null && base.TradingDates.Count > 0)
        //    //{
        //    //    Debug.WriteLine("====================");
        //    //    for (int i = 0; i < base.TradingDates.Count; i++)
        //    //    {
        //    //        if (i < base.AdjustedTimeSeries.Count && base.TradingDates[i] == base.AdjustedTimeSeries[i].TradeDate)
        //    //        {
        //    //            Debug.Write(base.TradingDates[i].ToString("yyyy-MM-dd") + "\t");
        //    //            MutualFundReport rpt = (MutualFundReport)base.AdjustedTimeSeries[i];
        //    //            Debug.Write((rpt.TotalShare / 10000).ToString("N2") + "\t");
        //    //            Debug.Write((rpt.TotalNetAsset / 10000).ToString("N2") + "\t");
        //    //            Debug.Write((rpt.TotalEquityAsset / 10000).ToString("N2") + "\t");
        //    //            Debug.WriteLine((rpt.TotalBondAsset / 10000).ToString("N2") + "\t");
        //    //        }
        //    //        else
        //    //        {
        //    //            Debug.Write(base.TradingDates[i].ToString("yyyy-MM-dd") + "\t");
        //    //            Debug.WriteLine("#N/A");
        //    //        }
        //    //    }
        //    //}
        //}
        #endregion
    }
}
