using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MarketDataAdapter
{
    public class MarketDataAdapterCaiHui : AMarketDataAdapter
    {
        #region 字段列表
        #region 交易日历: AShareCalendar
        public new const string C_Col_TradeDays = "ENTRYDATE";
        #endregion

        #region 基本信息: AShareDescription
        //public new const string C_Col_WindCode = "s_info_windcode";
        public new const string C_Col_Code = "SYMBOL";
        public new const string C_Col_Name = "SNAME";
        public new const string C_Col_ExchMarket = "EXCHANGE";
        //public new const string C_Col_ListBoard = "s_info_listboard";
        public new const string C_Col_ListDate = "LISTDATE";
        public new const string C_Col_DelistDate = "ENDDATE";
        #endregion

        #region 股价信息: AShareEODPrices
        public new const string C_Col_Trade_Date = "TDATE";
        public new const string C_Col_PreClose = "LCLOSE";
        public new const string C_Col_Open = "TOPEN";
        public new const string C_Col_High = "HIGH";
        public new const string C_Col_Low = "LOW";
        public new const string C_Col_Close = "TCLOSE";
        public new const string C_Col_Volume = "VOTURNOVER";
        public new const string C_Col_Amount = "VATURNOVER";
        //public new const string C_Col_AdjPreClose = "s_dq_adjpreclose";
        //public new const string C_Col_AdjOpen = "s_dq_adjopen";
        //public new const string C_Col_AdjHigh = "s_dq_adjhigh";
        //public new const string C_Col_AdjLow = "s_dq_adjlow";
        //public new const string C_Col_AdjClose = "s_dq_adjclose";
        //public new const string C_Col_AdjFactor = "s_dq_adjfactor";
        public new const string C_Col_AvgPrice = "AVGPRICE";
        //public new const string C_Col_TradeStatus = "s_dq_tradestatus";
        #endregion

        #region 资产负债表 AShareBalanceSheet
        public new const string C_Col_ReportPeriod = "Reportdate";
        public new const string C_Col_AnnouncementDate = "Publishdate";
        public new const string C_Col_StatementType = "ReportStyle";           //11=合并报表期末 21=母公司报表期末
        public new const string C_Col_CompanyType = "ITPROFILE4";

        public new const string C_Col_MonetoryCap = "CBSheet1";
        public new const string C_Col_AccountRecievable = "CBSheet7";
        public new const string C_Col_Prepay = "CBSheet11";                     //预付款项
        public new const string C_Col_FixAssets = "CBSheet105";
        public new const string C_Col_ConstructionInProgress = "CBSheet34";     //在建工程
        public new const string C_Col_Inventories = "CBSheet14";
        public new const string C_Col_TotalCurrentAssets = "CBSheet21";
        public new const string C_Col_TotalAssets = "CBSheet46";
        public new const string C_Col_TotalCurrentLiability = "CBSheet66";
        public new const string C_Col_TotalLiability = "CBSheet77";
        public new const string C_Col_TotalEquityInclMinInt = "CBSheet86";
        #endregion

        #region 利润表 AShareIncome
        public new const string C_Col_TotalOperationCost = "CINST65";       //营业总成本
        public new const string C_Col_OperationCost = "CINST3";             //营业成本
        public new const string C_Col_SellingExpence = "CINST8";            //销售费用
        public new const string C_Col_AdminExpence = "CINST9";              //管理费用
        public new const string C_Col_FinanceExpence = "CINST10";           //财务费用
        public new const string C_Col_NetInvestmentIncome = "CINST13";      //投资净收益
        public new const string C_Col_NetProfitExclMinInt = "CINST58";      //净利润(不含少数股东损益)
        public new const string C_Col_TotalOperatingRevenue = "CINST61";
        public new const string C_Col_OperatingRevenue = "CINST1";
        public new const string C_Col_TotalProfit = "CINST19";
        public new const string C_Col_NetProfitInclMinInt = "CINST24";
        public new const string C_Col_NetProfitAfterDedNRLP = "MFRatio_R3";      //扣除非经常性损益的净利润
        #endregion
        #endregion
        
        #region 辅助功能
        protected override ExchangeType getExchangeType(string exchange)
        {
            switch (exchange)
            {
                case "CNSESH":
                    return ExchangeType.SSE;

                case "CNSESZ":
                    return ExchangeType.SZSE;

                default:
                    throw new Exception(C_Msg_UnknownExchange);
            }
        }
        protected override ListedBoardType getListedBoardType(string code)
        {
            string synbol = code.Substring(0, 3);
            switch (synbol)
            {
                case "300":
                    return ListedBoardType.CYB;

                case "002":
                    return ListedBoardType.ZXB;

                case "000":
                case "001":
                case "600":
                case "601":
                case "603":
                    return ListedBoardType.ZB;

                default:
                    throw new Exception(C_Msg_UnknownListedBoard);
            }
        }
        protected override FinancialStatementType getStatementType(string statementType)
        {
            switch (statementType)
            {
                case "11":
                    return FinancialStatementType.Merged;
                case "12":
                    return FinancialStatementType.MergedUpdated;
                case "13":
                    return FinancialStatementType.MergedAdjusted;

                case "21":
                    return FinancialStatementType.Parent;
                case "22":
                    return FinancialStatementType.ParentUpdated;
                case "23":
                    return FinancialStatementType.ParentAdjusted;

                default:
                    throw new Exception(C_Msg_UnknownStatementType);
            }
        }
        protected override CompanyType getCompanyType(string companyType)
        {
            string key = companyType.Substring(0, 3);
            switch (key)
            {
                case "001":
                    return CompanyType.Bank;
                case "003":
                    return CompanyType.Insurance;
                case "005":
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

                        d.Code = oRow[C_Col_Code].ToString();
                        d.Name = oRow[C_Col_Name].ToString();
                        d.Exchange = this.getExchangeType(oRow[C_Col_ExchMarket].ToString());
                        d.ListDate = this.getDateTimeValue(oRow[C_Col_ListDate]);
                        d.DelistDate = this.getDateTimeValue(oRow[C_Col_DelistDate]);

                        d.WindCode = base.getWindCode(d.Code, d.Exchange);
                        d.ListedBoard = this.getListedBoardType(oRow[C_Col_Code].ToString());

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
                        d.Volume = base.getDoubleValue(oRow[C_Col_Volume]);           // 股
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
                            d.Average = base.getDoubleValue(oRow[C_Col_AvgPrice]);

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

                        d.TradeDay = base.getDateTimeValueBy8Digits(oRow[C_Col_Trade_Date]);
                        d.Exchange = this.getExchangeType(oRow[C_Col_ExchMarket].ToString());

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

                        d.WindCode = base.getWindCode(oRow[C_Col_Code].ToString(), exchange);
                        d.ReportPeriod = getDateTimeValue(oRow[C_Col_ReportPeriod]);
                        d.AnnouncementDate = getDateTimeValue(oRow[C_Col_AnnouncementDate]);
                        d.StatementType = getStatementType(oRow[C_Col_StatementType].ToString());
                        d.CorpType = getCompanyType(oRow[C_Col_CompanyType].ToString());
                        d.Inventories = getDoubleValue(oRow[C_Col_Inventories]);
                        d.TotalAssets = getDoubleValue(oRow[C_Col_TotalAssets]);
                        d.TotalCurrentAssets = getDoubleValue(oRow[C_Col_TotalCurrentAssets]);
                        d.TotalCurrentLiability = getDoubleValue(oRow[C_Col_TotalCurrentLiability]);
                        d.TotalEquityInclMinInt = getDoubleValue(oRow[C_Col_TotalEquityInclMinInt]);
                        d.TotalLiability = getDoubleValue(oRow[C_Col_TotalLiability]);

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

                        d.WindCode = base.getWindCode(oRow[C_Col_Code].ToString(), exchange);
                        d.ReportPeriod = getDateTimeValue(oRow[C_Col_ReportPeriod]);
                        d.AnnouncementDate = getDateTimeValue(oRow[C_Col_AnnouncementDate]);
                        d.StatementType = getStatementType(oRow[C_Col_StatementType].ToString());
                        d.CorpType = getCompanyType(oRow[C_Col_CompanyType].ToString());
                        d.TotalOperatingRevenue = getDoubleValue(oRow[C_Col_TotalOperatingRevenue]);
                        d.OperatingRevenue = getDoubleValue(oRow[C_Col_OperatingRevenue]);
                        d.TotalProfit = getDoubleValue(oRow[C_Col_TotalProfit]);
                        d.NetProfitInclMinInt = getDoubleValue(oRow[C_Col_NetProfitInclMinInt]);
                        //d.NetProfitAfterDedNRLP = getDoubleValue(oRow[C_Col_NetProfitAfterDedNRLP]);

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