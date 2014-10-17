using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MarketDataAdapter
{
    public class MarketDataAdapterJuYuan : AMarketDataAdapter
    {
        #region 字段列表
        #region 交易日历: AShareCalendar
        public new const string C_Col_TradeDays = "tradingdate";
        #endregion

        #region 基本信息: AShareDescription
        //public new const string C_Col_WindCode = "s_info_windcode";
        public new const string C_Col_Code = "secucode";
        public new const string C_Col_Name = "secuabbr";
        public new const string C_Col_ExchMarket = "secumarket";
        public new const string C_Col_ListBoard = "listedsector";
        public new const string C_Col_ListDate = "listeddate";
        //public new const string C_Col_DelistDate = "s_info_delistdate";        
        #endregion

        #region 股价信息: AShareEODPrices
        public new const string C_Col_Trade_Date = "Tradingday";
        public new const string C_Col_PreClose = "PrevClosePrice";
        public new const string C_Col_Open = "OpenPrice";
        public new const string C_Col_High = "HighPrice";
        public new const string C_Col_Low = "LowPrice";
        public new const string C_Col_Close = "ClosePrice";
        public new const string C_Col_Volume = "TurnoverVolume";
        public new const string C_Col_Amount = "TurnoverValue";
        //public new const string C_Col_AdjPreClose = "s_dq_adjpreclose";
        //public new const string C_Col_AdjOpen = "s_dq_adjopen";
        //public new const string C_Col_AdjHigh = "s_dq_adjhigh";
        //public new const string C_Col_AdjLow = "s_dq_adjlow";
        //public new const string C_Col_AdjClose = "s_dq_adjclose";
        //public new const string C_Col_AdjFactor = "s_dq_adjfactor";
        //public new const string C_Col_AvgPrice = "s_dq_avgprice";
        //public new const string C_Col_TradeStatus = "s_dq_tradestatus";
        #endregion

        #region 资产负债表: AShareBalanceSheet
        public new const string C_Col_ReportPeriod = "EndDate";
        public new const string C_Col_AnnouncementDate = "InfoPublDate";
        public new const string C_Col_StatementType = "IfMerged";                   //1=合并报表 2=母公司报表
        private const string C_Col_AdjustmentType = "Ifadjusted";                   //2=未调整 Others=调整
        private const string C_Col_BulletinType = "BulletinType";                   //70=临时公告，更正公告
        public new const string C_Col_CompanyType = "EnterpriseType";

        public new const string C_Col_MonetoryCap = "CashEquivalents";
        public new const string C_Col_AccountRecievable = "AccountReceivable";
        public new const string C_Col_Prepay = "AdvancePayment";                        //预付款项
        public new const string C_Col_FixAssets = "FixedAssets";
        public new const string C_Col_ConstructionInProgress = "ConstruInProcess"; //在建工程
        public new const string C_Col_Inventories = "Inventories";
        public new const string C_Col_TotalCurrentAssets = "TotalCurrentAssets";
        public new const string C_Col_TotalAssets = "TotalAssets";
        public new const string C_Col_TotalCurrentLiability = "TotalCurrentLiability";
        public new const string C_Col_TotalLiability = "TotalLiability";
        public new const string C_Col_TotalEquityInclMinInt = "TotalShareholderEquity";
        #endregion

        #region 利润表: AShareIncome
        public new const string C_Col_TotalOperationCost = "TotalOperatingCost";        //营业总成本
        public new const string C_Col_OperationCost = "OperatingCost";                  //营业成本
        public new const string C_Col_SellingExpence = "OperatingExpense";              //销售费用
        public new const string C_Col_AdminExpence = "AdministrationExpense";           //管理费用
        public new const string C_Col_FinanceExpence = "FinancialExpense";              //财务费用
        public new const string C_Col_NetInvestmentIncome = "InvestIncome";             //投资净收益
        public new const string C_Col_NetProfitExclMinInt = "NPParentCompanyOwners";    //净利润(不含少数股东损益)

        public new const string C_Col_TotalOperatingRevenue = "TotalOperatingRevenue";
        public new const string C_Col_OperatingRevenue = "OperatingRevenue";
        public new const string C_Col_TotalProfit = "TotalProfit";
        public new const string C_Col_NetProfitInclMinInt = "NetProfit";                //净利润(包含少数股东收益)
        public new const string C_Col_NetProfitAfterDedNRLP = "NetProfitCut";           //扣除非经常性损益的净利润
        #endregion
        #endregion

        #region 辅助功能
        protected override ExchangeType getExchangeType(string exchange)
        {
            switch (exchange)
            {
                case "83":
                    return ExchangeType.SSE;

                case "90":
                    return ExchangeType.SZSE;

                default:
                    throw new Exception(C_Msg_UnknownExchange);
            }
        }
        protected override ListedBoardType getListedBoardType(string listedBorad)
        {
            switch (listedBorad)
            {
                case "6":
                    return ListedBoardType.CYB;

                case "2":
                    return ListedBoardType.ZXB;

                case "1":
                    return ListedBoardType.ZB;

                default:
                    throw new Exception(C_Msg_UnknownListedBoard);
            }
        }
        protected override FinancialStatementType getStatementType(string statementType)
        {
            string[] statements = statementType.Split("|".ToCharArray());
            string statement = statements[0];
            string adjustmentType = statements[1];
            string updateType = statements[2];

            if (statement == "1")
            {
                if (updateType == "70")
                {
                    switch (adjustmentType)
                    {
                        case "1":
                        case "6":
                        case "7":
                        case "8":
                            return FinancialStatementType.MergedAdjusted;
                        case "2":
                            return FinancialStatementType.MergedUpdated;

                        default:
                            break;
                    }
                }
                else
                {
                    switch (adjustmentType)
                    {
                        case "1":
                        case "6":
                        case "7":
                        case "8":
                            return FinancialStatementType.MergedAdjusted;
                        case "2":
                            return FinancialStatementType.Merged;

                        default:
                            break;
                    }
                }
            }
            else if(statement == "2")
            {
                if (updateType == "70")
                {
                    switch (adjustmentType)
                    {
                        case "1":
                        case "6":
                        case "7":
                        case "8":
                            return FinancialStatementType.ParentAdjusted;
                        case "2":
                            return FinancialStatementType.ParentUpdated;

                        default:
                            break;
                    }
                }
                {
                    switch (adjustmentType)
                    {
                        case "1":
                        case "6":
                        case "7":
                        case "8":
                            return FinancialStatementType.ParentAdjusted;
                        case "2":
                            return FinancialStatementType.Parent;

                        default:
                            break;
                    }
                }
            }

            //无匹配
            throw new Exception(C_Msg_UnknownStatementType);
        }
        private FinancialStatementType getStatementType(string statementType, string adjustmentType, string updateType)
        {
            string statement = statementType + "|" + adjustmentType + "|" + updateType;
            return getStatementType(statement);
        }
        protected override CompanyType getCompanyType(string companyType)
        {
            switch (companyType)
            {
                case "13":
                    return CompanyType.Bank;
                case "35":
                    return CompanyType.Insurance;
                case "31":
                    return CompanyType.Security;
                default:
                    return CompanyType.NonFinancial;
            }
        }
        #endregion

        #region 实现接口
        public override List<AShareDescription> GetAShareDescription(DataTable dtDataSrc)
        {
            try
            {
                List<AShareDescription> list = new List<AShareDescription>();

                if (dtDataSrc != null && dtDataSrc.Rows.Count > 0)
                {
                    foreach (DataRow oRow in dtDataSrc.Rows)
                    {
                        AShareDescription d = new AShareDescription();

                        //d.WindCode = oRow[C_Col_WindCode].ToString();
                        d.Code = oRow[C_Col_Code].ToString();
                        d.Name = oRow[C_Col_Name].ToString();
                        d.Exchange = this.getExchangeType(oRow[C_Col_ExchMarket].ToString());
                        d.ListedBoard = this.getListedBoardType(oRow[C_Col_ListBoard].ToString());
                        d.ListDate = base.getDateTimeValue(oRow[C_Col_ListDate]);
                        //d.DelistDate = base.getDateTimeValueBy8Digits(oRow[C_Col_DelistDate]);

                        d.WindCode = getWindCode(d.Code, d.Exchange);

                        //名称特别处理
                        d.Name = d.Name.Replace(" ", "");
                        d.Name = d.Name.Replace("Ａ", "A");

                        list.Add(d);
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override List<AShareEODPrices> GetAShareEODPrices(DataTable dtDataSrc)
        {
            try
            {
                List<AShareEODPrices> list = new List<AShareEODPrices>();

                if (dtDataSrc != null && dtDataSrc.Rows.Count > 0)
                {
                    foreach (DataRow oRow in dtDataSrc.Rows)
                    {
                        AShareEODPrices d = new AShareEODPrices();

                        ExchangeType exchange = getExchangeType(oRow[C_Col_ExchMarket].ToString());
                        d.WindCode = base.getWindCode(oRow[C_Col_Code].ToString(), exchange);

                        d.TradeDate = base.getDateTimeValueBy8Digits(oRow[C_Col_Trade_Date]);
                        d.Volume = base.getDoubleValue(oRow[C_Col_Volume]);           //股
                        d.Amount = base.getDoubleValue(oRow[C_Col_Amount]);           //元
                        //if (d.Amount != null)
                        //    d.Amount = Math.Truncate(Math.Round(d.Amount.Value, 1));  //保留小数

                        d.PreClose = base.getDoubleValue(oRow[C_Col_PreClose]);

                        if (d.Volume == 0)
                        {
                            //若停牌则所有值设为昨收盘
                            d.Status = TradingStatus.Suspend;

                            d.Open = d.PreClose;
                            d.High = d.PreClose;
                            d.Low = d.PreClose;
                            d.Close = d.PreClose;
                            d.Average = d.PreClose;

                            //px.AdjustedFactor = base.getDoubleValue(oRow[CaiHuiConsts.C_Col_AdjFactor]);
                            //px.AdjustedPreClose = px.PreClose * px.AdjustedFactor;
                            //px.AdjustedOpen = px.Open * px.AdjustedFactor;
                            //px.AdjustedHigh = px.High * px.AdjustedFactor;
                            //px.AdjustedLow = px.Low * px.AdjustedFactor;
                            //px.AdjustedClose = px.Close * px.AdjustedFactor;
                            //px.AdjustedAverage = (px.Average != null && px.AdjustedFactor != null) ? px.Average * px.AdjustedFactor : null;
                        }
                        else
                        {
                            d.Open = base.getDoubleValue(oRow[C_Col_Open]);
                            d.High = base.getDoubleValue(oRow[C_Col_High]);
                            d.Low = base.getDoubleValue(oRow[C_Col_Low]);
                            d.Close = base.getDoubleValue(oRow[C_Col_Close]);
                            d.Average = Math.Round(d.Amount.Value / d.Volume.Value, 4);

                            //px.AdjustedFactor = base.getDoubleValue(oRow[CaiHuiConsts.C_Col_AdjFactor]);
                            //px.AdjustedPreClose = px.PreClose * px.AdjustedFactor;
                            //px.AdjustedOpen = px.Open * px.AdjustedFactor;
                            //px.AdjustedHigh = px.High * px.AdjustedFactor;
                            //px.AdjustedLow = px.Low * px.AdjustedFactor;
                            //px.AdjustedClose = px.Close * px.AdjustedFactor;
                            //px.AdjustedAverage = (px.Average != null && px.AdjustedFactor != null) ? px.Average * px.AdjustedFactor : null;
                        }
                        list.Add(d);
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override List<AShareCalendar> GetAShareCalendar(DataTable dtDataSrc)
        {
            try
            {
                List<AShareCalendar> list = new List<AShareCalendar>();

                if (dtDataSrc != null && dtDataSrc.Rows.Count > 0)
                {
                    foreach (DataRow oRow in dtDataSrc.Rows)
                    {
                        AShareCalendar d = new AShareCalendar();

                        d.Exchange = this.getExchangeType(oRow[C_Col_ExchMarket].ToString());
                        d.TradeDay = base.getDateTimeValue(oRow[C_Col_TradeDays]);

                        list.Add(d);
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override List<AShareBalanceSheet> GetAShareBalanceSheet(DataTable dtDataSrc)
        {
            try
            {
                List<AShareBalanceSheet> list = new List<AShareBalanceSheet>();

                if (dtDataSrc != null && dtDataSrc.Rows.Count > 0)
                {
                    foreach (DataRow oRow in dtDataSrc.Rows)
                    {
                        AShareBalanceSheet d = new AShareBalanceSheet();

                        ExchangeType exchange = getExchangeType(oRow[C_Col_ExchMarket].ToString());
                        d.WindCode = getWindCode(oRow[C_Col_Code].ToString(), exchange);

                        d.ReportPeriod = getDateTimeValue(oRow[C_Col_ReportPeriod]);
                        d.StatementType = this.getStatementType(
                                                oRow[C_Col_StatementType].ToString(), 
                                                oRow[C_Col_AdjustmentType].ToString(),
                                                oRow[C_Col_BulletinType].ToString()
                                                );

                        d.AnnouncementDate = getDateTimeValue(oRow[C_Col_AnnouncementDate]);
                        d.TotalAssets = getDoubleValue(oRow[C_Col_TotalAssets]);
                        d.TotalCurrentAssets = getDoubleValue(oRow[C_Col_TotalCurrentAssets]);
                        d.Inventories = getDoubleValue(oRow[C_Col_Inventories]);
                        d.TotalLiability = getDoubleValue(oRow[C_Col_TotalLiability]);
                        d.TotalCurrentLiability = getDoubleValue(oRow[C_Col_TotalCurrentLiability]);
                        d.TotalEquityInclMinInt = getDoubleValue(oRow[C_Col_TotalEquityInclMinInt]);
                        d.CorpType = this.getCompanyType(oRow[C_Col_CompanyType].ToString());

                        d.MonetoryCap = getDoubleValue(oRow[C_Col_MonetoryCap]);
                        d.AccountRecievable = getDoubleValue(oRow[C_Col_AccountRecievable]);
                        d.Prepay = getDoubleValue(oRow[C_Col_Prepay]);
                        d.ConstructionInProgress = getDoubleValue(oRow[C_Col_ConstructionInProgress]);
                        d.FixAssets = getDoubleValue(oRow[C_Col_FixAssets]);

                        list.Add(d);
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override List<AShareIncome> GetAShareIncome(DataTable dtDataSrc)
        {
            try
            {
                List<AShareIncome> list = new List<AShareIncome>();

                if (dtDataSrc != null && dtDataSrc.Rows.Count > 0)
                {
                    foreach (DataRow oRow in dtDataSrc.Rows)
                    {
                        AShareIncome d = new AShareIncome();

                        ExchangeType exchange = getExchangeType(oRow[C_Col_ExchMarket].ToString());
                        d.WindCode = getWindCode(oRow[C_Col_Code].ToString(), exchange);

                        d.ReportPeriod = getDateTimeValueBy8Digits(oRow[C_Col_ReportPeriod]);
                        d.StatementType = this.getStatementType(
                                                oRow[C_Col_StatementType].ToString(),
                                                oRow[C_Col_AdjustmentType].ToString(),
                                                oRow[C_Col_BulletinType].ToString()
                                                );

                        d.AnnouncementDate = getDateTimeValueBy8Digits(oRow[C_Col_AnnouncementDate]);
                        d.TotalOperatingRevenue = getDoubleValue(oRow[C_Col_TotalOperatingRevenue]);
                        d.OperatingRevenue = getDoubleValue(oRow[C_Col_OperatingRevenue]);
                        d.TotalProfit = getDoubleValue(oRow[C_Col_TotalProfit]);
                        d.NetProfitInclMinInt = getDoubleValue(oRow[C_Col_NetProfitInclMinInt]);
                        //d.NetProfitAfterDedNRLP = getDoubleValue(oRow[C_Col_NetProfitAfterDedNRLP]);
                        d.CorpType = this.getCompanyType(oRow[C_Col_CompanyType].ToString());

                        d.TotalOperationCost = getDoubleValue(oRow[C_Col_TotalOperationCost]);
                        d.OperationCost = getDoubleValue(oRow[C_Col_OperationCost]);
                        d.SellingExpence = getDoubleValue(oRow[C_Col_SellingExpence]);
                        d.AdminExpence = getDoubleValue(oRow[C_Col_AdminExpence]);
                        d.FinanceExpence = getDoubleValue(oRow[C_Col_FinanceExpence]);
                        d.NetInvestmentIncome = getDoubleValue(oRow[C_Col_NetInvestmentIncome]);
                        d.NetProfitExclMinInt = getDoubleValue(oRow[C_Col_NetProfitExclMinInt]);

                        list.Add(d);
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion        
    }
}
