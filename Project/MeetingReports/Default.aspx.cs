using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TextBoxReportDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            LoadReport();
        }
    }

    protected void DropDownListReportDate_SelectedIndexChanged(object sender, EventArgs e)
    {

        LoadReport();
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        LoadReport();
    }

    private void LoadReport()
    {
        DateTime reportDate = Convert.ToDateTime(TextBoxReportDate.Text);
        DataTable oDT = DBService.GetReport(reportDate);
        reportContent.InnerHtml = CommonService.LoadReportHTML(oDT, reportDate, false,"");
    }

    protected void btnExportToWord_Click(object sender, EventArgs e)
    {
        string charSet = "GB2312";
        Response.Charset = charSet;
        Response.HeaderEncoding = System.Text.Encoding.GetEncoding(charSet);
        Response.ContentEncoding = System.Text.Encoding.GetEncoding(charSet);

        DateTime reportDate = Convert.ToDateTime(TextBoxReportDate.Text);
        string strYear = reportDate.ToString("yyyy");
        string strMonth = reportDate.ToString("MM");
        string strDay = reportDate.ToString("dd");

        string fileName = DBService.GetTitle(); 
        fileName = fileName.Replace("yyyy",strYear);
        fileName = fileName.Replace("yy", strYear.Substring(2, 2));
        fileName = fileName.Replace("MM", strMonth);
        fileName = fileName.Replace("dd", strDay);
        fileName += ".doc";

        Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
        Response.ContentType = "application/ms-word";
        this.EnableViewState = false;
        StringWriter tw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(tw);

        reportContent.RenderControl(hw);
        //this.Page.EnableViewState = false;
        //this.RenderControl(hw);

        Response.Write(tw.ToString());
        Response.End(); 
    }
}
