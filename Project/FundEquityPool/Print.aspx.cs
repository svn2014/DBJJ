using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Print : System.Web.UI.Page
{
    private static bool _isEquity = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string stockCode = Request["code"].ToString();
            string industryOrRating = Request["IndustryOrRating"].ToString();
            _isEquity = (Request["IsEquity"].ToString() == "1") ? true : false;
            string pool = Request["pool"].ToString();
            int analystId = Convert.ToInt32(Request["analystId"]);
            int hedgefundId = Convert.ToInt32(Request["hedgefundId"]);
            DateTime startDate, endDate;

            try
            {
                startDate = Convert.ToDateTime(Request["startdate"]);
                endDate = Convert.ToDateTime(Request["enddate"]);
            }
            catch (Exception)
            {
                startDate = new DateTime(1900,1,1);
                endDate = DateTime.Today;
            }

            if (!_isEquity)
            {
                GridViewPrint.Columns[2].HeaderText = "到期日";
            }

            this.ShowPrintForm(stockCode, industryOrRating, pool, analystId, _isEquity, startDate, endDate, hedgefundId);
        }
    }

    private DataTable _dtEquityPool = null;
    private void ShowPrintForm(string equityCode, string industryCode, string pool, int analystId, bool isEquity, DateTime startDate, DateTime endDate, int hedgefundId)
    {
        bool inBasePool = true, inCorePool = true, inRestPool = false, inProhPool = false;

        switch (pool.ToString())
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
            _dtEquityPool = DataService.GetInstance().GetFundSecurityPool(equityCode.Trim().ToUpper()
                                    , industryCode.ToString()
                                    , inBasePool, inCorePool, inRestPool, inProhPool
                                    , analystId
                                    , isEquity
                                    , startDate
                                    , endDate
                                    , hedgefundId
                                    );                        

            GridViewPrint.DataSource = _dtEquityPool;
            GridViewPrint.DataBind();
            spErrMsg.InnerText = "共" + _dtEquityPool.Rows.Count + "条记录!";
        }
        catch (Exception ex)
        {
            spErrMsg.InnerText = ex.Message;
        }
    }
}