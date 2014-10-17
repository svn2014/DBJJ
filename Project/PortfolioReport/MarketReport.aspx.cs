using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PortfolioEvaluation;
using System.Data;

public partial class MarketReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                LabelStatus.Visible = false;

                DateTime startDate = Convert.ToDateTime(Request["startdate"]);
                DateTime endDate = Convert.ToDateTime(Request["enddate"]);

                loadMarketData(startDate,endDate);

                //根据基金持仓筛选
                try
                {
                    string fundCode = Request["code"].ToString();
                    string authCode = Request["authcode"].ToString();

                    if (fundCode.Length > 0)
                        updateMarketDataByGZB(fundCode, authCode);
                }
                catch (Exception)
                { }

                //根据股票代码筛选
                try
                {
                    string equityCode = "";
                    equityCode = Request["equitycode"].ToString();  //e.g.600001.SH,000001.SZ

                    //从股票池中抽取股票列表
                    if (equityCode.ToLower() == "pool")
                        equityCode = DataService.GetInstance().GetEquityCodesInPool();

                    if (equityCode.Trim().Length > 0)
                        updateMarketDataByEquityCode(equityCode.Trim());
                }
                catch (Exception)
                { }


                bindData();
            }
            catch (Exception ex)
            {
                LabelStatus.Text = ex.Message;
                LabelStatus.Visible = true;
            }
        }
    }

    private static DataTable _IPOSEO = null;
    private static DataTable _BlockTrade = null;
    private static DataTable _MarginTrade = null;
    private static DataTable _StrangeTrade = null;
    private static DataTable _RestrictStock = null;
    private static DataTable _MarketNews = null;
    private void updateMarketDataByGZB(string fundcode, string authcode)
    {
        if (authcode != DataService.GetInstance().GetAuthorizationCode())
        {
            LabelStatus.Text = "无效的授权码";
            LabelStatus.Visible = true;
            return;
        }

        DateTime lastTradeDay = DateTime.Today.AddDays(-1);

        //倒序排列交易日，获得最后一个交易日
        DataTable dtTradingDays = DataService.GetInstance().GetTradingDaysList(DateTime.Today.AddDays(-10), DateTime.Today.AddDays(-1));
        dtTradingDays.DefaultView.Sort = "trade_days desc";
        dtTradingDays = dtTradingDays.DefaultView.ToTable();
        if (dtTradingDays != null && dtTradingDays.Rows.Count > 0)
        {
            string str = dtTradingDays.Rows[0]["trade_days"].ToString();
            lastTradeDay = new DateTime(
                                Convert.ToInt16(str.Substring(0,4)),
                                Convert.ToInt16(str.Substring(4,2)),
                                Convert.ToInt16(str.Substring(6,2))
                                );
        }

        //读取最后交易日持仓数据
        IFundDataAdapter IFDA = FundDataAdaptorFactory.GetAdaptor(FundDataAdaptorFactory.AdapterType.YSS);
        DataTable dtGZB = DataService.GetInstance().GetGZB(lastTradeDay, lastTradeDay, fundcode, null);
        Portfolio p = new Portfolio(fundcode, "", Portfolio.PortfolioType.MutualFund, Portfolio.PortfolioCategoryI.EquityFund, Portfolio.PortfolioCategoryII.OpenEnd);
        p.UpdatePositionByGZB(IFDA.BuildGZBList(dtGZB));

        List<AssetPosition> equityList = p.EquityPositionList;
        List<string> equityCodeList = new List<string>();
        if (equityList == null || equityList.Count == 0)
            return;

        //根据持仓数据修改显示项目
        string filter = "s_info_windcode in (";
        for (int i = 0; i < equityList.Count; i++)
        {
            filter += "'" + equityList[i].WindCode + "',";
            equityCodeList.Add(equityList[i].WindCode.Trim().ToUpper());
        }
        filter = filter.Substring(0, filter.Length - 1) + ")";

        _BlockTrade.DefaultView.RowFilter = filter;
        _MarginTrade.DefaultView.RowFilter = filter;
        _StrangeTrade.DefaultView.RowFilter = filter;
        _RestrictStock.DefaultView.RowFilter = filter;
        _IPOSEO.DefaultView.RowFilter = filter;
        _MarketNews = DataService.GetInstance().GetMarketNews(equityCodeList);
    }

    private void updateMarketDataByEquityCode(string equitycodelist)
    {
        List<string> equityCodeList = new List<string>();
        string[] codeList = equitycodelist.Split(",".ToCharArray());

        string filter = "s_info_windcode in (";
        for (int i = 0; i < codeList.Length; i++)
        {
            filter += "'" + codeList[i].Trim().ToUpper() + "',";
            equityCodeList.Add(codeList[i].Trim().ToUpper());
        }
        filter = filter.Substring(0, filter.Length - 1) + ")";

        _BlockTrade.DefaultView.RowFilter = filter;
        _MarginTrade.DefaultView.RowFilter = filter;
        _StrangeTrade.DefaultView.RowFilter = filter;
        _RestrictStock.DefaultView.RowFilter = filter;
        _IPOSEO.DefaultView.RowFilter = filter;
        _MarketNews = DataService.GetInstance().GetMarketNews(equityCodeList);
    }

    private void loadMarketData(DateTime start, DateTime end)
    {
        _BlockTrade = DataService.GetInstance().GetBlockTrade(start, end);
        _MarginTrade = DataService.GetInstance().GetMarginTrade(start, end);
        _StrangeTrade = DataService.GetInstance().GetStrangeTrade(start, end);
        _RestrictStock = DataService.GetInstance().GetRestrictStock(start, end);
        _IPOSEO = DataService.GetInstance().GetIPOSEO(start, end);
    }

    private void bindData()
    {
        GridViewBlockTrade.DataSource = _BlockTrade;
        GridViewBlockTrade.DataBind();

        GridViewMarginTrade.DataSource = _MarginTrade;
        GridViewMarginTrade.DataBind();

        GridViewStrangeTrade.DataSource = _StrangeTrade;
        GridViewStrangeTrade.DataBind();

        GridViewRestrictStock.DataSource = _RestrictStock;
        GridViewRestrictStock.DataBind();

        GridViewIPOSEO.DataSource = _IPOSEO;
        GridViewIPOSEO.DataBind();

        GridViewNews.DataSource = _MarketNews;
        GridViewNews.DataBind();
    }

    private void formatGridValue(object sender, GridViewRowEventArgs e)
    {
        GridView gv = ((GridView)sender);
        string gvName = gv.ID;

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                string colName = gv.Columns[i].HeaderText;
                string text = e.Row.Cells[i].Text.Trim().Replace("&nbsp;", "");
                DateTime dDate;
                string strVal = text;

                #region 全局效果
                switch (colName)
                {
                    //设置近日背景色
                    case "解禁日期":
                    case "公告日期":
                    case "日期":
                        if (text.Length > 0)
                        {
                            dDate = Convert.ToDateTime(text);
                            if ((dDate - DateTime.Today).Days >= -1)
                                e.Row.Cells[i].BackColor = System.Drawing.Color.Yellow;
                        }
                        break;

                    case "类型":
                        if (text == "3日")
                            e.Row.Cells[i].BackColor = System.Drawing.Color.Yellow;
                        break;

                    default:
                        break;
                }
                #endregion
                
            }
        }
    }

    protected void GridViewBlockTrade_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ((GridView)sender).PageIndex = e.NewPageIndex;
        ((GridView)sender).DataSource = _BlockTrade;
        ((GridView)sender).DataBind();
    }

    protected void GridViewMarginTrade_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ((GridView)sender).PageIndex = e.NewPageIndex;
        ((GridView)sender).DataSource = _MarginTrade;
        ((GridView)sender).DataBind();
    }
    protected void GridViewBlockTrade_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        formatGridValue(sender, e);
    }
    protected void GridViewMarginTrade_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        formatGridValue(sender, e);
    }
    protected void GridViewNews_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        formatGridValue(sender, e);
    }
    protected void GridViewNews_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ((GridView)sender).PageIndex = e.NewPageIndex;
        ((GridView)sender).DataSource = _MarketNews;
        ((GridView)sender).DataBind();
    }
    protected void GridViewStrangeTrade_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        formatGridValue(sender, e);
    }
    protected void GridViewStrangeTrade_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ((GridView)sender).PageIndex = e.NewPageIndex;
        ((GridView)sender).DataSource = _StrangeTrade;
        ((GridView)sender).DataBind();
    }
    protected void GridViewRestrictStock_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        formatGridValue(sender, e);
    }
    protected void GridViewRestrictStock_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ((GridView)sender).PageIndex = e.NewPageIndex;
        ((GridView)sender).DataSource = _RestrictStock;
        ((GridView)sender).DataBind();
    }
    protected void GridViewIPOSEO_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        formatGridValue(sender, e);
    }
    protected void GridViewIPOSEO_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ((GridView)sender).PageIndex = e.NewPageIndex;
        ((GridView)sender).DataSource = _IPOSEO;
        ((GridView)sender).DataBind();
    }
}