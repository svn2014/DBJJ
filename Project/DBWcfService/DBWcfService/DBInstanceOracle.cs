using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;

namespace DBWcfService
{
    public class DBInstanceOracle : DBInstanceBase
    {
        public DBInstanceOracle()
        {
            try
            {
                _DBConnection = new OracleConnection(_ConnectionString);
                _DBConnection.Open();
            }
            catch (Exception ex)
            {                
                throw ex;
            }            
        }

        public DBInstanceOracle(string connString)
        {
            try
            {
                if (connString == null || connString.Length == 0)
                {
                    _DBConnection = new OracleConnection(_ConnectionString);
                    _DBConnection.Open();
                }
                else
                {
                    _DBConnection = new OracleConnection(connString);
                    _DBConnection.Open();
                }
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
                BiuldCommandObject(procedureName, procedureParameter, CommandType.StoredProcedure);
                _DBCommand.ExecuteNonQuery();

                if (outputParameterName == null || outputParameterName.Trim().Length == 0)
                    return null;
                else
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
                BiuldCommandObject(procedureName, procedureParameter, CommandType.StoredProcedure);
                DataSet ds = new DataSet();
                OracleDataAdapter da = new OracleDataAdapter((OracleCommand)_DBCommand);
                da.Fill(ds);

                return ds;
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
                BiuldCommandObject(sql, null, CommandType.Text);
                DataSet ds = new DataSet();
                OracleDataAdapter da = new OracleDataAdapter((OracleCommand)_DBCommand);
                da.Fill(ds);

                return ds;
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        protected override void BiuldCommandObject(string procedureName, List<ProcedureParameter> procedureParameter, CommandType commandType)
        {
            try
            {
                _DBCommand = new OracleCommand();
                _DBCommand.Connection = (OracleConnection)_DBConnection;
                _DBCommand.CommandType = commandType;
                _DBCommand.CommandText = procedureName;

                if (procedureParameter != null)
                {
                    foreach (ProcedureParameter para in procedureParameter)
                    {
                        if (para.Name == null)
                            continue;

                        OracleParameter oNewPara = new OracleParameter();
                        oNewPara.ParameterName = para.Name;
                        oNewPara.Direction = para.Direction;

                        if (para.Size > 0)
                            oNewPara.Size = para.Size;

                        if (para.Type != ProcedureParameter.DBType.Unknown)
                            oNewPara.OracleType = GetMappedDBType(para.Type);

                        if (para.Value != null)
                            oNewPara.Value = para.Value;

                        _DBCommand.Parameters.Add(oNewPara);
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private OracleType GetMappedDBType(ProcedureParameter.DBType dbType)
        {
            switch (dbType)
            {
                case ProcedureParameter.DBType.Bit:
                    throw new Exception("Bit不能用于Oracle");

                case ProcedureParameter.DBType.Char:
                    return OracleType.Char;

                case ProcedureParameter.DBType.VarChar:
                    return OracleType.VarChar;

                case ProcedureParameter.DBType.NVarChar:
                    return OracleType.NVarChar;

                case ProcedureParameter.DBType.NClob:
                    return OracleType.NClob;

                case ProcedureParameter.DBType.Float:
                    return OracleType.Number;

                case ProcedureParameter.DBType.Int:
                    return OracleType.Int16;

                case ProcedureParameter.DBType.BigInt:
                    return OracleType.Int32;

                case ProcedureParameter.DBType.Date:
                case ProcedureParameter.DBType.DateTime:
                    return OracleType.DateTime;

                case ProcedureParameter.DBType.Cursor:
                    return OracleType.Cursor;

                default:
                    return OracleType.NVarChar;
            }
        }
    }
}