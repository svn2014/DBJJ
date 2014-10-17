using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Security
{
    public class SeriesNetAssetValue : ATimeSeries
    {
        #region 基础方法
        public SeriesNetAssetValue(string code, DateTime start, DateTime end) : base(code, start, end) { }
        public override void Load()
        {
            try
            {
                DataManager.GetDataLoader().LoadMutualFundNAV(this);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override void Calculate()
        {
            //====================
            //计算涨跌幅: 时间序列全部降序排列
            //====================
            if (base.AdjustedTimeSeries == null || base.AdjustedTimeSeries.Count == 0)
                return;

            double nav0 = 1, nav1 = 1;
            DateTime dt0 = DateTime.Today, dt1 = DateTime.Today;
            for (int i = 0; i < this.AdjustedTimeSeries.Count; i++)
            {
                NetAssetValue item = (NetAssetValue)this.AdjustedTimeSeries[i];
                if ((i + 1) < this.AdjustedTimeSeries.Count)
                    item.UpAndDown.KLineDay1 = item.AccumUnitNAV / ((NetAssetValue)this.AdjustedTimeSeries[i + 1]).AccumUnitNAV - 1;
                if ((i + 2) < this.AdjustedTimeSeries.Count)
                    item.UpAndDown.KLineDay2 = item.AccumUnitNAV / ((NetAssetValue)this.AdjustedTimeSeries[i + 2]).AccumUnitNAV - 1;
                if ((i + 3) < this.AdjustedTimeSeries.Count)
                    item.UpAndDown.KLineDay3 = item.AccumUnitNAV / ((NetAssetValue)this.AdjustedTimeSeries[i + 3]).AccumUnitNAV - 1;
                if ((i + 4) < this.AdjustedTimeSeries.Count)
                    item.UpAndDown.KLineDay4 = item.AccumUnitNAV / ((NetAssetValue)this.AdjustedTimeSeries[i + 4]).AccumUnitNAV - 1;
                if ((i + 5) < this.AdjustedTimeSeries.Count)
                    item.UpAndDown.KLineDay5 = item.AccumUnitNAV / ((NetAssetValue)this.AdjustedTimeSeries[i + 5]).AccumUnitNAV - 1;
                if ((i + 10) < this.AdjustedTimeSeries.Count)
                    item.UpAndDown.KLineDay10 = item.AccumUnitNAV / ((NetAssetValue)this.AdjustedTimeSeries[i + 10]).AccumUnitNAV - 1;
                if ((i + 20) < this.AdjustedTimeSeries.Count)
                    item.UpAndDown.KLineDay20 = item.AccumUnitNAV / ((NetAssetValue)this.AdjustedTimeSeries[i + 20]).AccumUnitNAV - 1;

                //期末值
                if (i==0)
                {
                    nav1 = item.AccumUnitNAV;
                    dt1 = item.TradeDate;
                }
                //期初值
                if (item.TradeDate >= base.TimeSeriesStart)
                {
                    nav0 = item.AccumUnitNAV;
                    dt0 = item.TradeDate;
                }
            }

            //持有期收益率
            base.HoldingPeriodInfo = nav0.ToString("N4") + "[" + dt0.ToString("yyyy-MM-dd") + "]-" + nav1.ToString("N4") + "[" + dt1.ToString("yyyy-MM-dd") + "]";
            base.HoldingPeriodReturn = nav1 / nav0 - 1;
        }
        #endregion
    }
}
