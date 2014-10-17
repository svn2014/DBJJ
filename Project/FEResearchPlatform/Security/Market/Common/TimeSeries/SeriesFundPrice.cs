using System;

namespace Security
{
    public class SeriesFundPrice: SeriesEquityPrice
    {
        #region 基础方法
        public SeriesFundPrice(string code, DateTime start, DateTime end) : base(code, start, end) { }
        public override void Load()
        {
            try
            {
                DataManager.GetDataLoader().LoadMutualFundPrice(this);
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }
        #endregion
    }
}
