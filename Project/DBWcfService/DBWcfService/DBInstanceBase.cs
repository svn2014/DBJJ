using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Common;
using System.Data;

namespace DBWcfService
{
    public abstract class DBInstanceBase
    {
        protected string _ConnectionString = "";
        protected DbConnection _DBConnection = null;
        protected DbCommand _DBCommand = null;

        public DBInstanceBase()
        {
            string type = System.Configuration.ConfigurationManager.ConnectionStrings["DBType"].ConnectionString;
            _ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[type].ConnectionString;
        }

        protected abstract void BiuldCommandObject(string procedureName, List<ProcedureParameter> procedureParameter, CommandType commandType);

        public abstract DataSet ExecuteStoredProcedure(string procedureName, List<ProcedureParameter> procedureParameter);
        public abstract DataSet ExecuteSQL(string sql);
        public abstract object ExecuteNonQuery(string procedureName, List<ProcedureParameter> procedureParameter, string outputParameterName);
    }
}