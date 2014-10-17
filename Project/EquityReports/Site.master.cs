using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SiteMaster : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //string currIP = Request.ServerVariables.Get("Local_Addr").ToString();
            //string dynamicIP = DBService.GetInstance().GetDynmicServerIP();

            //if (currIP != dynamicIP && dynamicIP != "*" && currIP != "127.0.0.1")
            //{
            //    string url = "http://" + dynamicIP + "/equityreport";
            //    Response.Redirect(url);
            //}

            //ReturnToHomePage.HRef = "http://" + DBService.GetInstance().GetHomeSiteIP();
        }
    }
}
