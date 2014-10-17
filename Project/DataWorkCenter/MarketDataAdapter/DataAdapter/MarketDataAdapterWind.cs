using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MarketDataAdapter
{
    public class MarketDataAdapterWind : AMarketDataAdapter
    {
        #region 字段列表
        #region 交易日历: AShareCalendar
        public new const string C_Col_TradeDays = "trade_days";
        #endregion

        #region 基本信息: AShareDescription
        public new const string C_Col_WindCode = "s_info_windcode";
        public new const string C_Col_Code = "s_info_code";
        public new const string C_Col_Name = "s_info_name";
        public new const string C_Col_ExchMarket = "s_info_exchmarket";
        public new const string C_Col_ListBoard = "s_info_listboard";
        public new const string C_Col_ListDate = "s_info_listdate";
        public new const string C_Col_DelistDate = "s_info_delistdate";
        #endregion

        #region 股价信息: AShareEODPrices
        public new const string C_Col_Trade_Date = "trade_dt";
        public new const string C_Col_PreClose = "s_dq_preclose";
        public new const string C_Col_Open = "s_dq_open";
        public new const string C_Col_High = "s_dq_high";
        public new const string C_Col_Low = "s_dq_low";
        public new const string C_Col_Close = "s_dq_close";
        public new const string C_Col_Volume = "s_dq_volume";
        public new const string C_Col_Amount = "s_dq_amount";
        public new const string C_Col_AdjPreClose = "s_dq_adjpreclose";
        public new const string C_Col_AdjOpen = "s_dq_adjopen";
        public new const string C_Col_AdjHigh = "s_dq_adjhigh";
        public new const string C_Col_AdjLow = "s_dq_adjlow";
        public new const string C_Col_AdjClose = "s_dq_adjclose";
        public new const string C_Col_AdjFactor = "s_dq_adjfactor";
        public new const string C_Col_AvgPrice = "s_dq_avgprice";
        public new const string C_Col_TradeStatus = "s_dq_tradestatus";
        public new const string C_Col_PctChange = "s_dq_pctchange";
        #endregion

        #region 资产负债表: AShareBalanceSheet
        public new const string C_Col_ReportPeriod = "report_period";
        public new const string C_Col_AnnouncementDate = "ann_dt";
        public new const string C_Col_StatementType = "statement_type"; //408001000=合并报表 408006000=母公司报表
        public new const string C_Col_CompanyType = "comp_type_code";

        public new const string C_Col_MonetoryCap = "monetary_cap";
        public new const string C_Col_AccountRecievable = "acct_rcv";
        public new const string C_Col_Prepay = "prepay";                        //预付款项
        public new const string C_Col_FixAssets = "fix_assets";
        public new const string C_Col_ConstructionInProgress = "const_in_prog"; //在建工程
        public new const string C_Col_Inventories = "inventories";
        public new const string C_Col_TotalCurrentAssets = "tot_cur_assets";
        public new const string C_Col_TotalAssets = "tot_assets";
        public new const string C_Col_TotalCurrentLiability = "tot_cur_liab";
        public new const string C_Col_TotalLiability = "tot_liab";
        public new const string C_Col_TotalEquityInclMinInt = "tot_shrhldr_eqy_incl_min_int";
        #endregion

        #region 利润表: AShareIncome
        public new const string C_Col_TotalOperationCost = "tot_oper_cost";                 //营业总成本
        public new const string C_Col_OperationCost = "less_oper_cost";                     //营业成本
        public new const string C_Col_SellingExpence = "less_selling_dist_exp";             //销售费用
        public new const string C_Col_AdminExpence = "less_gerl_admin_exp";                 //管理费用
        public new const string C_Col_FinanceExpence = "less_fin_exp";                      //财务费用
        public new const string C_Col_NetInvestmentIncome = "plus_net_invest_inc";          //投资净收益
        public new const string C_Col_NetProfitExclMinInt = "net_profit_excl_min_int_inc";  //净利润(不含少数股东损益)
        public new const string C_Col_TotalOperatingRevenue = "tot_oper_rev";
        public new const string C_Col_OperatingRevenue = "oper_rev";
        public new const string C_Col_TotalProfit = "tot_profit";
        public new const string C_Col_NetProfitInclMinInt = "net_profit_incl_min_int_inc";
        public new const string C_Col_NetProfitAfterDedNRLP = "net_profit_after_ded_nr_lp"; //扣除非经常性损益的净利润
        #endregion
        #endregion

        #region 辅助功能
        protected override ExchangeType getExchangeType(string exchange)
        {
            switch (exchange)
            {
                case "SSE":
                    return ExchangeType.SSE;

                case "SZSE":
                    return ExchangeType.SZSE;

                default:
                    throw new Exception(C_Msg_UnknownExchange);
            }
        }
        protected override ListedBoardType getListedBoardType(string listedBorad)
        {
            switch (listedBorad)
            {
                case "434001000":
                    return ListedBoardType.CYB;

                case "434003000":
                    return ListedBoardType.ZXB;

                case "434004000":
                    return ListedBoardType.ZB;

                default:
                    throw new Exception(C_Msg_UnknownListedBoard);
            }
        }
        protected override FinancialStatementType getStatementType(string statementType)
        {
            switch (statementType)
            {
                case "408001000":
                    return FinancialStatementType.Merged;
                case "408004000":
                    return FinancialStatementType.MergedAdjusted;
                case "408005000":
                    return FinancialStatementType.MergedUpdated;
                case "408006000":
                    return FinancialStatementType.Parent;
                case "408009000":
                    return FinancialStatementType.ParentAdjusted;
                case "408010000":
                    return FinancialStatementType.ParentUpdated;

                default:
                    throw new Exception(C_Msg_UnknownStatementType);
            }
        }
        protected override CompanyType getCompanyType(string companyType)
        {
            try
            {
                return (CompanyType)Enum.Parse(typeof(CompanyType), companyType, true);
            }
            catch (Exception)
            {
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

                        d.WindCode = oRow[C_Col_WindCode].ToString();
                        d.Code = oRow[C_Col_Code].ToString();
                        d.Name = oRow[C_Col_Name].ToString();
                        d.Exchange = this.getExchangeType(oRow[C_Col_ExchMarket].ToString());
                        d.ListedBoard = this.getListedBoardType(oRow[C_Col_ListBoard].ToString());
                        d.ListDate = base.getDateTimeValueBy8Digits(oRow[C_Col_ListDate]);
                        d.DelistDate = base.getDateTimeValueBy8Digits(oRow[C_Col_DelistDate]);

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

                        d.WindCode = oRow[C_Col_WindCode].ToString();
                        d.TradeDate = base.getDateTimeValueBy8Digits(oRow[C_Col_Trade_Date]);
                        d.Volume = base.getDoubleValue(oRow[C_Col_Volume]) * 100;      //手     =>  股
                        d.Amount = (base.getDoubleValue(oRow[C_Col_Amount]) * 1000);   //千元   =>  元
                        //if (d.Amount != null)
                        //    d.Amount = Math.Truncate(Math.Round(d.Amount.Value, 1));  //保留小数

                        if (d.Volume == 0)
                            d.Status = TradingStatus.Suspend;
                        else
                        {
                            if (oRow.Table.Columns.Contains(C_Col_TradeStatus))
                            {
                                string status = oRow[C_Col_TradeStatus].ToString();
                                switch (status)
                                {
                                    case "XR":
                                        d.Status = TradingStatus.ExRight;
                                        break;

                                    case "XD":
                                        d.Status = TradingStatus.ExDividend;
                                        break;

                                    case "DR":
                                        d.Status = TradingStatus.ExRightAndDividend;
                                        break;

                                    default:
                                        d.Status = TradingStatus.Trading;
                                        break;
                                }
                            }
                            else
                                d.Status = TradingStatus.Trading;
                        }

                        d.PreClose = base.getDoubleValue(oRow[C_Col_PreClose]);
                        d.Open = base.getDoubleValue(oRow[C_Col_Open]);
                        d.High = base.getDoubleValue(oRow[C_Col_High]);
                        d.Low = base.getDoubleValue(oRow[C_Col_Low]);
                        d.Close = base.getDoubleValue(oRow[C_Col_Close]);

                        if (oRow.Table.Columns.Contains(C_Col_AvgPrice))
                            d.Average = base.getDoubleValue(oRow[C_Col_AvgPrice]);

                        //px.AdjustedFactor = base.getDoubleValue(oRow[WindConsts.C_Col_AdjFactor]);
                        //px.AdjustedPreClose = base.getDoubleValue(oRow[WindConsts.C_Col_AdjPreClose]);
                        //px.AdjustedOpen = base.getDoubleValue(oRow[WindConsts.C_Col_AdjOpen]);
                        //px.AdjustedHigh = base.getDoubleValue(oRow[WindConsts.C_Col_AdjHigh]);
                        //px.AdjustedLow = base.getDoubleValue(oRow[WindConsts.C_Col_AdjLow]);
                        //px.AdjustedClose = base.getDoubleValue(oRow[WindConsts.C_Col_AdjClose]);

                        //px.AdjustedAverage = (px.Average != null && px.AdjustedFactor != null) ? px.Average * px.AdjustedFactor : null;

                        d.PctChange = base.getDoubleValue(oRow[C_Col_PctChange]) / 100; //%
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
                        d.TradeDay = base.getDateTimeValueBy8Digits(oRow[C_Col_TradeDays]);

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

                        d.WindCode = oRow[C_Col_WindCode].ToString().ToUpper();
                        d.ReportPeriod = getDateTimeValueBy8Digits(oRow[C_Col_ReportPeriod]);
                        d.StatementType = this.getStatementType(oRow[C_Col_StatementType].ToString());

                        d.AnnouncementDate = getDateTimeValueBy8Digits(oRow[C_Col_AnnouncementDate]);
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

                        d.WindCode = oRow[C_Col_WindCode].ToString().ToUpper();
                        d.ReportPeriod = getDateTimeValueBy8Digits(oRow[C_Col_ReportPeriod]);
                        d.StatementType = this.getStatementType(oRow[C_Col_StatementType].ToString());

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