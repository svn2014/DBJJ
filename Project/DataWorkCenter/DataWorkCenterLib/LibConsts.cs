using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataWorkCenterLib
{
    public class LibConsts
    {
        #region 以下常数不可以随便修改
        public const int C_Opmode_Locked = 2;
        public static DateTime C_Null_Date = new DateTime(1900, 1, 1);

        public const string C_Col_DataSrcName = "name";
        public const string C_Col_DataVandor = "vandor";
        public const string C_Col_DBType = "dbtype";
        public const string C_Col_DBConnection = "dbconnection";
        public const string C_Col_SaveDB = "SaveDB";

        public const string C_Col_TableName = "tablename";
        public const string C_Col_MainSource = "mainsrc";
        public const string C_Col_SubSource = "subsrc";
        public const string C_Col_CheckSource = "chksrc";
        #endregion

        public const string C_Msg_StartUpdate = "正在更新";
        public const string C_Msg_EndUpdate = "更新完毕";
        public const string C_Msg_NotActive = "未激活";

        public const string C_Msg_ErrorWithUpdate = "更新数据表时出错";
        public const string C_Msg_MissingDataLoaderManager = "没有设定数据源";
        public const string C_Msg_MissingMainSource = "主数据源无记录";
        public const string C_Msg_MissingSubSource = "副数据源无记录";
    }
}
