using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PortfolioEvaluation;
using System.Data;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
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

            TextBoxStartDate.Text = startdate.ToString("yyyy-MM-dd");
            TextBoxEndDate.Text = enddate.ToString("yyyy-MM-dd");

            _PortfolioList = DataService.GetInstance().GetPortfolioList();
            DropDownListFund.DataSource = _PortfolioList;
            DropDownListFund.DataValueField = "code";
            DropDownListFund.DataTextField = "name";
            DropDownListFund.DataBind();
            DropDownListFund.Items.Insert(0,new ListItem("--请选择--",""));
        }
    }

    private DataTable _PortfolioList = null;
}
