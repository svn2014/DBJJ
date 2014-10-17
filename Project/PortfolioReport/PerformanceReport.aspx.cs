using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PortfolioEvaluation;

public partial class PerformanceReport : System.Web.UI.Page
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
                string fundCode = Request["code"].ToString();
                string authCode = Request["authcode"].ToString();

                if (authCode != DataService.GetInstance().GetAuthorizationCode())
                {
                    LabelStatus.Text = "授权码错误！";
                    LabelStatus.Visible = true;
                    return;
                }

                if (fundCode.Length > 0)
                {
                    reportDIV.Visible = true;
                    buildPerformanceReport(startDate, endDate, fundCode);
                    aTradeDetail.HRef = "TransactionReport.aspx?code=" + fundCode;
                }
                else
                {
                    LabelStatus.Text = "未选择投资组合！";
                    LabelStatus.Visible = true;
                }
            }
            catch (Exception ex)
            {
                LabelStatus.Text = ex.Message + "\r" + ex.StackTrace.ToString();
                LabelStatus.Visible = true;
            }
        }
    }

    private static HTMLOutput _htmloutput = null;
    private void buildPerformanceReport(DateTime reportStart, DateTime reportEnd, string fundCode)
    {
        try
        {
            IFundDataAdapter IFDA = FundDataAdaptorFactory.GetAdaptor(FundDataAdaptorFactory.AdapterType.YSS);
            IMarketDataAdapter IMDA = MarketDataAdaptorFactory.GetAdaptor(MarketDataAdaptorFactory.AdapterType.Wind);

            //基金类型数据
            DataTable dtFund = DataService.GetInstance().GetPortfolioList();            
            DataRow oFundRow = null;
            if (dtFund != null)
            {
                for (int i = 0; i < dtFund.Rows.Count; i++)
                {
                    string fcode = dtFund.Rows[i]["Code"].ToString();
                    if (fcode == fundCode)
                    {
                        oFundRow = dtFund.Rows[i];
                        break;
                    }
                }
            }

            if (oFundRow == null)
            {
                LabelStatus.Text = "未知的代码！";
                return;
            }

            string fundName = oFundRow["Name"].ToString();
            string isHedgeFund = oFundRow["IsHedgeFund"].ToString();
            string assetType = oFundRow["AssetType"].ToString();
            string operationType = oFundRow["opType"].ToString();

            Portfolio.PortfolioType pType = Portfolio.PortfolioType.MutualFund;
            Portfolio.PortfolioCategoryI pCategoryI = Portfolio.PortfolioCategoryI.EquityFund;
            Portfolio.PortfolioCategoryII pCategoryII = Portfolio.PortfolioCategoryII.OpenEnd;

            switch (assetType)
            {
                case "E":   //Equity
                    pCategoryI = Portfolio.PortfolioCategoryI.EquityFund;
                    break;
                case "B":   //Bond
                    pCategoryI = Portfolio.PortfolioCategoryI.BondFund;
                    break;
                case "M":   //Monetary
                    pCategoryI = Portfolio.PortfolioCategoryI.MonetaryFund;
                    break;
                default:
                    break;
            }

            switch (operationType)
            {
                case "O":   //Open End
                    pCategoryII = Portfolio.PortfolioCategoryII.OpenEnd;
                    break;
                case "C":   //Close End
                    pCategoryII = Portfolio.PortfolioCategoryII.CloseEnd;
                    break;
                default:
                    break;
            }

            if (isHedgeFund == "1")
                pType = Portfolio.PortfolioType.HedgeFunds;
            else
                pType = Portfolio.PortfolioType.MutualFund;


            //market data
            DataTable dtGZB = DataService.GetInstance().GetGZB(reportStart, reportEnd, fundCode, null);
            DataTable dtTradingDays = DataService.GetInstance().GetTradingDaysList(reportStart, reportEnd);
            DataTable dtPriceInfo = DataService.GetInstance().GetStockPrices(reportStart, reportEnd);             //1Month:   2400 * 22 = 52800 Rows
            DataTable dtEquityInfo = DataService.GetInstance().GetAShareStockList(null);                          //Total:    2400 Rows
            DataTable dtCapitalInfo = DataService.GetInstance().GetFreeFloatCapital(reportEnd, null);             //Total:    2400 Rows
            DataTable dtIndustryInfo = DataService.GetInstance().GetSWIndustryList();                             //Total:      23 Rows

            //==============================
            //Benchmark
            //==============================
            DataTable dtBenchmarkList = DataService.GetInstance().GetPortfolioBenchmarkList(fundCode);
            string defaultBMCode = "000300.SH";
            BenchmarkPortfolio bmf = new BenchmarkPortfolio(reportStart, reportEnd, reportStart, reportEnd);
            if (dtBenchmarkList.Rows.Count > 0)
            {
                foreach (DataRow oRow in dtBenchmarkList.Rows)
                {
                    string benchmarkcode = "";
                    string benchmarkname = "";
                    double benchmarkweight = 0;
                    double constrate = 0;

                    if (oRow["BENCHMARKCODE"] != DBNull.Value)
                        benchmarkcode = oRow["BENCHMARKCODE"].ToString();
                    if (oRow["BENCHMARKNAME"] != DBNull.Value)
                        benchmarkname = oRow["BENCHMARKNAME"].ToString();
                    if (oRow["BENCHMARKWEIGHT"] != DBNull.Value)
                        benchmarkweight = Convert.ToDouble(oRow["BENCHMARKWEIGHT"]);
                    if (oRow["CONSTRATE"] != DBNull.Value)
                        constrate = Convert.ToDouble(oRow["CONSTRATE"]);

                    if (benchmarkcode.Length > 0)
                    {
                        //e.g. 沪深300指数
                        DataTable dtBenchmarkIndex = DataService.GetInstance().GetIndexPrices(reportStart, reportEnd, benchmarkcode);
                        bmf.AddBenchmarkIndex(new BenchmarkDef(benchmarkcode, benchmarkname, benchmarkweight, 0, IMDA.GetPriceList(dtBenchmarkIndex)));
                        defaultBMCode = benchmarkcode;
                    }
                    else
                    {
                        //e.g. 银行存款
                        bmf.AddBenchmarkIndex(new BenchmarkDef(null, benchmarkname, benchmarkweight, constrate, null));
                    }
                }
            }

            DataTable dtBMWeight = DataService.GetInstance().GetIndexWeights(defaultBMCode, reportEnd);  
            bmf.BuildBenchmarkComponents(IMDA.GetPositionList(dtBMWeight), IMDA.GetPriceList(dtPriceInfo));

            //==============================
            //Performance Evaluator   
            //==============================
            PerformanceEvaluator pe = new PerformanceEvaluator(reportStart, reportEnd, fundCode, fundName, pType, pCategoryI, pCategoryII);
            pe.BuildPortfolios(IFDA.BuildGZBList(dtGZB), IMDA.GetPositionList(dtEquityInfo), IMDA.GetPriceList(dtPriceInfo), IMDA.GetPositionList(dtCapitalInfo));
            
            //更新债券到期日等
            string bondcodelist = "";
            pe.GetBondPositions(out bondcodelist);
            if (bondcodelist.Length > 0)
            {
                DataTable dtBondInfo = DataService.GetInstance().GetChinaBondList(bondcodelist);
                pe.UpdateBondPositions(IMDA.GetPositionList(dtBondInfo));
            }

            //评估
            pe.Evaluate(bmf, IMDA.GetIndustryList(dtIndustryInfo));

            //==============================
            //Output
            //==============================
            _htmloutput = new HTMLOutput(pe);

            //基本信息
            spFundInfo.InnerHtml = _htmloutput.GetFundInfo();

            //流动性风险: 股票/债券
            GridViewEquityPositionDetail.DataSource = _htmloutput.GetEquityPositionTable();
            GridViewEquityPositionDetail.DataBind();
            GridViewBondPositionDetail.DataSource = _htmloutput.GetPureBondPositionTable();
            GridViewBondPositionDetail.DataBind();
            GridViewCBondPositionDetail.DataSource = _htmloutput.GetConvertableBondPositionTable();
            GridViewCBondPositionDetail.DataBind();
            GridViewCashPositionDetail.DataSource = _htmloutput.GetCashPositionTable();
            GridViewCashPositionDetail.DataBind();
            GridViewOtherPositionDetail.DataSource = _htmloutput.GetOtherPositionTable();
            GridViewOtherPositionDetail.DataBind();

            //绩效归因
            GridViewIndustry.DataSource = _htmloutput.GetIndustryTable();
            GridViewIndustry.DataBind();
        }
        catch (Exception ex)
        {
            LabelStatus.Visible = true;
            LabelStatus.Text = ex.Message;
        }
    }

    protected void GridViewPositionDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        ((GridView)sender).PageIndex = e.NewPageIndex;
        ((GridView)sender).DataSource = _htmloutput.GetEquityPositionTable();
        ((GridView)sender).DataBind();
    }
    protected void GridViewBondPositionDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ((GridView)sender).PageIndex = e.NewPageIndex;
        ((GridView)sender).DataSource = _htmloutput.GetPureBondPositionTable();
        ((GridView)sender).DataBind();
    }
    protected void GridViewCBondPositionDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ((GridView)sender).PageIndex = e.NewPageIndex;
        ((GridView)sender).DataSource = _htmloutput.GetConvertableBondPositionTable();
        ((GridView)sender).DataBind();
    }
    protected void GridViewOtherPositionDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ((GridView)sender).PageIndex = e.NewPageIndex;
        ((GridView)sender).DataSource = _htmloutput.GetOtherPositionTable();
        ((GridView)sender).DataBind();
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
                string text = e.Row.Cells[i].Text.Trim().Replace("&nbsp;","");
                string text_col2 = e.Row.Cells[1].Text.Trim().Replace("&nbsp;", "");    //第二列
                string text_col3 = e.Row.Cells[2].Text.Trim().Replace("&nbsp;", "");    //第三列
                double dVal = 0;
                string strVal = text;

                #region 全局效果
                switch (colName)
                {
                    //设置收益率颜色
                    case "收益率":
                    case "净价收益率":
                    case "全价收益率":
                    case "年化":
                    case "增加值":
                    case "组合回报":
                    case "基准回报":
                    case "配置回报":
                    case "选股回报":
                    case "交叉回报":
                    case "期间回报":
                        if (text.Length > 0)
                        {
                            strVal = text.Substring(0, text.Length - 1); //去掉%
                            dVal = Convert.ToDouble(strVal);
                            if (dVal > 0)
                                e.Row.Cells[i].ForeColor = System.Drawing.Color.Red;
                            else if (dVal < 0)
                                e.Row.Cells[i].ForeColor = System.Drawing.Color.DarkGreen;
                        }
                        break;
                        
                    //监视：流动周期超过0.3
                    case "流动周期":
                        if (text.Length > 0)
                        {
                            dVal = Convert.ToDouble(text);
                            if (dVal > 0.3)
                                e.Row.Cells[i].BackColor = System.Drawing.Color.Yellow;
                        }
                        break;

                    default:
                        break;
                }
                #endregion

                #region 股票债券持仓
                switch (gvName)
                {
                    case "GridViewEquityPositionDetail":
                    case "GridViewBondPositionDetail":
                    case "GridViewCBondPositionDetail":
                        switch (colName)
                        {
                            //监视：单个股票占总资产比例超过2%
                            case "占净值(%)":
                                if (text.Length > 0)
                                {
                                    strVal = text.Substring(0, text.Length - 1); //去掉%
                                    dVal = Convert.ToDouble(strVal);
                                    if (dVal >= 4.5 && text_col2.Length > 0)
                                        e.Row.Cells[i].BackColor = System.Drawing.Color.Yellow;
                                    else if (dVal >= 7.5 && text_col2.Length > 0)
                                        e.Row.Cells[i].BackColor = System.Drawing.Color.Red;
                                }
                                break;

                            case "到期日期":
                                try
                                {
                                    if (text.Length > 0)
                                    {
                                        DateTime maturityDate = Convert.ToDateTime(text);
                                        int daysBeforeMaturity = (maturityDate - DateTime.Today).Days;

                                        //30天内到期 或者 30内付息
                                        if (daysBeforeMaturity < 30 ||
                                            (daysBeforeMaturity - ((int)daysBeforeMaturity / 365) * 365) < 30
                                           )
                                            e.Row.Cells[i].BackColor = System.Drawing.Color.Yellow;

                                        //5天内到期 或者 5内付息
                                        if (daysBeforeMaturity <= 5 ||
                                            (daysBeforeMaturity - ((int)daysBeforeMaturity / 365) * 365) <= 5
                                           )
                                            e.Row.Cells[i].BackColor = System.Drawing.Color.LightCoral;
                                    }
                                }
                                catch (Exception)
                                {
                                    e.Row.Cells[i].BackColor = System.Drawing.Color.Yellow;
                                }
                                break;

                            default:
                                break;
                        }
                        break;

                    default:
                        break;
                }
                #endregion

                #region 纯债收益率
                switch (gvName)
                {
                    case "GridViewBondPositionDetail":
                        switch (colName)
                        {
                            //监视：纯债收益率为负
                            case "收益率":
                                if (text.Length > 0)
                                {
                                    strVal = text.Substring(0, text.Length - 1); //去掉%
                                    dVal = Convert.ToDouble(strVal);
                                    if (dVal < 0)
                                        e.Row.Cells[i].BackColor = System.Drawing.Color.Yellow;
                                }
                                break;

                            default:
                                break;
                        }
                        break;

                    default:
                        break;
                }
                #endregion

                #region 现金
                switch (gvName)
                {
                    case "GridViewCashPositionDetail":
                        switch (colName)
                        {
                            //监视：现金小于1000万或者10%
                            case "现金科目":
                                if (text.Trim() == "活期存款")
                                {
                                    //现金小于1000万
                                    strVal = text_col2.Replace(",", "");
                                    dVal = Convert.ToDouble(strVal);
                                    if (dVal <1000)
                                        e.Row.Cells[1].BackColor = System.Drawing.Color.Yellow;

                                    //现金小于10%
                                    strVal = text_col3.Substring(0, text_col3.Length - 1);
                                    dVal = Convert.ToDouble(strVal);
                                    if (dVal < 10)
                                        e.Row.Cells[2].BackColor = System.Drawing.Color.Yellow;
                                }
                                break;

                            default:
                                break;
                        }
                        break;

                    default:
                        break;
                }
                #endregion

                //0值不显示
                if (e.Row.Cells[i].Text == "0.00%" || e.Row.Cells[i].Text == "0.00")
                    e.Row.Cells[i].Text = "-";
            }
        }
    }

    protected void GridViewIndustry_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        formatGridValue(sender,e);
    }
    protected void GridViewEquityPositionDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        formatGridValue(sender,e);        
    }
    protected void GridViewBondPositionDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        formatGridValue(sender,e);
    }
    protected void GridViewCBondPositionDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        formatGridValue(sender, e);
    }
    protected void GridViewOtherPositionDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        formatGridValue(sender,e);
    }
}