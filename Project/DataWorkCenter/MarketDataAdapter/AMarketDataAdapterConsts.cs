using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarketDataAdapter
{
    public abstract partial class AMarketDataAdapter
    {
        public const string C_Msg_UnknownExchange = "未知的交易所代码";
        public const string C_Msg_UnknownListedBoard = "未知的上市板块";
        public const string C_Msg_UnknownStatementType = "未知的报表类型";

        #region 更新信息
        public const string C_Col_Opdate = "opdate";
        public const string C_Col_Opmode = "opmode";
        #endregion

        #region 交易日历: AShareCalendar
        public const string C_Col_TradeDays = "trade_days";
        #endregion

        #region 基本信息: AShareDescription
        public const string C_Col_WindCode = "s_info_windcode";
        public const string C_Col_Code = "s_info_code";
        public const string C_Col_Name = "s_info_name";
        public const string C_Col_ExchMarket = "secumarket";
        public const string C_Col_ListBoard = "s_info_listboard";
        public const string C_Col_ListDate = "s_info_listdate";
        public const string C_Col_DelistDate = "s_info_delistdate";
        #endregion

        #region 股价信息: AShareEODPrices
        public const string C_Col_Trade_Date = "trade_dt";
        public const string C_Col_PreClose = "s_dq_preclose";
        public const string C_Col_Open = "s_dq_open";
        public const string C_Col_High = "s_dq_high";
        public const string C_Col_Low = "s_dq_low";
        public const string C_Col_Close = "s_dq_close";
        public const string C_Col_Volume = "s_dq_volume";
        public const string C_Col_Amount = "s_dq_amount";
        public const string C_Col_AdjPreClose = "s_dq_adjpreclose";
        public const string C_Col_AdjOpen = "s_dq_adjopen";
        public const string C_Col_AdjHigh = "s_dq_adjhigh";
        public const string C_Col_AdjLow = "s_dq_adjlow";
        public const string C_Col_AdjClose = "s_dq_adjclose";
        public const string C_Col_AdjFactor = "s_dq_adjfactor";
        public const string C_Col_AvgPrice = "s_dq_avgprice";
        public const string C_Col_TradeStatus = "s_dq_tradestatus";
        #endregion

        #region 资产负债表: AShareBalanceSheet
        public const string C_Col_ReportPeriod = "report_period";
        public const string C_Col_AnnouncementDate = "ann_dt";
        public const string C_Col_StatementType = "statement_type"; //408001000=合并报表 408006000=母公司报表
        public const string C_Col_CompanyType = "comp_type_code";

        public const string C_Col_MonetoryCap = "monetary_cap";
        public const string C_Col_AccountRecievable = "acct_rcv";
        public const string C_Col_Prepay = "prepay";                        //预付款项
        public const string C_Col_FixAssets = "fix_assets";                 //固定资产净额
        public const string C_Col_ConstructionInProgress = "const_in_prog"; //在建工程

        public const string C_Col_Inventories = "inventories";
        public const string C_Col_TotalCurrentAssets = "tot_cur_assets";
        public const string C_Col_TotalAssets = "tot_assets";
        public const string C_Col_TotalCurrentLiability = "tot_cur_liab";
        public const string C_Col_TotalLiability = "tot_liab";
        public const string C_Col_TotalEquityInclMinInt = "tot_shrhldr_eqy_incl_min_int";
        #endregion

        #region 利润表: AShareIncome
        public const string C_Col_TotalOperationCost = "tot_oper_cost";                 //营业总成本
        public const string C_Col_OperationCost = "less_oper_cost";                     //营业成本
        public const string C_Col_SellingExpence = "less_selling_dist_exp";             //销售费用
        public const string C_Col_AdminExpence = "less_gerl_admin_exp";                 //管理费用
        public const string C_Col_FinanceExpence = "less_fin_exp";                      //财务费用
        public const string C_Col_NetInvestmentIncome = "plus_net_invest_inc";          //投资净收益
        public const string C_Col_NetProfitExclMinInt = "net_profit_excl_min_int_inc";  //净利润(不含少数股东损益)

        public const string C_Col_TotalOperatingRevenue = "tot_oper_rev";
        public const string C_Col_OperatingRevenue = "oper_rev";
        public const string C_Col_TotalProfit = "tot_profit";
        public const string C_Col_NetProfitInclMinInt = "net_profit_incl_min_int_inc";  //净利润(含少数股东损益)
        public const string C_Col_NetProfitAfterDedNRLP = "net_profit_after_ded_nr_lp"; //扣除非经常性损益的净利润
        #endregion
    }
}
