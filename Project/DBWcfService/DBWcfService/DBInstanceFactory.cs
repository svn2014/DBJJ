
using System;
namespace DBWcfService
{
    public class DBInstanceFactory
    {
        public enum DBInstanceType
        {
            SqlServer,
            Oracle,
            OleDb
        }

        private static DBInstanceBase _Instance = null;
        public static DBInstanceBase GetDBInstance()
        {
            if (_Instance == null)
            {
                string strDBType = System.Configuration.ConfigurationManager.ConnectionStrings["DBType"].ConnectionString;
                DBInstanceType dbType = (DBInstanceType)Enum.Parse(typeof(DBInstanceType), strDBType, true);
                _Instance = GetDBInstance(dbType);
            }

            return _Instance;
        }

        public static DBInstanceBase GetDBInstance(DBInstanceType DBType)
        {
            switch (DBType)
            {
                case DBInstanceType.SqlServer:
                    return null;

                case DBInstanceType.Oracle:
                    return new DBInstanceOracle();

                case DBInstanceType.OleDb:
                    return null;

                default:
                    return null;
            }
        }
    }
}