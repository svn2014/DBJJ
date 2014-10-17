using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class QuotePrint : System.Web.UI.Page
{
    #region Load
    private void LoadIPOInfo(string code)
    {
        DataTable dt = DataService.GetInstance().GetIPOs(code);

        if (dt.Rows.Count > 0 && code.Length>0)
        {
            DataRow row = dt.Rows[0];
            lblSName.Text = row["SNAME"].ToString();
            lblCode2.Text = "(" + row["PurchaseCodeOnNet"].ToString() + ")";
            lblQuoteStart.Text = Convert.ToDateTime(row["QuoteStart"]).ToString("yyyy-MM-dd");
            lblQuoteEnd.Text = Convert.ToDateTime(row["QuoteEnd"]).ToString("yyyy-MM-dd");
            PrimaryUnderwriter.Text = row["PrimaryUnderwriter"].ToString();
            Contacts.Text = row["Contacts"].ToString();
            Phone.Text = row["Phones"].ToString();
        }
        else
        {
            lblCode2.Text = "";
            lblSName.Text = "";
            lblQuoteStart.Text = "";
            lblQuoteEnd.Text = "";
            PrimaryUnderwriter.Text = "";
            Contacts.Text = "";
            Phone.Text = "";
        }
    }

    private void LoadAnalystInfo(int analystid)
    {
        DataTable dt = DataService.GetInstance().GetAnalyst();
        dt.DefaultView.RowFilter = "AnalystId=" + analystid;
        dt = dt.DefaultView.ToTable(); 

        if (dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];
            lblAnaPho.Text = row["Phone"].ToString();
            lblFax.Text = row["Fax"].ToString();
            lblEmail.Text = row["Email"].ToString();
        }
        else
        {
            lblAnaPho.Text = "";
            lblFax.Text = "";
            lblEmail.Text = "";
        }
    }

    private void LoadFundInfo(string code, bool isUpdateNAV)
    {
        DateTime start = DateTime.Today.AddDays(-30);
        DateTime end = DateTime.Today.AddDays(-1);

        double nav = Convert.ToDouble(lblFundNAVNum.Text);
        string navDate = lblFundNAVDate.Text, trustee = lblTrustee.Text;
        double price = 0, volume = 0, amount = 0, percent=0;

        if (code.Length > 0)
        {
            if (isUpdateNAV)
            {
                //GZB
                DataTable dtGZB = DataService.GetInstance().GetGZB(start, end, code, "701基金资产净值：");
                dtGZB.DefaultView.Sort = "FDate DESC";
                dtGZB = dtGZB.DefaultView.ToTable();

                if (dtGZB.Rows.Count > 0)
                {
                    DataRow row = dtGZB.Rows[0];
                    nav = Convert.ToDouble(row["FZqsz"]) / 10000;
                    navDate = "万元(" + Convert.ToDateTime(row["FDate"]).ToString("yyyy-MM-dd") + ")";
                }

                //Trustee
                DataTable dtFunds = DataService.GetInstance().GetPortfolioList();
                dtFunds.DefaultView.RowFilter = "CODE='" + code + "'";
                dtFunds = dtFunds.DefaultView.ToTable();
                if (dtFunds.Rows.Count > 0)
                {
                    DataRow row = dtFunds.Rows[0];
                    trustee = row["Trustee"].ToString();
                }
            }

            price = (tbPrice.Text.Length == 0) ? 0 : Convert.ToDouble(tbPrice.Text);
            volume = (tbVolume.Text.Length == 0) ? 0 : Convert.ToDouble(tbVolume.Text);
            amount = price * volume / 10000;
            if (nav > 0)
                percent = amount / nav;
        }
        else
        {
            nav = 0;
            amount = 0;
        }

        if (nav > 0)
        {
            lblFundNAV.Text = nav.ToString("N2");
            lblFundNAVDate.Text = navDate;
            lblFundNAVNum.Text = nav.ToString();
            lblTrustee.Text = trustee;
        }
        else
        {
            lblFundNAV.Text = "";
            lblFundNAVDate.Text = "";
            lblFundNAVNum.Text = "0";
            lblTrustee.Text = "";
        }

        if (amount > 0)
        {
            lblShares.Text = (volume / 10000).ToString("N2") + "万股"; ;
            lblAmount.Text = amount.ToString("N2");
            lblPercent.Text = percent.ToString("P2");
        }
        else
        {
            lblShares.Text = "";
            lblAmount.Text = "";
            lblPercent.Text = "";
        }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //绑定下拉表
            DataTable dtFunds = DataService.GetInstance().GetPortfolioList();
            dtFunds.DefaultView.RowFilter = "AssetType='E'";
            ddlFunds.DataSource = dtFunds;
            ddlFunds.DataValueField = "CODE";
            ddlFunds.DataTextField = "NAME";
            ddlFunds.DataBind();
            ddlFunds.Items.Insert(0, new ListItem("--未填--", ""));

            DataTable dtAnalysts = DataService.GetInstance().GetAnalyst();
            ddlAnalyst.DataSource = dtAnalysts;
            ddlAnalyst.DataValueField = "AnalystId";
            ddlAnalyst.DataTextField = "AnalystName";
            ddlAnalyst.DataBind();
            ddlAnalyst.Items.Insert(0, new ListItem("--未填--", "0"));

            //获取传递参数
            string fundcode = "";
            string symbol = "";
            string analyst = "0";

            if (Request.QueryString["fcode"] != null)
                fundcode = Request.QueryString["fcode"].ToString();

            if (Request.QueryString["scode"] != null)
                symbol = Request.QueryString["scode"].ToString();

            if (Request.QueryString["acode"] != null)
                analyst = Request.QueryString["acode"].ToString();

            int analystid = 0;
            try
            {
                analystid = Convert.ToInt16(analyst);
            }
            catch (Exception)
            {
                analystid = 0;
            }

            //设置数据
            this.tbCode.Text = symbol;
            this.LoadIPOInfo(symbol);

            this.ddlAnalyst.SelectedValue = analystid.ToString();
            this.LoadAnalystInfo(analystid);

            this.ddlFunds.SelectedValue = fundcode;
            this.LoadFundInfo(fundcode, true);
        }
    }
    protected void btnRefreshCode_Click(object sender, EventArgs e)
    {
        string code = tbCode.Text;
        LoadIPOInfo(code);
    }
    protected void ddlAnalyst_SelectedIndexChanged(object sender, EventArgs e)
    {
        int id = Convert.ToInt16(ddlAnalyst.SelectedValue);
        this.LoadAnalystInfo(id);
    }
    protected void ddlFunds_SelectedIndexChanged(object sender, EventArgs e)
    {
        string code = ddlFunds.SelectedValue;
        this.LoadFundInfo(code, true);
    }
    protected void btnRefreshAmount_Click(object sender, EventArgs e)
    {
        string code = ddlFunds.SelectedValue;
        this.LoadFundInfo(code, false);
    }
}