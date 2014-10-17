using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using DBService;
using GZBService;
using CHService;

/// <summary>
///DataService 的摘要说明
/// </summary>
public class DataService
{
    private static DataService _Instance;
    public static DataService GetInstance()
    {
        if (_Instance == null)
            _Instance = new DataService();

        return _Instance;
    }

    public DataTable GetGZB(DateTime startDate, DateTime endDate, string fundCode, string itemCode)
    {
        try
        {
            ServiceSoapClient client = new ServiceSoapClient();
            DataSet ds = client.GetGzb("fadata", "fa*0926", startDate, endDate, fundCode, itemCode);

            if (ds.Tables.Count > 0)
                return ds.Tables[0];
        }
        catch (Exception ex)
        {
            throw new Exception("" + ex.Message);
        }

        return null;
    }
    
    public DataTable GetIndexWeights(string code, DateTime endDate)
    {
        DateTime monthEndDate = new DateTime(endDate.Year, endDate.Month, 1).AddDays(-1);
        CHServiceClient client = new CHServiceClient();
        return client.GetIndexWeights(code, monthEndDate).Tables[0];
    }

    public DataTable GetIndexPrices(DateTime startDate, DateTime endDate, string code)
    {
        CHServiceClient client = new CHServiceClient();
        return client.GetIndexPrices(startDate, endDate, code).Tables[0];
    }

    public DataTable GetStockPrices(DateTime startDate, DateTime endDate)
    {
        CHServiceClient client = new CHServiceClient();
        DataSet dsPrices;
        DataTable dtOutput = null;

        int i = 0, pageSize = 20;
        while (startDate.AddDays(pageSize * i) <= endDate)
        {
            DateTime start, end;
            start = (startDate.AddDays(i * pageSize));
            end = (startDate.AddDays(pageSize * (i + 1) - 1) > endDate) ? endDate : startDate.AddDays(pageSize * (i + 1) - 1);
            dsPrices = client.GetStockPrices(start, end, null);

            if (dtOutput == null)
                dtOutput = dsPrices.Tables[0].Copy();
            else
            {
                foreach (DataRow oRow in dsPrices.Tables[0].Rows)
                {
                    dtOutput.ImportRow(oRow);
                }
            }

            i++;
        }

        return dtOutput;
    }

    public DataTable GetAShareStockList(string code)
    {
        CHServiceClient client = new CHServiceClient();
        return client.GetAShareStockList(code, null, null, "1").Tables[0];
    }

    public DataTable GetChinaBondList(string code)
    {
        CHServiceClient client = new CHServiceClient();

        if(code.Contains(","))
            return client.GetChinaBondsList(code).Tables[0];
        else
            return client.GetChinaBondList(code,null).Tables[0];
    }

    public DataTable GetFreeFloatCapital(DateTime endDate, string code)
    {
        CHServiceClient client = new CHServiceClient();
        return client.GetFreeFloatCapital(endDate, code).Tables[0];
    }

    public DataTable GetSWIndustryList()
    {
        CHServiceClient client = new CHServiceClient();
        return client.GetSWIndustryList().Tables[0];
    }

    public DataTable GetTradingDaysList(DateTime start, DateTime end)
    {
        CHServiceClient client = new CHServiceClient();
        return client.GetTradingDays(start, end).Tables[0];
    }

    private DataTable _PortfoliloList = null;
    public DataTable GetPortfolioList()
    {
        //if (_PortfoliloList == null)
        //{
            DBServiceClient client = new DBServiceClient();
            _PortfoliloList = client.GetPortfolios(null).Tables[0];
        //}

        return _PortfoliloList;
    }

    public DataTable GetTebonPortfolios(bool isTebon)
    {
        DBServiceClient client = new DBServiceClient();
        string sql = @"select * from db5_t_portfoliolist t where IsActive='1' ";
        if (isTebon)
        {
            sql += " AND istebon ='1' ";
        }
        else
        {
            sql += " AND istebon <>'1' ";
        }

        sql += " ORDER BY  assettype, startdate";

        DataSet ds = client.ExecuteSQL(sql);
        return ds.Tables[0];
    }

    public DataTable GetPortfolioBenchmarkList(string portfolioCode)
    {
        DBServiceClient client = new DBServiceClient();
        return client.GetPortfolioBenchmarks(portfolioCode).Tables[0];
    }

    public DataTable GetFundBenchmark(string fundcode, string dates)
    {
        CHServiceClient client = new CHServiceClient();
        string sql = @"Select Symbol, Tdate, FundBDY1 AS BMYield, FundBDY2 AS BMIndex
                        From FundBDY 
                        Where symbol IN ('" + fundcode + @"')
                        AND Tdate IN (" + dates + @")
                        Order by symbol, tdate Desc";

        DataSet ds = client.ExecuteSQL(sql);

        return ds.Tables[0];
    }

    public DataTable GetBlockTrade(DateTime start, DateTime end)
    {
        CHServiceClient client = new CHServiceClient();
        DataTable dt = client.GetBlockTrade(start, end, null).Tables[0];
        dt.DefaultView.Sort = "trade_dt desc, s_block_amount desc";
        return dt.DefaultView.ToTable();
    }

    public DataTable GetMarginTrade(DateTime start, DateTime end)
    {
        CHServiceClient client = new CHServiceClient();
        DataTable dt = client.GetMarginTrade(start, end, null).Tables[0];
        dt.DefaultView.Sort = "trade_dt desc, s_margin_salesofborrowedsec desc";
        return dt.DefaultView.ToTable();
    }

    public DataTable GetStrangeTrade(DateTime start, DateTime end)
    {
        CHServiceClient client = new CHServiceClient();
        DataTable dt = client.GetStrangeTrade(start, end, null).Tables[0];
        dt.DefaultView.Sort = "s_strange_enddate desc, s_info_windcode, s_strange_type, s_strange_trade, s_strange_buyamount desc, s_strange_sellamount desc";
        return dt.DefaultView.ToTable();
    }

    public DataTable GetRestrictStock(DateTime start, DateTime end)
    {
        end = end.AddDays(90);
        CHServiceClient client = new CHServiceClient();
        DataTable dt = client.GetRestrictStock(start, end, null).Tables[0];
        dt.DefaultView.Sort = "s_info_listdate, s_share_lst_amount desc";
        return dt.DefaultView.ToTable();
    }

    //首发，增发
    public DataTable GetIPOSEO(DateTime start, DateTime end)
    {
        CHServiceClient client = new CHServiceClient();
        DataTable dt = client.GetIPOSEO(start, end, null).Tables[0];
        dt.DefaultView.Sort = "s_fellow_offeringdate desc, s_info_windcode";
        return dt.DefaultView.ToTable();
    }

    public string GetAuthorizationCode()
    {
        DBServiceClient client = new DBServiceClient();
        return client.GetAuthrizationCode();
    }

    public DataTable GetMarketNews(List<string> codeList)
    {
        DateTime end = DateTime.Today;
        DateTime start = DateTime.Today.AddDays(-14);

        CHServiceClient client = new CHServiceClient();

        if (codeList == null || codeList.Count == 0)
            return null;

        DataTable dtOutput = null;
        string codes = "";

        foreach (string code in codeList)
        {
            codes += code + ",";
        }

        dtOutput = client.GetMarketNews(start, end, codes.Substring(0, codes.Length - 1)).Tables[0];

        if (dtOutput != null)
        {
            //dtOutput.DefaultView.Sort = "trade_dt desc, s_info_windcode";
            dtOutput = dtOutput.DefaultView.ToTable();
        }

        return dtOutput;
    }

    public DataTable GetMarketNewsContent(string newsId)
    {
        CHServiceClient client = new CHServiceClient();
        DataTable dt = client.GetMarketNewsContent(newsId).Tables[0];
        return dt;
    }

    public string GetEquityCodesInPool()
    { 
        DateTime start = new DateTime(1970,1,1);
        DateTime end = DateTime.Today;
        DataTable dt = GetFundSecurityPool(null, null, true, false, false, false, 0, true, start, end, 0);

        if (dt == null || dt.Rows.Count == 0)
            return "";

        string codes = "";
        foreach (DataRow oRow in dt.Rows)
        {
            codes += oRow["s_info_windcode"].ToString() + ",";
        }
        codes = codes.Substring(0, codes.Length - 1);

        return codes;
    }

    private DataTable GetFundSecurityPool(string stockCode, string industryOrRating, bool inBasePool, bool inCorePool, bool inRestPool, bool inProhPool, int analystId, bool isEquity, DateTime startdate, DateTime enddate, int hedgefundId)
    {
        try
        {
            DBServiceClient client = new DBServiceClient();
            ProcedureParameter[] paraList = new ProcedureParameter[11];

            if (stockCode == null || stockCode.Length == 0 || stockCode == "*")
                stockCode = "%";
            stockCode = stockCode.Replace("*", "%");

            if (industryOrRating == null || industryOrRating.Length == 0 || industryOrRating == "*")
                industryOrRating = "%";
            industryOrRating = industryOrRating.Replace("*", "%");

            paraList[1] = new ProcedureParameter(); paraList[1].Name = "i_stockcode"; paraList[1].Value = stockCode.ToUpper(); paraList[1].Direction = ParameterDirection.Input; paraList[1].Type = ProcedureParameter.DBType.NVarChar;
            paraList[2] = new ProcedureParameter(); paraList[2].Name = "i_inbasepool"; paraList[2].Value = inBasePool ? "1" : null; paraList[2].Direction = ParameterDirection.Input; paraList[2].Type = ProcedureParameter.DBType.Char;
            paraList[3] = new ProcedureParameter(); paraList[3].Name = "i_incorepool"; paraList[3].Value = inCorePool ? "1" : null; paraList[3].Direction = ParameterDirection.Input; paraList[3].Type = ProcedureParameter.DBType.Char;
            paraList[4] = new ProcedureParameter(); paraList[4].Name = "i_inprohpool"; paraList[4].Value = inProhPool ? "1" : null; paraList[4].Direction = ParameterDirection.Input; paraList[4].Type = ProcedureParameter.DBType.Char;
            paraList[5] = new ProcedureParameter(); paraList[5].Name = "o_cursor"; paraList[5].Type = ProcedureParameter.DBType.Cursor; paraList[5].Direction = ParameterDirection.Output;
            paraList[6] = new ProcedureParameter(); paraList[6].Name = "i_analystid"; paraList[6].Value = analystId; paraList[6].Direction = ParameterDirection.Input; paraList[6].Type = ProcedureParameter.DBType.Int;
            paraList[7] = new ProcedureParameter(); paraList[7].Name = "i_inrestpool"; paraList[7].Value = inRestPool ? "1" : null; paraList[7].Direction = ParameterDirection.Input; paraList[7].Type = ProcedureParameter.DBType.Char;
            paraList[8] = new ProcedureParameter(); paraList[8].Name = "i_startdate"; paraList[8].Value = startdate.ToString("yyyyMMdd"); paraList[8].Direction = ParameterDirection.Input; paraList[8].Type = ProcedureParameter.DBType.NVarChar;
            paraList[9] = new ProcedureParameter(); paraList[9].Name = "i_enddate"; paraList[9].Value = enddate.ToString("yyyyMMdd"); paraList[9].Direction = ParameterDirection.Input; paraList[9].Type = ProcedureParameter.DBType.NVarChar;
            paraList[10] = new ProcedureParameter(); paraList[10].Name = "i_hedgefundid"; paraList[10].Value = hedgefundId; paraList[10].Direction = ParameterDirection.Input; paraList[10].Type = ProcedureParameter.DBType.Int;

            paraList[0] = new ProcedureParameter(); paraList[0].Value = industryOrRating; paraList[0].Direction = ParameterDirection.Input; paraList[0].Type = ProcedureParameter.DBType.NVarChar;

            DataSet ds = null;

            if (isEquity)
            {
                paraList[0].Name = "i_swindustry";
                ds = client.ExecuteStoredProcedure("db2_proc_selectfundequitypool", paraList);
            }
            else
            {
                paraList[0].Name = "i_rating";
                ds = client.ExecuteStoredProcedure("db2_proc_selectfundbondpool", paraList);
            }

            return ds.Tables[0];
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}