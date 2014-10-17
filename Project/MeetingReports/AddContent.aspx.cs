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
            DataTable oDT_Category = DBService.GetCategory();
            DataTable oDT_SubmitDate = DBService.GetSubmitDates();

            DropDownListCategory.DataSource = oDT_Category;
            DropDownListCategory.DataValueField = "CategoryId";
            DropDownListCategory.DataTextField = "CategoryName";
            DropDownListCategory.DataBind();
            DropDownListCategory.AutoPostBack = true;

            DropDownListSubmitDate.DataSource = oDT_SubmitDate;
            DropDownListSubmitDate.DataValueField = "SubmitDate";
            DropDownListSubmitDate.DataTextField = "SubmitDateText";
            DropDownListSubmitDate.DataBind();
            DropDownListSubmitDate.AutoPostBack = true;

            LoadContent();
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        DateTime submitDate = Convert.ToDateTime(DropDownListSubmitDate.SelectedValue);
        int categoryId = Convert.ToInt32(DropDownListCategory.SelectedValue);
        string content = TextBoxContent.Text;
        string keywords = TextBoxKeywords.Text;

        try
        {
            DBService.SubmitReport(submitDate, categoryId,keywords, content);
            lblSubmitInfo.Text = "提交成功";
        }
        catch (Exception ex)
        {
            lblSubmitInfo.Text = ex.Message;
        }        
    }
    protected void DropDownListSubmitDate_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadContent();
    }
    protected void DropDownListCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadContent();
    }

    private static bool _hasContent = false;
    private void LoadContent()
    {
        DateTime reportDate = Convert.ToDateTime(DropDownListSubmitDate.SelectedValue);
        int categoryId = Convert.ToInt16(DropDownListCategory.SelectedValue);

        DataTable oDT = DBService.GetContent(reportDate, categoryId);

        if (oDT.Rows.Count > 0)
        {
            DataRow oRow = oDT.Rows[0];

            TextBoxKeywords.Text = oRow["KeyWords"].ToString();
            TextBoxContent.Text = oRow["Content"].ToString();

            _hasContent = true;
        }
        else
        {
            TextBoxKeywords.Text = "";
            TextBoxContent.Text = "";
            _hasContent = false;
        }
    }
}
