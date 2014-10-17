using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PortfolioEvaluation;

public partial class TransactionReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                LabelStatus.Visible = false;

                string fundCode = Request["code"].ToString();
                _htmloutput = HTMLOutput.GetActiveInstance(fundCode);

                if (_htmloutput == null)                    
                {
                    LabelStatus.Text = "必须从《持仓与归因分析》中调用本页面！";
                    LabelStatus.Visible = true;
                }
                else
                {
                    //基本信息
                    spFundInfo.InnerHtml = _htmloutput.GetFundInfo();

                    //交易明细
                    GridViewTradingDetail.DataSource = _htmloutput.GetTransactionTable();
                    GridViewTradingDetail.DataBind();

                    //申赎明细
                    GridViewSubscribeRedeem.DataSource = _htmloutput.GetSubscribeRedeemTable();
                    GridViewSubscribeRedeem.DataBind();
                }
            }
            catch (Exception ex)
            {
                LabelStatus.Text = ex.Message;
                LabelStatus.Visible = true;
            }
        }
    }

    //如果有活动的对象，则调用
    private static HTMLOutput _htmloutput = null;
    private void formatGridValue(GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                //0值不显示
                if (e.Row.Cells[i].Text == "0.00%" || e.Row.Cells[i].Text == "0.00")
                    e.Row.Cells[i].Text = "-";
            }
        }
    }

    protected void GridViewTradingDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        formatGridValue(e);
    }

    protected void GridViewTradingDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ((GridView)sender).PageIndex = e.NewPageIndex;
        ((GridView)sender).DataSource = _htmloutput.GetTransactionTable();
        ((GridView)sender).DataBind();
    }
    protected void GridViewSubscribeRedeem_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ((GridView)sender).PageIndex = e.NewPageIndex;
        ((GridView)sender).DataSource = _htmloutput.GetSubscribeRedeemTable();
        ((GridView)sender).DataBind();
    }
    protected void GridViewSubscribeRedeem_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        formatGridValue(e);
    }
}