using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PortfolioAnalyze;

public partial class AttributionReport : System.Web.UI.Page
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
                    buildPerformanceReport(startDate, endDate, fundCode);
                    spFundInfo.InnerText = "基金代码："+ fundCode + " 期间：" + startDate.ToString("yyyy-MM-dd") + "至" + endDate.ToString("yyyy-MM-dd");
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

    private void buildPerformanceReport(DateTime reportStart, DateTime reportEnd, string fundCode)
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
            DataTable dtGZB = DataService.GetInstance().GetGZB(reportStart, reportEnd, fundCode, null);
            Performance p = new Performance();
            p.LoadData(dtGZB, Security.Portfolio.PortfolioDataAdapterType.YSS, reportStart, reportEnd, "000906");
            p.Attribute();
            
            //绩效归因
            GridViewIndustry.DataSource = p.GetAttributionTable();
            GridViewIndustry.DataBind();
        }
        catch (Exception ex)
        {
            LabelStatus.Visible = true;
            LabelStatus.Text = ex.Message;
        }
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
                    case "收益率":
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
                                    if (dVal >= 4.5 && text_col2.Length > 0)
                                        e.Row.Cells[i].BackColor = System.Drawing.Color.Yellow;
                                    else if (dVal >= 7.5 && text_col2.Length > 0)
                                        e.Row.Cells[i].BackColor = System.Drawing.Color.Red;
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
                            case "收益率":
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
                                    if (dVal < 10)
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
}