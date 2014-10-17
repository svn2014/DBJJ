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
        if (!this.IsPostBack)
        {
            GridViewGroupByIndustry.DataSource = DataService.GetInstance().GetStatisticData(true);
            GridViewGroupByIndustry.DataBind();

            GridViewGroupByAnalyst.DataSource = DataService.GetInstance().GetStatisticData(false);
            GridViewGroupByAnalyst.DataBind();
        }
    }
}
