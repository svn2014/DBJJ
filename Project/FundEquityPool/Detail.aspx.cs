using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class About : System.Web.UI.Page
{
    private static bool _isEquity = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            int hedgefundId = Convert.ToInt32(Request["hedgefundId"]);
            string strCode = Request["Code"].ToString().Trim().ToUpper();
            string industryOrRating = Request["IndustryOrRating"];
            _isEquity = (Request["IsEquity"].ToString() == "1") ? true : false;
            string stkName = "";
            string industryName = "";
            string strReason = "(无)";
            string strPoolName = "";
            string strAnalyst = "(未知)";

            if (strCode.Length == 0)
                return;

            try
            {
                DataTable dtPool = DataService.GetInstance().GetFundSecurityPool(strCode, null, false, false, false, false, 0, _isEquity, new DateTime(1900, 1, 1), DateTime.Today, hedgefundId);
                if (dtPool.Rows.Count > 0)
                {
                    stkName = dtPool.Rows[0]["s_info_name"].ToString();
                    industryName = dtPool.Rows[0]["SW_IND_NAME1"].ToString();
                    industryOrRating = dtPool.Rows[0]["s_info_indexcode"].ToString();
                    strReason = dtPool.Rows[0]["reason"].ToString();
                    strAnalyst = dtPool.Rows[0]["analystName"].ToString();

                    if (dtPool.Rows[0]["inbasepool"] != DBNull.Value)
                    {
                        strPoolName += "基础库 ";
                        LabelPoolName.ForeColor = System.Drawing.Color.Blue;
                    }

                    if (dtPool.Rows[0]["incorepool"] != DBNull.Value)
                    {
                        strPoolName += "核心库 ";
                        LabelPoolName.ForeColor = System.Drawing.Color.Blue;
                    }

                    if (dtPool.Rows[0]["inrestpool"] != DBNull.Value)
                    {
                        strPoolName += "限制库 ";
                        LabelPoolName.ForeColor = System.Drawing.Color.Red;
                    }

                    if (dtPool.Rows[0]["inprohpool"] != DBNull.Value)
                    {
                        strPoolName += "禁止库 ";
                        LabelPoolName.ForeColor = System.Drawing.Color.Red;
                    }

                    if (strPoolName.Trim().Length == 0)
                    {
                        strPoolName = "(未分配)";
                        LabelPoolName.ForeColor = System.Drawing.Color.Red;
                    }
                }

                LabelCode.Text = strCode;
                LabelName.Text = stkName;
                LabelIndustry.Text = industryName;
                TextBoxReason.Text = (strReason.Length==0)?"(无)":strReason;
                LabelPoolName.Text = strPoolName;
                LabelAnalyst.Text = strAnalyst;

                if (_isEquity)
                    spIndustryOrRating.InnerText = "行业";
                else
                    spIndustryOrRating.InnerText = "到期日";

                DataTable dtHistory = DataService.GetInstance().GetFundSecurityPoolHistory(strCode, new DateTime(1900, 1, 1), DateTime.Today, null, null);
                GridViewHistory.DataSource = dtHistory;
                GridViewHistory.DataBind();

                LoadReportTable(strCode, industryOrRating);
            }
            catch (Exception ex)
            {
                TextBoxReason.Text = ex.Message;
            }
        }
    }

    private void LoadReportTable(string strCode, string industryCode)
    {
        DataTable dtReports = DataService.GetInstance().GetReportList(strCode);       //公司研究报告
        DataTable dtReports2 = DataService.GetInstance().GetReportList(industryCode); //行业研究报告

        if (dtReports2 != null)
        {
            foreach (DataRow oRow in dtReports2.Rows)
            {
                DataRow newRow = dtReports.NewRow();

                //Add Industry reports
                foreach (DataColumn oCol in dtReports.Columns)
                    newRow[oCol.ColumnName] = oRow[oCol.ColumnName];

                dtReports.Rows.Add(newRow);
            }
        }

        //Provide report URL
        string reportURL = DataService.GetInstance().GetReportServerURL() + "?ReportId=";

        foreach (DataRow oRow in dtReports.Rows)
        {
            oRow["URL"] = reportURL + oRow["reportid"].ToString();
            //oRow["fileURL"] = reportURL + oRow["reportid"].ToString();
        }

        GridViewReports.DataSource = dtReports;
        GridViewReports.DataBind();
    }
}
