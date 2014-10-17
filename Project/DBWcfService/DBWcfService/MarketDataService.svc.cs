using System;
using System.Data;
using System.Collections.Generic;

namespace DBWcfService
{
    public class MarketDataService : IMarketDataService
    {
        public DataSet GetHS300Weight(DateTime endDate)
        {
            try
            {
                List<ProcedureParameter> paraList = new List<ProcedureParameter>();
                paraList.Add(new ProcedureParameter("i_date", endDate.ToString("yyyyMMdd")));
                paraList.Add(new ProcedureParameter("o_cursor", ProcedureParameter.DBType.Cursor, ParameterDirection.Output));

                return DBInstanceFactory.GetDBInstance().ExecuteStoredProcedure("mkt_proc_selecths300weight", paraList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetIndexPrices(DateTime startDate, DateTime endDate, string code)
        {
            try
            {
                List<ProcedureParameter> paraList = new List<ProcedureParameter>();
                paraList.Add(new ProcedureParameter("i_startdate", startDate.ToString("yyyyMMdd")));
                paraList.Add(new ProcedureParameter("i_enddate", endDate.ToString("yyyyMMdd")));
                paraList.Add(new ProcedureParameter("i_code", code));
                paraList.Add(new ProcedureParameter("o_cursor", ProcedureParameter.DBType.Cursor, ParameterDirection.Output));

                return DBInstanceFactory.GetDBInstance().ExecuteStoredProcedure("mkt_proc_selectindexprices", paraList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetStockPrices(DateTime startDate, DateTime endDate, string code)
        {
            try
            {
                List<ProcedureParameter> paraList = new List<ProcedureParameter>();
                paraList.Add(new ProcedureParameter("i_startdate", startDate.ToString("yyyyMMdd")));
                paraList.Add(new ProcedureParameter("i_enddate", endDate.ToString("yyyyMMdd")));
                paraList.Add(new ProcedureParameter("i_code", code));
                paraList.Add(new ProcedureParameter("o_cursor", ProcedureParameter.DBType.Cursor, ParameterDirection.Output));

                return DBInstanceFactory.GetDBInstance().ExecuteStoredProcedure("mkt_proc_selectequityprices", paraList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAShareStockList(string code, string exchange, string charIsST, string charIsActive)
        {
            try
            {
                List<ProcedureParameter> paraList = new List<ProcedureParameter>();
                paraList.Add(new ProcedureParameter("i_exchange", exchange));
                paraList.Add(new ProcedureParameter("i_isst", charIsST));
                paraList.Add(new ProcedureParameter("i_isactive", charIsActive));                
                paraList.Add(new ProcedureParameter("i_code", code));
                paraList.Add(new ProcedureParameter("o_cursor", ProcedureParameter.DBType.Cursor, ParameterDirection.Output));

                return DBInstanceFactory.GetDBInstance().ExecuteStoredProcedure("mkt_proc_selectasharelist", paraList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetFreeFloatCapital(DateTime endDate, string code)
        {
            try
            {
                List<ProcedureParameter> paraList = new List<ProcedureParameter>();
                paraList.Add(new ProcedureParameter("i_date", endDate.ToString("yyyyMMdd")));
                paraList.Add(new ProcedureParameter("i_code", code));
                paraList.Add(new ProcedureParameter("o_cursor", ProcedureParameter.DBType.Cursor, ParameterDirection.Output));

                return DBInstanceFactory.GetDBInstance().ExecuteStoredProcedure("mkt_proc_selectfreecapital", paraList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public DataSet GetSWIndustryList()
        {
            try
            {
                List<ProcedureParameter> paraList = new List<ProcedureParameter>();
                paraList.Add(new ProcedureParameter("o_cursor", ProcedureParameter.DBType.Cursor, ParameterDirection.Output));

                return DBInstanceFactory.GetDBInstance().ExecuteStoredProcedure("mkt_proc_selectswindustry", paraList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTradingDays(DateTime startDate, DateTime endDate)
        {
            try
            {
                List<ProcedureParameter> paraList = new List<ProcedureParameter>();
                paraList.Add(new ProcedureParameter("o_cursor", ProcedureParameter.DBType.Cursor, ParameterDirection.Output));
                paraList.Add(new ProcedureParameter("i_startdate", ProcedureParameter.DBType.NVarChar, ParameterDirection.Input, startDate.ToString("yyyyMMdd")));
                paraList.Add(new ProcedureParameter("i_enddate", ProcedureParameter.DBType.NVarChar, ParameterDirection.Input, endDate.ToString("yyyyMMdd")));
                return DBInstanceFactory.GetDBInstance().ExecuteStoredProcedure("mkt_proc_selecttradedays", paraList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetChinaBondList(string code, string exchange)
        {
            try
            {
                List<ProcedureParameter> paraList = new List<ProcedureParameter>();
                paraList.Add(new ProcedureParameter("i_exchange", exchange));
                paraList.Add(new ProcedureParameter("i_code", code));
                paraList.Add(new ProcedureParameter("o_cursor", ProcedureParameter.DBType.Cursor, ParameterDirection.Output));

                return DBInstanceFactory.GetDBInstance().ExecuteStoredProcedure("mkt_proc_selectchinabondlist", paraList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
