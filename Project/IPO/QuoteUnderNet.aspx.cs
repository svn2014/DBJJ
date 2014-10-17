using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class QuoteUnderNet : System.Web.UI.Page
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

    private void loadDetail(DataTable dtIPOs, string fundcodes)
    {
        string[] codelist = fundcodes.Split(",".ToCharArray());
        string html = "";

        if (codelist.Length > 0)
        {
            foreach (string code in codelist)
            {
                html += this.getDetailTable(code, dtIPOs);
            }
        }

        this.investmentdetail.InnerHtml = html;
    }
    private string getDetailTable(string fundcode, DataTable dtIPOs)
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

        #region Table模版1
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
                    <td align='right'>申购量(万股)</td>
                    <td align='right'>申购金额(万元)</td>
                    <td align='right'>占比</td>
                    <td align='right'>预估配售上限</td>
                    <td align='right'>占比</td>
                    <td>托管行是否关联方</td>
                </tr>";
        #endregion

        #region Tr模板
        string tr = "";
        if (dtIPOs.Rows.Count > 0)
        {
            string radiostyle = " style='background-color: #FFFF00; font-size: large; font-weight: bold'";
            string unicode = fundcode + "_";
            foreach (DataRow row in dtIPOs.Rows)
            {
                unicode = fundcode + "_" + row["Symbol"].ToString();
                string trName = row["SName"].ToString();
                double minVolume = ((row["DownLimitVolUnderNet"] == DBNull.Value) ? 0 : Convert.ToDouble(row["DownLimitVolUnderNet"]));

                tr += @"
                    <tr>
                        <td><input id='chkJoin" + unicode + @"' type='checkbox' checked " + radiostyle + @" onclick='joinIn(this,""" + unicode + @""");'>参与</input></td>
                        <td>" + trName + @"</td>
                        <td align='right'><span id='spPrice" + unicode + @"'>0.00</span><input type='hidden' value='0' id='hPrice" + unicode + @"' /></td>
                        <td align='right'>
                            <input id='calVolume" + unicode + @"' type='text' value='" + minVolume + @"' style='width:80px;text-align:right; background-color: #FFFF00; font-size: large; font-weight: bold;' />
                        </td>
                        <td align='right'><span style='font-size: large; font-weight: bold' id='calAmount" + unicode + @"'></span></td>
                        <td align='right'><span id='calPct" + unicode + @"'></span></td>
                        <td align='right'><span id='calEstAmt" + unicode + @"'></span></td>
                        <td align='right'><span id='calEstPct" + unicode + @"'></span></td>
                        <td><input type='radio' name='group2_" + unicode + @"' " + radiostyle + @">是</input><input type='radio' name='group2_" + unicode + @"' " + radiostyle + @">否</input></td>
                    </tr>";
            }

            tr += @"<tr style='font-size: large; font-weight: bold' align='right'>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td>合计</td>
                        <td><span id='spTotalAmt" + fundcode + @"'>0</span></td>
                        <td><span id='spTotalPct" + fundcode + @"'>0</span></td>
                        <td><span id='spEstTotalAmt" + fundcode + @"'>0</span></td>
                        <td><span id='spEstTotalPct" + fundcode + @"'>0</span></td>
                        <td></td>
                    </tr>";
        }
        #endregion
        
        #region Table模版2
        table += tr + @"
                <tr>
                    <td colspan='4' valign='top'>
                        是否申请资金冻结：
                        <input id='chkFrozenYes" + fundcode + @"' type='radio' name='grpFrozen" + fundcode + @"' checked style='background-color: #FFFF00; font-size: large; font-weight: bold'>是</input>
                        <input id='chkFrozenNo" + fundcode + @"' type='radio' name='grpFrozen" + fundcode + @"' style='background-color: #FFFF00; font-size: large; font-weight: bold'>否</input>
                        <ul>
                            <li>冻结日：<input id='txtFrozenStart" + fundcode + @"' type='text' value='' style='width:100px;background-color: #FFFF00; font-size: large; font-weight: bold;' /></li>
                            <li>解冻日：<input id='txtFrozenEnd" + fundcode + @"' type='text' value='' style='width:100px;background-color: #FFFF00; font-size: large; font-weight: bold;' /></li>
                            <li>冻结资金作为申购款划付时自动解冻</li>
                            <li>未获配售而退回的申购款不再冻结</li>
                        </ul>
                    </td>
                    <td colspan='5' valign='top'>
                        不申请资金冻结请说明理由：<br/>
                        <div style='width: 100%' align='center'>
                            <textarea rows='8' style='border-style: none; width: 90%; overflow: hidden;background-color: #FFFF00;'></textarea>
                        </div>
                        投资总监意见：<br/><br/>
                    </td>
                </tr>
                <tr style='height:60px'>
                    <td colspan='9' valign='top'>投资经理 签字/日期</td>
                </tr>
                </table>
                <hr />";
        #endregion

        return table;
    }

    private void loadSummary(DataTable dtIPOs, string fundcodes)
    {
        string[] aryfundcodes = fundcodes.Split(",".ToCharArray());
        string html1 = "", html2 = "";

        if (aryfundcodes.Length > 0)
        {
            foreach (string code in aryfundcodes)
            {
                html1 += this.getSummaryTable1(code, dtIPOs);
            }
        }
        this.investmentsummary1.InnerHtml = html1;


        if (dtIPOs.Rows.Count > 0)
        {
            foreach (DataRow row in dtIPOs.Rows)
            {
                html2 += this.getSummaryTable2(aryfundcodes, row);
            }
        }
        this.investmentsummary2.InnerHtml = html2;
    }
    private string getSummaryTable1(string fundcode, DataTable dtIPOs)
    {
        string fundName = "";
        
        #region 基金名称
        DataTable dtFunds = DataService.GetInstance().GetPortfolioList();
        dtFunds.DefaultView.RowFilter = "CODE='" + fundcode + "'";
        dtFunds = dtFunds.DefaultView.ToTable();
        if (dtFunds.Rows.Count > 0)
        {
            DataRow row = dtFunds.Rows[0];
            fundName = row["Name"].ToString() + "(" + fundcode + ")";
        }
        #endregion

        #region Table模版1
        string table = @"
                <table style='border:1px solid; font-size:small' cellspacing='0' cellpadding='0' width='100%'>
                <tr>
                    <td>" + fundName + @"</td>
                    <td colspan='6'>" + fundcode + @"</td>
                </tr>
                <tr>
                    <td width='10%'></td>
                    <td width='15%'>股票名称</td>
                    <td width='15%'>股票代码</td>
                    <td width='15%' align='right'>申购价格</td>
                    <td width='15%' align='right'>申购数量(万股)</td>
                    <td align='right'>申购金额(万元)</td>
                    <td align='center'>操作记录</td>
                </tr>
                <tr>
                    <td></td>
                    <td colspan='6'><hr /></td>
                </tr>";
        #endregion

        #region Tr模板
        string tr = "";
        if (dtIPOs.Rows.Count > 0)
        {
            string unicode = "";
            foreach (DataRow row in dtIPOs.Rows)
            {
                unicode = fundcode + "_" + row["Symbol"].ToString();
                tr += @"
                    <tr id='trSummary1" + unicode + @"'>
                        <td></td>
                        <td>" + row["SName"].ToString() + @"</td>
                        <td>" + row["Symbol"].ToString() + "," + row["PurchaseCodeOnNet"].ToString() + @"</td>
                        <td align='right'><span id='spSPrice1" + unicode + @"'>0</span></td>
                        <td align='right'><span id='spSVolume1" + unicode + @"'>0</span></td>
                        <td align='right'><span id='spSAmount1" + unicode + @"'>0</span></td>
                        <td></td>
                    </tr>";
            }
        }
        #endregion

        #region Table模版2
        table += tr + @"</table>";
        #endregion

        return table;
    }
    private string getSummaryTable2(string[] aryfundcodes, DataRow rStock)
    {
        #region Table模版1
        string table = @"
                <table style='border:1px solid; font-size:small' cellspacing='0' cellpadding='0' width='100%'>
                <tr>
                    <td>" + rStock["SName"].ToString() + @"</td>
                    <td colspan='6'>" + rStock["Symbol"].ToString() + "," + rStock["PurchaseCodeOnNet"].ToString() + @"</td>
                </tr>
                <tr>
                    <td width='10%'></td>
                    <td width='15%'>组合名称</td>
                    <td width='15%'>组合代码</td>
                    <td width='15%' align='right'>申购价格</td>
                    <td width='15%' align='right'>申购数量(万股)</td>
                    <td align='right'>申购金额(万元)</td>
                    <td align='center'>操作记录</td>
                </tr>
                <tr>
                    <td></td>
                    <td colspan='6'><hr /></td>
                </tr>";
        #endregion

        #region Tr模板
        DataTable dtFunds = DataService.GetInstance().GetPortfolioList();
        string tr = "";
        if (aryfundcodes.Length > 0)
        {
            string unicode = "";
            foreach (string fundcode in aryfundcodes)
            {
                unicode = fundcode + "_" + rStock["Symbol"].ToString();
                string fundName = "";
                DataRow[] rows = dtFunds.Select("CODE='" + fundcode + "'");
                if (rows.Length > 0)
                    fundName = rows[0]["Name"].ToString();
                else
                    fundName = "找不到名称";

                tr += @"
                    <tr id='trSummary2" + unicode + @"'>
                        <td></td>
                        <td>" + fundName + @"</td>
                        <td>" + fundcode + @"</td>
                        <td align='right'><span id='spSPrice2" + unicode + @"'>0</span></td>
                        <td align='right'><span id='spSVolume2" + unicode + @"'>0</span></td>
                        <td align='right'><span id='spSAmount2" + unicode + @"'>0</span></td>
                        <td></td>
                    </tr>";
            }
        }
        #endregion

        #region Table模版2
        table += tr + @"</table>";
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
            dtIPOs.DefaultView.Sort = "Symbol";
            dtIPOs = dtIPOs.DefaultView.ToTable();
            GridViewIPOInfo.DataSource = dtIPOs;
            GridViewIPOInfo.DataBind();
            GridViewIPOQuote.DataSource = dtIPOs;
            GridViewIPOQuote.DataBind();

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
            this.loadDetail(dtIPOs, fundcodes);
            this.loadSummary(dtIPOs, fundcodes);

            //承销商
            this.loadUnderwriters(ipocodes);
        }
    }
    protected void ddlAnalyst_SelectedIndexChanged(object sender, EventArgs e)
    {
        int id = Convert.ToInt16(ddlAnalyst.SelectedValue);
        this.LoadAnalystInfo(id);
    }
}