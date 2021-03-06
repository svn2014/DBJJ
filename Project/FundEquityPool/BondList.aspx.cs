﻿using System;
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
            DataTable dt = DataService.GetInstance().GetIndustryCodes();

            DropDownListPoolType.Items.Add(new ListItem("基础库", "1000"));
            DropDownListPoolType.Items.Add(new ListItem("核心库", "1100"));
            DropDownListPoolType.Items.Add(new ListItem("限制库", "0010"));
            DropDownListPoolType.Items.Add(new ListItem("禁止库", "0001"));
            DropDownListPoolType.Items.Add(new ListItem("所有库", "*"));

            DataTable dtAnalyst = DataService.GetInstance().GetAnalyst();
            DropDownListAnalyst.DataSource = dtAnalyst;
            DropDownListAnalyst.DataValueField = "AnalystId";
            DropDownListAnalyst.DataTextField = "AnalystName";
            DropDownListAnalyst.DataBind();
            DropDownListAnalyst.Items.Insert(0, new ListItem("--任意--", "0"));

            DataTable dtHedgeFund = DataService.GetInstance().GetHedgefundList();
            DropDownListHedgefund.DataSource = dtHedgeFund;
            DropDownListHedgefund.DataValueField = "HedgeFundId";
            DropDownListHedgefund.DataTextField = "HedgeFundName";
            DropDownListHedgefund.DataBind();

            TextBoxStartDate.Text = "2012-1-1";
            TextBoxEndDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
        }
    }

    private static DataTable _dtBondPool = null;
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        bool inBasePool = true, inCorePool = true, inRestPool=false, inProhPool=false;
        int hedgefundId = Convert.ToInt16(DropDownListHedgefund.SelectedValue);

        switch (DropDownListPoolType.SelectedValue.ToString())
        {
            case "1100"://核心库
                inBasePool = true;
                inCorePool = true;
                inRestPool = false;
                inProhPool = false;
                break;
            case "1000"://基础库
                inBasePool = true;
                inCorePool = false;
                inRestPool = false;
                inProhPool = false;
                break;
            case "0010"://限制库
                inBasePool = false;
                inCorePool = false;
                inRestPool = true;
                inProhPool = false;
                break;
            case "0001"://禁止库
                inBasePool = false;
                inCorePool = false;
                inRestPool = false;
                inProhPool = true;
                break;
            default:
                inBasePool = false;
                inCorePool = false;
                inRestPool = false;
                inProhPool = false;
                break;
        }

        try
        {
            _dtBondPool = DataService.GetInstance().GetFundSecurityPool(TextBoxStockcode.Text.ToString().Trim().ToUpper()
                                    , TextBoxRating.Text
                                    , inBasePool, inCorePool, inRestPool, inProhPool
                                    , Convert.ToInt16(DropDownListAnalyst.SelectedValue)
                                    , false
                                    , Convert.ToDateTime(TextBoxStartDate.Text)
                                    , Convert.ToDateTime(TextBoxEndDate.Text)
                                    , hedgefundId
                                    );

            foreach (DataRow oRow in _dtBondPool.Rows)
            {
                oRow["URL"] = "Detail.aspx?code=" + oRow["s_info_windcode"].ToString()
                    + "&IndustryOrRating=" + oRow["SW_IND_NAME1"].ToString()
                    + "&hedgefundId=" + hedgefundId
                    + "&IsEquity=0";
            }

            GridViewFundEquityPool.DataSource = _dtBondPool;
            GridViewFundEquityPool.DataBind();

            lblMsg.Text = "共" + _dtBondPool.Rows.Count + "条记录";
            aPrint.HRef = "Print.aspx?code=" + TextBoxStockcode.Text.ToString().Trim().ToUpper()
                                    + "&IndustryOrRating=" + TextBoxRating.Text
                                    + "&pool=" + DropDownListPoolType.SelectedValue.ToString()
                                    + "&analystId=" + DropDownListAnalyst.SelectedValue
                                    + "&hedgefundId=" + hedgefundId
                                    + "&IsEquity=0"
                                    + "&startdate=" + TextBoxStartDate.Text
                                    + "&enddate=" + TextBoxEndDate.Text
                                    ;
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
        }        
    }

    protected void GridViewFundEquityPool_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ((GridView)sender).PageIndex = e.NewPageIndex;
        ((GridView)sender).DataSource = _dtBondPool;
        ((GridView)sender).DataBind();
    }
}
