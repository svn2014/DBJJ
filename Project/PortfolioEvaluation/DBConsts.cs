using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortfolioEvaluation
{
    class DBConsts
    {
        #region 估值表字段
        public const string C_GZB_ColName_FDate = "FDATE";              //日期
        public const string C_GZB_ColName_FKmbm = "FKMBM";              //科目编码
        public const string C_GZB_ColName_FKmmc = "FKMMC";              //科目名称
        public const string C_GZB_ColName_FHqjg = "FHQJG";              //行情价格
        public const string C_GZB_ColName_FZqsl = "FZQSL";              //证券数量
        public const string C_GZB_ColName_FZqcb = "FZQCB";              //证券成本
        public const string C_GZB_ColName_FZqsz = "FZQSZ";              //证券市值
        public const string C_GZB_ColName_FGz_zz = "FGZ_ZZ";            //估值增值
        public const string C_GZB_ColName_FSz_Jz_bl = "FSZ_JZ_BL";      //市值占基金净值比例(以%的字符串形式表示)
        public const string C_GZB_ColName_FCb_Jz_bl = "FCB_JZ_BL";      //成本占基金净值比例(以%的字符串形式表示)
        #endregion

        #region 会计科目
        #region 资产：现金类
        public const string C_GZB_KMBM_CashDeposit = "1002";              //银行存款
        public const string C_GZB_KMBM_CashDeposit_HQ = "100201";         //活期存款
        public const string C_GZB_KMBM_CashDeposit_DQ = "100202";         //定期存款
        public const string C_GZB_KMBM_CashDeposit_XD = "100203";         //协定存款
        public const string C_GZB_KMBM_CashDeposit_TZ = "100204";         //通知存款

        public const string C_GZB_KMBM_LiqReserve = "1021";               //清算备付金
        public const string C_GZB_KMBM_Margin = "1031";                   //存出保证金
        #endregion

        #region 资产：其他投资标的
        public const string C_GZB_KMBM_RevRepo = "1202";                   //买入返售金融资产
        public const string C_GZB_KMBM_Repo_SH_PL = "12020101";            //上交所质押式回购
        public const string C_GZB_KMBM_Repo_SH_BO = "12020102";            //上交所买断式回购
        public const string C_GZB_KMBM_Repo_SZ_PL = "12020201";            //深交所质押式回购
        public const string C_GZB_KMBM_Repo_SZ_BO = "12020202";            //深交所买断式回购
        public const string C_GZB_KMBM_Repo_IB_PL = "12020301";            //银行间质押式回购
        public const string C_GZB_KMBM_Repo_IB_BO = "12020302";            //银行间买断式回购
        public const string C_GZB_KMBM_TheRepo = "2202";                   //正回购

        public const string C_GZB_KMBM_Fund = "1105";                      //基金投资
        public const string C_GZB_KMBM_Fund_SH = "11050101";               //上交所基金
        public const string C_GZB_KMBM_Fund_SZ = "11050201";               //深交所基金
        public const string C_GZB_KMBM_Fund_OF = "11050301";               //开放式基金
        public const string C_GZB_KMBM_Fund_OF_M = "11050401";             //开放式货币基金

        public const string C_GZB_KMBM_Warrant = "1106";                   //权证投资

        #endregion

        #region 资产：股票类
        public const string C_GZB_KMBM_Stock = "1102";                      //股票投资
        public const string C_GZB_KMBM_Stock_SH = "11020101";               //沪
        public const string C_GZB_KMBM_SEO_SH = "11020401";                 //沪增发
        public const string C_GZB_KMBM_IPO_SH = "11020301";                 //沪首发
        public const string C_GZB_KMBM_Stock_SZ = "11023101";               //深
        public const string C_GZB_KMBM_SEO_SZ = "11023401";                 //深增发
        public const string C_GZB_KMBM_IPO_SZ = "11023301";                 //深首发
        public const string C_GZB_KMBM_Stock_CY = "11023701";               //创业板
        #endregion

        #region 资产：债券类
        public const string C_GZB_KMBM_Bond = "1103";                       //债券投资
        public const string C_GZB_KMBM_Bond_SH_Govn = "11031101";           //上交所国债
        public const string C_GZB_KMBM_Bond_SH_CB = "11031201";             //上交所可转债      
        public const string C_GZB_KMBM_Bond_SH_UZ = "11037201";               //上交所未上市可转债
        public const string C_GZB_KMBM_Bond_SH_Corp = "11031301";           //上交所企债
        public const string C_GZB_KMBM_Bond_SZ_Govn = "11033101";           //深交所国债
        public const string C_GZB_KMBM_Bond_SZ_CB = "11033201";             //深交所可转债        
        public const string C_GZB_KMBM_Bond_SZ_Corp = "11033301";           //深交所企债
        public const string C_GZB_KMBM_Bond_SZ_STFB = "11037601";           //深交所短期融资券

        public const string C_GZB_KMBM_Bond_IB_Govn = "11035101";           //银行间国债
        public const string C_GZB_KMBM_Bond_IB_Corp = "11035301";           //银行间企业债
        public const string C_GZB_KMBM_Bond_IB_FinB = "11035401";           //银行间金融债
        public const string C_GZB_KMBM_Bond_IB_Bill = "11035501";           //银行间央行票据
        public const string C_GZB_KMBM_Bond_IB_POFB = "11036901";           //银行间政策性金融债
        public const string C_GZB_KMBM_Bond_IB_STFB = "11037001";           //银行间短期融资券
        #endregion

        #region 资产：股利券息类
        public const string C_GZB_KMBM_Dividend = "1203";                   //股利        

        public const string C_GZB_KMBM_Dividend_SH = "120301";              //应收上交所股利  
        public const string C_GZB_KMBM_Dividend_OF = "120302";              //应收开放式基金红利
        public const string C_GZB_KMBM_Dividend_SZ = "120303";              //应收深交所股利 

        public const string C_GZB_KMBM_Dividend_SH_E = "12030101";          //应收上交所股票股利
        public const string C_GZB_KMBM_Dividend_SH_F = "12030102";          //应收上交所基金红利        
        public const string C_GZB_KMBM_Dividend_SZ_E = "12030301";          //应收深交所股票股利
        public const string C_GZB_KMBM_Dividend_SZ_F = "12030302";          //应收深交所基金红利
        public const string C_GZB_KMBM_Dividend_CY_E = "12030401";          //应收深交所创业板股利
        public const string C_GZB_KMBM_Dividend_OF_F = "12030201";          //应收开放式基金红利
        public const string C_GZB_KMBM_Dividend_OF_M = "12030202";          //应收开放式货币基金红利
        
        public const string C_GZB_KMBM_TotalInterest = "1204";              //全部应收利息
        public const string C_GZB_KMBM_CashInterest = "120401";             //存款利息
        public const string C_GZB_KMBM_BondInterest = "120410";             //债券利息
        public const string C_GZB_KMBM_RepoInterest = "120491";             //应收买入返售利息 -- 回购利息

        public const string C_GZB_KMBM_CashInterest_HQ = "12040101";        //应收活期存款利息
        public const string C_GZB_KMBM_CashInterest_DQ = "12040102";        //应收定期存款利息
        public const string C_GZB_KMBM_CashInterest_XD = "12040103";        //应收协议存款利息
        public const string C_GZB_KMBM_CashInterest_TZ = "12040104";        //应收通知存款利息
        public const string C_GZB_KMBM_LiqReserveInt = "120402";            //应收清算备付金利息

        public const string C_GZB_KMBM_BondInterest_SH_Govn = "12041011";   //上交所国债利息
        public const string C_GZB_KMBM_BondInterest_SH_CB = "12041012";
        public const string C_GZB_KMBM_BondInterest_SH_Corp = "12041013";
        public const string C_GZB_KMBM_BondInterest_SZ_Govn = "12041031";   //深交所国债利息
        public const string C_GZB_KMBM_BondInterest_SZ_CB = "12041032";
        public const string C_GZB_KMBM_BondInterest_SZ_Corp = "12041033";
        #endregion

        #region 资产：合计类
        public const string C_GZB_KMBM_NAV = "902今日单位净值：";            //单位净值
        public const string C_GZB_KMBM_CumNAV = "905累计单位净值：";         //累计单位净值
        
        public const string C_GZB_KMBM_PIC = "601实收资本";                  //基金份额
        public const string C_GZB_KMBM_PIC2 = "601实收基金";                 //基金份额        
        #endregion

        #region 其他科目
        public const string C_GZB_KMBM_Purchase = "1207";                  //应收申购款
        public const string C_GZB_KMBM_OtherReceivable = "1221";           //其他应收款
        
        //1501	待摊费用                	
        //2221	应交税金
        //2231	应付利息
        //2232	应付利润
        
        public const string C_GZB_KMBM_Redemption = "2203";                 //应付赎回款        	
        public const string C_GZB_KMBM_RedempFee = "2204";                  //应付赎回费
        public const string C_GZB_KMBM_ManagementFee = "2206";              //应付管理人报酬
        public const string C_GZB_KMBM_TrusteeFee = "2207";                 //应付托管费
        public const string C_GZB_KMBM_SalesFee = "2208";                   //应付销售服务费
        public const string C_GZB_KMBM_TransCost = "2209";                  //应付交易费用

        public const string C_GZB_KMBM_OtherPayment = "2241";               //其他应付款
        public const string C_GZB_KMBM_Prepayment = "2501";                 //预提费用
        public const string C_GZB_KMBM_Settlement = "3003";                 //证券清算款
        #endregion
        #endregion

        #region Wind字段
        public const string C_Mkt_ColumnName_WindCode = "s_info_windcode";
        public const string C_Mkt_ColumnName_Trade_Date = "trade_dt";
        public const string C_Mkt_ColumnName_StockName = "s_info_name";
        public const string C_Mkt_ColumnName_SWIndustryName = "sw_ind_name1";
        public const string C_Mkt_ColumnName_SWIndustryIndexCode = "s_info_indexcode";
        public const string C_Mkt_ColumnName_Volume = "s_dq_volume";
        public const string C_Mkt_ColumnName_Amount = "s_dq_amount";
        public const string C_Mkt_ColumnName_AdjClose = "s_dq_adjclose";
        public const string C_Mkt_ColumnName_Close = "s_dq_close";
        public const string C_Mkt_ColumnName_PreClose = "s_dq_preclose";
        public const string C_Mkt_ColumnName_AdjFactor = "s_dq_adjfactor";
        public const string C_Mkt_ColumnName_AvgPrice = "s_dq_avgprice";
        public const string C_Mkt_ColumnName_FreeFloatCapital = "s_share_freeshares";
        public const string C_Mkt_ColumnName_ConWindCode = "s_con_windcode";
        public const string C_Mkt_ColumnName_ConWeight = "i_weight";
        public const string C_Mkt_ColumnName_IndustryName = "industriesname";
        public const string C_Mkt_ColumnName_PctChange = "s_dq_pctchange";
        public const string C_Mkt_ColumnName_Trade_Days = "trade_days";
        public const string C_Mkt_ColumnName_MaturityDate = "b_info_maturitydate";
        #endregion

        #region 报表字段
        public const string C_ColumnName_Code = "Code"; public const string C_ColumnName_Code_Text = "代码";
        public const string C_ColumnName_Name = "Name"; public const string C_ColumnName_Name_Text = "名称";
        public const string C_ColumnName_HoldingVolume = "HoldingVolume"; public const string C_ColumnName_HoldingVolume_Text = "持有量";
        public const string C_ColumnName_HoldingMV = "HoldingMV"; public const string C_ColumnName_HoldingMV_Text = "持有市值";
        public const string C_ColumnName_pctofNAV = "pctofNAV"; public const string C_ColumnName_pctofNAV_Text = "占比净值";
        public const string C_ColumnName_PortfolioWeight = "PortfolioWeight"; public const string C_ColumnName_PortfolioWeight_Text = "组合权重";
        public const string C_ColumnName_PortfolioReturnPct = "PortfolioReturnPct"; public const string C_ColumnName_PortfolioReturnPct_Text = "组合回报率";
        public const string C_ColumnName_PortfolioReturn = "PortfolioReturn"; public const string C_ColumnName_PortfolioReturn_Text = "组合回报";
        public const string C_ColumnName_PortfolioNetReturnPct = "PortfolioNetReturnPct"; public const string C_ColumnName_PortfolioNetReturnPct_Text = "组合净回报率";
        public const string C_ColumnName_PortfolioNetReturn = "PortfolioNetReturn"; public const string C_ColumnName_PortfolioNetReturn_Text = "组合净回报";
        public const string C_ColumnName_BenchmarkWeight = "BenchmarkWeight"; public const string C_ColumnName_BenchmarkWeight_Text = "基准权重";
        public const string C_ColumnName_BenchmarkReturnPct = "BenchmarkReturnPct"; public const string C_ColumnName_BenchmarkReturnPct_Text = "基准回报率";
        public const string C_ColumnName_BMWghtDiff = "BMWghtDiff";           public const string C_ColumnName_BMWghtDiff_Text = "权重差"; 
        public const string C_ColumnName_pctofListed = "pctofListed"; public const string C_ColumnName_pctofListed_Text = "占比流通股";
        public const string C_ColumnName_Turnover = "Turnover"; public const string C_ColumnName_Turnover_Text = "换手率";
        public const string C_ColumnName_LiqCycle = "LiqCycle"; public const string C_ColumnName_LiqCycle_Text = "流动周期";
        public const string C_ColumnName_Industry = "Industry"; public const string C_ColumnName_Industry_Text = "行业";
        public const string C_ColumnName_PureSectorAllowcation = "PureSectorAllowcation"; public const string C_ColumnName_PureSectorAllowcation_Text = "配置收益";
        public const string C_ColumnName_AllowcationSelectionInteraction = "AllowcationSelectionInteraction"; public const string C_ColumnName_AllowcationSelectionInteraction_Text = "交叉收益";
        public const string C_ColumnName_WithinSectorSelection = "WithinSectorSelection"; public const string C_ColumnName_WithinSectorSelection_Text = "选股收益";
        public const string C_ColumnName_TotalValueAdded = "TotalValueAdded"; public const string C_ColumnName_TotalValueAdded_Text = "增加值";
        public const string C_ColumnName_AnnualYield = "AnnualYield"; public const string C_ColumnName_AnnualYield_Text = "年化收益率";

        public const string C_ColumnName_TransactionDate = "TransactionDate"; public const string C_ColumnName_TransactionDate_Text = "交易日期";
        public const string C_ColumnName_TransactionType = "TransactionType"; public const string C_ColumnName_TransactionType_Text = "交易类型";
        public const string C_ColumnName_TradingPrice = "TradingPrice"; public const string C_ColumnName_TradingPrice_Text = "交易均价";
        public const string C_ColumnName_MarketPrice = "MarketPrice"; public const string C_ColumnName_MarketPrice_Text = "最新价";
        public const string C_ColumnName_TradingVolume = "TradingVolume"; public const string C_ColumnName_TradingVolume_Text = "交易量";
        public const string C_ColumnName_TransactionReturn = "TransactionReturn"; public const string C_ColumnName_TransactionReturn_Text = "交易回报";


        public const string C_ColumnName_ReportDate = "ReportDate"; public const string C_ColumnName_ReportDate_Text = "报告日期";
        public const string C_ColumnName_UnitCumNAV = "UnitCumNAV"; public const string C_ColumnName_UnitCumNAV_Text = "累计单位净值";
        public const string C_ColumnName_PIC = "PIC"; public const string C_ColumnName_PIC_Text = "当前份额";
        public const string C_ColumnName_Subscribe = "Subscribe"; public const string C_ColumnName_Subsribe_Text = "申购";
        public const string C_ColumnName_Redeem = "Redeem"; public const string C_ColumnName_Redeem_Text = "赎回";
        public const string C_ColumnName_EquityWeight = "EquityWeight"; public const string C_ColumnName_EquityWeight_Text = "股票仓位";
        public const string C_ColumnName_BondWeight = "BondWeight"; public const string C_ColumnName_BondWeight_Text = "债券仓位";
        public const string C_ColumnName_YieldOnEquity = "YieldOnEquity"; public const string C_ColumnName_YieldOnEquity_Text = "股票收益率";
        public const string C_ColumnName_NetYieldOnBond = "NetYieldOnBond"; public const string C_ColumnName_NetYieldOnBond_Text = "债券净收益率";
        public const string C_ColumnName_MaturityDate = "MaturityDate"; public const string C_ColumnName_MaturityDate_Text = "到期日";

        public const string C_ColumnName_IndexPrice = "indexprices";
        public const string C_ColumnName_EquityPositionPct = "equitypositionpct";

        public const string C_Text_CashPortfolio = "现金";
        public const string C_Text_EquityPortfolio = "股票组合";
        public const string C_Text_BondPortfolio = "债券组合";
        #endregion

        #region Error Message
        public const string C_ErrMsg_EmptyFund = "基金估值表为空";
        public const string C_ErrMsg_EmptyPrice = "无价格数据";
        public const string C_ErrMsg_ErrorPositionDetail = "持仓明细数据错误";
        public const string C_ErrMsg_NoIndustry = "无行业数据";
        public const string C_ErrMsg_NoBenchmark = "未设置比较基准";
        public const string C_ErrMsg_ErrorInBenchmark = "比较基准错误";
        public const string C_ErrMsg_MissingAssets = "丢失新入资产";
        #endregion
    }
}
