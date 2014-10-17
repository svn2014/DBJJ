using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class About : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TextBoxStartDate.Text = DateTime.Today.AddDays(-30).ToString("yyyy-MM-dd");
            TextBoxEndDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
        }
    }
}
