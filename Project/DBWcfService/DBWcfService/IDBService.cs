using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace DBWcfService
{
    [ServiceContract]
    public interface IDBService
    {
        [OperationContract]
        object ExecuteNonQuery(string procedureName, List<ProcedureParameter> procedureParameter, string outputParameterName);

        [OperationContract]
        DataSet ExecuteStoredProcedure(string procedureName, List<ProcedureParameter> procedureParameter);

        [OperationContract]
        DataSet ExecuteSQL(string sql);

        [OperationContract]
        DataSet GetWebConfigsByProject(string project);

        [OperationContract]
        string GetHomeSiteIP();

        [OperationContract]
        string GetAlternativeSiteIP();

        [OperationContract]
        string GetAuthrizationCode();

        [OperationContract]
        DataSet GetAnalysts();

        [OperationContract]
        DataSet GetHedgeFunds();

        [OperationContract]
        DataSet GetPortfolios(string fundcode);

        [OperationContract]
        DataSet GetPortfolioBenchmarks(string portfolioCode);
    }

    [DataContract]
    public class ProcedureParameter
    {
        public enum DBType
        {
            Bit,
            Char,
            VarChar,
            NVarChar,
            NClob,   //Long String
            Float,
            Int,
            BigInt,
            Date,
            DateTime,
            Cursor,  //Oracle
            Unknown
        }

        int _size = 0;
        string _name = null;
        object _value = null;
        DBType _type = DBType.Unknown;
        ParameterDirection _direction = ParameterDirection.Input;

        public ProcedureParameter() { }
        public ProcedureParameter(string name, object value)
        {
            _name = name;
            _value = value;
        }
        public ProcedureParameter(string name, DBType type, ParameterDirection direction)
        {
            _name = name;
            _type = type;
            _direction = direction;
        }
        public ProcedureParameter(string name, DBType type, ParameterDirection direction, object value)
        {
            _name = name;
            _type = type;
            _direction = direction;
            _value = value;
        }

        [DataMember]
        public int Size
        {
            get { return _size; }
            set { _size = value; }
        }

        [DataMember]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [DataMember]
        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }

        [DataMember]
        public DBType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        [DataMember]
        public ParameterDirection Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }
    }
}
