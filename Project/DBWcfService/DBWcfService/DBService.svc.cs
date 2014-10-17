using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System;

namespace DBWcfService
{
    public class DBService : IDBService
    {
        DataSet IDBService.ExecuteStoredProcedure(string procedureName, List<ProcedureParameter> procedureParameter)
        {
            return DBInstanceFactory.GetDBInstance().ExecuteStoredProcedure(procedureName, procedureParameter);
        }

        public object ExecuteNonQuery(string procedureName, List<ProcedureParameter> procedureParameter, string outputParameterName)
        {
            return DBInstanceFactory.GetDBInstance().ExecuteNonQuery(procedureName, procedureParameter, outputParameterName);
        }

        public DataSet ExecuteSQL(string sql)
        {
            return DBInstanceFactory.GetDBInstance().ExecuteSQL(sql);
        }




        public DataSet GetWebConfigsByProject(string project)
        {
            List<ProcedureParameter> paraList = new List<ProcedureParameter>();
            paraList.Add(new ProcedureParameter("i_project", project));
            paraList.Add(new ProcedureParameter("o_cursor", ProcedureParameter.DBType.Cursor, ParameterDirection.Output));

            return DBInstanceFactory.GetDBInstance().ExecuteStoredProcedure("db0_proc_selectwebconfig", paraList);
        }

        public string GetHomeSiteIP()
        {
            DataSet ds = GetWebConfigsByProject("POTL");
            DataTable dt = ds.Tables[0];

            DataRow[] oRows = dt.Select("KEY = 'IP'");

            string ip = null;
            if (oRows.Length > 0)
                ip = oRows[0]["VALUE"].ToString();

            return ip;
        }

        public string GetAlternativeSiteIP()
        {
            DataSet ds = GetWebConfigsByProject("EQTY");
            DataTable dt = ds.Tables[0];

            DataRow[] oRows = dt.Select("KEY = 'IP'");

            string ip = null;
            if (oRows.Length > 0)
                ip = oRows[0]["VALUE"].ToString();

            return ip;
        }
                



        public DataSet GetAnalysts()
        {
            List<ProcedureParameter> paraList = new List<ProcedureParameter>();
            paraList.Add(new ProcedureParameter("o_cursor", ProcedureParameter.DBType.Cursor, ParameterDirection.Output));
            return DBInstanceFactory.GetDBInstance().ExecuteStoredProcedure("db1_proc_selectanalyst", paraList);
        }

        public DataSet GetHedgeFunds()
        {
            List<ProcedureParameter> paraList = new List<ProcedureParameter>();
            paraList.Add(new ProcedureParameter("o_cursor", ProcedureParameter.DBType.Cursor, ParameterDirection.Output));
            return DBInstanceFactory.GetDBInstance().ExecuteStoredProcedure("db1_proc_selecthedgefund", paraList);
        }

        public DataSet GetPortfolios(string fundcode)
        {
            List<ProcedureParameter> paraList = new List<ProcedureParameter>();
            paraList.Add(new ProcedureParameter("i_fundcode", ProcedureParameter.DBType.NVarChar, ParameterDirection.Input, fundcode));
            paraList.Add(new ProcedureParameter("i_startdate", ProcedureParameter.DBType.NVarChar, ParameterDirection.Input, null));
            paraList.Add(new ProcedureParameter("i_enddate", ProcedureParameter.DBType.NVarChar, ParameterDirection.Input, null));
            paraList.Add(new ProcedureParameter("o_cursor", ProcedureParameter.DBType.Cursor, ParameterDirection.Output));
            return DBInstanceFactory.GetDBInstance().ExecuteStoredProcedure("db5_proc_selectportfoliolist", paraList);
        }

        public DataSet GetPortfolioBenchmarks(string portfolioCode)
        {
            List<ProcedureParameter> paraList = new List<ProcedureParameter>();
            paraList.Add(new ProcedureParameter("i_fundcode", ProcedureParameter.DBType.NVarChar, ParameterDirection.Input, portfolioCode));
            paraList.Add(new ProcedureParameter("o_cursor", ProcedureParameter.DBType.Cursor, ParameterDirection.Output));
            return DBInstanceFactory.GetDBInstance().ExecuteStoredProcedure("db5_proc_selectbenchmarklist", paraList);
        }
        
        public string GetAuthrizationCode()
        {
            DateTime authDate = DateTime.Now;
            string authCode = authDate.ToString("yyyyMMddHH");
            authCode += DateTime.Today.DayOfWeek.ToString().Substring(0, 1);
            return authCode;
        }
    }
}
