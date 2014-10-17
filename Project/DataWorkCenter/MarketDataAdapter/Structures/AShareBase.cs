using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MarketDataAdapter
{
    public class AShareBase
    {
        #region Consts
        //Ref. in DataLoaderProcessor
        public const string C_Method_CleanBy = "CleanBy";
        public const string C_Property_PrimaryKey = "PrimaryKey";

        public const string C_Msg_MissingSubValue = "副数据源无记录";
        public const string C_Msg_MissingMainValue = "主数据源无记录";
        public const string C_Msg_ValueInconsistent = "数据不一致";
        #endregion
 
        public virtual void CleanBy(object compareObj, object checkObj, string code, string tableName, string tradeDate) 
        {
            try
            {
                if (compareObj == null)
                {
                    DataMessageManager.GetInstance().AddMessage(DataMessageType.Warning, C_Msg_MissingSubValue, code, null, tableName, tradeDate);
                    return;
                }

                Type objType = this.GetType();
                FieldInfo[] fields = this.GetType().GetFields();

                foreach (FieldInfo fi in fields)
                {
                    object mainValue = fi.GetValue(this);
                    object subValue = fi.GetValue(compareObj);
                    object chkValue = null;

                    if (checkObj != null)
                        chkValue = fi.GetValue(checkObj);

                    if (mainValue == null)
                    {
                        if (subValue == null)
                        {
                            //数据源都没有数据
                        }
                        else
                        {
                            //主数据源缺少数据                            
                            if (!isEqualNull(subValue))
                                DataMessageManager.GetInstance().AddMessage(DataMessageType.Warning, C_Msg_MissingMainValue, code, fi.Name, mainValue, subValue, chkValue, tableName, tradeDate);
                        }
                    }
                    else
                    {                        
                        if (subValue == null)
                        {
                            //副数据源缺少数据
                            if (!isEqualNull(mainValue))
                                DataMessageManager.GetInstance().AddMessage(DataMessageType.Infomation, C_Msg_MissingSubValue, code, fi.Name, mainValue, subValue, chkValue, tableName, tradeDate);
                        }
                        else
                        {
                            //数据源都有数据，检查一致性并作记录
                            if (mainValue.Equals(subValue))
                            {
                                //数据一致
                            }
                            else
                            {
                                if (mainValue.GetType().IsClass)
                                {
                                    try
                                    {
                                        MethodInfo method = (MethodInfo)MethodBase.GetCurrentMethod();
                                        mainValue.GetType().InvokeMember(method.Name, BindingFlags.InvokeMethod, null, mainValue, new object[] { subValue, chkValue, code, tableName, tradeDate });
                                    }
                                    catch (Exception)
                                    {
                                        DataMessageManager.GetInstance().AddMessage(DataMessageType.Warning, C_Msg_ValueInconsistent, code, fi.Name, mainValue, subValue, chkValue, tableName, tradeDate);
                                    }
                                }
                                else if (mainValue.GetType() == typeof(double))
                                {
                                    //0.013 == 0.012
                                    double dMain = Convert.ToDouble(mainValue); dMain = Math.Round(dMain, 2);
                                    double dSub = Convert.ToDouble(subValue); dSub = Math.Round(dSub, 2);

                                    if (dMain != dSub)
                                        DataMessageManager.GetInstance().AddMessage(DataMessageType.Warning, C_Msg_ValueInconsistent, code, fi.Name, mainValue, subValue, chkValue, tableName, tradeDate);
                                }
                                else
                                {
                                    //不一致
                                    DataMessageManager.GetInstance().AddMessage(DataMessageType.Warning, C_Msg_ValueInconsistent, code, fi.Name, mainValue, subValue, chkValue, tableName, tradeDate);
                                }                                
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(code + ":" + ex.Message, ex);
            }
        }

        private bool isEqualNull(object obj)
        {
            bool isConsistency = true;
            if (obj.GetType() == typeof(double))
            {
                double dSub = Convert.ToDouble(obj); dSub = Math.Round(dSub, 2);
                if (dSub != 0)              //null == 0
                    isConsistency = false;
            }
            else if (obj.GetType() == typeof(string))
            {
                string strSub = obj.ToString().Trim();
                if (strSub.Length > 0)      //null == ""
                    isConsistency = false;
            }
            else
            {
                isConsistency = false;
            }

            return isConsistency;
        }
    }
}
