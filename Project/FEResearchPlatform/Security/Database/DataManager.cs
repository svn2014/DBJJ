using System;
using System.Collections.Generic;

namespace Security
{
    public enum DatabaseType
    {
        SqlServer,
        Oracle
    }

    public enum DataVendor
    {
        CaiHui,     //财汇
        Wind,       //万得
        JuYuan,     //聚源
        DZH,        //大智慧
        ZhaoYang    //朝阳永续
    }

    public class DataManager
    {
        #region 构建数据库
        private static ADataLoader GetDataLoader(DataVendor vendor, ADBInstance instance)
        {
            switch (vendor)
            {
                case DataVendor.CaiHui:
                    return new DataLoaderCH(instance);

                case DataVendor.Wind:
                    throw new NotImplementedException();

                case DataVendor.JuYuan:
                    throw new NotImplementedException();

                case DataVendor.DZH:
                    throw new NotImplementedException();

                case DataVendor.ZhaoYang:
                    throw new NotImplementedException();

                default:
                    throw new NotImplementedException();
            }
        }
        private static ADBInstance GetDBInstance(DatabaseType type, string conn)
        {
            if (conn.Length == 0)
                return null;

            switch (type)
            {
                case DatabaseType.Oracle:
                    return new DBInstanceOracle(conn);

                case DatabaseType.SqlServer:
                    throw new NotImplementedException();
                    
                default:
                    throw new NotImplementedException();
            }
        }

        private static ADataLoader _DataLoader;
        public static void Initiate(DataVendor vendor, DatabaseType type, string conn)
        {
            ADBInstance db = GetDBInstance(type, conn);
            _DataLoader = GetDataLoader(vendor, db);
        }
        public static ADataLoader GetDataLoader()
        {
            if (_DataLoader == null)
                throw new Exception(Message.C_Msg_DB5);

            return _DataLoader;
        }
        #endregion

        #region 常用函数
        public static readonly DateTime C_Null_Date = new DateTime(1900, 1, 1);
        public static DateTime ConvertToDate(object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return C_Null_Date;

            try
            {
                string strDate = obj.ToString();
                if (strDate.Length == 8)
                    return new DateTime(Convert.ToInt16(strDate.Substring(0, 4)),
                                        Convert.ToInt16(strDate.Substring(4, 2)),
                                        Convert.ToInt16(strDate.Substring(6, 2)));
                else
                    return Convert.ToDateTime(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }                
        }
        public static double ConvertToDouble(object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return 0;

            try
            {
                return Convert.ToDouble(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            } 
        }
        #endregion
    }
}
