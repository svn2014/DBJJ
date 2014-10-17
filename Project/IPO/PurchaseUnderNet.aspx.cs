using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class PurchaseUnderNet : System.Web.UI.Page
{
    private void LoadAnalystInfo(int analystid)
    {
        DataTable dt = DataService.GetInstance().GetAnalyst();
        dt.DefaultView.RowFilter = "AnalystId=" + analystid;
        dt = dt.DefaultView.ToTable();

        if (dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];
            lblAnaPho.Text = row["Phone"].ToString();
            lblEmail.Text = row["Email"].ToString();
        }
        else
        {
            lblAnaPho.Text = "";
            lblEmail.Text = "";
        }
    }

    private void loadInvestmentDetail(DataTable dtIPOs, string fundcodes)
    {
        string[] codelist = fundcodes.Split(",".ToCharArray());
        string html = "";

        if (codelist.Length > 0)
        {
            DataTable dtIPOUndernet = DataService.GetInstance().GetIPOUndernet();
            foreach (string code in codelist)
            {
                html += this.getTableHtml(code, dtIPOs, dtIPOUndernet);
            }
        }

        this.investmentdetail.InnerHtml = html;
    }
    private string getTableHtml(string fundcode, DataTable dtIPOs, DataTable dtIPOUndernet)
    {
        DateTime start = DateTime.Today.AddDays(-30);
        DateTime end = DateTime.Today.AddDays(-1);
        double nav = 0;
        string navDate = "", trustee = "", fundName = "";

        #region "净值"
        DataTable dtGZB = DataService.GetInstance().GetGZB(start, end, fundcode, "701基金资产净值：");
        dtGZB.DefaultView.Sort = "FDate DESC";
        dtGZB = dtGZB.DefaultView.ToTable();

        if (dtGZB.Rows.Count > 0)
        {
            DataRow row = dtGZB.Rows[0];
            nav = Convert.ToDouble(row["FZqsz"]) / 10000;
            navDate = "万元(" + Convert.ToDateTime(row["FDate"]).ToString("yyyy-MM-dd") + ")";
        }
        #endregion

        #region 托管行
        DataTable dtFunds = DataService.GetInstance().GetPortfolioList();
        dtFunds.DefaultView.RowFilter = "CODE='" + fundcode + "'";
        dtFunds = dtFunds.DefaultView.ToTable();
        if (dtFunds.Rows.Count > 0)
        {
            DataRow row = dtFunds.Rows[0];
            fundName = row["Name"].ToString() + "(" + fundcode + ")";
            trustee = row["Trustee"].ToString();
        }
        #endregion

        #region 表头
        string table = @"
                <table width='100%' style='border:1px solid; font-size:small' cellspacing='0' cellpadding='0'>
                <tr>
                    <td colspan='9'><strong style='font-size: large; font-weight: bold'>" + fundName + @"</strong></td>
                </tr>
                <tr>
                    <td colspan='2'>净资产</td>
                    <td colspan='2'>" + nav.ToString("N2") + navDate + @"<input type='hidden' value='" + nav + @"' id='calnav" + fundcode + @"' /></td>
                    <td colspan='2'>托管银行</td>
                    <td colspan='3'>" + trustee + @"</td>
                </tr>
                <tr>
                    <td>是否参与</td>
                    <td>名称</td>
                    <td align='right'>发行价</td>
                    <td align='right'>询价股份(万股)</td>
                    <td align='right'>申购股份(万股)</td>
                    <td align='right'>申购金额(万元)</td>
                    <td align='right'>占比</td>
                    <td align='center'>是否参与过网下询价</td>
                    <td align='center'>托管行是否关联方</td>
                </tr>";
        #endregion

        #region 模板
        string tr = "";
        if (dtIPOs.Rows.Count > 0)
        {
            string radiostyle = " style='background-color: #FFFF00; font-size: large; font-weight: bold'";
            string unicode = fundcode + "_";
            foreach (DataRow row in dtIPOs.Rows)
            {
                //检查网下IPO
                dtIPOUndernet.DefaultView.RowFilter = "FundCode = '" + fundcode + "' AND StockCode='" + row["Symbol"].ToString() + "'";
                DataTable dtTmp = dtIPOUndernet.DefaultView.ToTable();

                unicode = fundcode + "_" + row["Symbol"].ToString();
                string trName = row["SName"].ToString();
                double trPrice = ((row["IssuePrice"] == DBNull.Value) ? 0 : Convert.ToDouble(row["IssuePrice"]));
                double dVol = 0;
                string strQuotedYes = "", strQuotedNo = "";

                if (dtTmp.Rows.Count > 0)
                {
                    //已参与网下
                    dVol = Convert.ToDouble(dtTmp.Rows[0]["volume"]);
                    strQuotedYes = " checked ";
                    strQuotedNo = "";
                }
                else
                {
                    //未参与网下
                    dVol = 0;
                    strQuotedYes = "";
                    strQuotedNo = " checked ";
                }

                tr += @"
                    <tr >
                        <td><input id='chkJoin" + unicode + @"' type='checkbox' checked " + radiostyle + @" onclick='joinIn(this,""" + unicode + @""");'>参与</input></td>
                        <td>" + trName + @"</td>
                        <td align='right'>" + trPrice.ToString("N2") + @"<input type='hidden' value='" + trPrice + @"' id='calPrice" + unicode + @"' /></td>
                        <td align='right'>
                            <input id='calVolume0" + unicode + @"' type='text' value='" + dVol + @"' style='width:80px;text-align:right; background-color: #FFFF00; font-size: large; font-weight: bold;' />
                        </td>
                        <td align='right'>
                            <input id='calVolume" + unicode + @"' type='text' value='" + dVol + @"' style='width:80px;text-align:right; background-color: #FFFF00; font-size: large; font-weight: bold;' />
                        </td>
                        <td align='right'><span id='calAmount" + unicode + @"'></span></td>
                        <td align='right'><span id='calPct" + unicode + @"'></span></td>
                        <td align='center'><input type='radio' name='group1_" + unicode + @"' " + radiostyle + @" " + strQuotedYes + @">是</input><input type='radio' name='group1_" + unicode + @"' " + radiostyle + strQuotedNo + @">否</input></td>
                        <td align='center'><input type='radio' name='group2_" + unicode + @"' " + radiostyle + @">是</input><input type='radio' name='group2_" + unicode + @"' " + radiostyle + @">否</input></td>
                    </tr>";
            }

            tr += @"<tr style='font-size: large; font-weight: bold' align='right'>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td>合计</td>
                        <td><span id='spTotalAmt" + fundcode + @"'>0</span></td>
                        <td><span id='spTotalPct" + fundcode + @"'>0</span></td>
                        <td></td>
                        <td></td>
                    </tr>";
        }
        #endregion
        
        #region 表尾
        table += tr + @"
                <tr style='height:60px'>
                    <td colspan='9' valign='top'>投资经理 签字/日期</td>
                </tr>
                </table>
                <hr />";
        #endregion

        return table;
    }

    private void loadUnderwriters(string ipocodes)
    {
        DataTable dt = DataService.GetInstance().GetUnderwriters(ipocodes);
        string code = "", precode = "";
        string ul = "";

        if (dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                code = row["Symbol"].ToString();
                if (code != precode)
                {
                    precode = code;
                    ul += "</ul>" + row["SNAME"].ToString() + "(" + row["Symbol"].ToString() + ")<ul>";
                }

                ul += "<li>" + row["TYPE"].ToString() + "：" + row["Underwriter"].ToString() + "</li>";
            }

            ul = ul.Substring(5);
        }
        else
        {
            ul = "";
        }

        this.divUnderwriter.InnerHtml = ul;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //获取传递参数
            string fundcodes = "";
            string ipocodes = "";
            string analystid = "0";

            if (Request.QueryString["fcode"] != null)
                fundcodes = Request.QueryString["fcode"].ToString();

            if (Request.QueryString["scode"] != null)
                ipocodes = Request.QueryString["scode"].ToString();

            if (Request.QueryString["acode"] != null)
                analystid = Request.QueryString["acode"].ToString();

            //存储
            this.hFundCodes.Value = fundcodes;
            this.hIPOCodes.Value = ipocodes;

            //新股代码
            ipocodes = "('" + ipocodes.Replace(",", "','") + "')";
            DataTable dtIPOs = DataService.GetInstance().GetIPOs("");
            dtIPOs.DefaultView.RowFilter = "Symbol IN " + ipocodes;
            dtIPOs = dtIPOs.DefaultView.ToTable();
            GridViewIPOInfo.DataSource = dtIPOs;
            GridViewIPOInfo.DataBind();

            //经办人
            DataTable dtAnalysts = DataService.GetInstance().GetAnalyst();
            ddlAnalyst.DataSource = dtAnalysts;
            ddlAnalyst.DataValueField = "AnalystId";
            ddlAnalyst.DataTextField = "AnalystName";
            ddlAnalyst.DataBind();
            ddlAnalyst.Items.Insert(0, new ListItem("--未填--", "0"));
            ddlAnalyst.SelectedValue = analystid;
            this.LoadAnalystInfo(Convert.ToInt16(analystid));

            //日期
            tbDate.Text = DateTime.Today.ToString("yyyy-MM-dd");

            //投资方案
            this.loadInvestmentDetail(dtIPOs, fundcodes);
            this.loadUnderwriters(ipocodes);
        }
    }
    protected void ddlAnalyst_SelectedIndexChanged(object sender, EventArgs e)
    {
        int id = Convert.ToInt16(ddlAnalyst.SelectedValue);
        this.LoadAnalystInfo(id);
    }
}