
namespace DataWorkCenterLib
{
    public enum DBInstanceType
    {
        SqlServer,
        Oracle,
        OleDb
    }

    public class DBInstanceFactory
    {
        public static ADBInstance GetDBInstance(DBInstanceType DBType,string connString)
        {
            if (connString.Length == 0)
                return null;

            switch (DBType)
            {
                case DBInstanceType.SqlServer:
                    return null;

                case DBInstanceType.Oracle:
                    return new DBInstanceOracle(connString);

                case DBInstanceType.OleDb:
                    return null;

                default:
                    return null;
            }
        }
    }
}