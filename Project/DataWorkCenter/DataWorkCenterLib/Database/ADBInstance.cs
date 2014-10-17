using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Common;
using System.Data;

namespace DataWorkCenterLib
{
    public abstract class ADBInstance
    {
        protected DbConnection _DBConnection = null;
        protected DbCommand _DBCommand = null;

        public abstract DataSet ExecuteSQL(string sql);
        public abstract void UpdateDB(string sql, DataTable dt);

        public abstract string ConvertSqlDate(DateTime date);

        public void Close()
        {
            if (_DBConnection.State != ConnectionState.Closed)
            {
                try
                {
                    _DBConnection.Close();
                }
                catch (Exception ex)
                {                    
                    throw ex;
                }
            }
        }
    }
}