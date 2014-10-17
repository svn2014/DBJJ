using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarketDataAdapter;
using System.Data;
using System.Collections;
using System.Reflection;

namespace DataWorkCenterLib
{
    public class DataLoaderManager
    {
        public string TableName = "";
        public ADataLoader MainSource = null;
        public ADataLoader SubSource = null;
        public ADataLoader CheckSource = null;
        public bool IsActive = true;
        public bool SaveToDB = false;
    }

    public class DataLoaderProcessor: IDataLoader
    {
        //测试模式：不用保存
        public bool OnTestMode = false;

        //运行参数
        private string Para_WindCode = "";
        private DateTime Para_StartDate = DateTime.Today.AddDays(-1);
        private DateTime Para_LoopingDate = DateTime.Today;
        private DateTime Para_EndDate = DateTime.Today;
        private bool CurrentActive = false;

        private DataCenter _DataCenter = null;        
        private List<ADataLoader> _DataLoaderList = new List<ADataLoader>();
        public List<DataLoaderManager> DataLoaderManagerList = new List<DataLoaderManager>();
        private ADBInstance _DBInstance = null;

        public void AddDataCenter(string dBType, string connString)
        {
            DBInstanceType dbInstanceType = (DBInstanceType)Enum.Parse(typeof(DBInstanceType), dBType, true);
            ADBInstance dbInstance = DBInstanceFactory.GetDBInstance(dbInstanceType, connString);
            this._DataCenter = new DataCenter(dbInstance);
            this._DBInstance = dbInstance;
        }

        public void BuildWorkerByDBConfig()
        {
            //==============================
            //搜索现有的数据源
            //==============================
            string sql = "select * from datacentersrc where active = 1";

            DataSet dsDataSrc = this._DBInstance.ExecuteSQL(sql);

            if (dsDataSrc == null || dsDataSrc.Tables.Count == 0 || dsDataSrc.Tables[0].Rows.Count == 0)
                return;

            foreach (DataRow oRow in dsDataSrc.Tables[0].Rows)
            {
                string dbType = oRow[LibConsts.C_Col_DBType].ToString();
                string dbConn = oRow[LibConsts.C_Col_DBConnection].ToString();
                string vandor = oRow[LibConsts.C_Col_DataVandor].ToString();


                DBInstanceType dbInstanceType = (DBInstanceType)Enum.Parse(typeof(DBInstanceType), dbType, true);
                ADBInstance inst = DBInstanceFactory.GetDBInstance(dbInstanceType, dbConn);
                VandorType vandorType = (VandorType)Enum.Parse(typeof(VandorType), vandor, true);
                ADataLoader loader = DataLoaderFactory.GetDataLoader(inst, vandorType);

                _DataLoaderList.Add(loader);
            }

            //==============================
            //搜索现有的数据表
            //==============================
            sql = @"Select A.Tablename, B.Vandor as Mainsrc, C.Vandor as Subsrc, D.Vandor as Chksrc, A.SaveDB
                    From datacenterconfig A
                    Left join datacentersrc B
                         ON A.Mainsrc = B.Src
                    Left join datacentersrc C
                         ON A.Subsrc = C.Src 
                    Left join datacentersrc D
                         ON A.Chksrc = D.Src ";

            if (OnTestMode)
                sql += "Where A.ActiveTest = 1";
            else
                sql += "Where A.Active = 1";

            DataSet dsDataTables = this._DBInstance.ExecuteSQL(sql);

            if (dsDataTables == null || dsDataTables.Tables.Count == 0 || dsDataTables.Tables[0].Rows.Count == 0)
                return;

            foreach (DataRow oRow in dsDataTables.Tables[0].Rows)
            {
                DataLoaderManager w = new DataLoaderManager();
                w.TableName = oRow[LibConsts.C_Col_TableName].ToString();

                //Main
                string strVandor = oRow[LibConsts.C_Col_MainSource].ToString();
                VandorType vandorType = (VandorType)Enum.Parse(typeof(VandorType), strVandor, true);
                w.MainSource = _DataLoaderList.Find(delegate(ADataLoader l) { return l.CurrentVandor == vandorType; });

                //Sub
                strVandor = (oRow[LibConsts.C_Col_SubSource] == DBNull.Value) ? "" : oRow[LibConsts.C_Col_SubSource].ToString();
                if (strVandor.Length > 0)
                {
                    vandorType = (VandorType)Enum.Parse(typeof(VandorType), strVandor, true);
                    w.SubSource = _DataLoaderList.Find(delegate(ADataLoader l) { return l.CurrentVandor == vandorType; });
                }

                //Check
                strVandor = (oRow[LibConsts.C_Col_CheckSource] == DBNull.Value) ? "" : oRow[LibConsts.C_Col_CheckSource].ToString();
                if (strVandor.Length > 0)
                {
                    vandorType = (VandorType)Enum.Parse(typeof(VandorType), strVandor, true);
                    w.CheckSource = _DataLoaderList.Find(delegate(ADataLoader l) { return l.CurrentVandor == vandorType; });
                }

                //Save
                if (oRow[LibConsts.C_Col_SaveDB] == DBNull.Value)
                {
                    w.SaveToDB = false;
                }
                else
                {
                    if (oRow[LibConsts.C_Col_SaveDB].ToString() == "1")
                        w.SaveToDB = true;
                    else
                        w.SaveToDB = false;
                }

                this.DataLoaderManagerList.Add(w);
            }
        }

        public void Run(string WindCode, DateTime StartDate, DateTime EndDate)
        {
            DataMessageManager.GetInstance().Clear();
            this.Para_WindCode = WindCode;
            this.Para_StartDate = StartDate;
            this.Para_EndDate = EndDate;
            this.CurrentActive = false;

            if (this.DataLoaderManagerList == null || this.DataLoaderManagerList.Count == 0)
            {
                DataMessageManager.GetInstance().AddMessage(DataMessageType.Infomation, LibConsts.C_Msg_MissingDataLoaderManager,null,null);
                return;
            }

            MethodInfo[] mi = typeof(IDataLoader).GetMethods();

            //调用IDataLoader中所有过程
            foreach (MethodInfo m in mi)
            {
                try
                {
                    DataMessageManager.GetInstance().AddMessage(DataMessageType.Infomation, LibConsts.C_Msg_StartUpdate, m.Name, null);
                    m.Invoke(this, null);
                }
                catch (Exception ex)
                {
                    DataMessageManager.GetInstance().AddMessage(DataMessageType.Error, LibConsts.C_Msg_ErrorWithUpdate + "(" + ex.Message + ")", null, "", m.Name, null);
                }                
            }

            this._DataCenter.SaveMessages();
        }

        public DataMessageManager GetMessageManager()
        {
            return DataMessageManager.GetInstance();
        }
        
        public void Close()
        {
            if (_DataLoaderList == null || _DataLoaderList.Count == 0)
                return;

            foreach (ADataLoader loader in _DataLoaderList)
            {
                loader.Close();
            }
        }


        

        #region Loader Methods
        private bool checkSrc<T, U>(List<T> mainSrc, List<U> subSrc, List<U> chkSrc, string tableName)
        {
            if (mainSrc == null || mainSrc.Count == 0)
            {
                DataMessageManager.GetInstance().AddMessage(DataMessageType.Warning, LibConsts.C_Msg_MissingMainSource, tableName, this.Para_LoopingDate.ToString("yyyyMMdd"));
                return false;
            }

            int mainCount = mainSrc.Count;
            int subCount = (subSrc == null) ? 0 : subSrc.Count;
            int chkCount = (chkSrc == null) ? 0 : chkSrc.Count;

            string msg = "数据计数";
            if (mainCount != subCount)
                msg += "不一致(" + (mainCount - subCount) + ")";

            DataMessageType msgType = DataMessageType.Infomation;
            if (mainCount != subCount)
                msgType = DataMessageType.Warning;

            DataMessageManager.GetInstance().AddMessage(msgType,
                (subCount == 0) ? LibConsts.C_Msg_MissingSubSource : msg,
                "计数",
                null,
                mainCount, subCount, chkCount,
                tableName,
                this.Para_LoopingDate.ToString("yyyyMMdd")
                );

            return true;
        }

        private List<T> LoopReadData<T>()
        {
            try
            {
                if (this.Para_EndDate == LibConsts.C_Null_Date)
                    return null;

                if (this.Para_StartDate == LibConsts.C_Null_Date)
                    this.Para_StartDate = this.Para_EndDate;


                //搜索到当前DataLoader
                List<T> dataList = null;
                int i = 0;
                while (true)
                {
                    DateTime currDay = this.Para_StartDate.AddDays(i);

                    if (currDay > this.Para_EndDate)
                        break;

                    //无需检查周末
                    //if (currDay.DayOfWeek == DayOfWeek.Sunday || currDay.DayOfWeek == DayOfWeek.Saturday)
                    //{
                    //    i++;
                    //    continue;
                    //}

                    this.Para_LoopingDate = currDay;

                    
                    dataList = this.ReadData<T>();

                    if (!this.CurrentActive)   //未激活则退出
                        break;

                    this.SaveData<T>(dataList);

                    i++;
                }

                //返回最后一个列表
                return dataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        

        private List<T> ReadData<T>()
        {
            try
            {
                string typeName = typeof(T).Name;

                //搜索到当前DataLoader
                DataLoaderManager dlm = DataLoaderManagerList.Find(delegate(DataLoaderManager w) { return w.TableName.ToLower() == typeName.ToLower(); });
                
                if (dlm == null || dlm.IsActive == false)
                {
                    if (dlm != null)
                        this.CurrentActive = dlm.IsActive;
                    else
                        this.CurrentActive = false;

                    DataMessageManager.GetInstance().AddMessage(DataMessageType.Infomation, LibConsts.C_Msg_NotActive, typeName, null);
                    return null;
                }

                //===========================
                //  Main Source
                //===========================
                if (dlm.MainSource == null)
                {
                    DataMessageManager.GetInstance().AddMessage(DataMessageType.Error, LibConsts.C_Msg_MissingMainSource, typeName, null);
                    return null;
                }

                List<T> mainSrcList = null, subSrcList, chkSrcList;
                object mainSrcObj = null, subSrcObj = null, chkSrcObj = null;
                
                //即将调用的方法名
                string methodName = "Get" + typeName;

                if (dlm.MainSource != null)
                {
                    //预设参数
                    dlm.MainSource.SetParameter(this.Para_WindCode, this.Para_StartDate, this.Para_EndDate, this.Para_LoopingDate);

                    //通过反射方法调用
                    mainSrcObj = dlm.MainSource.GetType().GetMethod(methodName).Invoke(dlm.MainSource, null);
                }

                //===========================
                //  Sub Source
                //===========================
                if (dlm.SubSource != null)
                {
                    //预设参数
                    dlm.SubSource.SetParameter(this.Para_WindCode, this.Para_StartDate, this.Para_EndDate, this.Para_LoopingDate);

                    //通过反射方法调用
                    subSrcObj = dlm.SubSource.GetType().GetMethod(methodName).Invoke(dlm.SubSource, null);
                }

                //===========================
                //  Check Source
                //===========================
                if (dlm.CheckSource != null)
                {
                    //预设参数
                    dlm.CheckSource.SetParameter(this.Para_WindCode, this.Para_StartDate, this.Para_EndDate, this.Para_LoopingDate);

                    //通过反射方法调用
                    chkSrcObj = dlm.CheckSource.GetType().GetMethod(methodName).Invoke(dlm.CheckSource, null);
                }


                if (mainSrcObj is List<T>)
                {
                    mainSrcList = mainSrcObj as List<T>;
                    subSrcList = subSrcObj as List<T>;
                    chkSrcList = chkSrcObj as List<T>;

                    if (!this.checkSrc(mainSrcList, subSrcList, chkSrcList,typeName))
                        return null;

                    //获得数据结构的主键
                    Type objType = mainSrcList[0].GetType();
                    FieldInfo fldPK = objType.GetField(AShareBase.C_Property_PrimaryKey);
                
                    foreach (T m in mainSrcList)
                    {
                        //记录主键值得变量
                        string pkInfo = "";

                        if (subSrcList != null)
                        {
                            //按主键搜索不同数据源中对应的记录
                            T s = subSrcList.Find(delegate(T d)
                                        {
                                            string primaryKey = fldPK.GetValue(d).ToString();
                                            string[] pkArray = primaryKey.Split(",".ToCharArray());

                                            pkInfo = "";
                                            foreach (string pk in pkArray)
                                            {
                                                FieldInfo fld = objType.GetField(pk.Trim());

                                                if (!fld.GetValue(d).Equals(fld.GetValue(m)))
                                                    return false;

                                                pkInfo += fld.GetValue(d).ToString() + "|";
                                            }

                                            pkInfo = pkInfo.Substring(0, pkInfo.Length - 1);
                                            return true;
                                        }
                                    );

                            T c = default(T);
                            if (chkSrcList != null)
                            {
                                c = chkSrcList.Find(delegate(T d)
                                            {
                                                string primaryKey = fldPK.GetValue(d).ToString();
                                                string[] pkArray = primaryKey.Split(",".ToCharArray());

                                                //pkInfo = "";
                                                foreach (string pk in pkArray)
                                                {
                                                    FieldInfo fld = objType.GetField(pk.Trim());

                                                    if (!fld.GetValue(d).Equals(fld.GetValue(m)))
                                                        return false;

                                                    //pkInfo += fld.GetValue(d).ToString() + "|";
                                                }

                                                //pkInfo = pkInfo.Substring(0, pkInfo.Length - 1);
                                                return true;
                                            }
                                        );
                            }

                            //比较记录
                            if (s != null)
                            {
                                MethodInfo method = m.GetType().GetMethod(AShareBase.C_Method_CleanBy);
                                method.Invoke(m, new object[] { s, c, pkInfo, typeof(T).Name, this.Para_LoopingDate.ToString("yyyyMMdd") });
                            }
                        }
                    }
                }

                return mainSrcList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SaveData<T>(List<T> srcList)
        {
            if (srcList == null || srcList.Count == 0 || this.OnTestMode)
                return;

            string typeName = typeof(T).Name;

            //搜索到当前DataLoader
            DataLoaderManager dlm = DataLoaderManagerList.Find(delegate(DataLoaderManager w) { return w.TableName.ToLower() == typeName.ToLower(); });

            if (dlm == null || dlm.IsActive == false)
            {
                //DataMessageManager.GetInstance().AddMessage(DataMessageType.Infomation, LibConsts.C_Msg_NotActive + "：" + typeName, "", "", null, null);
                return;
            }

            //保存入数据库
            if (dlm.SaveToDB)
            {
                Type dcType = this._DataCenter.GetType();
                MethodInfo mi = dcType.GetMethod("Save" + typeName);
                mi.Invoke(this._DataCenter, new object[] { srcList });
            }
        }

        public List<AShareDescription> GetAShareDescription()
        {
            try
            {
                List<AShareDescription> srcList = this.ReadData<AShareDescription>();
                this.SaveData<AShareDescription>(srcList);
                return srcList;
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        public List<AShareEODPrices> GetAShareEODPrices()
        {
            try
            {
                return (LoopReadData<AShareEODPrices>());
            }
            catch (Exception ex)
            {                
                throw ex;
            }           
        }
        
        public List<AShareCalendar> GetAShareCalendar()
        {
            try
            {
                List<AShareCalendar> srcList = this.ReadData<AShareCalendar>();
                this.SaveData<AShareCalendar>(srcList);
                return srcList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<AShareBalanceSheet> GetAShareBalanceSheet()
        {
            try
            {
                return (LoopReadData<AShareBalanceSheet>());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<AShareIncome> GetAShareIncome()
        {
            try
            {
                return (LoopReadData<AShareIncome>());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
