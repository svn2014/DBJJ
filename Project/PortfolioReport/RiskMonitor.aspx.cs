using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using RiskMonitor;
using Security;
using Security.Portfolio;

public partial class About : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DateTime enddate = DateTime.Today.AddDays(-1);
            DateTime startdate = enddate;

            switch (enddate.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    startdate = enddate.AddDays(-3);
                    break;
                case DayOfWeek.Tuesday:
                case DayOfWeek.Wednesday:
                case DayOfWeek.Thursday:
                case DayOfWeek.Friday:
                    startdate = enddate.AddDays(-1);
                    break;
                case DayOfWeek.Sunday:
                    enddate = enddate.AddDays(-2);
                    startdate = enddate.AddDays(-1);
                    break;
                default:
                    break;
            }

            TextBoxEndDate.Text = enddate.ToString("yyyy-MM-dd");
            lblcode.Text = DataService.GetInstance().GetAuthorizationCode();

            _PortfolioList = DataService.GetInstance().GetPortfolioList();
            DropDownListFund.DataSource = _PortfolioList;
            DropDownListFund.DataValueField = "code";
            DropDownListFund.DataTextField = "name";
            DropDownListFund.DataBind();
            DropDownListFund.Items.Insert(0, new ListItem("--请选择--", ""));
        }
    }

    private DataTable _PortfolioList = null;
    protected void btnRiskMonitor_Click(object sender, EventArgs e)
    {
        try
        {
            LabelStatus.Visible = false;
            string fundCode = this.DropDownListFund.SelectedValue.ToString();
            string fundName = this.DropDownListFund.SelectedItem.Text;
            string authCode = this.TextBoxAuthCode.Text;
            DateTime endDate = Convert.ToDateTime(this.TextBoxEndDate.Text);

            if (authCode != DataService.GetInstance().GetAuthorizationCode())
            {
                LabelStatus.Text = "授权码错误！";
                LabelStatus.Visible = true;
                return;
            }

            if (fundCode.Length > 0)
            {
                DataTable dtGZB = DataService.GetInstance().GetGZB(endDate,endDate,fundCode,"");
                List<CheckParameter> paralist = new List<CheckParameter>();

                ARiskMonitor monitor = new ARiskMonitor();
                switch (fundCode)
                {
                    case "000300":  //德利货币
                        monitor = new RiskMonitorMF();
                        paralist.Add(new CheckParameter(CheckMethod.CheckBondGroupAverageMaturity, CheckOperator.GreaterThan, 120, 110, "货币基金的平均剩余期限应当小于120天"));
                        paralist.Add(new CheckParameter(CheckMethod.CheckBondRating, CheckOperator.LessThan, "AAA", "AAA", "企业债评级应当不低于AAA"));
                        paralist.Add(new CheckParameter(CheckMethod.CheckBondMaturity, CheckOperator.GreaterThan, 397, 365, "剩余期限应当小于397天"));
                        paralist.Add(new CheckParameter(CheckMethod.CheckBondValueDeviation, CheckOperator.GreaterThan, 0.005, 0.00, "债券估值偏离应当低于0.5%"));
                        break;
                    case "167701":  //德信债券
                        paralist.Add(new CheckParameter(CheckMethod.CheckBondRating, CheckOperator.LessThan, "AA-", "AA-", "债券评级应当介于AA-~AA+"));
                        paralist.Add(new CheckParameter(CheckMethod.CheckBondRating, CheckOperator.GreaterThan, "AA+", "AA+", "债券评级应当介于AA-~AA+"));
                        paralist.Add(new CheckParameter(CheckMethod.CheckBondMaturity, CheckOperator.LessThan, 365, 365, "债券剩余期限应当介于1~7年"));
                        paralist.Add(new CheckParameter(CheckMethod.CheckBondMaturity, CheckOperator.GreaterThan, 365 * 7, 365 * 7, "债券剩余期限应当介于1~7年"));
                        //paralist.Add(new CheckParameter(CheckMethod.CheckBondValueDeviation, CheckOperator.GreaterThan, 0.01, 0.008, "债券估值偏离应当低于0.5%"));
                        break;
                    default:
                        break;
                }

                //构造组合
                monitor.BuildPortfolio(dtGZB, PortfolioDataAdapterType.YSS, endDate);                
                
                //通用预警
                paralist.Add(new CheckParameter(CheckMethod.CheckCashPct, CheckOperator.LessThan, 0.05, 0.08, "持有现金占基金资产净值比例不得低于5%"));
                paralist.Add(new CheckParameter(CheckMethod.CheckPositionPct, CheckOperator.GreaterThan, 0.1, 0.08, "单个证券持仓市值占基金资产净值比例不得超过10%"));
                paralist.Add(new CheckParameter(CheckMethod.CheckBondPayment, CheckOperator.LessThan, 15, 30, "债券下一付息日前15-30日提示"));
                paralist.Add(new CheckParameter(CheckMethod.CheckBondGroupPositionPct, CheckOperator.GreaterThan, 0.1, 0.08, "同一公司发行的债券持仓市值占基金资产净值比例不得超过10%"));
                paralist.Add(new CheckParameter(CheckMethod.CheckBondMaturity, CheckOperator.LessThan, 30, 60, "债券即将到期"));

                //运行
                monitor.Check(paralist);

                MonitorReport rpt = monitor.Report;
                DataTable dtReport = rpt.GetReportTable();
                DataView vwFailed = new DataView(dtReport, "STATUS='Fail'", "ORDER, CODE", DataViewRowState.CurrentRows);
                DataView vwWarining = new DataView(dtReport, "STATUS='Warning'", "ORDER, CODE", DataViewRowState.CurrentRows);
                                
                this.LabelFundName.Text = fundName + "(" + fundCode + ")";
                this.GridViewRiskMonitor_Failed.Caption = "警告：";
                this.GridViewRiskMonitor_Failed.DataSource = vwFailed;
                this.GridViewRiskMonitor_Failed.DataBind();
                this.GridViewRiskMonitor_Warning.Caption = "预警：";
                this.GridViewRiskMonitor_Warning.DataSource = vwWarining;
                this.GridViewRiskMonitor_Warning.DataBind();

                DataTable dtPortfolioIndicator = monitor.GetPortfolioIndicator();
                this.GridViewPortfolio.Caption = "组合指标";
                this.GridViewPortfolio.DataSource = dtPortfolioIndicator;
                this.GridViewPortfolio.DataBind();

                DataTable dtEquity = monitor.GetPositionTable(SecurityType.Equity);
                //DataView vwEquity = new DataView(dtEquity, "", "MARKETVALUEPCT DESC", DataViewRowState.CurrentRows);
                this.GridViewEquityPosition.Caption = "股票持仓（"+ monitor.SecurityPortfolio.EquityHoldings.Position.MarketValuePct.ToString("P2") +"）";
                this.GridViewEquityPosition.DataSource = dtEquity;
                this.GridViewEquityPosition.DataBind();

                DataTable dtBond = monitor.GetPositionTable(SecurityType.Bond);
                //DataView vwBond = new DataView(dtBond, "", "MARKETVALUEPCT DESC", DataViewRowState.CurrentRows);
                this.GridViewBondPosition.Caption = "债券持仓（" + monitor.SecurityPortfolio.BondHoldings.Position.MarketValuePct.ToString("P2") + "）";
                this.GridViewBondPosition.DataSource = dtBond;
                this.GridViewBondPosition.DataBind();

                DataTable dtFund = monitor.GetPositionTable(SecurityType.Fund);
                //DataView vwFund = new DataView(dtBond, "", "MARKETVALUEPCT DESC", DataViewRowState.CurrentRows);
                this.GridViewFundPosition.Caption = "基金持仓（" + monitor.SecurityPortfolio.FundHoldings.Position.MarketValuePct.ToString("P2") + "）";
                this.GridViewFundPosition.DataSource = dtFund;
                this.GridViewFundPosition.DataBind();

                DataTable dtDeposit = monitor.GetPositionTable(SecurityType.Deposit);
                //DataView vwDeposit = new DataView(dtDeposit, "", "MARKETVALUEPCT DESC", DataViewRowState.CurrentRows);
                this.GridViewDepositPosition.Caption = "存款（" + (monitor.SecurityPortfolio.DepositHoldings.Position.MarketValuePct + monitor.SecurityPortfolio.CashHoldings.Position.MarketValuePct).ToString("P2") + "）";
                this.GridViewDepositPosition.DataSource = dtDeposit;
                this.GridViewDepositPosition.DataBind();

                DataTable dtRevRepo = monitor.GetPositionTable(SecurityType.RevRepo);
                //DataView vwRevRepo = new DataView(dtRevRepo, "", "MARKETVALUEPCT DESC", DataViewRowState.CurrentRows);
                this.GridViewRevRepoPosition.Caption = "逆回购（" + monitor.SecurityPortfolio.RevRepoHoldings.Position.MarketValuePct.ToString("P2") + "）";
                this.GridViewRevRepoPosition.DataSource = dtRevRepo;
                this.GridViewRevRepoPosition.DataBind();

                DataTable dtTheRepo = monitor.GetPositionTable(SecurityType.TheRepo);
                //DataView vwTheRepo = new DataView(dtTheRepo, "", "MARKETVALUEPCT DESC", DataViewRowState.CurrentRows);
                this.GridViewTheRepoPosition.Caption = "正回购（" + (-monitor.SecurityPortfolio.TheRepoHoldings.Position.MarketValuePct).ToString("P2") + "）";
                this.GridViewTheRepoPosition.DataSource = dtTheRepo;
                this.GridViewTheRepoPosition.DataBind();
            }
            else
            {
                LabelStatus.Text = "未选择投资组合！";
                LabelStatus.Visible = true;
            }
        }
        catch (Exception ex)
        {
            LabelStatus.Text = ex.Message + "\r" + ex.StackTrace.ToString();
            LabelStatus.Visible = true;
        }
    }
}
