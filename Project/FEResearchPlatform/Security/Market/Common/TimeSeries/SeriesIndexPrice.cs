using System;

namespace Security
{
    public class SeriesIndexPrice : SeriesEquityPrice
    {
        #region 基础方法
        public SeriesIndexPrice(string code, DateTime start, DateTime end) : base(code, start, end) { }
        public override void Load()
        {
            try
            {
                DataManager.GetDataLoader().LoadIndexPrice(this);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
