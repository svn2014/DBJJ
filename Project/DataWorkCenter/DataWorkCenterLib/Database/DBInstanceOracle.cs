using System;
using System.Data;
using System.Data.OracleClient;

namespace DataWorkCenterLib
{
    public class DBInstanceOracle : ADBInstance
    {
        public DBInstanceOracle(string connString)
        {
            try
            {
                _DBConnection = new OracleConnection(connString);
                _DBConnection.Open();
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        public override DataSet ExecuteSQL(string sql)
        {
            try
            {
                _DBCommand = new OracleCommand();
                _DBCommand.Connection = (OracleConnection)_DBConnection;
                _DBCommand.CommandType = CommandType.Text;
                _DBCommand.CommandText = sql;

                DataSet ds = new DataSet();
                OracleDataAdapter da = new OracleDataAdapter((OracleCommand)_DBCommand);
                OracleCommandBuilder cb = new OracleCommandBuilder(da);

                da.Fill(ds);

                return ds;
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        public override void UpdateDB(string sql, DataTable dt)
        {
            try
            {
                OracleDataAdapter da = new OracleDataAdapter(sql, (OracleConnection)_DBConnection);
                OracleCommandBuilder cb = new OracleCommandBuilder(da);
                da.Update(dt);
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        public override string ConvertSqlDate(DateTime date)
        {
            string sql = "to_date('" + date.ToString("yyyyMMdd") + "','yyyyMMdd')";
            return sql;
        }
    }
}