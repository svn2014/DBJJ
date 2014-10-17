using System;
using System.Data;
using System.Web.UI.WebControls;
using PortfolioAnalyze;
using Security;
using Security.Portfolio;

public partial class PositionReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                LabelStatus.Visible = false;

                DateTime startDate = Convert.ToDateTime(Request["startdate"]);
                DateTime endDate = Convert.ToDateTime(Request["enddate"]);
                string fundCode = Request["code"].ToString();
                string authCode = Request["authcode"].ToString();

                if (authCode != DataService.GetInstance().GetAuthorizationCode())
                {
                    LabelStatus.Text = "授权码错误！";
                    LabelStatus.Visible = true;
                    return;
                }

                if (fundCode.Length > 0)
                {
                    reportDIV.Visible = true;
                    buildPositionReport(startDate, endDate, fundCode);
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

    private void buildPositionReport(DateTime reportStart, DateTime reportEnd, string fundCode)
    {
        try
        {
            //基金类型数据
            DataTable dtFund = DataService.GetInstance().GetPortfolioList();
            DataRow oFundRow = null;
            if (dtFund != null)
            {
                for (int i = 0; i < dtFund.Rows.Count; i++)
                {
                    string fcode = dtFund.Rows[i]["Code"].ToString();
                    if (fcode == fundCode)
                    {
                        oFundRow = dtFund.Rows[i];
                        break;
                    }
                }
            }

            if (oFundRow == null)
            {
                LabelStatus.Text = "未知的代码！";
                return;
            }

            string fundName = oFundRow["Name"].ToString();
            string benchmarkname = oFundRow["Benchmark"].ToString();
            string benchmarkcode = oFundRow["BenchmarkCode"].ToString();
            DataTable dtGZB = DataService.GetInstance().GetGZB(reportStart, reportEnd, fundCode, null);
            Performance p = new Performance();
            p.LoadData(dtGZB, Security.Portfolio.PortfolioDataAdapterType.YSS, reportStart, reportEnd, benchmarkcode);
            p.Attribute();

            //基本信息
            string html = this.getFundInfo(p, fundCode, fundName, benchmarkname);
            spFundInfo.InnerHtml = html;

            //归因分析
            GridViewIndustry.DataSource = p.GetAttributionTable();
            GridViewIndustry.DataBind();

            //持仓分析
            GridViewEquityPositionDetail.DataSource = p.GetPositionTable(SecurityType.Equity);
            GridViewEquityPositionDetail.DataBind();
            GridViewEquityPositionDetail.Caption = "股票持仓（"+p.PortfGroup.GetLatestPortfolio().EquityHoldings.Position.MarketValuePct.ToString("P2") +"）";

            GridViewBondPositionDetail.DataSource = p.GetPositionTable(SecurityType.Bond);
            GridViewBondPositionDetail.DataBind();
            GridViewBondPositionDetail.Caption = "债券持仓（" + p.PortfGroup.GetLatestPortfolio().BondHoldings.Position.MarketValuePct.ToString("P2") + "）";

            GridViewFundPositionDetail.DataSource = p.GetPositionTable(SecurityType.Fund);
            GridViewFundPositionDetail.DataBind();
            GridViewFundPositionDetail.Caption = "基金持仓（" + p.PortfGroup.GetLatestPortfolio().FundHoldings.Position.MarketValuePct.ToString("P2") + "）";

            GridViewRevRepoPositionDetail.DataSource = p.GetPositionTable(SecurityType.RevRepo);
            GridViewRevRepoPositionDetail.DataBind();
            GridViewRevRepoPositionDetail.Caption = "逆回购持仓（" + p.PortfGroup.GetLatestPortfolio().RevRepoHoldings.Position.MarketValuePct.ToString("P2") + "）";

            GridViewTheRepoPositionDetail.DataSource = p.GetPositionTable(SecurityType.TheRepo);
            GridViewTheRepoPositionDetail.DataBind();
            GridViewTheRepoPositionDetail.Caption = "正回购持仓（" + p.PortfGroup.GetLatestPortfolio().TheRepoHoldings.Position.MarketValuePct.ToString("P2") + "）";

            GridViewCashPositionDetail.DataSource = p.GetPositionTable(SecurityType.Deposit);
            GridViewCashPositionDetail.DataBind();
            GridViewCashPositionDetail.Caption = "现金持仓（" + p.PortfGroup.GetLatestPortfolio().CashHoldings.Position.MarketValuePct.ToString("P2") + "）";

            //交易记录
            GridViewTransactionDetail.DataSource = p.GetTransactionTable();
            GridViewTransactionDetail.DataBind();

            //消息
            GridViewMessages.DataSource = MessageManager.GetInstance().GetMessageTable();
            GridViewMessages.DataBind();
        }
        catch (Exception ex)
        {
            LabelStatus.Visible = true;
            LabelStatus.Text = ex.Message;
        }
    }

    private string _tablestyle = "style=\"border:1px solid black;\"";
    private string _cellstyle = "bgcolor='#507CD1' style='color: #FFFFFF'";
    private string getFundInfo(Performance perf, string code, string name, string benchmarkname)
    {
        Portfolio p = perf.PortfGroup.GetLatestPortfolio();
        Index b = perf.Benchmark;
        double benchmarkyield =0;
        if(b != null)
            benchmarkyield =b.Position.AccumulatedYieldIndex-1;

        string html = "<table width=\"100%\" " + this._tablestyle + ">";
        html += @"
                <tr>
                    <td width='10%' " + _cellstyle + @">组合名称</td>
                    <td width='40%'>" + name + "(" + code + @")</td>
                    <td width='10%' " + _cellstyle + @">组合收益率</td>
                    <td width='10%' align='right' >" + (p.Position.AccumulatedYieldIndex-1).ToString("P2") + @"</td>
                    <td width='10%' " + _cellstyle + @">基准收益率</td>
                    <td  align='right'>" + benchmarkyield.ToString("P2") + @"</td>
                </tr>
                <tr>
                    <td " + _cellstyle + @">业绩基准</td>
                    <td>" + benchmarkname + @"</td>
                    <td " + _cellstyle + @">股票仓位</td>
                    <td align='right'  style='background-color:Yellow' >" + p.EquityHoldings.Position.MarketValuePct.ToString("P2") + @"</td>
                    <td " + _cellstyle + @">股票收益率</td>
                    <td align='right' >" + (p.EquityHoldings.Position.AccumulatedYieldIndex-1).ToString("P2") + @"</td>
                </tr>
                <tr>
                    <td " + _cellstyle + @">组合份额</td>
                    <td>" + (p.Shares / 10000).ToString("N2") + @"万份 / "
                      + (p.NetAssetValue / 10000).ToString("N2") + @"万元 [份额/净值]"
                      + @"</td>
                    <td " + _cellstyle + @">债券仓位</td>
                    <td align='right' >" +  p.BondHoldings.Position.MarketValuePct.ToString("P2") + @"</td>
                    <td " + _cellstyle + @">债券收益率</td>
                    <td align='right' >" + (p.BondHoldings.Position.AccumulatedYieldIndex-1).ToString("P2") + @"</td>
                </tr>
                <tr>
                    <td " + _cellstyle + @">组合净值</td>
                    <td>" + p.UnitNetAssetValue.ToString("N4") + @"/"
                      + p.AccumUnitNetAssetValue.ToString("N4") + @" [单位净值/累计单位净值]"
                      + @"</td>
                    <td " + _cellstyle + @">现金比例</td>
                    <td align='right'>" + p.CashHoldings.Position.MarketValuePct.ToString("P2") + @"</td>
                    <td " + _cellstyle + @">报告期间</td>
                    <td align='right'>" + perf.StartDate.ToString("yyyy-MM-dd") + "至" + perf.EndDate.ToString("yyyy-MM-dd") + @"</td>
                </tr>";

        html += "</table>";
        return html;
    }


    private void formatGridValue(object sender, GridViewRowEventArgs e)
    {
        GridView gv = ((GridView)sender);
        string gvName = gv.ID;

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {                               
                string colName = gv.Columns[i].HeaderText;
                string text = e.Row.Cells[i].Text.Trim().Replace("&nbsp;","");
                string text_col2 = e.Row.Cells[1].Text.Trim().Replace("&nbsp;", "");    //第二列
                string text_col3 = e.Row.Cells[2].Text.Trim().Replace("&nbsp;", "");    //第三列
                double dVal = 0;
                string strVal = text;

                #region 全局效果
                switch (colName)
                {
                    //设置收益率颜色
                    case "收益率(%)":
                    case "净价收益率":
                    case "全价收益率":
                    case "年化":
                    case "增加值":
                    case "组合回报":
                    case "基准回报":
                    case "配置回报":
                    case "选股回报":
                    case "交叉回报":
                    case "期间回报":
                        if (text.Length > 0)
                        {
                            strVal = text.Substring(0, text.Length - 1); //去掉%
                            dVal = Convert.ToDouble(strVal);
                            if (dVal > 0)
                                e.Row.Cells[i].ForeColor = System.Drawing.Color.Red;
                            else if (dVal < 0)
                                e.Row.Cells[i].ForeColor = System.Drawing.Color.DarkGreen;
                        }
                        break;
                        
                    //监视：流动周期超过0.3
                    case "流动周期":
                        if (text.Length > 0)
                        {
                            dVal = Convert.ToDouble(text);
                            if (dVal > 0.3)
                                e.Row.Cells[i].BackColor = System.Drawing.Color.Yellow;
                        }
                        break;

                    default:
                        break;
                }
                #endregion

                #region 股票债券持仓
                switch (gvName)
                {
                    case "GridViewEquityPositionDetail":
                    case "GridViewBondPositionDetail":
                    case "GridViewCBondPositionDetail":
                        switch (colName)
                        {
                            //监视：单个股票占总资产比例超过2%
                            case "占净值(%)":
                                if (text.Length > 0)
                                {
                                    strVal = text.Substring(0, text.Length - 1); //去掉%
                                    dVal = Convert.ToDouble(strVal);
                                    if (dVal >= 8 && text_col2.Length > 0)
                                        e.Row.Cells[i].BackColor = System.Drawing.Color.Yellow;
                                }
                                break;

                            case "到期日期":
                                try
                                {
                                    if (text.Length > 0)
                                    {
                                        DateTime maturityDate = Convert.ToDateTime(text);
                                        int daysBeforeMaturity = (maturityDate - DateTime.Today).Days;

                                        //30天内到期 或者 30内付息
                                        if (daysBeforeMaturity < 30 ||
                                            (daysBeforeMaturity - ((int)daysBeforeMaturity / 365) * 365) < 30
                                           )
                                            e.Row.Cells[i].BackColor = System.Drawing.Color.Yellow;

                                        //5天内到期 或者 5内付息
                                        if (daysBeforeMaturity <= 5 ||
                                            (daysBeforeMaturity - ((int)daysBeforeMaturity / 365) * 365) <= 5
                                           )
                                            e.Row.Cells[i].BackColor = System.Drawing.Color.LightCoral;
                                    }
                                }
                                catch (Exception)
                                {
                                    e.Row.Cells[i].BackColor = System.Drawing.Color.Yellow;
                                }
                                break;

                            default:
                                break;
                        }
                        break;

                    default:
                        break;
                }
                #endregion

                #region 纯债收益率
                switch (gvName)
                {
                    case "GridViewBondPositionDetail":
                        switch (colName)
                        {
                            //监视：纯债收益率为负
                            case "收益率(%)":
                                if (text.Length > 0)
                                {
                                    strVal = text.Substring(0, text.Length - 1); //去掉%
                                    dVal = Convert.ToDouble(strVal);
                                    if (dVal < 0)
                                        e.Row.Cells[i].BackColor = System.Drawing.Color.Yellow;
                                }
                                break;

                            default:
                                break;
                        }
                        break;

                    default:
                        break;
                }
                #endregion

                #region 现金
                switch (gvName)
                {
                    case "GridViewCashPositionDetail":
                        switch (colName)
                        {
                            //监视：现金小于1000万或者10%
                            case "现金科目":
                                if (text.Trim() == "活期存款")
                                {
                                    //现金小于1000万
                                    strVal = text_col2.Replace(",", "");
                                    dVal = Convert.ToDouble(strVal);
                                    if (dVal <1000)
                                        e.Row.Cells[1].BackColor = System.Drawing.Color.Yellow;

                                    //现金小于10%
                                    strVal = text_col3.Substring(0, text_col3.Length - 1);
                                    dVal = Convert.ToDouble(strVal);
                                    if (dVal < 5)
                                        e.Row.Cells[2].BackColor = System.Drawing.Color.Yellow;
                                }
                                break;

                            default:
                                break;
                        }
                        break;

                    default:
                        break;
                }
                #endregion

                //0值不显示
                if (e.Row.Cells[i].Text == "0.00%" || e.Row.Cells[i].Text == "0.00")
                    e.Row.Cells[i].Text = "-";
            }
        }
    }

    protected void GridViewIndustry_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        formatGridValue(sender,e);
    }
    protected void GridViewEquityPositionDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        formatGridValue(sender,e);        
    }
    protected void GridViewBondPositionDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        formatGridValue(sender,e);
    }
    protected void GridViewFundPositionDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        formatGridValue(sender, e);
    }
    protected void GridViewCBondPositionDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        formatGridValue(sender, e);
    }
    protected void GridViewOtherPositionDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        formatGridValue(sender,e);
    }
    protected void GridViewTransactionDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //formatGridValue(sender,e);
    }
    protected void GridViewMessagesDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //formatGridValue(sender,e);
    }
}