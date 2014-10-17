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
            //DataTable dtFunds = DataService.GetInstance().GetPortfolioList();
            //dtFunds.DefaultView.RowFilter = "AssetType='E'";
            //cblFunds.DataSource = dtFunds;
            //cblFunds.DataValueField = "CODE";
            //cblFunds.DataTextField = "NAME";
            //cblFunds.DataBind();

            //DataTable dtAnalysts = DataService.GetInstance().GetAnalyst();
            //ddlAnalyst.DataSource = dtAnalysts;
            //ddlAnalyst.DataValueField = "AnalystId";
            //ddlAnalyst.DataTextField = "AnalystName";
            //ddlAnalyst.DataBind();
            //ddlAnalyst.Items.Insert(0, new ListItem("--未填--", "0"));
        }
    }
}
