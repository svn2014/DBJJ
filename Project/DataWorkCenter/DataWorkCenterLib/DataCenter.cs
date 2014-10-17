using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarketDataAdapter;
using System.Data;

namespace DataWorkCenterLib
{
    public class DataCenter: IDataSaver
    {
        public DataLoaderWind DataLoader = null;
        private ADBInstance _DBInstance = null;
        public DataCenter(ADBInstance dbInstance)
        {
            DataLoader = new DataLoaderWind(dbInstance);
            _DBInstance = dbInstance;
        }

        #region Assistant
        private object convertToDBValue(double? val)
        {
            if (val == null)
                return DBNull.Value;
            else
                return val.Value;
        }
        private object convertToDBValue(DateTime? val)
        {
            if (val == null)
                return DBNull.Value;
            else
                return val.Value.ToString("yyyyMMdd");
        }
        private bool checkLocker(DataRow oRow)
        { 
            if(oRow[AMarketDataAdapter.C_Col_Opmode] == DBNull.Value)
                return false;

            if (Convert.ToInt16(oRow[AMarketDataAdapter.C_Col_Opmode]) == LibConsts.C_Opmode_Locked)
                return true;
            else
                return false;
        }
        #endregion

        #region Saver
        public void SaveAShareDescription(List<AShareDescription> lstSrc)
        {
            //try
            //{
                if (lstSrc == null || lstSrc.Count == 0)
                    return;

                string sql = @"Select * 
                                From asharedescription 
                                Where 1=1 ";                

                DataSet ds = _DBInstance.ExecuteSQL(sql);

                if (ds == null || ds.Tables.Count == 0)
                    return;

                DataTable dt = ds.Tables[0];
                DataRow oRow = null;
                DateTime opDate = DateTime.Now;

                foreach (AShareDescription d in lstSrc)
                {
                    DataRow[] oRows = dt.Select("s_info_windcode = '" + d.WindCode + "'");

                    if (oRows.Length == 0)
                    {
                        oRow = dt.NewRow();
                        oRow[AMarketDataAdapter.C_Col_WindCode] = d.WindCode;
                        //oRow[AMarketDataAdapter.C_Col_Opmode] = 0;  //与Wind不一样
                    }
                    else
                    {
                        oRow = oRows[0];

                        //opmode=2 表示该行已经锁定修改，避免被改回错误值
                        if (checkLocker(oRow))
                            continue;

                        //oRow[AMarketDataAdapter.C_Col_Opmode] = 1;  //与Wind不一样
                    }
                    oRow[AMarketDataAdapter.C_Col_Opdate] = opDate; //与Wind不一样

                    oRow[AMarketDataAdapter.C_Col_Code] = d.Code;
                    oRow[AMarketDataAdapter.C_Col_Name] = d.Name;
                    oRow[AMarketDataAdapter.C_Col_ExchMarket] = d.Exchange.ToString();
                    oRow[AMarketDataAdapter.C_Col_ListBoard] = WindEnum.GetInstance().GetEnumTypeValue<ListedBoardType>((int)d.ListedBoard);
                    oRow[AMarketDataAdapter.C_Col_ListDate] = (d.ListDate == null) ? null : d.ListDate.Value.ToString("yyyyMMdd");
                    oRow[AMarketDataAdapter.C_Col_DelistDate] = (d.DelistDate == null) ? null : d.DelistDate.Value.ToString("yyyyMMdd");

                    if (oRows.Length == 0)
                        dt.Rows.Add(oRow);
                }

                _DBInstance.UpdateDB(sql, dt);
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        public void SaveAShareEODPrices(List<AShareEODPrices> lstSrc)
        {
            //每次只能保存同一个日期的数据
            try
            {
                if (lstSrc == null || lstSrc.Count == 0)
                    return;

                string sql = @"Select * 
                                From ashareeodprices 
                                Where trade_dt ='" + lstSrc[0].TradeDate.Value.ToString("yyyyMMdd") + "'";

                DataSet ds = _DBInstance.ExecuteSQL(sql);

                if (ds == null || ds.Tables.Count == 0)
                    return;

                DataTable dt = ds.Tables[0];
                DataRow oRow = null;
                DateTime opDate = DateTime.Now;

                foreach (AShareEODPrices p in lstSrc)
                {
                    DataRow[] oRows = dt.Select("s_info_windcode = '" + p.WindCode + "' AND TRADE_DT = '" + p.TradeDate.Value.ToString("yyyyMMdd") + "'");

                    if (oRows.Length == 0)
                    {
                        oRow = dt.NewRow();
                        oRow[AMarketDataAdapter.C_Col_WindCode] = p.WindCode;
                        oRow[AMarketDataAdapter.C_Col_Trade_Date] = p.TradeDate.Value.ToString("yyyyMMdd");
                        //oRow[AMarketDataAdapter.C_Col_Opmode] = 0;  //与Wind不一样
                    }
                    else
                    {
                        oRow = oRows[0];
                        
                        //opmode=2 表示该行已经锁定修改，避免被改回错误值
                        if (checkLocker(oRow))
                            continue;

                        //oRow[AMarketDataAdapter.C_Col_Opmode] = 1;  //与Wind不一样
                    }
                    oRow[AMarketDataAdapter.C_Col_Opdate] = opDate; //与Wind不一样

                    oRow[AMarketDataAdapter.C_Col_PreClose] = p.PreClose;
                    oRow[AMarketDataAdapter.C_Col_Open] = p.Open;
                    oRow[AMarketDataAdapter.C_Col_High] = p.High;
                    oRow[AMarketDataAdapter.C_Col_Low] = p.Low;
                    oRow[AMarketDataAdapter.C_Col_Close] = p.Close;
                    oRow[AMarketDataAdapter.C_Col_AvgPrice] = p.Low;
                    oRow[AMarketDataAdapter.C_Col_Volume] = p.Volume/100;       //手
                    oRow[AMarketDataAdapter.C_Col_Amount] = p.Amount/1000;      //千元
                    oRow[AMarketDataAdapter.C_Col_TradeStatus] = WindEnum.GetInstance().GetEnumTypeValue<TradingStatus>((int)p.Status);

                    //oRow[AMarketDataAdapter.C_Col_AdjPreClose] = p.AdjustedPreClose;
                    //oRow[AMarketDataAdapter.C_Col_AdjOpen] = p.AdjustedOpen;
                    //oRow[AMarketDataAdapter.C_Col_AdjHigh] = p.AdjustedHigh;
                    //oRow[AMarketDataAdapter.C_Col_AdjLow] = p.AdjustedLow;
                    //oRow[AMarketDataAdapter.C_Col_AdjClose] = p.AdjustedClose;                    
                    //oRow[AMarketDataAdapter.C_Col_AdjFactor] = p.AdjustedFactor;                    

                    if (oRows.Length == 0)
                        dt.Rows.Add(oRow);
                }

                _DBInstance.UpdateDB(sql, dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveAShareCalendar(List<AShareCalendar> lstSrc)
        {
            try
            {
                if (lstSrc == null || lstSrc.Count == 0)
                    return;

                string sql = @"select * from asharecalendar ";

                DataSet ds = _DBInstance.ExecuteSQL(sql);

                if (ds == null || ds.Tables.Count == 0)
                    return;

                DataTable dt = ds.Tables[0];
                DataRow oRow = null;
                DateTime opDate = DateTime.Now;

                foreach (AShareCalendar d in lstSrc)
                {
                    DataRow[] oRows = dt.Select("trade_days = '" + d.TradeDay.Value.ToString("yyyyMMdd") + "' AND s_info_exchmarket = '" + d.Exchange.ToString() + "'");

                    if (oRows.Length == 0)
                    {
                        oRow = dt.NewRow();
                        oRow[AMarketDataAdapter.C_Col_TradeDays] = d.TradeDay.Value.ToString("yyyyMMdd");
                        oRow[AMarketDataAdapter.C_Col_ExchMarket] = d.Exchange.ToString();
                        //oRow[AMarketDataAdapter.C_Col_Opmode] = 0;  //与Wind不一样
                        oRow[AMarketDataAdapter.C_Col_Opdate] = opDate; //与Wind不一样

                        dt.Rows.Add(oRow);
                    }
                   
                    //找到则无须添加
                }

                _DBInstance.UpdateDB(sql, dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        

        public void SaveAShareBalanceSheet(List<AShareBalanceSheet> lstSrc)
        {
            //每次只能保存同一个日期的数据
            try
            {
                if (lstSrc == null || lstSrc.Count == 0)
                    return;

                string sql = @"Select * 
                                From asharebalancesheet 
                                Where ann_dt ='" + lstSrc[0].AnnouncementDate.Value.ToString("yyyyMMdd") + "'";

                DataSet ds = _DBInstance.ExecuteSQL(sql);

                if (ds == null || ds.Tables.Count == 0)
                    return;

                DataTable dt = ds.Tables[0];
                DataRow oRow = null;
                DateTime opDate = DateTime.Now;

                foreach (AShareBalanceSheet p in lstSrc)
                {
                    DataRow[] oRows = dt.Select("s_info_windcode = '" + p.WindCode + "' "
                                        + "AND report_period = '" + p.ReportPeriod.Value.ToString("yyyyMMdd") + "' "
                                        + "AND statement_Type = '" + WindEnum.GetInstance().GetEnumTypeValue<FinancialStatementType>((int)p.StatementType) + "' ");

                    if (oRows.Length == 0)
                    {
                        oRow = dt.NewRow();
                        oRow[AMarketDataAdapter.C_Col_WindCode] = p.WindCode;
                        oRow[AMarketDataAdapter.C_Col_ReportPeriod] = p.ReportPeriod.Value.ToString("yyyyMMdd");
                        oRow[AMarketDataAdapter.C_Col_StatementType] = WindEnum.GetInstance().GetEnumTypeValue<FinancialStatementType>((int)p.StatementType);
                    }
                    else
                    {
                        oRow = oRows[0];

                        //opmode=2 表示该行已经锁定修改，避免被改回错误值
                        if (checkLocker(oRow))
                            continue;
                    }
                    oRow[AMarketDataAdapter.C_Col_Opdate] = opDate; //与Wind不一样

                    oRow[AMarketDataAdapter.C_Col_AnnouncementDate] = convertToDBValue(p.AnnouncementDate);
                    oRow[AMarketDataAdapter.C_Col_TotalAssets] = convertToDBValue(p.TotalAssets);
                    oRow[AMarketDataAdapter.C_Col_TotalCurrentAssets] = convertToDBValue(p.TotalCurrentAssets);
                    oRow[AMarketDataAdapter.C_Col_TotalCurrentLiability] = convertToDBValue(p.TotalCurrentLiability);
                    oRow[AMarketDataAdapter.C_Col_TotalLiability] = convertToDBValue(p.TotalLiability);
                    oRow[AMarketDataAdapter.C_Col_TotalEquityInclMinInt] = convertToDBValue(p.TotalEquityInclMinInt);
                    oRow[AMarketDataAdapter.C_Col_Inventories] = convertToDBValue(p.Inventories);
                    oRow[AMarketDataAdapter.C_Col_CompanyType] = (int)p.CorpType;

                    oRow[AMarketDataAdapter.C_Col_MonetoryCap] = convertToDBValue(p.MonetoryCap);
                    oRow[AMarketDataAdapter.C_Col_AccountRecievable] = convertToDBValue(p.AccountRecievable);
                    oRow[AMarketDataAdapter.C_Col_Prepay] = convertToDBValue(p.Prepay);
                    oRow[AMarketDataAdapter.C_Col_ConstructionInProgress] = convertToDBValue(p.ConstructionInProgress);
                    oRow[AMarketDataAdapter.C_Col_FixAssets] = convertToDBValue(p.FixAssets);

                    if (oRows.Length == 0)
                        dt.Rows.Add(oRow);
                }

                _DBInstance.UpdateDB(sql, dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveAShareIncome(List<AShareIncome> lstSrc)
        {
            //每次只能保存同一个日期的数据
            try
            {
                if (lstSrc == null || lstSrc.Count == 0)
                    return;

                string sql = @"Select * 
                                From AShareIncome 
                                Where ann_dt ='" + lstSrc[0].AnnouncementDate.Value.ToString("yyyyMMdd") + "'";

                DataSet ds = _DBInstance.ExecuteSQL(sql);

                if (ds == null || ds.Tables.Count == 0)
                    return;

                DataTable dt = ds.Tables[0];
                DataRow oRow = null;
                DateTime opDate = DateTime.Now;

                foreach (AShareIncome p in lstSrc)
                {
                    DataRow[] oRows = dt.Select("s_info_windcode = '" + p.WindCode + "' "
                                    + "AND report_period = '" + p.ReportPeriod.Value.ToString("yyyyMMdd") + "' "
                                    + "AND statement_Type = '" + WindEnum.GetInstance().GetEnumTypeValue<FinancialStatementType>((int)p.StatementType) + "' "
                                    );

                    if (oRows.Length == 0)
                    {
                        oRow = dt.NewRow();
                        oRow[AMarketDataAdapter.C_Col_WindCode] = p.WindCode;
                        oRow[AMarketDataAdapter.C_Col_ReportPeriod] = p.ReportPeriod.Value.ToString("yyyyMMdd");
                        oRow[AMarketDataAdapter.C_Col_StatementType] = WindEnum.GetInstance().GetEnumTypeValue<FinancialStatementType>((int)p.StatementType);
                    }
                    else
                    {
                        oRow = oRows[0];

                        //opmode=2 表示该行已经锁定修改，避免被改回错误值
                        if (checkLocker(oRow))
                            continue;
                    }
                    oRow[AMarketDataAdapter.C_Col_Opdate] = opDate; //与Wind不一样

                    oRow[AMarketDataAdapter.C_Col_AnnouncementDate] = convertToDBValue(p.AnnouncementDate);
                    oRow[AMarketDataAdapter.C_Col_TotalOperatingRevenue] = convertToDBValue(p.TotalOperatingRevenue);
                    oRow[AMarketDataAdapter.C_Col_OperatingRevenue] = convertToDBValue(p.OperatingRevenue);
                    oRow[AMarketDataAdapter.C_Col_TotalProfit] = convertToDBValue(p.TotalProfit);
                    oRow[AMarketDataAdapter.C_Col_NetProfitInclMinInt] = convertToDBValue(p.NetProfitInclMinInt);
                    //oRow[AMarketDataAdapter.C_Col_NetProfitAfterDedNRLP] = convertToDBValue(p.NetProfitAfterDedNRLP);
                    oRow[AMarketDataAdapter.C_Col_CompanyType] = (int)p.CorpType;

                    oRow[AMarketDataAdapter.C_Col_TotalOperationCost] = convertToDBValue(p.TotalOperationCost);
                    oRow[AMarketDataAdapter.C_Col_OperationCost] = convertToDBValue(p.OperationCost);
                    oRow[AMarketDataAdapter.C_Col_SellingExpence] = convertToDBValue(p.SellingExpence);
                    oRow[AMarketDataAdapter.C_Col_AdminExpence] = convertToDBValue(p.AdminExpence);
                    oRow[AMarketDataAdapter.C_Col_FinanceExpence] = convertToDBValue(p.FinanceExpence);
                    oRow[AMarketDataAdapter.C_Col_NetInvestmentIncome] = convertToDBValue(p.NetInvestmentIncome);
                    oRow[AMarketDataAdapter.C_Col_NetProfitExclMinInt] = convertToDBValue(p.NetProfitExclMinInt);

                    if (oRows.Length == 0)
                        dt.Rows.Add(oRow);
                }

                _DBInstance.UpdateDB(sql, dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Additional Saver
        private const string C_Col_Opdate = "OPDATE";
        private const string C_Col_Type = "MSGTYPE";
        private const string C_Col_TradeDate = "TRADE_DT";
        private const string C_Col_FieldName = "FIELDNAME";
        private const string C_Col_MainValue = "MAINVALUE";
        private const string C_Col_SubValue = "SUBVALUE";
        private const string C_Col_CheckValue = "CHKVALUE";
        private const string C_Col_Message = "MESSAGE";
        private const string C_Col_Code = "CODE";
        private const string C_Col_TableName = "TABLENAME";
        private const string C_Col_OrderId = "ORDERID";

        public void SaveMessages()
        {
            try
            {
                List<DataMessage> msgList = DataMessageManager.GetInstance().GetMessages();

                if (msgList == null || msgList.Count == 0)
                    return;
                
                string sql = @"select * from datalog where 1=0 ";
                DataSet ds = _DBInstance.ExecuteSQL(sql);
                DataTable dt = ds.Tables[0];

                foreach (DataMessage dm in msgList)
                {
                    DataRow oRow = dt.NewRow();

                    oRow[C_Col_Opdate] = DateTime.Now;
                    oRow[C_Col_Type] = dm.Type.ToString().Substring(0,1).ToUpper();
                    oRow[C_Col_Code] = dm.Code;
                    oRow[C_Col_FieldName] = dm.Field;
                    oRow[C_Col_MainValue] = dm.MainValue;
                    oRow[C_Col_SubValue] = dm.SubValue;
                    oRow[C_Col_CheckValue] = dm.CheckValue;
                    oRow[C_Col_Message] = dm.Message;
                    oRow[C_Col_TableName] = dm.TableName;
                    oRow[C_Col_TradeDate] = dm.TradeDate;
                    oRow[C_Col_OrderId] = dm.Order;

                    dt.Rows.Add(oRow);
                }

                _DBInstance.UpdateDB(sql, dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
