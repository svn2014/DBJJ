using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Collections;

namespace PortfolioEvaluation
{
    public enum AssetCategory
    {
        Repo,
        Equity,
        Bond,
        ConvertableBond,
        Fund,
        Warrant,
        CashDeposit,
        Other
    }

    public class Portfolio : IComparable, ICloneable
    {
        #region 接口
        public int CompareTo(object obj)
        {
            try
            {
                //Order by ReportDate Desc
                Portfolio p = (Portfolio)obj;
                if (this.ReportDate > p.ReportDate)
                    return -1;  
                else
                    return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object Clone()
        {
            Portfolio newPortfolio = new Portfolio(this.Code, this.Name, this.Type, this.CategoryI, this.CategoryII);

            FieldInfo[] fields = this.GetType().GetFields();

            foreach (FieldInfo fi in fields)
            {
                object val = fi.GetValue(this);
                fi.SetValue(newPortfolio, val);
            }

            return newPortfolio;
        }
        #endregion

        #region 枚举
        public enum PortfolioType
        {
            MutualFund,
            HedgeFunds
        }

        public enum PortfolioCategoryI
        {
            EquityFund,
            HybridFund,
            BondFund,
            MonetaryFund
        }

        public enum PortfolioCategoryII
        {
            OpenEnd,
            CloseEnd
        }        
        #endregion

        #region 属性
        public string Code = "";
        public string Name = "";
        public DateTime ReportDate = DateTime.Today;
        public PortfolioType Type = PortfolioType.MutualFund;
        public PortfolioCategoryI CategoryI = PortfolioCategoryI.EquityFund;
        public PortfolioCategoryII CategoryII = PortfolioCategoryII.OpenEnd;

        public double PIC = 0;        //基金份额
        public double UnitNAV = 0;    //单位净值
        public double UnitCumNAV = 0; //单位累计净值

        public double ReturnOnEquity = 0;   //元
        public double ReturnOnBond = 0;     //考虑利息收入
        public double NetReturnOnBond = 0;  //不考虑利息收入
        public double ReturnOnCash = 0;
        public double ReturnOnOther = 0;

        public double CostOnEquity = 0;   //元
        public double CostOnBond = 0;
        public double CostOnCash = 0;
        public double CostOnOther = 0;
        
        public double YieldOnEquity = 0;    //%
        public double YieldOnBond = 0;      //考虑利息收入
        public double NetYieldOnBond = 0;   //不考虑利息收入
        public double YieldOnCash = 0;
        public double YieldOnOther = 0;
        public double YieldOnPortfolio = 0; //%
        #endregion

        #region 过程
        public Portfolio(string code, string name, PortfolioType type, PortfolioCategoryI categoryI, PortfolioCategoryII categoryII) 
        {
            this.Code = code;
            this.Name = name;
            this.Type = type;
            this.CategoryI = categoryI;
            this.CategoryII = categoryII;
        }
    
        #region 组合持仓-估值表数据
        private List<GZBItem> _GZBList;
        public void UpdatePositionByGZB(List<GZBItem> gzbList)
        {
            if (gzbList.Count > 0)
                this.ReportDate = gzbList[0].ItemDate;
            else
                throw new Exception(DBConsts.C_ErrMsg_EmptyFund);

            //保留当前日期的数据
            _GZBList = gzbList.FindAll(delegate(GZBItem gzb) { return gzb.ItemDate == this.ReportDate; });

            loadPositionSummary();

            if (!checkDataBalance())
                throw new Exception(DBConsts.C_ErrMsg_MissingAssets);

            loadCashPositions();
            loadAssetPositions();
            loadDividendInterest();
        }

        private bool checkDataBalance()
        {
            //检查资产合计=100%
            double sum = 0;
            sum += CashInDepositPct + CashLiqReservePct + CashMarginPct + CashTotalInterestPct + CashTotalDividendPct + CashPurchasePct
                + EquityPositionPct + BondPositionPct + RevRepoPositionPct + FundPositionPct + WarrantPositionPct
                + CashRedemptionPct + CashRedempFeePct + CashManageFeePct + CashTrusteeFeePct + CashTransCostPct + CashSalesFeePct
                + CashOtherPaymentPct + CashPrepaymentPct + CashSettlementPct;

            sum -= TheRepoPositionPct;  //扣除正回购

            this.CashAllPositionPct = sum - (EquityPositionPct + BondPositionPct + FundPositionPct + WarrantPositionPct + RevRepoPositionPct);

            if (Math.Abs(sum - 1) < 0.01)
                return true;
            else
                return false;
        }

        private void loadPositionSummary()
        {
            #region 资产 = 负债 + 权益
            //回购比例(%)
            int idx = _GZBList.FindIndex(delegate(GZBItem gzb) { return (gzb.ItemDate == this.ReportDate && gzb.ItemCode == DBConsts.C_GZB_KMBM_RevRepo); });
            if (idx >= 0)
            {
                this.RevRepoPositionPct = _GZBList[idx].HoldingMVPctofNAV;
                this.RevRepoPositionAmt = _GZBList[idx].HoldingMarketValue;
            }

            //正回购
            idx = _GZBList.FindIndex(delegate(GZBItem gzb) { return (gzb.ItemDate == this.ReportDate && gzb.ItemCode == DBConsts.C_GZB_KMBM_TheRepo); });
            if (idx >= 0)
            {
                this.TheRepoPositionPct = _GZBList[idx].HoldingMVPctofNAV;
                this.TheRepoPositionAmt = _GZBList[idx].HoldingMarketValue;
            }

            //基金比例(%)
            idx = _GZBList.FindIndex(delegate(GZBItem gzb) { return (gzb.ItemDate == this.ReportDate && gzb.ItemCode == DBConsts.C_GZB_KMBM_Fund); });
            if (idx >= 0)
            {
                this.FundPositionPct = _GZBList[idx].HoldingMVPctofNAV;
                this.FundPositionAmt = _GZBList[idx].HoldingMarketValue;
            }

            //权证比例(%)
            idx = _GZBList.FindIndex(delegate(GZBItem gzb) { return (gzb.ItemDate == this.ReportDate && gzb.ItemCode == DBConsts.C_GZB_KMBM_Warrant); });
            if (idx >= 0)
            {
                this.WarrantPositionPct = _GZBList[idx].HoldingMVPctofNAV;
                this.WarrantPositionAmt = _GZBList[idx].HoldingMarketValue;
            }

            //股票比例(%)
            idx = _GZBList.FindIndex(delegate(GZBItem gzb) { return (gzb.ItemDate == this.ReportDate && gzb.ItemCode == DBConsts.C_GZB_KMBM_Stock); });
            if (idx >= 0)
            {
                this.EquityPositionPct = _GZBList[idx].HoldingMVPctofNAV;
                this.EquityPositionAmt = _GZBList[idx].HoldingMarketValue;
            }

            //债券比例(%)
            idx = _GZBList.FindIndex(delegate(GZBItem gzb) { return (gzb.ItemDate == this.ReportDate && gzb.ItemCode == DBConsts.C_GZB_KMBM_Bond); });
            if (idx >= 0)
            {
                this.BondPositionPct = _GZBList[idx].HoldingMVPctofNAV;
                this.BondPositionAmt = _GZBList[idx].HoldingMarketValue;
            }

            //银行存款(%)
            idx = _GZBList.FindIndex(delegate(GZBItem gzb) { return (gzb.ItemDate == this.ReportDate && gzb.ItemCode == DBConsts.C_GZB_KMBM_CashDeposit); });
            if (idx >= 0)
            {
                this.CashInDepositPct = _GZBList[idx].HoldingMVPctofNAV;
                this.CashInDepositAmt = _GZBList[idx].HoldingMarketValue;
            }

            //清算备付金(%)
            idx = _GZBList.FindIndex(delegate(GZBItem gzb) { return (gzb.ItemDate == this.ReportDate && gzb.ItemCode == DBConsts.C_GZB_KMBM_LiqReserve); });
            if (idx >= 0)
            {
                this.CashLiqReservePct = _GZBList[idx].HoldingMVPctofNAV;
                this.CashLiqReserveAmt = _GZBList[idx].HoldingMarketValue;
            }

            //存出保证金(%)
            idx = _GZBList.FindIndex(delegate(GZBItem gzb) { return (gzb.ItemDate == this.ReportDate && gzb.ItemCode == DBConsts.C_GZB_KMBM_Margin); });
            if (idx >= 0)
            {
                this.CashMarginPct = _GZBList[idx].HoldingMVPctofNAV;
                this.CashMarginAmt = _GZBList[idx].HoldingMarketValue;
            }

            //应收申购款(%)
            idx = _GZBList.FindIndex(delegate(GZBItem gzb) { return (gzb.ItemDate == this.ReportDate && gzb.ItemCode == DBConsts.C_GZB_KMBM_Purchase); });
            if (idx >= 0)
            {
                this.CashPurchasePct = _GZBList[idx].HoldingMVPctofNAV;
                this.CashSubscribeAmt = _GZBList[idx].HoldingMarketValue;
            }

            //-应付赎回款(%)
            idx = _GZBList.FindIndex(delegate(GZBItem gzb) { return (gzb.ItemDate == this.ReportDate && gzb.ItemCode == DBConsts.C_GZB_KMBM_Redemption); });
            if (idx >= 0)
            {
                this.CashRedemptionPct = -_GZBList[idx].HoldingMVPctofNAV;
                this.CashRedeemAmt = -_GZBList[idx].HoldingMarketValue;
            }

            //-应付赎回费(%)
            idx = _GZBList.FindIndex(delegate(GZBItem gzb) { return (gzb.ItemDate == this.ReportDate && gzb.ItemCode == DBConsts.C_GZB_KMBM_RedempFee); });
            if (idx >= 0)
            {
                this.CashRedempFeePct = -_GZBList[idx].HoldingMVPctofNAV;
                this.CashRedempFeeAmt = -_GZBList[idx].HoldingMarketValue;
            }

            //-应付管理人报酬(%)
            idx = _GZBList.FindIndex(delegate(GZBItem gzb) { return (gzb.ItemDate == this.ReportDate && gzb.ItemCode == DBConsts.C_GZB_KMBM_ManagementFee); });
            if (idx >= 0)
            {
                this.CashManageFeePct = -_GZBList[idx].HoldingMVPctofNAV;
                this.CashManageFeeAmt = -_GZBList[idx].HoldingMarketValue;
            }

            //-应付托管费(%)
            idx = _GZBList.FindIndex(delegate(GZBItem gzb) { return (gzb.ItemDate == this.ReportDate && gzb.ItemCode == DBConsts.C_GZB_KMBM_TrusteeFee); });
            if (idx >= 0)
            {
                this.CashTrusteeFeePct = -_GZBList[idx].HoldingMVPctofNAV;
                this.CashTrusteeFeeAmt = -_GZBList[idx].HoldingMarketValue;
            }

            //-应付销售服务费(%)
            idx = _GZBList.FindIndex(delegate(GZBItem gzb) { return (gzb.ItemDate == this.ReportDate && gzb.ItemCode == DBConsts.C_GZB_KMBM_SalesFee); });
            if (idx >= 0)
            {
                this.CashSalesFeePct = -_GZBList[idx].HoldingMVPctofNAV;
                this.CashSalesFeeAmt = -_GZBList[idx].HoldingMarketValue;
            }

            //-应付交易费用(%)
            idx = _GZBList.FindIndex(delegate(GZBItem gzb) { return (gzb.ItemDate == this.ReportDate && gzb.ItemCode == DBConsts.C_GZB_KMBM_TransCost); });
            if (idx >= 0)
            {
                this.CashTransCostPct = -_GZBList[idx].HoldingMVPctofNAV;
                this.CashTransCostAmt = -_GZBList[idx].HoldingMarketValue;
            }

            //-其他应付款(%)
            idx = _GZBList.FindIndex(delegate(GZBItem gzb) { return (gzb.ItemDate == this.ReportDate && gzb.ItemCode == DBConsts.C_GZB_KMBM_OtherPayment); });
            if (idx >= 0)
            {
                this.CashOtherPaymentPct = -_GZBList[idx].HoldingMVPctofNAV;
                this.CashOtherPaymentAmt = -_GZBList[idx].HoldingMarketValue;
            }

            //-预提费用(%)
            idx = _GZBList.FindIndex(delegate(GZBItem gzb) { return (gzb.ItemDate == this.ReportDate && gzb.ItemCode == DBConsts.C_GZB_KMBM_Prepayment); });
            if (idx >= 0)
            {
                this.CashPrepaymentPct = -_GZBList[idx].HoldingMVPctofNAV;
                this.CashPrepaymentAmt = -_GZBList[idx].HoldingMarketValue;
            }

            //-证券清算款(%)：估值表中为负，不必转负
            idx = _GZBList.FindIndex(delegate(GZBItem gzb) { return (gzb.ItemDate == this.ReportDate && gzb.ItemCode == DBConsts.C_GZB_KMBM_Settlement); });
            if (idx >= 0)
            {
                this.CashSettlementPct = _GZBList[idx].HoldingMVPctofNAV;
                this.CashSettlementAmt = _GZBList[idx].HoldingMarketValue;
            }
            #endregion

            #region 份额净值
            //PIC   //基金份额
            idx = _GZBList.FindIndex(delegate(GZBItem gzb) { 
                                    return (gzb.ItemDate == this.ReportDate
                                        && (
                                            gzb.ItemCode == DBConsts.C_GZB_KMBM_PIC || 
                                            gzb.ItemCode == DBConsts.C_GZB_KMBM_PIC2
                                            )
                                        );
                                    });
            if (idx >= 0)
                this.PIC = _GZBList[idx].HoldingMarketValue;

            //Unit NAV  //单位净值
            idx = _GZBList.FindIndex(delegate(GZBItem gzb) { return (gzb.ItemDate == this.ReportDate && gzb.ItemCode == DBConsts.C_GZB_KMBM_NAV); });
            if (idx >= 0)
                this.UnitNAV = Convert.ToDouble(_GZBList[idx].ItemName);

            //Unit Cumulative NAV  //累计单位净值
            idx = _GZBList.FindIndex(delegate(GZBItem gzb) { return (gzb.ItemDate == this.ReportDate && gzb.ItemCode == DBConsts.C_GZB_KMBM_CumNAV); });
            if (idx >= 0)
                this.UnitCumNAV = Convert.ToDouble(_GZBList[idx].ItemName);
            #endregion

            #region 利息
            //全部应收利息占比
            idx = _GZBList.FindIndex(delegate(GZBItem gzb) { return (gzb.ItemDate == this.ReportDate && gzb.ItemCode == DBConsts.C_GZB_KMBM_TotalInterest); });
            if (idx >= 0)
                this.CashTotalInterestPct = _GZBList[idx].HoldingMVPctofNAV;

            //全部应收股利占比
            idx = _GZBList.FindIndex(delegate(GZBItem gzb) { return (gzb.ItemDate == this.ReportDate && gzb.ItemCode == DBConsts.C_GZB_KMBM_Dividend); });
            if (idx >= 0)
                this.CashTotalDividendPct = _GZBList[idx].HoldingMVPctofNAV;
            
            //全部应收利息
            idx = _GZBList.FindIndex(delegate(GZBItem gzb) { return (gzb.ItemDate == this.ReportDate && gzb.ItemCode == DBConsts.C_GZB_KMBM_TotalInterest); });
            if (idx >= 0)
                this.CashTotalInterestAmt = _GZBList[idx].HoldingMarketValue;

            //债券利息
            idx = _GZBList.FindIndex(delegate(GZBItem gzb) { return (gzb.ItemDate == this.ReportDate && gzb.ItemCode == DBConsts.C_GZB_KMBM_BondInterest); });
            if (idx >= 0)
                this.BondInterestAmt = _GZBList[idx].HoldingMarketValue;

            //股票股利: 不包含基金股利
            //上交所
            idx = _GZBList.FindIndex(delegate(GZBItem gzb) { return (gzb.ItemDate == this.ReportDate && gzb.ItemCode == DBConsts.C_GZB_KMBM_Dividend_SH); });
            if (idx >= 0)
                this.EquityDividendAmt = _GZBList[idx].HoldingMarketValue;
            //深交所 
            idx = _GZBList.FindIndex(delegate(GZBItem gzb) { return (gzb.ItemDate == this.ReportDate && gzb.ItemCode == DBConsts.C_GZB_KMBM_Dividend_SZ); });
            if (idx >= 0)
                this.EquityDividendAmt += _GZBList[idx].HoldingMarketValue;
            #endregion
        }

        private void loadCashPositions()
        {
            AssetPosition p;
            CashAllPositionAmt = 0;

            //活期存款
            int idxPosition = _GZBList.FindIndex(delegate(GZBItem gzb) { return (gzb.ItemDate == this.ReportDate && gzb.ItemCode == DBConsts.C_GZB_KMBM_CashDeposit_HQ); });
            int idxInterest = _GZBList.FindIndex(delegate(GZBItem gzb) { return (gzb.ItemDate == this.ReportDate && gzb.ItemCode == DBConsts.C_GZB_KMBM_CashInterest_HQ); });
            if (idxPosition >= 0)
            {
                p = new AssetPosition();
                p.ReportDate = this.ReportDate;
                p.Code = DBConsts.C_GZB_KMBM_CashDeposit_HQ; //现金代码6位
                p.WindCode = p.Code + ".CD";
                p.Name = "活期存款";
                p.HoldingMarketValue = _GZBList[idxPosition].HoldingMarketValue;
                p.Pctof_Nav = _GZBList[idxPosition].HoldingMVPctofNAV;

                if (idxInterest >= 0)
                    p.AccruedInterestAndDividend = _GZBList[idxInterest].HoldingMarketValue;

                this.CashPositionList.Add(p);
                CashAllPositionAmt += p.HoldingMarketValue;
            }

            //定期存款
            idxPosition = _GZBList.FindIndex(delegate(GZBItem gzb) { return (gzb.ItemDate == this.ReportDate && gzb.ItemCode == DBConsts.C_GZB_KMBM_CashDeposit_DQ); });
            idxInterest = _GZBList.FindIndex(delegate(GZBItem gzb) { return (gzb.ItemDate == this.ReportDate && gzb.ItemCode == DBConsts.C_GZB_KMBM_CashInterest_DQ); });
            if (idxPosition >= 0)
            {
                p = new AssetPosition();
                p.ReportDate = this.ReportDate;
                p.Code = DBConsts.C_GZB_KMBM_CashDeposit_DQ; //现金代码6位
                p.WindCode = p.Code + ".CD";
                p.Name = "定期存款";
                p.HoldingMarketValue = _GZBList[idxPosition].HoldingMarketValue;
                p.Pctof_Nav = _GZBList[idxPosition].HoldingMVPctofNAV;

                if (idxInterest >= 0)
                    p.AccruedInterestAndDividend = _GZBList[idxInterest].HoldingMarketValue;

                this.CashPositionList.Add(p);
                CashAllPositionAmt += p.HoldingMarketValue;
            }

            //协定存款
            idxPosition = _GZBList.FindIndex(delegate(GZBItem gzb) { return (gzb.ItemDate == this.ReportDate && gzb.ItemCode == DBConsts.C_GZB_KMBM_CashDeposit_XD); });
            idxInterest = _GZBList.FindIndex(delegate(GZBItem gzb) { return (gzb.ItemDate == this.ReportDate && gzb.ItemCode == DBConsts.C_GZB_KMBM_CashInterest_XD); });
            if (idxPosition >= 0)
            {
                p = new AssetPosition();
                p.ReportDate = this.ReportDate;
                p.Code = DBConsts.C_GZB_KMBM_CashDeposit_XD; //现金代码6位
                p.WindCode = p.Code + ".CD";
                p.Name = "协定存款";
                p.HoldingMarketValue = _GZBList[idxPosition].HoldingMarketValue;
                p.Pctof_Nav = _GZBList[idxPosition].HoldingMVPctofNAV;

                if (idxInterest >= 0)
                    p.AccruedInterestAndDividend = _GZBList[idxInterest].HoldingMarketValue;

                this.CashPositionList.Add(p);
                CashAllPositionAmt += p.HoldingMarketValue;
            }

            //通知存款
            idxPosition = _GZBList.FindIndex(delegate(GZBItem gzb) { return (gzb.ItemDate == this.ReportDate && gzb.ItemCode == DBConsts.C_GZB_KMBM_CashDeposit_TZ); });
            idxInterest = _GZBList.FindIndex(delegate(GZBItem gzb) { return (gzb.ItemDate == this.ReportDate && gzb.ItemCode == DBConsts.C_GZB_KMBM_CashInterest_TZ); });
            if (idxPosition >= 0)
            {
                p = new AssetPosition();
                p.ReportDate = this.ReportDate;
                p.Code = DBConsts.C_GZB_KMBM_CashDeposit_TZ; //现金代码6位
                p.WindCode = p.Code + ".CD";
                p.Name = "通知存款";
                p.HoldingMarketValue = _GZBList[idxPosition].HoldingMarketValue;
                p.Pctof_Nav = _GZBList[idxPosition].HoldingMVPctofNAV;

                if (idxInterest >= 0)
                    p.AccruedInterestAndDividend = _GZBList[idxInterest].HoldingMarketValue;

                this.CashPositionList.Add(p);
                CashAllPositionAmt += p.HoldingMarketValue;
            }

            //清算备付金（数值上面已经计算）
            if (this.CashLiqReserveAmt > 0)
            {
                idxInterest = _GZBList.FindIndex(delegate(GZBItem gzb) { return (gzb.ItemDate == this.ReportDate && gzb.ItemCode == DBConsts.C_GZB_KMBM_LiqReserveInt); });

                p = new AssetPosition();
                p.ReportDate = this.ReportDate;
                p.Code = DBConsts.C_GZB_KMBM_LiqReserve; //现金代码4位
                p.WindCode = p.Code + ".CD";
                p.Name = "清算备付金";
                p.HoldingMarketValue = this.CashLiqReserveAmt;
                p.Pctof_Nav = this.CashLiqReservePct;

                if (idxInterest >= 0)
                    p.AccruedInterestAndDividend = _GZBList[idxInterest].HoldingMarketValue;

                this.CashPositionList.Add(p);
                CashAllPositionAmt += p.HoldingMarketValue;
            }

            //存出保证金（数值上面已经计算）
            if (this.CashMarginAmt > 0)
            {
                //没有利息
                //idxInterest = _GZBList.FindIndex(delegate(GZBItem gzb) { return (gzb.ItemDate == this.ReportDate && gzb.ItemCode == DBConsts.); });

                p = new AssetPosition();
                p.ReportDate = this.ReportDate;
                p.Code = DBConsts.C_GZB_KMBM_Margin; //现金代码4位
                p.WindCode = p.Code + ".CD";
                p.Name = "存出保证金";
                p.HoldingMarketValue = this.CashMarginAmt;
                p.Pctof_Nav = this.CashMarginPct;

                //没有利息
                //if (idxInterest >= 0)
                //    p.AccruedInterestAndDividend = _GZBList[idxInterest].HoldingMarketValue;

                this.CashPositionList.Add(p);
                CashAllPositionAmt += p.HoldingMarketValue;
            }

            //证券清算款（数值上面已经计算）
            if (this.CashSettlementAmt != 0)
            {
                p = new AssetPosition();
                p.ReportDate = this.ReportDate;
                p.Code = DBConsts.C_GZB_KMBM_Settlement; //现金代码4位
                p.WindCode = p.Code + ".CD";
                p.Name = "证券清算款";
                p.HoldingMarketValue = this.CashSettlementAmt;
                p.Pctof_Nav = this.CashSettlementPct;
                this.CashPositionList.Add(p);
                
                //不计入现金头寸
                //CashAllPositionAmt += p.HoldingMarketValue;
            }
            
            //Sort
            this.CashPositionList.Sort();
        }

        private void loadAssetPositions()
        {
            List<GZBItem> gzbList = _GZBList.FindAll(delegate(GZBItem gzb) { return (gzb.ItemDate == this.ReportDate && gzb.ItemCode.Length >= 14);});

            for (int i = 0; i < gzbList.Count; i++)
            {
                string assetCode4 = gzbList[i].ItemCode.Substring(0, 4);

                switch (assetCode4)
                {
                    case DBConsts.C_GZB_KMBM_Stock:
                    case DBConsts.C_GZB_KMBM_Bond:
                    case DBConsts.C_GZB_KMBM_RevRepo:
                    case DBConsts.C_GZB_KMBM_Fund:
                    case DBConsts.C_GZB_KMBM_Warrant:
                        parsePositionDetail(gzbList[i]);
                        break;
                    default:
                        break;
                }
            }

            //Sort PostionList
            this.EquityPositionList.Sort();
            this.BondPositionList.Sort();
            this.OtherPositionList.Sort();
        }

        private Hashtable _htCodeDict = null;
        private void parsePositionDetail(GZBItem gzbItem)
        {
            #region 定制字母数字转换表
            if (_htCodeDict == null)
            {
                _htCodeDict = new Hashtable();
                _htCodeDict.Add("A", "10");
                _htCodeDict.Add("B", "11");
                _htCodeDict.Add("C", "12");
                _htCodeDict.Add("D", "13");
                _htCodeDict.Add("E", "14");
                _htCodeDict.Add("F", "15");
                _htCodeDict.Add("G", "16");
                _htCodeDict.Add("H", "17");
                _htCodeDict.Add("I", "18");
                _htCodeDict.Add("J", "19");
                _htCodeDict.Add("K", "20");
                _htCodeDict.Add("L", "21");
                _htCodeDict.Add("M", "22");
                _htCodeDict.Add("N", "23");
                _htCodeDict.Add("O", "24");
                _htCodeDict.Add("P", "25");
                _htCodeDict.Add("Q", "26");
                _htCodeDict.Add("R", "27");
                _htCodeDict.Add("S", "28");
                _htCodeDict.Add("T", "29");
                _htCodeDict.Add("U", "30");
                _htCodeDict.Add("V", "31");
                _htCodeDict.Add("W", "32");
                _htCodeDict.Add("X", "33");
                _htCodeDict.Add("Y", "34");
                _htCodeDict.Add("Z", "35");

            }
            #endregion

            //==============================
            //输入项目代码必须14位及以上
            //==============================
            try
            {
                //1102 01 01    600060
                //股票 沪  成本  代码
                string itemCode = gzbItem.ItemCode;
                string itemCostOrInterest = itemCode.Substring(6, 2);
                string assetCode8 = itemCode.Substring(0, 8);
                string assetCode4 = itemCode.Substring(0, 4);
                string tradeCode = gzbItem.ItemCode.Substring(8); //交易所6位,银行间7-9位
                string windCode = tradeCode;

                if (itemCostOrInterest != "01")   //成本01  利息10
                    return;

                #region 查询资产类别
                AssetCategory assetCategory = AssetCategory.Other;
                switch (assetCode4)
                {
                    case DBConsts.C_GZB_KMBM_CashDeposit:
                        assetCategory = AssetCategory.CashDeposit;
                        break;

                    case DBConsts.C_GZB_KMBM_RevRepo:
                        assetCategory = AssetCategory.Repo;
                        break;

                    case DBConsts.C_GZB_KMBM_Stock:
                        assetCategory = AssetCategory.Equity;
                        break;

                    case DBConsts.C_GZB_KMBM_Bond:
                        assetCategory = AssetCategory.Bond;
                        break;

                    case DBConsts.C_GZB_KMBM_Fund:
                        assetCategory = AssetCategory.Fund;
                        break;

                    case DBConsts.C_GZB_KMBM_Warrant:
                        assetCategory = AssetCategory.Warrant;
                        break;

                    default:
                        assetCategory = AssetCategory.Other;
                        break;
                }
                #endregion

                #region 查询资产后缀
                switch (assetCode8)
                {
                    case DBConsts.C_GZB_KMBM_Stock_SH:
                    case DBConsts.C_GZB_KMBM_IPO_SH:
                    case DBConsts.C_GZB_KMBM_SEO_SH:
                    case DBConsts.C_GZB_KMBM_Bond_SH_CB:
                    case DBConsts.C_GZB_KMBM_Bond_SH_UZ:
                    case DBConsts.C_GZB_KMBM_Bond_SH_Corp:
                    case DBConsts.C_GZB_KMBM_Bond_SH_Govn:
                    case DBConsts.C_GZB_KMBM_Repo_SH_PL:
                    case DBConsts.C_GZB_KMBM_Repo_SH_BO:
                    case DBConsts.C_GZB_KMBM_Fund_SH:
                        windCode += ".SH";
                        break;
                    case DBConsts.C_GZB_KMBM_Stock_SZ:
                    case DBConsts.C_GZB_KMBM_Stock_CY:
                    case DBConsts.C_GZB_KMBM_SEO_SZ:
                    case DBConsts.C_GZB_KMBM_IPO_SZ:
                    case DBConsts.C_GZB_KMBM_Bond_SZ_CB:
                    case DBConsts.C_GZB_KMBM_Bond_SZ_Corp:
                    case DBConsts.C_GZB_KMBM_Bond_SZ_Govn:
                    case DBConsts.C_GZB_KMBM_Repo_SZ_PL:
                    case DBConsts.C_GZB_KMBM_Repo_SZ_BO:
                    case DBConsts.C_GZB_KMBM_Fund_SZ:
                    case DBConsts.C_GZB_KMBM_Bond_SZ_STFB:
                        windCode += ".SZ";
                        break;

                    case DBConsts.C_GZB_KMBM_Bond_IB_Govn:
                    case DBConsts.C_GZB_KMBM_Bond_IB_FinB:
                    case DBConsts.C_GZB_KMBM_Bond_IB_Bill:
                    case DBConsts.C_GZB_KMBM_Bond_IB_POFB:
                    case DBConsts.C_GZB_KMBM_Repo_IB_PL:
                    case DBConsts.C_GZB_KMBM_Repo_IB_BO:
                        windCode += ".IB";
                        break;

                    case DBConsts.C_GZB_KMBM_Bond_IB_Corp:
                        windCode = tradeCode;
                        windCode = windCode.Replace("A", "10");
                        windCode = windCode.Replace("B", "11");
                        windCode = windCode.Replace("C", "12");
                        windCode = windCode.Replace("D", "13");
                        windCode = windCode.Replace("E", "14");
                        windCode += ".IB";
                        break;

                    case DBConsts.C_GZB_KMBM_Bond_IB_STFB:
                        //对于04 11 64 004这种债券，代码结构为券种（两位）+发行年份（两位）+发行机构（两位）+发行期数（三位）可以采用以下方式：
                        //  券种两位转换为一位04则为4，年份两位转换为一位11则为B，发行主体不变，期数转换为两位004则为04。
                        //  041164004则为4B6404                        
                        string bondCode = "0" + tradeCode.Substring(0, 1);
                        string yearCode = tradeCode.Substring(1, 1);
                        string issuerCode = tradeCode.Substring(2, 2);
                        string termCode1 = tradeCode.Substring(4, 1);
                        string termCode2 = tradeCode.Substring(5, 1);

                        if (_htCodeDict.Contains(yearCode))
                            yearCode = _htCodeDict[yearCode].ToString();

                        if (_htCodeDict.Contains(termCode1))
                            termCode1 = _htCodeDict[termCode1].ToString();
                        else
                            termCode1 = "0" + termCode1;

                        windCode = bondCode + yearCode + issuerCode + termCode1 + termCode2 + ".IB";
                        break;

                    case DBConsts.C_GZB_KMBM_Fund_OF:
                    case DBConsts.C_GZB_KMBM_Fund_OF_M:
                        windCode += ".OF";
                        break;

                    //case DBConsts.C_GZB_KMBM_SEO_SZ:
                    //case DBConsts.C_GZB_KMBM_SEO_SH:
                    //    //增发项目：忽略之；股份收到后会自动记入股票项目
                    //    return;
                    default:
                        throw new Exception("未知的金融资产：" + gzbItem.ItemCode + "|" + gzbItem.ItemName);
                }
                #endregion

                AssetPosition p = new AssetPosition();
                p.ReportDate = this.ReportDate;
                p.Code = tradeCode;
                p.WindCode = windCode;
                p.Name = gzbItem.ItemName;
                p.Price.TradeDate = gzbItem.ItemDate;
                p.Price.Close = gzbItem.CurrentPrice;
                p.HoldingQuantity = gzbItem.HoldingQuantity;
                p.HoldingCost = gzbItem.HoldingCost;
                p.HoldingMarketValue = gzbItem.HoldingMarketValue;
                p.Pctof_Nav = gzbItem.HoldingMVPctofNAV;
                p.Category = assetCategory;

                switch (assetCategory)
                {
                    case AssetCategory.Equity:
                        p.Pctof_Portfolio = p.HoldingMarketValue / this.EquityPositionAmt;
                        this.EquityPositionList.Add(p);
                        break;

                    case AssetCategory.Bond:
                        p.Pctof_Portfolio = p.HoldingMarketValue / this.BondPositionAmt;
                        
                        //区分转债与纯债
                        switch (assetCode8)
	                    {
                            case DBConsts.C_GZB_KMBM_Bond_SH_CB:
                            case DBConsts.C_GZB_KMBM_Bond_SZ_CB:
                                p.Category = AssetCategory.ConvertableBond;
                                break;

                            default:
                                break;
	                    }

                        this.BondPositionList.Add(p);
                        break;

                    case AssetCategory.Repo:
                    case AssetCategory.Fund:
                    case AssetCategory.Warrant:
                        p.Pctof_Portfolio = p.HoldingMarketValue / (this.RevRepoPositionAmt + this.FundPositionAmt + this.WarrantPositionAmt);
                        this.OtherPositionList.Add(p);
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(DBConsts.C_ErrMsg_ErrorPositionDetail + ex.Message, ex);
            }
        }

        private void loadDividendInterest()
        {
            //==============================
            //输入项目代码必须14位及以上
            //==============================
            try
            {
                List<GZBItem> gzbList = _GZBList.FindAll(delegate(GZBItem gzb) { return (gzb.ItemDate == this.ReportDate && gzb.ItemCode.Length == 14); });

                for (int i = 0; i < gzbList.Count; i++)
                {
                    string category6 = gzbList[i].ItemCode.Substring(0, 6);
                    string code = gzbList[i].ItemCode.Substring(8); //交易所6位,银行间9位
                    int idx = -1;

                    switch (category6)
                    {
                        case DBConsts.C_GZB_KMBM_BondInterest:
                            idx = this.BondPositionList.FindIndex(delegate(AssetPosition p) { return (p.Code == code);});
                            if (idx >= 0)
                            {
                                //债券利息
                                this.BondPositionList[idx].AccruedInterestAndDividend = gzbList[i].HoldingMarketValue;
                            }
                            break;

                        case DBConsts.C_GZB_KMBM_RepoInterest:
                            idx = this.OtherPositionList.FindIndex(delegate(AssetPosition p) { return (p.Code == code);});
                            if (idx >= 0)
                            {
                                //回购利息
                                this.OtherPositionList[idx].AccruedInterestAndDividend = gzbList[i].HoldingMarketValue;
                            }
                            break;

                        case DBConsts.C_GZB_KMBM_Dividend_SH:
                        case DBConsts.C_GZB_KMBM_Dividend_SZ:
                            idx = this.EquityPositionList.FindIndex(delegate(AssetPosition p) { return (p.Code == code);});
                            if (idx >= 0)
                            {
                                //股票股利: 目前股票股利不根据股票代码细分，只有一个总数值
                                this.EquityPositionList[idx].AccruedInterestAndDividend = gzbList[i].HoldingMarketValue;
                            }
                            break;

                        case DBConsts.C_GZB_KMBM_Dividend_OF:
                            idx = this.OtherPositionList.FindIndex(delegate(AssetPosition p) { return (p.Code == code); });
                            if (idx >= 0)
                            {
                                //基金股利
                                this.OtherPositionList[idx].AccruedInterestAndDividend = gzbList[i].HoldingMarketValue;
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }
        #endregion

        #region 其他资产
        public double RevRepoPositionPct = 0;
        public double RevRepoPositionAmt = 0;
        public double TheRepoPositionPct = 0;
        public double TheRepoPositionAmt = 0;
        public double FundPositionPct = 0;
        public double FundPositionAmt = 0;
        public double WarrantPositionPct = 0;
        public double WarrantPositionAmt = 0;
        public List<AssetPosition> OtherPositionList = new List<AssetPosition>();
        #endregion

        #region 股票
        public double EquityPositionPct = 0;
        public double EquityPositionAmt = 0;
        public double EquityDividendAmt = 0;
        public List<AssetPosition> EquityPositionList = new List<AssetPosition>();
        public List<EquityIndustry> EquityIndustryList = new List<EquityIndustry>();

        public void UpdateEquityByMarket(List<AssetPosition> equityList, List<EquityPrice> priceList, List<AssetPosition> capitalList)
        {
            #region Loop Position
            for (int i = 0; i < EquityPositionList.Count; i++)
            {
                string windcode = EquityPositionList[i].WindCode;

                //==========================
                //Industry
                //==========================
                #region Update Industry
                if (equityList != null && equityList.Count > 0)
                {
                    int idxEquity = equityList.FindIndex(delegate(AssetPosition e) { return (e.WindCode == windcode); });
                    if (idxEquity >= 0)
                    {
                        EquityPositionList[i].Name = equityList[idxEquity].Name;
                        EquityPositionList[i].SWIndustryName = equityList[idxEquity].SWIndustryName;
                        EquityPositionList[i].SWIndustryIndex = equityList[idxEquity].SWIndustryIndex;

                        //update industry list
                        int idxIndustry = EquityIndustryList.FindIndex(delegate(EquityIndustry ind) { return ind.SWIndustryIndex == EquityPositionList[i].SWIndustryIndex; });
                        if (idxIndustry >= 0)
                        {
                            EquityIndustryList[idxIndustry].PortfolioWeight += EquityPositionList[i].Pctof_Portfolio;
                            EquityIndustryList[idxIndustry].HoldingMarketValue += EquityPositionList[i].HoldingMarketValue;
                        }
                        else
                        {
                            EquityIndustry industry = new EquityIndustry();
                            industry.SWIndustryName = EquityPositionList[i].SWIndustryName;
                            industry.SWIndustryIndex = EquityPositionList[i].SWIndustryIndex;
                            industry.PortfolioWeight = EquityPositionList[i].Pctof_Portfolio;
                            industry.HoldingMarketValue = EquityPositionList[i].HoldingMarketValue;
                            EquityIndustryList.Add(industry);
                        }
                    }
                }
                #endregion

                //==========================
                //Prices
                //==========================
                #region Update Prices
                if (priceList !=null && priceList.Count > 0)
                {
                    int idxPrice = priceList.FindIndex(delegate(EquityPrice p) { return (p.WindCode == windcode && p.TradeDate == this.ReportDate); });
                    if(idxPrice >=0 )
                    {
                        EquityPositionList[i].TradingVolume = priceList[idxPrice].Volume;
                        EquityPositionList[i].Price.AdjustedClose = priceList[idxPrice].AdjustedClose;
                        EquityPositionList[i].Price.Close = priceList[idxPrice].Close;
                        EquityPositionList[i].Price.AdjustedAverage = priceList[idxPrice].AdjustedAverage;
                        EquityPositionList[i].Price.Average = priceList[idxPrice].Average;
                        if (EquityPositionList[i].TradingVolume > 0)
                            EquityPositionList[i].LiquidityCycle = EquityPositionList[i].HoldingQuantity / EquityPositionList[i].TradingVolume;

                        //调整特殊值
                        if (EquityPositionList[i].Price.Close == 0)
                            EquityPositionList[i].Price.Close = priceList[idxPrice].PreClose;

                        if (EquityPositionList[i].Price.Average == 0)
                            EquityPositionList[i].Price.Average = EquityPositionList[i].Price.Close;

                        if (EquityPositionList[i].Price.AdjustedAverage == 0)
                            EquityPositionList[i].Price.AdjustedAverage = EquityPositionList[i].Price.AdjustedClose;
                    }
                }
                #endregion

                //==========================
                //Capital
                //==========================
                if (capitalList != null && capitalList.Count > 0)
                {
                    int idxCapital = capitalList.FindIndex(delegate(AssetPosition p) { return (p.Code == windcode); });
                    if (idxCapital >= 0)
                        EquityPositionList[i].FreefloatCapital = capitalList[idxCapital].FreefloatCapital;
                }
            }
            #endregion
        }
        #endregion

        #region 债券
        public double BondPositionPct = 0;
        public double BondPositionAmt = 0;
        public double BondInterestAmt = 0;
        public List<AssetPosition> BondPositionList = new List<AssetPosition>();

        public void UpdateBondByMarket(List<AssetPosition> bondList)
        {
            #region Loop Position
            for (int i = 0; i < BondPositionList.Count; i++)
            {
                string windcode = BondPositionList[i].WindCode;

                //==========================
                //到期日
                //==========================
                #region Update MaturityDate
                if (bondList != null && bondList.Count > 0)
                {
                    int idxBond = bondList.FindIndex(delegate(AssetPosition e) { return (e.WindCode == windcode); });
                    if (idxBond >= 0)
                    {
                        BondPositionList[i].BondMaturityDate = bondList[idxBond].BondMaturityDate;
                    }
                }
                #endregion
            }
            #endregion
        }
        #endregion

        #region 现金
        public double CashAllPositionPct = 0;   //现金类头寸占比
        public double CashAllPositionAmt = 0;   //现金类头寸 = 各类存款 + 清算备付金， 不包含各类保证金（无利息收入）
        public List<AssetPosition> CashPositionList = new List<AssetPosition>();
        #endregion

        #region 合计项目
        //利息
        public double CashTotalDividendPct = 0; //全部应收股利占比
        public double CashTotalInterestPct = 0; //全部应收利息占比
        public double CashTotalInterestAmt = 0; //全部应收利息
        
        //资产项目
        public double CashInDepositPct = 0;     //银行存款
        public double CashInDepositAmt = 0;
        public double CashLiqReservePct = 0;    //清算备付金
        public double CashLiqReserveAmt = 0;
        public double CashMarginPct = 0;        //存出保证金
        public double CashMarginAmt = 0;
        public double CashPurchasePct = 0;      //应收申购款
        public double CashSubscribeAmt = 0;

        //负债项目
        public double CashRedemptionPct = -0;   //应付赎回款
        public double CashRedeemAmt = -0;
        public double CashRedempFeePct = -0;    //应付赎回费
        public double CashRedempFeeAmt = -0;
        public double CashManageFeePct = -0;    //应付管理费
        public double CashManageFeeAmt = -0;
        public double CashTrusteeFeePct = -0;   //应付托管费
        public double CashTrusteeFeeAmt = -0;
        public double CashTransCostPct = -0;    //应付交易费用
        public double CashTransCostAmt = -0;
        public double CashSalesFeePct = -0;    //应付销售服务费
        public double CashSalesFeeAmt = -0;
        public double CashOtherPaymentPct = -0; //其他应付款
        public double CashOtherPaymentAmt = -0;
        public double CashPrepaymentPct = -0;   //预提费用
        public double CashPrepaymentAmt = -0;
        public double CashSettlementPct = -0;   //证券清算款
        public double CashSettlementAmt = -0;
        #endregion
        #endregion            
    }
}
