using System;
using System.Data;
using System.Diagnostics;
using Security;
using Security.Portfolio;
using System.Collections.Generic;
using System.Collections;

namespace PortfolioAnalyze
{
    public class EquityAttibution: IComparable
    {
        public string IndustryName;
        public double MarketValue;
        public double PortfolioWeight;
        public double BenchmarkWeight;
        public double PortfolioReturn;
        public double BenchmarkReturn;

        public double IndustryAttribution;
        public double SecurityAttribution;
        public double CrossAttribution;
        public double ValueAdded;
        
        public int CompareTo(object obj)
        {
            EquityAttibution eax = (EquityAttibution)obj;
            EquityAttibution eay = this;

            if (eax.PortfolioWeight > eay.PortfolioWeight)
                return 1;
            else if (eax.PortfolioWeight == eay.PortfolioWeight)
            {
                if (eax.BenchmarkWeight >= eay.BenchmarkWeight)
                    return 1;
                else
                    return -1;
            }
            else
                return -1;
        }
    }

    public class Performance
    {
        #region Consts
        public const string C_ColName_Industry = "INDUSTRY";
        public const string C_ColName_MarketValue = "MARKETVALUE";
        public const string C_ColName_PortfWeight = "PORTFWEIGHT";
        public const string C_ColName_BenchWeight = "BENCHWEIGHT";
        public const string C_ColName_PortfReturn = "PORTFRET";
        public const string C_ColName_BenchReturn = "BENCHRET";
        public const string C_ColName_IndustryAttr = "INDUSTRYATTR";
        public const string C_ColName_SecurityAttr = "SECURITYATTR";
        public const string C_ColName_CrossAttr = "CROSSATTR";
        public const string C_ColName_ValueAdded = "VALUEADDED";
        #endregion

        public PortfolioGroup PortfGroup;   //按时间顺序排列
        public Index Benchmark;
        public DateTime StartDate, EndDate;

        #region 加载数据
        public void LoadData(DataTable dtGZB, PortfolioDataAdapterType type, DateTime start, DateTime end, string benchmarkcode)
        {
            //加载数据
            try
            {
                //纠正时间
                if (end < start)
                {
                    DateTime tmp = end;
                    end = start;
                    start = tmp;
                }

                if (end > DateTime.Today.AddDays(-1))
                    end = DateTime.Today.AddDays(-1);

                //读取估值表
                IPortfolioDataAdapter adapter = PortfolioDataAdaptorFactory.GetAdapter(type);
                this.PortfGroup = adapter.BuildPortfolios(dtGZB, start, end);

                this.StartDate = this.PortfGroup.ExchangeTradingDays[0];
                this.EndDate = this.PortfGroup.ExchangeTradingDays[this.PortfGroup.ExchangeTradingDays.Count - 1];

                //计算收益率和持仓
                if (PortfGroup.IsDataComplete() && PortfGroup.Portfolios.Count > 1)
                    this.Calculate();
                else
                    throw new Exception(Message.C_Msg_PD3);

                //无需比较基准
                if (benchmarkcode == null || benchmarkcode.Length == 0)
                    return;

                //建立比较基准并计算收益率
                this.Benchmark = new Index(benchmarkcode);
                this.Benchmark.SetDatePeriod(start, end);
                this.Benchmark.LoadData(DataInfoType.IndexComponents);
                this.Benchmark.LoadData(DataInfoType.HistoryTradePrice);
                this.Benchmark.ComponentsCalculate();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        
        //计算组合在固定期间的收益率
        private void Calculate()
        { 
            //加载价格数据
            foreach (ASecurityGroup g in this.PortfGroup.MergedPortfolio.SecurityGroupList)
            {
                g.SetDatePeriod(this.StartDate, this.EndDate);
                g.LoadData(DataInfoType.SecurityInfo);
                g.LoadData(DataInfoType.HistoryTradePrice);
                g.LoadData(DataInfoType.HistoryFundNAV);
                g.LoadData(DataInfoType.HistoryDividend);
            }

            //更新投资组合的股息数据
            this.PortfGroup.UpdateDividend();

            //计算收益率: portfolio按时间顺序排列
            //  最后一个交易日的portfolio记录了累计的收益率
            //  第一个交易日为参考基准，不参与收益率的计算，全零
            for (int i = 1; i < this.PortfGroup.Portfolios.Count; i++)
            {
                try
                {
                    Portfolio pCurrent = this.PortfGroup.Portfolios[i];
                    Portfolio pPrevious = this.PortfGroup.Portfolios[i - 1];
                    pCurrent.Calculate(pPrevious, ref this.PortfGroup.MergedPortfolio);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        #endregion

        #region 归因分析
        private List<EquityAttibution> EquityAttributions = new List<EquityAttibution>();
        public List<EquityAttibution> Attribute()
        {
            if (this.Benchmark == null || this.Benchmark.Components == null || this.Benchmark.Components.Count == 0)
            {
                MessageManager.GetInstance().AddMessage(MessageType.Information, "未设置比较基准", "");
                return null;
            }

            ASecurityGroup benchmarkcomponents = this.Benchmark.GetLatestComponents();
            if(benchmarkcomponents.SecurityList == null || benchmarkcomponents.SecurityList.Count == 0 || benchmarkcomponents.SecurityList[0].Type != SecurityType.Equity)
                throw new Exception("比较基准设置错误，仅股票型可用");

            List<EquityGroup> bmindustrylist = ((EquityGroup)benchmarkcomponents).IndustryList;
            EquityGroup portfolio = (EquityGroup)this.PortfGroup.GetLatestPortfolio().EquityHoldings;
            List<EquityGroup> pindustrylist = portfolio.IndustryList;

            //基准总回报：
            //  1) benchmarkcomponents.Position.AccumulatedYieldIndex - 1;  //指数成份的区间加权平均收益率，这个不准确，因为一个月内成分权重会随着涨跌变化
            //  2) this.Benchmark.Position.CurrentYield;                    //指数的区间收益率，这个准确                
            double totalbmreturn = this.Benchmark.Position.CurrentYield;

            this.EquityAttributions.Clear();
            foreach (EquityGroup bmindustry in bmindustrylist)
            {
                EquityGroup pindustry = pindustrylist.Find(delegate(EquityGroup eg) { return eg.Name == bmindustry.Name; });
                if (pindustry == null)
                {
                    pindustry = new EquityGroup();
                    pindustry.Name = bmindustry.Name;
                }
                //•行业配置回报 = (组合权重 - 基准权重) * (基准回报 - 基准总回报)，反应投资组合的行业配置效应；
                //•行业内选股回报 = 基准权重 * (组合回报 - 基准回报)，反应投资组合行业内的选股效应；
                //•交叉回报 = (组合权重 - 基准权重) * (组合回报 - 基准回报)，反应行业配置和行业内选股的联合效应；
                //•增加值 = 组合权重 * 组合回报 - 基准权重 * 基准回报，反应主动性管理给投资组合带来的超额收益效应；
                EquityAttibution ea = new EquityAttibution();
                ea.IndustryName = bmindustry.Name;
                ea.MarketValue = pindustry.Position.MarketValue;
                ea.PortfolioWeight = pindustry.Position.MarketValuePct;
                ea.BenchmarkWeight = bmindustry.Position.MarketValuePct;
                ea.PortfolioReturn = pindustry.Position.AccumulatedYieldIndex - 1;
                ea.BenchmarkReturn = bmindustry.Position.AccumulatedYieldIndex - 1;

                
                ea.IndustryAttribution = (ea.PortfolioWeight - ea.BenchmarkWeight) * (ea.BenchmarkReturn - totalbmreturn);
                ea.SecurityAttribution = ea.BenchmarkWeight * (ea.PortfolioReturn - ea.BenchmarkReturn);
                ea.CrossAttribution = (ea.PortfolioWeight - ea.BenchmarkWeight) * (ea.PortfolioReturn - ea.BenchmarkReturn);
                ea.ValueAdded = ea.PortfolioWeight * ea.PortfolioReturn - ea.BenchmarkWeight * ea.BenchmarkReturn;

                this.EquityAttributions.Add(ea);
            }

            //按市值排序
            this.EquityAttributions.Sort();

            //整个股票组合
            EquityAttibution equityattr = new EquityAttibution();
            equityattr.IndustryName = "股票组合";
            equityattr.MarketValue = portfolio.Position.MarketValue;
            equityattr.PortfolioWeight = portfolio.Position.MarketValuePct;
            equityattr.PortfolioReturn = portfolio.Position.AccumulatedYieldIndex - 1;
            equityattr.BenchmarkReturn = totalbmreturn;
            equityattr.ValueAdded = equityattr.PortfolioReturn - equityattr.BenchmarkReturn;
            this.EquityAttributions.Add(equityattr);

            return this.EquityAttributions;
        }
        public DataTable GetAttributionTable()
        {
            DataTable dtAttr = new DataTable();
            dtAttr.Columns.Add(new DataColumn(C_ColName_Industry, typeof(string)));
            dtAttr.Columns.Add(new DataColumn(C_ColName_MarketValue, typeof(double)));
            dtAttr.Columns.Add(new DataColumn(C_ColName_PortfWeight, typeof(double)));
            dtAttr.Columns.Add(new DataColumn(C_ColName_BenchWeight, typeof(double)));
            dtAttr.Columns.Add(new DataColumn(C_ColName_PortfReturn, typeof(double)));
            dtAttr.Columns.Add(new DataColumn(C_ColName_BenchReturn, typeof(double)));
            dtAttr.Columns.Add(new DataColumn(C_ColName_IndustryAttr, typeof(double)));
            dtAttr.Columns.Add(new DataColumn(C_ColName_SecurityAttr, typeof(double)));
            dtAttr.Columns.Add(new DataColumn(C_ColName_CrossAttr, typeof(double)));
            dtAttr.Columns.Add(new DataColumn(C_ColName_ValueAdded, typeof(double)));

            foreach (EquityAttibution ea in this.EquityAttributions)
            {
                DataRow r = dtAttr.NewRow();
                r[C_ColName_Industry] = ea.IndustryName;
                r[C_ColName_MarketValue] = ea.MarketValue / 10000;
                r[C_ColName_PortfWeight] = ea.PortfolioWeight;
                r[C_ColName_BenchWeight] = ea.BenchmarkWeight;
                r[C_ColName_PortfReturn] = ea.PortfolioReturn;
                r[C_ColName_BenchReturn] = ea.BenchmarkReturn;
                r[C_ColName_IndustryAttr] = ea.IndustryAttribution;
                r[C_ColName_SecurityAttr] = ea.SecurityAttribution;
                r[C_ColName_CrossAttr] = ea.CrossAttribution;
                r[C_ColName_ValueAdded] = ea.ValueAdded;
                dtAttr.Rows.Add(r);
            }

            return dtAttr;
        }
        #endregion

        #region 持仓分析
        public DataTable GetPositionTable(SecurityType type)
        {
            DataTable dt = this.PortfGroup.GetLatestPortfolio().GetPositionTable(type);
            dt.DefaultView.Sort = AHistoryDataVendor.C_ColName_MarketValuePct + " DESC";
            return dt;
        }

        private DataTable _TransactionTable;
        public DataTable GetTransactionTable()
        {
            if (_TransactionTable == null)
            {
                _TransactionTable = new DataTable();
                _TransactionTable.Columns.Add(AHistoryDataVendor.C_ColName_Order, typeof(int));
                _TransactionTable.Columns.Add(AHistoryDataVendor.C_ColName_TradeDate, typeof(DateTime));
                _TransactionTable.Columns.Add(AHistoryDataVendor.C_ColName_SecurityType, typeof(string));
                _TransactionTable.Columns.Add(AHistoryDataVendor.C_ColName_Code, typeof(string));
                _TransactionTable.Columns.Add(AHistoryDataVendor.C_ColName_TradeAction, typeof(string));
                _TransactionTable.Columns.Add(AHistoryDataVendor.C_ColName_Name, typeof(string));
                _TransactionTable.Columns.Add(AHistoryDataVendor.C_ColName_Price, typeof(double));
                _TransactionTable.Columns.Add(AHistoryDataVendor.C_ColName_Quantity, typeof(double));
                _TransactionTable.Columns.Add(AHistoryDataVendor.C_ColName_MarketValue, typeof(double));
            }
            _TransactionTable.Rows.Clear();

            foreach (Portfolio p in this.PortfGroup.Portfolios)
            {
                foreach (ASecurityGroup g in p.AllGroupList)
                {
                    if (g.SecurityList != null)
                    {
                        foreach (ASecurity s in g.SecurityList)
                        {
                            foreach (TransactionInfo t in s.Position.Transactions)
                            {
                                DataRow r = this._TransactionTable.NewRow();
                                r[AHistoryDataVendor.C_ColName_TradeDate] = t.TradingDay;
                                r[AHistoryDataVendor.C_ColName_Code] = t.Code;
                                r[AHistoryDataVendor.C_ColName_Name] = t.Name;
                                r[AHistoryDataVendor.C_ColName_Price] = t.Price;
                                r[AHistoryDataVendor.C_ColName_Quantity] = t.Quantity / 10000;
                                r[AHistoryDataVendor.C_ColName_MarketValue] = t.Price * t.Quantity / 10000;

                                switch (t.Action)
                                {
                                    case TradeAction.Buy:
                                    case TradeAction.Long:
                                        r[AHistoryDataVendor.C_ColName_TradeAction] = "买入";
                                        break;
                                    case TradeAction.Sell:
                                    case TradeAction.Short:
                                        r[AHistoryDataVendor.C_ColName_TradeAction] = "卖出";
                                        break;
                                    case TradeAction.Increase:
                                        r[AHistoryDataVendor.C_ColName_TradeAction] = "增持";
                                        break;
                                    case TradeAction.Decrease:
                                        r[AHistoryDataVendor.C_ColName_TradeAction] = "减持";
                                        break;
                                    default:
                                        r[AHistoryDataVendor.C_ColName_TradeAction] = "";
                                        break;
                                }

                                switch (s.Type)
                                {
                                    case SecurityType.Equity:
                                        r[AHistoryDataVendor.C_ColName_Order] = 1;
                                        r[AHistoryDataVendor.C_ColName_SecurityType] = "股票";
                                        break;
                                    case SecurityType.Bond:
                                        r[AHistoryDataVendor.C_ColName_Order] = 2;
                                        r[AHistoryDataVendor.C_ColName_SecurityType] = "债券";
                                        break;
                                    case SecurityType.Fund:
                                        r[AHistoryDataVendor.C_ColName_Order] = 3;
                                        r[AHistoryDataVendor.C_ColName_SecurityType] = "基金";
                                        break;
                                    case SecurityType.RevRepo:
                                        r[AHistoryDataVendor.C_ColName_Order] = 4;
                                        r[AHistoryDataVendor.C_ColName_SecurityType] = "逆回购";
                                        break;
                                    case SecurityType.TheRepo:
                                        r[AHistoryDataVendor.C_ColName_Order] = 5;
                                        r[AHistoryDataVendor.C_ColName_SecurityType] = "正回购";
                                        break;
                                    default:
                                        r[AHistoryDataVendor.C_ColName_SecurityType] = "";
                                        break;
                                }

                                this._TransactionTable.Rows.Add(r);
                            }
                        }
                    }
                }
            }

            this._TransactionTable.DefaultView.Sort = AHistoryDataVendor.C_ColName_TradeDate + " DESC, " + AHistoryDataVendor.C_ColName_Order + " ASC";
            return this._TransactionTable;
        }
        #endregion
    }
}
