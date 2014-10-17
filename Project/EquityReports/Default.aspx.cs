using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataTable dtAnalyst = DBService.GetInstance().GetAnalyst();
            DataTable dtReportType1 = DBService.GetInstance().GetReportType(1);
            DataTable dtReportType2 = DBService.GetInstance().GetReportType(2);

            DropDownListAnalyst.DataSource = dtAnalyst;
            DropDownListAnalyst.DataValueField = "AnalystId";
            DropDownListAnalyst.DataTextField = "AnalystName";
            DropDownListAnalyst.DataBind();
            DropDownListAnalyst.Items.Insert(0, new ListItem("--任意--", "0"));

            DropDownListType1.DataSource = dtReportType1;
            DropDownListType1.DataValueField = "reportTypeId";
            DropDownListType1.DataTextField = "reportTypeName";
            DropDownListType1.DataBind();
            DropDownListType1.Items.Insert(0, new ListItem("--任意--", "0"));

            DropDownListType2.DataSource = dtReportType2;
            DropDownListType2.DataValueField = "reportTypeId";
            DropDownListType2.DataTextField = "reportTypeName";
            DropDownListType2.DataBind();
            DropDownListType2.Items.Insert(0, new ListItem("--任意--", "0"));

            DropDownListStockCode.Items.Add(new ListItem("--任意--", "*"));

            TextBoxStartDate.Text = DateTime.Today.AddDays(-90).ToString("yyyy-MM-dd");
            TextBoxEndDate.Text = DateTime.Today.ToString("yyyy-MM-dd");

            ParseURLParameters();
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            SearchReport(
                0,
                Convert.ToInt16(DropDownListAnalyst.SelectedValue),
                DropDownListStockCode.SelectedValue.ToString(),
                Convert.ToInt16(DropDownListType1.SelectedValue),
                Convert.ToInt16(DropDownListType2.SelectedValue),
                TextBoxReportName.Text,
                TextBoxKeywords.Text,
                Convert.ToDateTime(TextBoxStartDate.Text),
                Convert.ToDateTime(TextBoxEndDate.Text)
            );
        }
        catch (Exception ex)
        {
            lblStatus.Text = ex.Message;
        }
    }

    private void ParseURLParameters()
    {
        int reportId = Convert.ToInt32(Request["ReportId"]);

        if (reportId > 0)
        {
            DateTime startDate = new DateTime(1900,1,1);
            DateTime endDate = DateTime.Today;
            SearchReport(reportId, 0, "*", 0, 0, "*", "*", startDate, endDate);
        }
    }

    private void SearchReport(int reportId, int analystid, string stockCode, int reportType1, int reportType2, string reportName, string keywords, DateTime startDate, DateTime endDate)
    {
        lblStatus.Text = "";

        DataTable dtResult = DBService.GetInstance().GetEquityReport(
            reportId,
            analystid,
            stockCode,
            reportType1,
            reportType2,
            reportName,
            keywords,
            startDate,
            endDate
        );

        GridViewReportList.DataSource = dtResult;
        GridViewReportList.DataBind();

        lblStatus.Text = "共" + dtResult.Rows.Count + "条记录";
    }

    protected void DropDownListType1_SelectedIndexChanged(object sender, EventArgs e)
    {
        DBService.UpdateDDL(DropDownListStockCode, Convert.ToInt16(DropDownListType1.SelectedValue),1); 
    }

    protected void GridViewReportList_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int reportId = Convert.ToInt16(e.Keys[0].ToString());

        DBService.GetInstance().DeleteEquityReport(reportId);
    }

    protected void GridViewReportList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        
    }
}
