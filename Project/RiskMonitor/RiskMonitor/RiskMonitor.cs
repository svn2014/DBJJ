using System.Data;
using Security;
using Security.Portfolio;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Reflection;

namespace RiskMonitor
{
    public enum CheckMethod
    {
        Null,
        CheckSecurityType,
        CheckCashPct,
        CheckPositionPct,
        CheckBondRating,
        CheckBondMaturity,
        CheckBondGroupAverageMaturity,
        CheckBondPayment,
        CheckBondGroupPositionPct,
        CheckBondValueDeviation
    }

    public enum CheckOperator
    { 
        GreaterThan,
        LessThan,
        Equal,
        NotEqual
    }

    public class CheckParameter
    {
        public CheckMethod Method = CheckMethod.Null;
        public CheckOperator Operator = CheckOperator.Equal;
        public object WarningValue = 0;
        public object FailedValue = 0;
        public string Description = "";

        public CheckParameter(CheckMethod method, CheckOperator op, object failedvalue, object warningvalue, string description)
        {
            this.Method = method;
            this.Operator = op;
            this.FailedValue = failedvalue;
            this.WarningValue = warningvalue;
            this.Description = description;
        }
    }

    public class ARiskMonitor
    {
        public Portfolio SecurityPortfolio;
        public virtual void BuildPortfolio(DataTable dtGZB, PortfolioDataAdapterType type, DateTime tradeday)
        {
            try
            {
                IPortfolioDataAdapter adapter = PortfolioDataAdaptorFactory.GetAdapter(type);
                PortfolioGroup group = adapter.BuildPortfolios(dtGZB, tradeday, tradeday);
                this.SecurityPortfolio = group.Portfolios[0];    //仅需要最新一期的持仓数据
            }
            catch (Exception ex)
            {                
                throw ex;
            }            
        }

        public MonitorReport Report = new MonitorReport();
        public virtual void Check(List<CheckParameter> paralist)
        {
            if (this.SecurityPortfolio == null)
                return;

            foreach (CheckParameter para in paralist)
	        {
                try
                {
                    MethodInfo m = this.GetType().GetMethod(para.Method.ToString(), BindingFlags.NonPublic | BindingFlags.Instance);
                    m.Invoke(this, new object[] { para });
                }
                catch (Exception ex)
                {
                    throw new Exception(para.Method.ToString() + ": ", ex);
                }
	        }
        }

        private bool compareValue(CheckOperator op, double value, double threshold)
        {
            switch (op)
            {
                case CheckOperator.GreaterThan:
                    if (value > threshold)
                        return true;
                    break;
                case CheckOperator.LessThan:
                    if (value < threshold)
                        return true;
                    break;
                case CheckOperator.Equal:
                    if (value == threshold)
                        return true;
                    break;
                case CheckOperator.NotEqual:
                    if (value != threshold)
                        return true;
                    break;
                default:
                    throw new Exception("比较操作符未定义");
            }

            return false;
        }
        private string getOperatorName(CheckOperator op)
        {
            switch (op)
            {
                case CheckOperator.GreaterThan:
                    return "大于";
                case CheckOperator.LessThan:
                    return "小于";
                case CheckOperator.Equal:
                    return "等于";
                case CheckOperator.NotEqual:
                    return "不等于";
                default:
                    return "--";
            }
        }

        #region 检查项目
        #region 文本
        protected const int C_Order_SecurityType = 100;
        protected const string C_Title_SecurityType = "证券类型";
        protected const string C_Desc_SecurityType = "持仓市值占资产净值比例为<2>，<OP><3>的阈值。(<1>)";

        protected const int C_Order_PositionPct = 200;
        protected const string C_Title_PositionPct = "个券持仓";
        protected const string C_Desc_PositionPct = "<1>为<2>，货币基金不得投资于该类证券。";

        protected const int C_Order_BondRating = 300;
        protected const string C_Title_BondRating = "信用评级";
        protected const string C_Desc_BondRating = "信用评级为<2>，<OP><3>或其等价评级。(<1>)";

        protected const int C_Order_BondMaturity = 400;
        protected const string C_Title_BondMaturity = "剩余期限";
        protected const string C_Desc_BondMaturity = "剩余期限为<2>，<OP><3>。(<1>)";
        protected const string C_Desc_BondGroupAverageMaturity = "平均剩余期限为<2>，<OP><3>。(<1>)";

        protected const int C_Order_BondIntPayment = 500;
        protected const string C_Title_BondIntPayment = "付息日期";
        protected const string C_Desc_BondIntPayment = "下一付息日为<2>，距今<OP><3>。(<1>)";

        protected const int C_Order_BondGroupPositionPct = 600;
        protected const string C_Title_BondGroupPositionPct = "同类持仓";
        protected const string C_Desc_BondGroupPositionPct = "<1>发行的证券[<2>]的持仓市值占资产净值比例为<3>，<OP><4>的阈值。";

        protected const int C_Order_CashPct = 700;
        protected const string C_Title_CashPct = "现金持仓";
        protected const string C_Desc_CashPct = "现金占基金资产净值比例为<1>，<OP><2>的阈值。";

        protected const int C_Order_BondValueDeviation = 800;
        protected const string C_Title_BondValueDeviation = "估值偏离";
        protected const string C_Desc_BondValueDeviation = "估值偏离为<2>，<OP><3>的阈值。(<1>，其中：基金估值=<4>，中债估值=<5>)";
        #endregion

        #region 通用
        /// <summary>
        /// 检查个券持仓占比
        /// </summary>
        protected virtual void CheckPositionPct(CheckParameter para)
        {
            string title = C_Title_PositionPct;
            string description = ""; ;
            int order = C_Order_PositionPct;

            double warningthreshhold = Convert.ToDouble(para.WarningValue);
            double failthreshhold = Convert.ToDouble(para.FailedValue);

            MonitorItem mi;
            int id = 0;
            foreach (ASecurityGroup g in this.SecurityPortfolio.SecurityGroupList)
            {
                if (g.SecurityList == null || g.SecurityList.Count == 0)
                    continue;

                foreach (ASecurity s in g.SecurityList)
                {
                    mi = new MonitorItem();
                    mi.code = s.Code;
                    mi.name = s.Name;
                    mi.title = title;
                    mi.order = order;
                    mi.securityid = id++;
                    mi.value = s.Position.MarketValuePct.ToString("P2");
                    mi.status = Status.Pass;

                    description = C_Desc_SecurityType.Replace("<1>", s.Name + "(" + s.Code + ")").Replace("<OP>", getOperatorName(para.Operator));
                    if (this.compareValue(para.Operator, s.Position.MarketValuePct, failthreshhold))
                    {
                        //检查fail放在前
                        //  确保当failthreshhold=warningthreshhold时，显示fail
                        mi.threshhold = failthreshhold.ToString("P2");
                        mi.status = Status.Fail;
                        mi.description = description.Replace("<2>", s.Position.MarketValuePct.ToString("P2")).Replace("<3>", failthreshhold.ToString("P2"));
                    }
                    else if (this.compareValue(para.Operator, s.Position.MarketValuePct, warningthreshhold))
                    {
                        mi.threshhold = warningthreshhold.ToString("P2");
                        mi.status = Status.Warning;
                        mi.description = description.Replace("<2>", s.Position.MarketValuePct.ToString("P2")).Replace("<3>", warningthreshhold.ToString("P2"));
                    }
                    else
                    {
                        mi.threshhold = warningthreshhold.ToString("P2");
                        mi.status = Status.Pass;
                        mi.description = "";
                    }

                    this.Report.Add(mi);
                }
            }
        }

        /// <summary>
        /// 检查现金存款占比
        /// </summary>
        protected virtual void CheckCashPct(CheckParameter para)
        {
            string title = C_Title_CashPct;
            string description = ""; ;
            int order = C_Order_CashPct;

            double warningthreshhold = Convert.ToDouble(para.WarningValue);
            double failedthreshhold = Convert.ToDouble(para.FailedValue);

            MonitorItem mi;
            int id = 0;

            ASecurityGroup g = this.SecurityPortfolio.CashHoldings;
            if (g.SecurityList == null || g.SecurityList.Count == 0)
                return;

            foreach (ASecurity s in g.SecurityList)
            {
                //仅检查活期存款
                if (s.Code != Deposit.C_Code_CurrentDeposit)
                    continue;

                mi = new MonitorItem();
                mi.code = s.Code;
                mi.name = s.Name;
                mi.title = title;
                mi.order = order;
                mi.securityid = id++;
                mi.status = Status.Pass;
                mi.value = s.Position.MarketValuePct.ToString("P2");

                description = C_Desc_CashPct.Replace("<OP>", getOperatorName(para.Operator));
                if (this.compareValue(para.Operator, s.Position.MarketValuePct, failedthreshhold))
                {
                    //检查fail放在前
                    //  确保当failthreshhold=warningthreshhold时，显示fail 
                    mi.threshhold = failedthreshhold.ToString("P2");
                    mi.status = Status.Fail;
                    mi.description = description.Replace("<1>", mi.value).Replace("<2>", mi.threshhold);
                }
                else if (this.compareValue(para.Operator, s.Position.MarketValuePct, warningthreshhold))
                {
                    mi.threshhold = warningthreshhold.ToString("P2");
                    mi.status = Status.Warning;
                    mi.description = description.Replace("<1>", mi.value).Replace("<2>", mi.threshhold);
                }
                else
                {
                    mi.threshhold = warningthreshhold.ToString("P2");
                    mi.status = Status.Pass;
                    mi.description = "";
                }

                this.Report.Add(mi);
            }
        }
        #endregion

        #region 债券
        /// <summary>
        /// 检查同一公司发行的债券持仓占比
        /// </summary>
        protected virtual void CheckBondGroupPositionPct(CheckParameter para)
        {
            int order = C_Order_BondGroupPositionPct; ;
            string title = C_Title_BondGroupPositionPct;
            string description;

            double warningthreshhold = Convert.ToDouble(para.WarningValue);
            double failedthreshhold = Convert.ToDouble(para.FailedValue);

            MonitorItem mi;
            ASecurityGroup g = this.SecurityPortfolio.BondHoldings;
            if (g.SecurityList == null || g.SecurityList.Count == 0)
                return;

            List<BondGroup.CompanyView> companylist = ((BondGroup)g).GetCompanyPositions();

            int id = 0;
            foreach (BondGroup.CompanyView cv in companylist)
            {
                //当前公司仅有一个债券被持有，不做检验
                if (cv.CompanyNumber == 1)
                {
                    id++;
                    continue;
                }

                mi = new MonitorItem();
                mi.order = order;
                mi.title = title;
                mi.securityid = id++;
                mi.status = Status.Pass;
                mi.value = cv.TotalPosition.MarketValuePct.ToString("P2");
                mi.name = cv.CompanyName;
                mi.code = "";
                string allcodes = "";
                foreach (PositionInfo p in cv.RelatedPositionList)
                {
                    allcodes += p.Name + "(" + p.Code + "),";
                }
                allcodes = allcodes.Substring(0, allcodes.Length - 1);

                description = C_Desc_BondGroupPositionPct.Replace("<1>", mi.name).Replace("<2>", allcodes).Replace("<OP>", getOperatorName(para.Operator));
                if (this.compareValue(para.Operator, cv.TotalPosition.MarketValuePct, failedthreshhold))
                {
                    //检查fail放在前
                    //  确保当failthreshhold=warningthreshhold时，显示fail 
                    mi.threshhold = failedthreshhold.ToString("P2");
                    mi.status = Status.Fail;
                    mi.description = description.Replace("<3>", cv.TotalPosition.MarketValuePct.ToString("P2")).Replace("<4>", failedthreshhold.ToString("P2"));
                }
                else if (this.compareValue(para.Operator, cv.TotalPosition.MarketValuePct, warningthreshhold))
                {
                    mi.threshhold = warningthreshhold.ToString("P2");
                    mi.status = Status.Warning;
                    mi.description = description.Replace("<3>", cv.TotalPosition.MarketValuePct.ToString("P2")).Replace("<4>", warningthreshhold.ToString("P2"));
                }
                else
                {
                    mi.threshhold = warningthreshhold.ToString("P2");
                    mi.status = Status.Pass;
                    mi.description = "";
                }

                this.Report.Add(mi);
            }
        }

        /// <summary>
        /// 检查债券评级
        /// </summary>
        protected virtual void CheckBondRating(CheckParameter para)
        {
            string title = C_Title_BondRating;
            string description;
            int order = C_Order_SecurityType;

            string failthreshhold = para.FailedValue.ToString();
            string warningthreshhold = para.WarningValue.ToString();
            int failRating = Bond.ConvertCreditRating(failthreshhold);
            int warnRating = Bond.ConvertCreditRating(warningthreshhold);

            MonitorItem mi;
            int id = 0;
            ASecurityGroup g = this.SecurityPortfolio.BondHoldings;
            if (g.SecurityList == null || g.SecurityList.Count == 0)
                return;

            foreach (ASecurity s in g.SecurityList)
            {
                mi = new MonitorItem();
                mi.code = s.Code;
                mi.name = s.Name;
                mi.title = title;
                mi.order = order;
                mi.securityid = id++;
                mi.status = Status.Pass;
                mi.value = ((Bond)s).CreditRate;

                switch (((Bond)s).SubType)
                {
                    case BondType.Treasury:
                    case BondType.Central:
                    case BondType.SpecialFinancial:
                        mi.value = "AAA";
                        break;
                    default:
                        break;
                }

                description = C_Desc_BondRating.Replace("<1>", s.Name + "(" + s.Code + ")").Replace("<OP>", getOperatorName(para.Operator));
                switch (s.Type)
                {
                    case SecurityType.Bond:
                        int currRating = Bond.ConvertCreditRating(mi.value);
                        if (this.compareValue(para.Operator, currRating, failRating))
                        {
                            //检查fail放在前
                            //  确保当failthreshhold=warningthreshhold时，显示fail 
                            mi.threshhold = failthreshhold;
                            mi.status = Status.Fail;
                            mi.description = description.Replace("<2>", mi.value).Replace("<3>", failthreshhold);

                            Debug.WriteLine(mi.description);
                        }
                        else if (this.compareValue(para.Operator, currRating, warnRating))
                        {
                            mi.threshhold = warningthreshhold;
                            mi.status = Status.Warning;
                            mi.description = description.Replace("<2>", mi.value).Replace("<3>", warningthreshhold);
                        }
                        else
                        {
                            mi.threshhold = warningthreshhold;
                            mi.status = Status.Pass;
                            mi.description = "";
                        }
                        break;
                    default:
                        //对于非债券类型，无需作错误提示，会在其他步骤中检验
                        break;
                }

                this.Report.Add(mi);
            }
        }

        /// <summary>
        /// 检查剩余期限
        /// </summary>
        protected virtual void CheckBondMaturity(CheckParameter para)
        {
            int order = C_Order_BondMaturity; ;
            string title = C_Title_BondMaturity;
            string description;

            double warningthreshhold = Convert.ToDouble(para.WarningValue);
            double failedthreshhold = Convert.ToDouble(para.FailedValue);
            double warningyears = warningthreshhold / 365;
            double failedyears = failedthreshhold / 365;

            MonitorItem mi;
            int id = 0;
            
            ASecurityGroup g = this.SecurityPortfolio.BondHoldings;
            if (g.SecurityList == null || g.SecurityList.Count == 0)
                return;

            foreach (ASecurity s in g.SecurityList)
            {
                mi = new MonitorItem();
                mi.code = s.Code;
                mi.name = s.Name;
                mi.title = title;
                mi.order = order;
                mi.securityid = id++;
                mi.status = Status.Pass;

                DateTime maturity = ((Bond)s).MaturityDate;
                double remainingdays = (maturity - DateTime.Today).Days;
                double remainingyears = remainingdays / 365;
                mi.value = remainingdays.ToString() + "天";

                description = C_Desc_BondMaturity.Replace("<1>", s.Name + "(" + s.Code + ")").Replace("<OP>", getOperatorName(para.Operator));
                switch (s.Type)
                {
                    case SecurityType.Bond:
                        if (this.compareValue(para.Operator, remainingdays, failedthreshhold))
                        {
                            //检查fail放在前
                            //  确保当failthreshhold=warningthreshhold时，显示fail 
                            mi.threshhold = failedthreshhold.ToString("N0") + "天";
                            mi.status = Status.Fail;
                            mi.description = description.Replace("<2>", remainingdays + "天(" + remainingyears.ToString("N2") + "年)").Replace("<3>", failedthreshhold + "天(" + failedyears.ToString("N2") + "年)");
                        }
                        else if (this.compareValue(para.Operator, remainingdays, warningthreshhold))
                        {
                            mi.threshhold = warningthreshhold.ToString("N0") + "天";
                            mi.status = Status.Warning;
                            mi.description = description.Replace("<2>", remainingdays + "天(" + remainingyears.ToString("N2") + "年)").Replace("<3>", warningthreshhold + "天(" + warningyears.ToString("N2") + "年)");
                        }
                        else
                        {
                            mi.threshhold = warningthreshhold.ToString("N0") + "天";
                            mi.status = Status.Pass;
                            mi.description = "";
                        }
                        break;
                    default:
                        //对于非债券类型，无需作错误提示，会在其他步骤中检验
                        break;
                }

                this.Report.Add(mi);
            }
        }
        protected virtual void CheckBondGroupAverageMaturity(CheckParameter para)
        {
            int order = C_Order_BondMaturity; ;
            string title = C_Title_BondMaturity;
            string description;

            double warningthreshhold = Convert.ToDouble(para.WarningValue);
            double failedthreshhold = Convert.ToDouble(para.FailedValue);
            double warningyears = warningthreshhold / 365;
            double failedyears = failedthreshhold / 365;

            MonitorItem mi;
            ASecurityGroup g = this.SecurityPortfolio.BondHoldings;
            if (g.SecurityList == null || g.SecurityList.Count == 0)
                return;

            mi = new MonitorItem();
            mi.code = "";
            mi.name = "债券组合";
            mi.title = title;
            mi.order = order;
            mi.securityid = g.SecurityList.Count;
            mi.status = Status.Pass;

            double remainingdays = ((BondGroup)g).GetAverageMaturity();
            double remainingyears = remainingdays / 365;
            mi.value = remainingdays.ToString() + "天";

            description = C_Desc_BondGroupAverageMaturity.Replace("<1>", mi.name).Replace("<OP>", getOperatorName(para.Operator));
            if (this.compareValue(para.Operator, remainingdays, failedthreshhold))
            {
                //检查fail放在前
                //  确保当failthreshhold=warningthreshhold时，显示fail 
                mi.threshhold = failedthreshhold.ToString("N0") + "天";
                mi.status = Status.Fail;
                mi.description = description.Replace("<2>", remainingdays.ToString("F2") + "天(" + remainingyears.ToString("N2") + "年)").Replace("<3>", failedthreshhold + "天(" + failedyears.ToString("N2") + "年)");
            }
            else if (this.compareValue(para.Operator, remainingdays, warningthreshhold))
            {
                mi.threshhold = warningthreshhold.ToString("N0") + "天";
                mi.status = Status.Warning;
                mi.description = description.Replace("<2>", remainingdays.ToString("F2") + "天(" + remainingyears.ToString("N2") + "年)").Replace("<3>", warningthreshhold + "天(" + warningyears.ToString("N2") + "年)");
            }
            else
            {
                mi.threshhold = warningthreshhold.ToString("N0") + "天";
                mi.status = Status.Pass;
                mi.description = "";
            }

            this.Report.Add(mi);
        }

        /// <summary>
        /// 检查付息日
        /// </summary>
        protected virtual void CheckBondPayment(CheckParameter para)
        {
            int order = C_Order_BondIntPayment; ;
            string title = C_Title_BondIntPayment;
            string description;

            double warningthreshhold = Convert.ToDouble(para.WarningValue);
            double failedthreshhold = Convert.ToDouble(para.FailedValue);

            MonitorItem mi;
            int id = 0;

            ASecurityGroup g = this.SecurityPortfolio.BondHoldings;
            if (g.SecurityList == null || g.SecurityList.Count == 0)
                return;

            foreach (ASecurity s in g.SecurityList)
            {
                mi = new MonitorItem();
                mi.code = s.Code;
                mi.name = s.Name;
                mi.title = title;
                mi.order = order;
                mi.securityid = id++;
                mi.status = Status.Pass;

                DateTime intPayment = ((Bond)s).IntNextPaymentDate;
                double remainingdays = (intPayment - DateTime.Today).Days;
                mi.value = remainingdays.ToString() + "天";

                description = C_Desc_BondIntPayment.Replace("<1>", s.Name + "(" + s.Code + ")").Replace("<OP>", getOperatorName(para.Operator));
                switch (s.Type)
                {
                    case SecurityType.Bond:
                        if (this.compareValue(para.Operator, remainingdays, failedthreshhold))
                        {
                            //检查fail放在前
                            //  确保当failthreshhold=warningthreshhold时，显示fail 
                            mi.threshhold = failedthreshhold.ToString("N0") + "天";
                            mi.status = Status.Fail;
                            mi.description = description.Replace("<2>", intPayment.ToString("yyyy-MM-dd")).Replace("<3>", failedthreshhold + "天");
                        }
                        else if (this.compareValue(para.Operator, remainingdays, warningthreshhold))
                        {
                            mi.threshhold = warningthreshhold.ToString("N0") + "天";
                            mi.status = Status.Warning;
                            mi.description = description.Replace("<2>", intPayment.ToString("yyyy-MM-dd")).Replace("<3>", warningthreshhold + "天");
                        }
                        else
                        {
                            mi.threshhold = warningthreshhold.ToString("N0") + "天";
                            mi.status = Status.Pass;
                            mi.description = "";
                        }
                        break;
                    default:
                        //对于非债券类型，无需作错误提示，会在其他步骤中检验
                        break;
                }

                this.Report.Add(mi);
            }
        }

        /// <summary>
        /// 检查债券估值偏离
        /// </summary>
        protected virtual void CheckBondValueDeviation(CheckParameter para)
        {
            try
            {
                int order = C_Order_BondValueDeviation; ;
                string title = C_Title_BondValueDeviation;
                string description;

                double warningthreshhold = Convert.ToDouble(para.WarningValue);
                double failedthreshhold = Convert.ToDouble(para.FailedValue);

                MonitorItem mi;
                int id = 0;

                ASecurityGroup g = this.SecurityPortfolio.BondHoldings;
                if (g.SecurityList == null || g.SecurityList.Count == 0)
                    return;

                if (((Bond)g.SecurityList[0]).HistoryBondIntrinsicValue.OriginalTimeSeries.Count ==0)
                {
                    g.LoadData(DataInfoType.HistoryBondValue);
                }

                foreach (ASecurity s in g.SecurityList)
                {
                    mi = new MonitorItem();
                    mi.code = s.Code;
                    mi.name = s.Name;
                    mi.title = title;
                    mi.order = order;
                    mi.securityid = id++;
                    mi.status = Status.Pass;

                    double basevalue = s.Position.MarketValue / s.Position.Quantity;
                    double estimatevalue = 0;
                    if (((Bond)s).HistoryBondIntrinsicValue.AdjustedTimeSeries != null)
                        estimatevalue = ((HistoryItemBondValue)((Bond)s).HistoryBondIntrinsicValue.AdjustedTimeSeries[0]).ClearPrice;

                    double deviation = Math.Abs(estimatevalue / basevalue - 1);
                    mi.value = deviation.ToString("P2");

                    description = C_Desc_BondValueDeviation.Replace("<1>", s.Name + "(" + s.Code + ")").Replace("<OP>", getOperatorName(para.Operator));
                    description = description.Replace("<4>", basevalue.ToString("N3")).Replace("<5>", estimatevalue.ToString("N4"));
                    switch (s.Type)
                    {
                        case SecurityType.Bond:
                            if (this.compareValue(para.Operator, deviation, failedthreshhold))
                            {
                                //检查fail放在前
                                //  确保当failthreshhold=warningthreshhold时，显示fail 
                                mi.threshhold = failedthreshhold.ToString("P2");
                                mi.status = Status.Fail;
                                mi.description = description.Replace("<2>", deviation.ToString("P2")).Replace("<3>", failedthreshhold.ToString("P2"));
                            }
                            else if (this.compareValue(para.Operator, deviation, warningthreshhold))
                            {
                                mi.threshhold = warningthreshhold.ToString("P2");
                                mi.status = Status.Warning;
                                mi.description = description.Replace("<2>", deviation.ToString("P2")).Replace("<3>", warningthreshhold.ToString("P2"));
                            }
                            else
                            {
                                mi.threshhold = warningthreshhold.ToString("P2");
                                mi.status = Status.Pass;
                                mi.description = "";
                            }
                            break;
                        default:
                            //对于非债券类型，无需作错误提示，会在其他步骤中检验
                            break;
                    }

                    this.Report.Add(mi);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #endregion

        public DataTable GetPositionTable(SecurityType type)
        {
            if (this.SecurityPortfolio == null)
                return null;

            DataTable dt = this.SecurityPortfolio.GetPositionTable(type);
            dt.DefaultView.Sort = "MARKETVALUEPCT DESC";
            return dt;
        }

        public DataTable GetPortfolioIndicator()
        {
            return this.SecurityPortfolio.GetPortfolioIndicator();
        }
    }
}
