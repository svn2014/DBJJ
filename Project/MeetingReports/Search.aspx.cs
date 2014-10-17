using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class About : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TextBoxEndDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            TextBoxStartDate.Text = DateTime.Today.AddDays(-30).ToString("yyyy-MM-dd");
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        LoadReport();
    }

    private void LoadReport()
    {
        if (TextBoxKeywords.Text.Length == 0 || TextBoxKeywords.Text == "*" || TextBoxKeywords.Text == "%")
        {
            reportContent.InnerHtml = "请输入关键词";
            return;
        }

        DateTime startDate = Convert.ToDateTime(TextBoxStartDate.Text);
        DateTime endDate = Convert.ToDateTime(TextBoxEndDate.Text);
        string keywords = "%" + TextBoxKeywords.Text.Replace(" ", "%") + "%";

        DataTable oDT = DBService.GetSearchResult(startDate, endDate, keywords);

        if (oDT.Rows.Count > 0)
            reportContent.InnerHtml = CommonService.LoadReportHTML(oDT, endDate, true, keywords);
        else
            reportContent.InnerHtml = "(无)";
    }
}
