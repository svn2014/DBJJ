using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortfolioEvaluation
{
    public class PerformanceEvaluator
    {
        #region 属性
        public DateTime StartDate = DateTime.Today.AddDays(-2);
        public DateTime EndDate = DateTime.Today.AddDays(-1);
        public string Code = "";
        public string Name = "";
        public Portfolio.PortfolioType Type = Portfolio.PortfolioType.MutualFund;
        public Portfolio.PortfolioCategoryI CategoryI = Portfolio.PortfolioCategoryI.EquityFund;
        public Portfolio.PortfolioCategoryII CategoryII = Portfolio.PortfolioCategoryII.OpenEnd;

        public BenchmarkPortfolio PortfolioBenchmark;
        public Portfolio PerformancePortfolio;
        #endregion

        #region 计算
        private List<Portfolio> _PortfolioList = new List<Portfolio>();
        private List<EquityPrice> _PriceList = new List<EquityPrice>();

        public PerformanceEvaluator(DateTime startDate, DateTime endDate, string code, string name,Portfolio.PortfolioType type, Portfolio.PortfolioCategoryI categoryI, Portfolio.PortfolioCategoryII categoryII)
        {
            if (startDate < endDate)
            {
                this.StartDate = startDate;
                this.EndDate = endDate;
            }
            else
            {
                this.EndDate = startDate;
                this.StartDate = endDate;
            }

            this.Code = code;
            this.Name = name;
            this.Type = type;
            this.CategoryI = categoryI;
            this.CategoryII = categoryII;
        }

        public void BuildPortfolios(List<GZBItem> gzbList, List<AssetPosition> equityList, List<EquityPrice> priceList, List<AssetPosition> capitalList)
        {
            if (gzbList.Count == 0)
                throw new Exception(DBConsts.C_ErrMsg_EmptyFund);

            if(priceList.Count == 0)
                throw new Exception(DBConsts.C_ErrMsg_EmptyPrice);

            this._PriceList = priceList;

            int diff = 0;
            DateTime currDate = this.StartDate.AddDays(diff);
            while (currDate < this.EndDate)
            {
                currDate = this.StartDate.AddDays(diff);

                List<GZBItem> selectedGZB = gzbList.FindAll(delegate(GZBItem gzb) { return (gzb.ItemDate == currDate); });
                List<EquityPrice> selectedPrices = priceList.FindAll(delegate(EquityPrice p) { return (p.TradeDate == currDate); });

                if (selectedGZB.Count > 0)
                {
                    Portfolio p = new Portfolio(this.Code, this.Name, this.Type, this.CategoryI, this.CategoryII);
                    p.UpdatePositionByGZB(selectedGZB);
                    p.UpdateEquityByMarket(equityList, selectedPrices, capitalList);
                    this._PortfolioList.Add(p);
                }

                diff++;
            }

            //Sort by ReportDate Desc
            this._PortfolioList.Sort();

            //根据估值表重新设置报告日期（已降序排列）
            this.EndDate = this._PortfolioList[0].ReportDate;
            this.StartDate = this._PortfolioList[this._PortfolioList.Count - 1].ReportDate;

            //Build Performance Result Portfolio, 0 = last day
            this.PerformancePortfolio = (Portfolio)this._PortfolioList[0].Clone();

            //=======================
            //Calculate Yield
            //=======================
            this.CalculatePerformance();
        }
        
        private void CalculatePerformance()
        {
            //已按时间降序排列
            for (int i = 0; i < this._PortfolioList.Count - 1; i++)
            {
                try
                {
                    #region 现金
                    foreach (AssetPosition pLoop in this._PortfolioList[i].CashPositionList)
                    {
                        //=========================
                        //计算单日利息回报
                        //=========================
                        AssetPosition pPrev = this._PortfolioList[i + 1].CashPositionList.Find(delegate(AssetPosition p) { return p.Code == pLoop.Code; });

                        if (pPrev == null)
                        {
                            pLoop.TotalReturn = pLoop.AccruedInterestAndDividend;
                            pLoop.TotalReturnPct = pLoop.TotalReturn / pLoop.HoldingMarketValue;
                        }
                        else
                        {
                            switch (pLoop.Code)
                            {
                                case DBConsts.C_GZB_KMBM_CashDeposit_DQ: //定期存款
                                case DBConsts.C_GZB_KMBM_CashDeposit_XD: //协议存款
                                case DBConsts.C_GZB_KMBM_CashDeposit_TZ: //通知存款
                                    //定期利息 = 今日应收利息 - 昨日应收利息
                                    //  定期存款：到期后同时支付本金利息，按本金比例分解已到期和未到期项目
                                    if (pLoop.HoldingMarketValue >= pPrev.HoldingMarketValue)
                                        pLoop.TotalReturn = pLoop.AccruedInterestAndDividend - pPrev.AccruedInterestAndDividend;
                                    else
                                    {
                                        //已经有部分到期，折算未到期部分昨天的应收利息，当天数较多，利息率差异较大时，误差较大
                                        double calIntPrev = pPrev.AccruedInterestAndDividend * pLoop.HoldingMarketValue / pPrev.HoldingMarketValue;
                                        pLoop.TotalReturn = pLoop.AccruedInterestAndDividend - calIntPrev;
                                    }

                                    //定期收益率 = 今日利息 / 今日本金
                                    pLoop.TotalReturnPct = pLoop.TotalReturn / pLoop.HoldingMarketValue;
                                    break;

                                case DBConsts.C_GZB_KMBM_CashDeposit_HQ: //活期存款
                                case DBConsts.C_GZB_KMBM_LiqReserve:     //清算备付金
                                    //活期利息 = 今日应收利息 - 昨日应收利息
                                    //  活期存款每季度21日支付利息，不会分批支付，不用分解计算
                                    pLoop.TotalReturn = pLoop.AccruedInterestAndDividend - pPrev.AccruedInterestAndDividend;
                                    if (pLoop.TotalReturn < 0)
                                        pLoop.TotalReturn = pLoop.AccruedInterestAndDividend;

                                    //活期收益率 = 今日利息 / 昨日本金
                                    pLoop.TotalReturnPct = pLoop.TotalReturn / pPrev.HoldingMarketValue;
                                    break;

                                default:
                                    break;
                            }
                        }

                        
                        //=========================
                        //计算期间利息回报
                        //=========================
                        if (i > 0)
                        {
                            AssetPosition pFind = this.PerformancePortfolio.CashPositionList.Find(delegate(AssetPosition p) { return p.WindCode == pLoop.WindCode; });

                            if (pFind == null)
                            {
                                //未找到该证券则添加：说明该证券买入过但已卖出
                                AssetPosition pSold = new AssetPosition();
                                pSold.Code = pLoop.Code;
                                pSold.Name = pLoop.Name;
                                pSold.WindCode = pLoop.WindCode;
                                pSold.TotalReturn = pLoop.TotalReturn;
                                pSold.TotalReturnPct = pLoop.TotalReturnPct;
                                this.PerformancePortfolio.CashPositionList.Add(pSold);
                            }
                            else
                            {
                                //找到了则更新
                                pFind.TotalReturn += pLoop.TotalReturn;
                                pFind.TotalReturnPct = (pFind.TotalReturnPct + 1) * (pLoop.TotalReturnPct + 1) - 1;
                            }
                        }
                    }
                    #endregion
                                      
                    #region 股票，债券，其他资产
                    //======================
                    //计算综合项目单日收益率
                    //======================
                    //基金净值增长
                    this._PortfolioList[i].YieldOnPortfolio = this._PortfolioList[i].UnitCumNAV / this._PortfolioList[i + 1].UnitCumNAV - 1;

                    //股票收益
                    double tmpData=0;
                    this._PortfolioList[i].YieldOnEquity = CalculatePositionYield(
                        this._PortfolioList[i].EquityPositionList,
                        this._PortfolioList[i + 1].EquityPositionList,
                        this._PortfolioList[i].EquityIndustryList,
                        out this._PortfolioList[i].CostOnEquity,
                        out this._PortfolioList[i].ReturnOnEquity,
                        out tmpData
                        );

                    //由于单个股票股利不在估值表中列出，组合收益率需要调整
                    if ((this._PortfolioList[i].EquityDividendAmt - this._PortfolioList[i+1].EquityDividendAmt > 0)
                        && this._PortfolioList[i].CostOnEquity > 0)
                    {
                        this._PortfolioList[i].YieldOnEquity = (this._PortfolioList[i].ReturnOnEquity + this._PortfolioList[i].EquityDividendAmt) / this._PortfolioList[i].CostOnEquity;
                    }

                    //债券收益: 考虑利息收入
                    this._PortfolioList[i].YieldOnBond = CalculatePositionYield(
                        this._PortfolioList[i].BondPositionList,
                        this._PortfolioList[i + 1].BondPositionList,
                        null,
                        out this._PortfolioList[i].CostOnBond,
                        out this._PortfolioList[i].ReturnOnBond,
                        out this._PortfolioList[i].NetReturnOnBond
                        );
                    //债券收益: 不考虑利息收入
                    if (this._PortfolioList[i].CostOnBond != 0)
                        this._PortfolioList[i].NetYieldOnBond = this._PortfolioList[i].NetReturnOnBond / this._PortfolioList[i].CostOnBond;
                    else
                        this._PortfolioList[i].NetYieldOnBond = 0;

                    //其他收益：基金，权证，回购
                    this._PortfolioList[i].YieldOnOther = CalculatePositionYield(
                        this._PortfolioList[i].OtherPositionList,
                        this._PortfolioList[i + 1].OtherPositionList,
                        null,
                        out this._PortfolioList[i].CostOnOther,
                        out this._PortfolioList[i].ReturnOnOther,
                        out tmpData
                        );

                    #region 行业 - 归因分析
                    foreach (EquityIndustry pLoop in this._PortfolioList[i].EquityIndustryList)
                    {
                        //计算收益率: 不能写在这里，因为引用的关系，_PortfolioList[0] 和 PerformancePortfolio 中的值同时改变了
                        //pLoop.ReturnPct = pLoop.ReturnAmt / pLoop.CostAmt;

                        //更新
                        EquityIndustry pFind = this.PerformancePortfolio.EquityIndustryList.Find(delegate(EquityIndustry p) { return p.SWIndustryIndex == pLoop.SWIndustryIndex; });

                        if (pFind == null)
                        {
                            //未找到该证券则添加：说明该证券买入过但已卖出
                            EquityIndustry pSold = new EquityIndustry();
                            pSold.SWIndustryIndex = pLoop.SWIndustryIndex;
                            pSold.SWIndustryName = pLoop.SWIndustryName;
                            pSold.ReturnAmt = pLoop.ReturnAmt;
                            pSold.CostAmt = pLoop.CostAmt;
                            pSold.ReturnPct = pLoop.ReturnAmt / pLoop.CostAmt;
                            this.PerformancePortfolio.EquityIndustryList.Add(pSold);
                        }
                        else
                        {
                            //找到了则更新
                            pFind.ReturnAmt = pLoop.ReturnAmt;
                            pFind.CostAmt = pLoop.CostAmt;
                            pFind.ReturnPct = (pFind.ReturnPct + 1) * (pLoop.ReturnAmt / pLoop.CostAmt + 1) - 1;
                        }
                    }
                    #endregion

                    //======================
                    //计算单个证券期间收益率
                    //======================        
                    if (i > 0)
                    {
                        #region 股票
                        foreach (AssetPosition pLoop in this._PortfolioList[i].EquityPositionList)
                        {
                            AssetPosition pFind = this.PerformancePortfolio.EquityPositionList.Find(delegate(AssetPosition p) { return p.WindCode == pLoop.WindCode; });

                            if (pFind == null)
                            {
                                //未找到该证券则添加：说明该证券买入过但已卖出
                                AssetPosition pSold = new AssetPosition();
                                pSold.Code = pLoop.Code;
                                pSold.Name = pLoop.Name;
                                pSold.WindCode = pLoop.WindCode;
                                pSold.TotalReturn = pLoop.TotalReturn;
                                pSold.TotalReturnPct = pLoop.TotalReturnPct;
                                this.PerformancePortfolio.EquityPositionList.Add(pSold);
                            }
                            else
                            {
                                //找到了则更新
                                pFind.TotalReturn += pLoop.TotalReturn;
                                pFind.TotalReturnPct = (pFind.TotalReturnPct + 1) * (pLoop.TotalReturnPct + 1) - 1;
                            }
                        }
                        #endregion

                        #region 债券
                        foreach (AssetPosition pLoop in this._PortfolioList[i].BondPositionList)
                        {
                            AssetPosition pFind = this.PerformancePortfolio.BondPositionList.Find(delegate(AssetPosition p) { return p.WindCode == pLoop.WindCode; });

                            if (pFind == null)
                            {
                                //未找到该证券则添加：说明该证券买入过但已卖出
                                AssetPosition pSold = new AssetPosition();
                                pSold.Code = pLoop.Code;
                                pSold.Name = pLoop.Name;
                                pSold.WindCode = pLoop.WindCode;
                                pSold.Category = pLoop.Category;
                                //全价
                                pSold.TotalReturn = pLoop.TotalReturn;
                                pSold.TotalReturnPct = pLoop.TotalReturnPct;
                                //净价
                                pSold.TotalNetReturn = pLoop.TotalNetReturn;
                                pSold.TotalNetReturnPct = pLoop.TotalNetReturnPct;
                                this.PerformancePortfolio.BondPositionList.Add(pSold);
                            }
                            else
                            {
                                //全价收益率
                                pFind.TotalReturn += pLoop.TotalReturn;
                                pFind.TotalReturnPct = (pFind.TotalReturnPct + 1) * (pLoop.TotalReturnPct + 1) - 1;
                                //净价收益率
                                pFind.TotalNetReturn += pLoop.TotalNetReturn;
                                pFind.TotalNetReturnPct = (pFind.TotalNetReturnPct + 1) * (pLoop.TotalNetReturnPct + 1) - 1;
                            }
                        }
                        #endregion

                        #region 基金，权证，回购
                        foreach (AssetPosition pLoop in this._PortfolioList[i].OtherPositionList)
                        {
                            AssetPosition pFind = this.PerformancePortfolio.OtherPositionList.Find(delegate(AssetPosition p) { return p.WindCode == pLoop.WindCode; });

                            if (pFind == null)
                            {
                                //未找到该证券则添加：说明该证券买入过但已卖出
                                AssetPosition pSold = new AssetPosition();
                                pSold.Code = pLoop.Code;
                                pSold.Name = pLoop.Name;
                                pSold.WindCode = pLoop.WindCode;
                                pSold.TotalReturn = pLoop.TotalReturn;
                                pSold.TotalReturnPct = pLoop.TotalReturnPct;
                                this.PerformancePortfolio.OtherPositionList.Add(pSold);
                            }
                            else
                            {
                                //找到了则更新
                                pFind.TotalReturn += pLoop.TotalReturn;
                                pFind.TotalReturnPct = (pFind.TotalReturnPct + 1) * (pLoop.TotalReturnPct + 1) - 1;
                            }
                        }
                        #endregion
                    }

                    #region 现金收益
                    double cashInterest = 0, cashBalance = 0;
                    foreach (AssetPosition pLoop in this._PortfolioList[i].CashPositionList)
                    {
                        //全部现金项目的利息总和
                        cashInterest += pLoop.TotalReturn;

                        //全部现金项目的本金总和
                        switch (pLoop.Code)
                        {
                            case DBConsts.C_GZB_KMBM_CashDeposit_DQ: //定期存款
                            case DBConsts.C_GZB_KMBM_CashDeposit_XD: //协议存款
                            case DBConsts.C_GZB_KMBM_CashDeposit_TZ: //通知存款
                                //定期存款：本期本金
                                cashBalance += pLoop.HoldingMarketValue;
                                break;

                            default:
                                AssetPosition pPrev = this._PortfolioList[i + 1].CashPositionList.Find(delegate(AssetPosition p) { return p.Code == pLoop.Code; });
                                if (pPrev == null)
                                    cashBalance += pLoop.HoldingMarketValue;
                                else
                                    //活期存款：上期本金
                                    cashBalance += pPrev.HoldingMarketValue;
                                break;
                        }
                    }
                    this._PortfolioList[i].YieldOnCash = cashInterest / cashBalance;
                    #endregion

                    //======================
                    //计算综合项目期间收益率
                    //======================
                    this.PerformancePortfolio.YieldOnEquity = (this.PerformancePortfolio.YieldOnEquity + 1) * (this._PortfolioList[i].YieldOnEquity + 1) - 1;
                    this.PerformancePortfolio.YieldOnBond = (this.PerformancePortfolio.YieldOnBond + 1) * (this._PortfolioList[i].YieldOnBond + 1) - 1;
                    this.PerformancePortfolio.YieldOnOther = (this.PerformancePortfolio.YieldOnOther + 1) * (this._PortfolioList[i].YieldOnOther + 1) - 1;
                    this.PerformancePortfolio.YieldOnCash = (this.PerformancePortfolio.YieldOnCash + 1) * (this._PortfolioList[i].YieldOnCash + 1) - 1;
                    #endregion

                    #region 申购赎回
                    SubscribeRedemp newSR = new SubscribeRedemp();
                    newSR.ReportDate = this._PortfolioList[i].ReportDate;
                    newSR.CurrentBalance = this._PortfolioList[i].PIC;
                    newSR.UnitCumNAV = this._PortfolioList[i].UnitCumNAV;
                    newSR.SubscribeAmount = this._PortfolioList[i].CashSubscribeAmt;
                    newSR.RedeemAmount = this._PortfolioList[i].CashRedeemAmt;
                    newSR.EquityWeight = this._PortfolioList[i].EquityPositionPct;
                    newSR.BondWeight = this._PortfolioList[i].BondPositionPct;
                    newSR.YieldOnEquity = this._PortfolioList[i].YieldOnEquity;
                    newSR.NetYieldOnBond = this._PortfolioList[i].NetYieldOnBond;
                    this.SubscribeRedempList.Add(newSR);

                    //===================================================
                    //以下试图计算单日赎回总额
                    //  但如果选择区间之前已经有赎回（连续交易日赎回），则数据误差很大
                    //  故放弃
                    //===================================================
                    ////已按时间降序排列，此处计算需要升序
                    //int idxRev = this._PortfolioList.Count - i - 1;

                    //SubscribeRedemp newSR = new SubscribeRedemp();
                    //newSR.ReportDate = this._PortfolioList[idxRev].ReportDate;
                    //newSR.CurrentBalance = this._PortfolioList[idxRev].PIC;
                    //newSR.UnitCumNAV = this._PortfolioList[idxRev].UnitCumNAV;
                    //newSR.SubscribeAmount = this._PortfolioList[idxRev].CashSubscribeAmt;

                    //if (i == 0)
                    //    newSR.RedeemAmount = this._PortfolioList[idxRev].CashRedeemAmt;
                    //else
                    //    newSR.RedeemAmount = this._PortfolioList[idxRev].CashRedeemAmt - this.SubscribeRedempList[this.SubscribeRedempList.Count - 1].RedeemAmount;

                    //this.SubscribeRedempList.Add(newSR);
                    #endregion
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            this.PerformancePortfolio.YieldOnPortfolio = this._PortfolioList[0].UnitCumNAV / this._PortfolioList[this._PortfolioList.Count - 1].UnitCumNAV - 1;
        }

        private double CalculatePositionYield(List<AssetPosition> positionListCurrPeriod, List<AssetPosition> positionListPrevPeriod, List<EquityIndustry> industryList, out double totoalCost, out double totalReturn, out double totalNetReturn)
        {
            //计算单日收益率
            try
            {
                if ((positionListCurrPeriod == null || positionListCurrPeriod.Count == 0) && (positionListPrevPeriod == null || positionListPrevPeriod.Count == 0))
                {
                    totoalCost = 0;
                    totalReturn = 0;
                    totalNetReturn = 0;
                    return 0;
                }

                double totalPositionReturn = 0, totalPositionNetReturn = 0, totalPositionCost = 0, currPositionCost = 0, dividendAndInterest = 0;
                
                DateTime currDate;
                if(positionListCurrPeriod !=null && positionListCurrPeriod.Count > 0)
                    currDate = positionListCurrPeriod[0].ReportDate;
                else
                    currDate = positionListPrevPeriod[0].ReportDate;

                #region 买入、增持、减持
                foreach (AssetPosition positionCurr in positionListCurrPeriod)
                {
                    //==========================
                    //股票: 当前股票是否出现在上期持仓中
                    //==========================
                    AssetPosition positionPrev = positionListPrevPeriod.Find(delegate(AssetPosition p) { return p.WindCode == positionCurr.WindCode; });

                    if (positionPrev != null)
                    {
                        if (positionCurr.HoldingQuantity > positionPrev.HoldingQuantity)
                        {
                            //增持(新买入股份会分摊成本均价) 
                            //  收益 = (今日市值  / 今日股份数 * 昨日股份数 - 昨日市值) 
                            //      + (今日市值  / 今日股份数 - 今日买入价) * 今日买入股份
                            //  成本 = 昨日市值 + 成本增加 = 昨日市值 + 今日买入价 * 今日买入股份
                            //  利息 = (昨日对应部分今日应计利息 - 昨日应计利息) + 0[今日新买部分]
                            double buyPx = (positionCurr.HoldingCost - positionPrev.HoldingCost) / (positionCurr.HoldingQuantity - positionPrev.HoldingQuantity);

                            positionCurr.RealizedReturn = 0;
                            positionCurr.UnrealizedReturn = (positionCurr.HoldingMarketValue / positionCurr.HoldingQuantity * positionPrev.HoldingQuantity - positionPrev.HoldingMarketValue)
                                                            + (positionCurr.HoldingMarketValue / positionCurr.HoldingQuantity - buyPx) * (positionCurr.HoldingQuantity - positionPrev.HoldingQuantity);
                            dividendAndInterest = positionCurr.AccruedInterestAndDividend / positionCurr.HoldingQuantity * positionPrev.HoldingQuantity - positionPrev.AccruedInterestAndDividend;
                            currPositionCost = positionPrev.HoldingMarketValue + (positionCurr.HoldingCost - positionPrev.HoldingCost);

                            //==========================
                            //新增交易:增持
                            //==========================
                            Transaction TS = new Transaction();
                            TS.TransactionDate = positionCurr.ReportDate;
                            TS.TransactionType = Transaction.TradingType.BuyMore;
                            TS.Code = positionCurr.WindCode;
                            TS.Name = positionCurr.Name;
                            TS.TradingPrice = buyPx;
                            TS.MarketPrice = positionCurr.Price.Close;
                            TS.IndustryName = positionCurr.SWIndustryName;
                            TS.TradingVolume = positionCurr.HoldingQuantity - positionPrev.HoldingQuantity;
                            TS.Pctof_Nav = (1 - positionPrev.HoldingMarketValue / positionCurr.HoldingMarketValue) * positionCurr.Pctof_Nav;
                            TS.Pctof_Portfolio = (1 - positionPrev.HoldingMarketValue / positionCurr.HoldingMarketValue) * positionCurr.Pctof_Portfolio;
                            TransactionList.Add(TS);
                        }
                        else if (positionCurr.HoldingQuantity < positionPrev.HoldingQuantity)
                        {
                            //减持(卖出股份形成实现收益计入银行存款)
                            //  收益 = (今日均价 - 昨日收盘价) * 今日卖出股份                //实现
                            //       + (今日市值 - 昨日市值 / 昨日股份数 * 今日股份数)       //浮动
                            //  成本 = 昨日市值
                            //  利息 = (今日应计利息 - 未出售部分昨日应计利息)
                            //       + 0(今日卖出部分)
                            
                            //如果当期没有价格,则按上期市值计算
                            double avgPx = positionPrev.HoldingMarketValue / positionPrev.HoldingQuantity;
                            int idxPrice = this._PriceList.FindIndex(delegate(EquityPrice p) { return (p.WindCode == positionCurr.WindCode && p.TradeDate == currDate); });
                            if (idxPrice >= 0 && this._PriceList[idxPrice].Average > 0)
                            {
                                avgPx = this._PriceList[idxPrice].Average;
                            }

                            positionCurr.RealizedReturn = (avgPx - positionPrev.HoldingMarketValue / positionPrev.HoldingQuantity)
                                                        * (positionPrev.HoldingQuantity - positionCurr.HoldingQuantity);
                            positionCurr.UnrealizedReturn = positionCurr.HoldingMarketValue - positionPrev.HoldingMarketValue / positionPrev.HoldingQuantity * positionCurr.HoldingQuantity;
                            dividendAndInterest = positionCurr.AccruedInterestAndDividend - positionPrev.AccruedInterestAndDividend /positionPrev.HoldingQuantity * positionCurr.HoldingQuantity;
                            currPositionCost = positionPrev.HoldingMarketValue;

                            //==========================
                            //新增交易: 减持
                            //==========================
                            Transaction TS = new Transaction();
                            TS.TransactionDate = positionCurr.ReportDate;
                            TS.TransactionType = Transaction.TradingType.SellSome;
                            TS.Code = positionCurr.WindCode;
                            TS.Name = positionCurr.Name;
                            TS.TradingPrice = avgPx;                    //假设以均价成交
                            TS.MarketPrice = positionCurr.Price.Close;
                            TS.IndustryName = positionCurr.SWIndustryName;
                            TS.TradingVolume = positionPrev.HoldingQuantity - positionCurr.HoldingQuantity;
                            TS.Pctof_Nav = (positionPrev.HoldingMarketValue / positionCurr.HoldingMarketValue - 1) * positionCurr.Pctof_Nav;
                            TS.Pctof_Portfolio = (positionPrev.HoldingMarketValue / positionCurr.HoldingMarketValue - 1) * positionCurr.Pctof_Portfolio;
                            TransactionList.Add(TS);
                        }
                        else
                        {
                            //无变动
                            //  收益 = 今日市值 - 昨日市值
                            //  成本 = 昨日市值
                            //  利息 = 今日应计利息 - 昨日应计利息
                            positionCurr.RealizedReturn = 0;
                            positionCurr.UnrealizedReturn = positionCurr.HoldingMarketValue - positionPrev.HoldingMarketValue;
                            dividendAndInterest = positionCurr.AccruedInterestAndDividend - positionPrev.AccruedInterestAndDividend;
                            currPositionCost = positionPrev.HoldingMarketValue;
                        }
                    }
                    else
                    {
                        //新买入
                        //  收益 = 今日市值 - 今日成本
                        //  成本 = 今日成本
                        //  利息 = 0[理论上计头不计尾，但由于无法获得数据，设为0]                        
                        dividendAndInterest = 0;

                        //债券的会计成本 = 交易成本 + 应收利息 * 20%
                        if (positionCurr.Category == AssetCategory.Bond || positionCurr.Category == AssetCategory.ConvertableBond)
                            //估值表中的利息为税后利息，所以要先恢复到税前再计算利息税
                            currPositionCost = positionCurr.HoldingCost - positionCurr.AccruedInterestAndDividend / 0.8 * 0.2;
                        else
                            currPositionCost = positionCurr.HoldingCost;

                        positionCurr.RealizedReturn = 0;
                        positionCurr.UnrealizedReturn = positionCurr.HoldingMarketValue - currPositionCost;

                        //==========================
                        //新增交易：买入
                        //==========================
                        Transaction TS = new Transaction();
                        TS.TransactionDate = positionCurr.ReportDate;
                        TS.TransactionType = Transaction.TradingType.BuyNew;
                        TS.Code = positionCurr.WindCode;
                        TS.Name = positionCurr.Name;
                        TS.TradingPrice = (positionCurr.HoldingQuantity != 0) ? currPositionCost / positionCurr.HoldingQuantity : 0;
                        TS.MarketPrice = positionCurr.Price.Close;
                        TS.IndustryName = positionCurr.SWIndustryName;
                        TS.TradingVolume = positionCurr.HoldingQuantity;
                        TS.Pctof_Nav = positionCurr.Pctof_Nav;
                        TS.Pctof_Portfolio = positionCurr.Pctof_Portfolio;
                        TransactionList.Add(TS);
                    }

                    //若(今日股利券息 - 昨日股利券息) < 0 说明前期应收利息已经转入现金
                    if (dividendAndInterest < 0)
                        dividendAndInterest = positionCurr.AccruedInterestAndDividend;

                    //总收益 = 实现收益 + 浮动收益 + (今日股利券息 - 昨日股利券息)
                    positionCurr.TotalReturn = positionCurr.RealizedReturn      
                                            + positionCurr.UnrealizedReturn
                                            + dividendAndInterest;
                    
                    positionCurr.TotalNetReturn = positionCurr.RealizedReturn
                                            + positionCurr.UnrealizedReturn;

                    positionCurr.TotalReturnPct = positionCurr.TotalReturn / currPositionCost;
                    positionCurr.TotalNetReturnPct = positionCurr.TotalNetReturn / currPositionCost;

                    //==========================
                    //行业
                    //==========================
                    if (industryList != null)
                    {
                        EquityIndustry selectedIndustry = industryList.Find(delegate(EquityIndustry ind) { return (ind.SWIndustryIndex == positionCurr.SWIndustryIndex); });

                        if (selectedIndustry != null)
                        {
                            selectedIndustry.ReturnAmt += positionCurr.TotalReturn;
                            selectedIndustry.CostAmt += currPositionCost;
                        }
                    }

                    //==========================
                    //总和
                    //==========================
                    totalPositionNetReturn += positionCurr.TotalNetReturn;
                    totalPositionReturn += positionCurr.TotalReturn;
                    totalPositionCost += currPositionCost;
                }
                #endregion

                #region 卖出
                foreach (AssetPosition positionPrev in positionListPrevPeriod)
                {
                    //==========================
                    //股票：上前股票是否出现在本期持仓中
                    //==========================
                    AssetPosition positionCurr = positionListCurrPeriod.Find(delegate(AssetPosition p) { return p.Code == positionPrev.Code; });

                    if (positionCurr == null)
                    {
                        //全部卖出
                        AssetPosition newPosition = new AssetPosition();
                        newPosition.Code = positionPrev.Code;
                        newPosition.WindCode = positionPrev.WindCode;
                        newPosition.Name = positionPrev.Name;
                        newPosition.FreefloatCapital = positionPrev.FreefloatCapital;
                        //newPosition.ReportDate = positionSub.ReportDate;
                        newPosition.SWIndustryName = positionPrev.SWIndustryName;
                        newPosition.SWIndustryIndex = positionPrev.SWIndustryIndex;
                        newPosition.HoldingCost = positionPrev.HoldingCost;
                        newPosition.Category = positionPrev.Category;

                        //如果当期没有价格,则按上期市值计算
                        double avgSelling = positionPrev.HoldingMarketValue;
                        int idxPrice = this._PriceList.FindIndex(delegate(EquityPrice p) { return (p.WindCode == positionPrev.WindCode && p.TradeDate == currDate); });
                        if (idxPrice >= 0 && this._PriceList[idxPrice].Average>0)
                        {
                            avgSelling = this._PriceList[idxPrice].Average * positionPrev.HoldingQuantity;
                        }

                        newPosition.RealizedReturn = avgSelling - positionPrev.HoldingMarketValue;
                        newPosition.UnrealizedReturn = 0;
                        newPosition.TotalReturn = newPosition.RealizedReturn + newPosition.UnrealizedReturn;
                        newPosition.TotalReturnPct = newPosition.TotalReturn / positionPrev.HoldingCost;
                        newPosition.TotalNetReturn = newPosition.TotalReturn;
                        newPosition.TotalNetReturnPct = newPosition.TotalReturnPct;
                        positionListCurrPeriod.Add(newPosition);

                        //==========================
                        //行业
                        //==========================
                        if (industryList != null)
                        {
                            EquityIndustry selectedIndustry = industryList.Find(delegate(EquityIndustry ind) { return (ind.SWIndustryIndex == newPosition.SWIndustryIndex); });

                            if (selectedIndustry != null)
                            {
                                selectedIndustry.ReturnAmt += newPosition.TotalReturn;
                                selectedIndustry.CostAmt += positionPrev.HoldingCost;
                            }
                            else
                            {
                                EquityIndustry newIndustry = new EquityIndustry();
                                newIndustry.SWIndustryIndex = newPosition.SWIndustryIndex;
                                newIndustry.SWIndustryName = newPosition.SWIndustryName;
                                newIndustry.PortfolioWeight = newPosition.Pctof_Portfolio;
                                //newIndustry.Return = newPosition.TotalReturnPct * newPosition.Weight;
                                newIndustry.ReturnAmt += newPosition.TotalReturn;
                                newIndustry.CostAmt += positionPrev.HoldingCost;
                                industryList.Add(newIndustry);
                            }
                        }

                        totalPositionNetReturn += newPosition.TotalNetReturn;
                        totalPositionReturn += newPosition.TotalReturn;
                        totalPositionCost += newPosition.HoldingCost;

                        //==========================
                        //新增交易：卖出
                        //==========================
                        Transaction TS = new Transaction();
                        TS.TransactionDate = positionListCurrPeriod[0].ReportDate;
                        TS.TransactionType = Transaction.TradingType.SellAll;
                        TS.Code = positionPrev.WindCode;
                        TS.Name = positionPrev.Name;
                        TS.TradingPrice = avgSelling / positionPrev.HoldingQuantity;    //假设以均价成交
                        TS.MarketPrice = (idxPrice >= 0 && this._PriceList[idxPrice].Close > 0) ? this._PriceList[idxPrice].Close : TS.TradingPrice;
                        TS.IndustryName = positionPrev.SWIndustryName;
                        TS.TradingVolume = positionPrev.HoldingQuantity;
                        TS.Pctof_Nav = positionPrev.Pctof_Nav;
                        TS.Pctof_Portfolio = positionPrev.Pctof_Portfolio;
                        TransactionList.Add(TS);
                    }
                }
                #endregion

                //Output
                totoalCost = totalPositionCost;
                totalReturn = totalPositionReturn;
                totalNetReturn = totalPositionNetReturn;
                return totalPositionReturn / totalPositionCost;
            }
            catch (Exception ex)
            {                
                throw ex;
            }            
        }

        public List<AssetPosition> GetBondPositions(out string bondcodelist)
        {
            bondcodelist="";

            if (this.PerformancePortfolio.BondPositionList == null || this.PerformancePortfolio.BondPositionList.Count == 0)
                return null;

            foreach (AssetPosition p in this.PerformancePortfolio.BondPositionList)
	        {
		        bondcodelist += p.WindCode + ",";
	        }

            bondcodelist = bondcodelist.Substring(0, bondcodelist.Length - 1);
            return this.PerformancePortfolio.BondPositionList;
        }

        public void UpdateBondPositions(List<AssetPosition> bondList)
        {
            if (this.PerformancePortfolio.BondPositionList == null || this.PerformancePortfolio.BondPositionList.Count == 0)
                return;

            foreach (AssetPosition p in this.PerformancePortfolio.BondPositionList)
            {
                AssetPosition foundBond = bondList.Find(delegate(AssetPosition e) { return (e.WindCode == p.WindCode); });

                if (foundBond != null)
                {
                    p.BondMaturityDate = foundBond.BondMaturityDate;
                }
            }
        }
        #endregion

        #region 评估
        public List<AssetEvaluation> IndustryPerformanceList = new List<AssetEvaluation>();
        public List<AssetEvaluation> EquityPerformanceList = new List<AssetEvaluation>();
        public List<AssetEvaluation> BondPerformanceList = new List<AssetEvaluation>();
        public List<AssetEvaluation> CashPerformanceList = new List<AssetEvaluation>();
        public List<AssetEvaluation> OtherPerformanceList = new List<AssetEvaluation>();
        public List<Transaction> TransactionList = new List<Transaction>();
        public List<SubscribeRedemp> SubscribeRedempList = new List<SubscribeRedemp>();

        public void Evaluate(BenchmarkPortfolio benchmark, List<EquityIndustry> industryList)
        {
            if (benchmark == null || benchmark.GetBenchmarkPosition().Count == 0)
                throw new Exception(DBConsts.C_ErrMsg_NoBenchmark);

            if(_PortfolioList == null || _PortfolioList.Count ==0)
                throw new Exception(DBConsts.C_ErrMsg_EmptyFund);

            if (industryList == null || industryList.Count == 0)
                throw new Exception(DBConsts.C_ErrMsg_NoIndustry);

            //Calculation
            this.PortfolioBenchmark = benchmark;
            this.IndustryPerformanceList.Clear();

            #region Industry Calculation
            //Cash
            AssetEvaluation cashItem = new AssetEvaluation();
            cashItem.Code = "_CASH_";
            cashItem.SWIndustryIndex = "_CASH_";
            cashItem.Name = "_CASH_";
            cashItem.OrderId = 1;
            cashItem.EconomicSector = DBConsts.C_Text_CashPortfolio;
            cashItem.PortfolioWeight = _PortfolioList[0].CashAllPositionPct;    //所有现金头寸
            cashItem.HoldingMarketValue = _PortfolioList[0].CashAllPositionAmt;
            cashItem.SectorBenchmarkWeight = 0;
            cashItem.PortfolioReturnPct = this.PerformancePortfolio.YieldOnCash;
            cashItem.PortfolioReturn = this.PerformancePortfolio.ReturnOnCash;
            cashItem.SectorBenchmarkReturnPct = 0;
            IndustryPerformanceList.Add(cashItem);

            if (benchmark.HasIndexComponent())
            {
                //Industry
                foreach (EquityIndustry industry in industryList)
                {
                    AssetEvaluation equityIndustryItem = new AssetEvaluation();
                    equityIndustryItem.SWIndustryIndex = industry.SWIndustryIndex;
                    equityIndustryItem.Code = industry.SWIndustryIndex;
                    equityIndustryItem.Name = industry.SWIndustryName;
                    equityIndustryItem.EconomicSector = industry.SWIndustryName;

                    //update by benchmark
                    EquityIndustry benchmarkIndustry = benchmark.GetBenchmarkIndustry().Find(delegate(EquityIndustry ind) { return (ind.SWIndustryIndex == industry.SWIndustryIndex); });

                    if (benchmarkIndustry != null)
                    {
                        equityIndustryItem.SectorBenchmarkWeight = benchmarkIndustry.PortfolioWeight;
                        equityIndustryItem.SectorBenchmarkReturnPct = benchmarkIndustry.ReturnPct;
                    }

                    //update by portfolio
                    EquityIndustry portfolioIndustry = this.PerformancePortfolio.EquityIndustryList.Find(delegate(EquityIndustry ind) { return (ind.SWIndustryIndex == industry.SWIndustryIndex); });

                    if (portfolioIndustry != null)
                    {
                        equityIndustryItem.PortfolioWeight = portfolioIndustry.PortfolioWeight;
                        equityIndustryItem.PortfolioReturnPct = portfolioIndustry.ReturnPct;
                        equityIndustryItem.HoldingMarketValue = portfolioIndustry.HoldingMarketValue;
                    }

                    //Performance Attribution
                    //投资组合经理因素: 行业配置
                    equityIndustryItem.PureSectorAllocation = (equityIndustryItem.PortfolioWeight - equityIndustryItem.SectorBenchmarkWeight)
                                                    * (equityIndustryItem.SectorBenchmarkReturnPct - benchmark.GetBenchmarkReturn());

                    //研究员因素: 个股选择
                    equityIndustryItem.WithinSectorSelection = equityIndustryItem.SectorBenchmarkWeight
                                                    * (equityIndustryItem.PortfolioReturnPct - equityIndustryItem.SectorBenchmarkReturnPct);

                    //中间因素
                    equityIndustryItem.AllocationSelectionInteraction = (equityIndustryItem.PortfolioWeight - equityIndustryItem.SectorBenchmarkWeight)
                                                    * (equityIndustryItem.PortfolioReturnPct - equityIndustryItem.SectorBenchmarkReturnPct);

                    //增加值
                    equityIndustryItem.TotalValueAdded = equityIndustryItem.PortfolioWeight * equityIndustryItem.PortfolioReturnPct
                                                    - equityIndustryItem.SectorBenchmarkWeight * equityIndustryItem.SectorBenchmarkReturnPct;

                    IndustryPerformanceList.Add(equityIndustryItem);
                }
            }

            //Total Summary
            AssetEvaluation equityItem = new AssetEvaluation();
            equityItem.Code = "_EQUITY_";
            equityItem.SWIndustryIndex = "_EQUITY_";
            equityItem.Name = "_EQUITY_";
            equityItem.OrderId = 2;
            equityItem.EconomicSector = DBConsts.C_Text_EquityPortfolio;
            equityItem.HoldingMarketValue = _PortfolioList[0].EquityPositionAmt;
            equityItem.PortfolioWeight = _PortfolioList[0].EquityPositionPct;
            equityItem.PortfolioReturnPct = this.PerformancePortfolio.YieldOnEquity;
            equityItem.PortfolioReturn = this.PerformancePortfolio.ReturnOnEquity; 
            equityItem.SectorBenchmarkReturnPct = benchmark.GetBenchmarkReturn();
            equityItem.TotalValueAdded = equityItem.PortfolioReturnPct - equityItem.SectorBenchmarkReturnPct;
            IndustryPerformanceList.Add(equityItem);

            AssetEvaluation bondItem = new AssetEvaluation();
            bondItem.Code = "_BOND_";
            bondItem.SWIndustryIndex = "_BOND_";
            bondItem.Name = "_BOND_";
            bondItem.OrderId = 3;
            bondItem.EconomicSector = DBConsts.C_Text_BondPortfolio;
            bondItem.HoldingMarketValue = _PortfolioList[0].BondPositionAmt;
            bondItem.PortfolioWeight = _PortfolioList[0].BondPositionPct;
            bondItem.PortfolioReturnPct = this.PerformancePortfolio.YieldOnBond;
            bondItem.PortfolioReturn = this.PerformancePortfolio.ReturnOnBond;
            bondItem.PortfolioNetReturn = this.PerformancePortfolio.NetReturnOnBond;
            bondItem.SectorBenchmarkReturnPct = 0;
            IndustryPerformanceList.Add(bondItem);
            #endregion

            #region Equity Position Calculation
            List<AssetPosition> bmpositionList = this.PortfolioBenchmark.GetBenchmarkPosition();
            foreach (AssetPosition position in PerformancePortfolio.EquityPositionList)
            {
                AssetPosition bmPosition = bmpositionList.Find(delegate(AssetPosition p) { return p.WindCode == position.WindCode; });

                AssetEvaluation positionItem = new AssetEvaluation();
                positionItem.Code = position.WindCode;
                positionItem.Name = position.Name;
                positionItem.pctofNAV = position.Pctof_Nav;
                positionItem.PortfolioWeight = position.Pctof_Portfolio;
                positionItem.SectorBenchmarkWeight = (bmPosition == null) ? 0 : bmPosition.Pctof_Portfolio;
                positionItem.PortfolioReturnPct = position.TotalReturnPct;
                positionItem.PortfolioReturn = position.TotalReturn;
                positionItem.SectorBenchmarkReturnPct = (bmPosition == null) ? 0 : bmPosition.TotalReturn;
                positionItem.pctofListedShare = ((position.FreefloatCapital == 0) ? -1 : (position.HoldingQuantity / position.FreefloatCapital));
                positionItem.TurnoverRate = ((position.FreefloatCapital == 0) ? -1 : (position.TradingVolume / position.FreefloatCapital));
                positionItem.LiquidityCycle = ((position.TradingVolume == 0) ? 0 : (position.HoldingQuantity / position.TradingVolume));
                positionItem.EconomicSector = position.SWIndustryName;
                positionItem.HoldingVolume = position.HoldingQuantity;
                positionItem.HoldingMarketValue = position.HoldingMarketValue;

                EquityPerformanceList.Add(positionItem);
            }
            #endregion

            #region Bond Position Calculation
            foreach (AssetPosition position in PerformancePortfolio.BondPositionList)
            {
                AssetEvaluation positionItem = new AssetEvaluation();
                positionItem.Code = position.WindCode;
                positionItem.Category = position.Category;
                positionItem.Name = position.Name;
                positionItem.pctofNAV = position.Pctof_Nav;
                positionItem.PortfolioWeight = position.Pctof_Portfolio;
                positionItem.PortfolioReturnPct = position.TotalReturnPct;
                positionItem.PortfolioNetReturnPct = position.TotalNetReturnPct;
                positionItem.PortfolioReturn = position.TotalReturn;
                positionItem.PortfolioNetReturn = position.TotalNetReturn;
                //positionItem.pctofListedShare = ((position.FreefloatCapital == 0) ? -1 : (position.HoldingQuantity / position.FreefloatCapital));
                //positionItem.TurnoverRate = ((position.FreefloatCapital == 0) ? -1 : (position.TradingVolume / position.FreefloatCapital));
                //positionItem.LiquidityCycle = ((position.TradingVolume == 0) ? -1 : (position.HoldingQuantity / position.TradingVolume));
                positionItem.HoldingVolume = position.HoldingQuantity;
                positionItem.HoldingMarketValue = position.HoldingMarketValue;
                positionItem.AnnualYield = positionItem.PortfolioReturnPct * 365 / (this.EndDate - this.StartDate).Days;
                positionItem.BondMaturityDate = position.BondMaturityDate;
                BondPerformanceList.Add(positionItem);
            }
            #endregion

            #region Other Position Calculation
            foreach (AssetPosition position in PerformancePortfolio.OtherPositionList)
            {
                AssetEvaluation positionItem = new AssetEvaluation();
                positionItem.Code = position.WindCode;
                positionItem.Name = position.Name;
                positionItem.pctofNAV = position.Pctof_Nav;
                positionItem.PortfolioWeight = position.Pctof_Portfolio;
                positionItem.PortfolioReturnPct = position.TotalReturnPct;
                positionItem.PortfolioReturn = position.TotalReturn;
                positionItem.HoldingVolume = position.HoldingQuantity;
                positionItem.HoldingMarketValue = position.HoldingMarketValue;
                OtherPerformanceList.Add(positionItem);
            }
            #endregion

            #region Cash Position Calculation
            foreach (AssetPosition position in PerformancePortfolio.CashPositionList)
            {
                AssetEvaluation positionItem = new AssetEvaluation();
                positionItem.Name = position.Name;
                positionItem.HoldingMarketValue = position.HoldingMarketValue;
                positionItem.pctofNAV = position.Pctof_Nav;
                positionItem.PortfolioReturnPct = position.TotalReturnPct;
                positionItem.PortfolioReturn = position.TotalReturn;
                positionItem.AnnualYield = positionItem.PortfolioReturnPct * 365 / (this.EndDate - this.StartDate).Days;
                CashPerformanceList.Add(positionItem);
            }
            #endregion

            //Sort by orderid, portfolio weight
            IndustryPerformanceList.Sort();
            EquityPerformanceList.Sort();
            BondPerformanceList.Sort();
            CashPerformanceList.Sort();
            OtherPerformanceList.Sort();

            TransactionList.Sort();
            SubscribeRedempList.Sort();
        }
        #endregion
    }
}
