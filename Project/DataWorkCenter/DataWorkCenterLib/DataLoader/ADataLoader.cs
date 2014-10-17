using System.Collections.Generic;
using MarketDataAdapter;
using System;
using System.Data;
using System.Reflection;

namespace DataWorkCenterLib
{
    public abstract class ADataLoader : IDataLoader
    {
        //数据库
        protected ADBInstance _DBInstance = null;
        protected AMarketDataAdapter _MarketDataAdapter = null;
        public VandorType CurrentVandor;

        protected string _WindCode = null;
        protected DateTime _StartDate = DateTime.Today.AddDays(-1);
        protected DateTime _EndDate = DateTime.Today;
        protected DateTime _LoopingDate = DateTime.Today.AddDays(-1);

        public void SetParameter(string WindCode, DateTime StartDate, DateTime EndDate, DateTime TradingDate)
        {
            this._WindCode = WindCode;
            this._StartDate = StartDate;
            this._EndDate = EndDate;
            this._LoopingDate = TradingDate;
        }

        public void Close()
        {
            if (_DBInstance != null)
                _DBInstance.Close();
        }

        protected virtual List<T> ReadDbData<T>(string sql)
        {
            try
            {
                DataSet ds = _DBInstance.ExecuteSQL(sql);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Type dcType = this._MarketDataAdapter.GetType();
                    MethodInfo mi = dcType.GetMethod("Get" + typeof(T).Name);

                    return (List<T>)mi.Invoke(this._MarketDataAdapter, new object[] { ds.Tables[0] });
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public abstract List<AShareDescription> GetAShareDescription();
        public abstract List<AShareCalendar> GetAShareCalendar();
        public abstract List<AShareEODPrices> GetAShareEODPrices();
        public abstract List<AShareBalanceSheet> GetAShareBalanceSheet();
        public abstract List<AShareIncome> GetAShareIncome();
    }
}
