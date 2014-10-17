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
            DataTable dtHedgeFund = DataService.GetInstance().GetHedgefundList();
            DropDownListHedgefund.DataSource = dtHedgeFund;
            DropDownListHedgefund.DataValueField = "HedgeFundId";
            DropDownListHedgefund.DataTextField = "HedgeFundName";
            DropDownListHedgefund.DataBind();

            DropDownListSecurityType.Items.Add(new ListItem("--任意--", ""));
            DropDownListSecurityType.Items.Add(new ListItem("股票", "E"));
            DropDownListSecurityType.Items.Add(new ListItem("债券", "B"));

            TextBoxStartDate.Text = DateTime.Today.AddDays(-90).ToString("yyyy-MM-dd");
            TextBoxEndDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
        }
    }

    private static DataTable _dtEquityPool = null;
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        bool? isEquity = null;

        if (DropDownListSecurityType.SelectedValue == "E")
            isEquity = true;
        else if (DropDownListSecurityType.SelectedValue == "B")
            isEquity = false;
        else
            isEquity = null;

        int hedgefundId = Convert.ToInt16(DropDownListHedgefund.SelectedValue);

        try
        {
            _dtEquityPool = DataService.GetInstance().GetFundSecurityPoolHistory(TextBoxStockcode.Text.ToString().Trim().ToUpper()
                                    , Convert.ToDateTime(TextBoxStartDate.Text)
                                    , Convert.ToDateTime(TextBoxEndDate.Text)
                                    , isEquity,hedgefundId
                                    );

            //foreach (DataRow oRow in _dtEquityPool.Rows)
            //{
            //    oRow["URL"] = "Detail.aspx?code=" + oRow["s_info_windcode"].ToString()
            //        + "&IndustryOrRating=" + oRow["s_info_indexcode"].ToString()
            //        + "&IsEquity=1";
            //}

            GridViewFundEquityPool.DataSource = _dtEquityPool;
            GridViewFundEquityPool.DataBind();

            lblMsg.Text = "共" + _dtEquityPool.Rows.Count + "条记录";
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
        }        
    }

    protected void GridViewFundEquityPool_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ((GridView)sender).PageIndex = e.NewPageIndex;
        ((GridView)sender).DataSource = _dtEquityPool;
        ((GridView)sender).DataBind();
    }
}
