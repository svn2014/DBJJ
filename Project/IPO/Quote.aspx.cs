using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Quote : System.Web.UI.Page
{
    private void loadPages(string pagename, bool flgSeperateProduct)
    {
        string ipocodes = "", analystid = "0", fundcodes = "";
        List<string> lstFundCodes = new List<string>();

        //基金代码
        foreach (ListItem item in cblFunds.Items)
            if (item.Selected)
            {
                fundcodes += "," + item.Value;
                lstFundCodes.Add(item.Value);
            }
        fundcodes = fundcodes.Substring(1);

        //股票代码
        foreach (ListItem item in cblIPOs.Items)
            if (item.Selected)
                ipocodes += "," + item.Value;
        ipocodes = ipocodes.Substring(1);

        //经办人
        analystid = ddlAnalyst.SelectedValue;

        //Window
        if (flgSeperateProduct)
        {
            foreach (string code in lstFundCodes)
            {
                Response.Write("<script>window.open('" + pagename + "?fcode=" + code + "&scode=" + ipocodes + "&acode=" + analystid + "','_blank')</script>");
            }
        }
        else
        {
            Response.Write("<script>window.open('" + pagename + "?fcode=" + fundcodes + "&scode=" + ipocodes + "&acode=" + analystid + "','_blank')</script>");
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataTable dtIPOs = DataService.GetInstance().GetIPOCodes();
            cblIPOs.DataSource = dtIPOs;
            cblIPOs.DataValueField = "Symbol";
            cblIPOs.DataTextField = "Name";
            cblIPOs.DataBind();

            DataTable dtFunds = DataService.GetInstance().GetPortfolioList();
            dtFunds.DefaultView.RowFilter = "AssetType='E'";
            cblFunds.DataSource = dtFunds;
            cblFunds.DataValueField = "CODE";
            cblFunds.DataTextField = "NAME";
            cblFunds.DataBind();

            //if (cblFunds.Items.Count>0)
            //{
            //    foreach (ListItem item in cblFunds.Items)
            //    {
            //        item.Selected = true;
            //    }
            //}

            DataTable dtAnalysts = DataService.GetInstance().GetAnalyst();
            ddlAnalyst.DataSource = dtAnalysts;
            ddlAnalyst.DataValueField = "AnalystId";
            ddlAnalyst.DataTextField = "AnalystName";
            ddlAnalyst.DataBind();
            ddlAnalyst.Items.Insert(0, new ListItem("--未填--", "0"));
        }
    }
    protected void btnQuote_Click(object sender, EventArgs e)
    {
        this.loadPages("QuoteUnderNet.aspx", false);
    }
    protected void btnPurchase1_Click(object sender, EventArgs e)
    {
        this.loadPages("PurchaseUnderNet.aspx",false);
    }
    protected void btnPurchase2_Click(object sender, EventArgs e)
    {
        this.loadPages("PurchaseOnNet.aspx", false);
    }
}
