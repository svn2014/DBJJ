using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class MarketNewsContent : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                string newsid = Request["newsid"].ToString();
                DataTable dt = DataService.GetInstance().GetMarketNewsContent(newsid);

                if (dt != null && dt.Rows.Count > 0)
                {
                    string title = dt.Rows[0]["TITLE"].ToString();
                    string medianame = dt.Rows[0]["Mediumname"].ToString();
                    string content = dt.Rows[0]["content"].ToString();

                    string textBody = "<B><H1>" + title + "</H1></B>";
                    textBody += "<B><h4>" + medianame + "</h4></B>";
                    textBody += "<p>" + content.Replace("\r", "</p><p>\r") + "</p>";

                    newscontent.InnerHtml = textBody;
                }
            }
            catch (Exception)
            {
                newscontent.InnerText = "无效的id";
            }            
        }
    }
}