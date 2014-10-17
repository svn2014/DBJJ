using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DBWcfService
{
    public class DBInstanceSqlServer : DBInstanceBase
    {
        public DBInstanceSqlServer()
        {
            try
            {
                _DBConnection = new SqlConnection(_ConnectionString);
                _DBConnection.Open();
            }
            catch (Exception ex)
            {                
                throw ex;
            }            
        }

        public override object ExecuteNonQuery(string procedureName, List<ProcedureParameter> procedureParameter, string outputParameterName)
        {
            try
            {
                BiuldStoredProcedure(procedureName, procedureParameter);
                _DBCommand.ExecuteNonQuery();
                return _DBCommand.Parameters[outputParameterName].Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override DataSet ExecuteStoredProcedure(string procedureName, List<ProcedureParameter> procedureParameter)
        {
            try
            {
                BiuldStoredProcedure(procedureName, procedureParameter);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter((SqlCommand)_DBCommand);
                da.Fill(ds);

                return ds;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        protected override void BiuldStoredProcedure(string procedureName, List<ProcedureParameter> procedureParameter)
        {
            try
            {
                _DBCommand = new SqlCommand();
                _DBCommand.Connection = (SqlConnection)_DBConnection;
                _DBCommand.CommandType = CommandType.StoredProcedure;
                _DBCommand.CommandText = procedureName;

                foreach (ProcedureParameter para in procedureParameter)
                {
                    SqlParameter oNewPara = new SqlParameter();
                    oNewPara.ParameterName = para.Name;
                    oNewPara.Direction = para.Direction;
                    oNewPara.Size = para.Size;
                    oNewPara.SqlDbType = GetMappedDBType(para.Type);
                    if (para.Value != null)
                        oNewPara.Value = para.Value;

                    _DBCommand.Parameters.Add(oNewPara);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private SqlDbType GetMappedDBType(ProcedureParameter.DBType dbType)
        {
            switch (dbType)
            {
                case ProcedureParameter.DBType.Bit:
                    return SqlDbType.Bit;

                case ProcedureParameter.DBType.Char:
                    return SqlDbType.Char;

                case ProcedureParameter.DBType.VarChar:
                    return SqlDbType.VarChar;

                case ProcedureParameter.DBType.NVarChar:
                    return SqlDbType.NVarChar;

                case ProcedureParameter.DBType.Float:
                    return SqlDbType.Float;

                case ProcedureParameter.DBType.Int:
                    return SqlDbType.Int;

                case ProcedureParameter.DBType.BigInt:
                    return SqlDbType.BigInt;

                case ProcedureParameter.DBType.Date:
                    return SqlDbType.Date;

                case ProcedureParameter.DBType.DateTime:
                    return SqlDbType.DateTime;

                case ProcedureParameter.DBType.Cursor:
                    throw new Exception("Cursor不能用于SqlServer");

                default:
                    return SqlDbType.NVarChar;
            }
        }
    }
}