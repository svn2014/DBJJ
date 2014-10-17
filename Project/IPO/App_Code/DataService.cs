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
    
    public DataTable GetTradingDaysList(DateTime start, DateTime end)
    {
        CHServiceClient client = new CHServiceClient();
        return client.GetTradingDays(start, end).Tables[0];
    }

    private DataTable _PortfoliloList = null;
    public DataTable GetPortfolioList()
    {
        DBServiceClient client = new DBServiceClient();
        _PortfoliloList = client.GetPortfolios(null).Tables[0];
        return _PortfoliloList;
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
        DateTime start = new DateTime(1970, 1, 1);
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

    public DataTable GetIPOs(string code)
    {
        string sqlDateRestrict = " AND C3.BEGINDATE > (sysdate - 100) ";
        if (code.Length > 0)
        {
            sqlDateRestrict = " AND (A.Symbol='" + code + @"' OR SYMBOL_COMP_CODE = '" + code + @"')";
        }

        CHServiceClient client = new CHServiceClient();
        string sql = @"SELECT A.Symbol,A.SName
            ,F.IndustryChg4 AS Industry
            ,F.IndustryChg8 AS IndustryCode
            ,H1.PE2_M1 AS IndPEOnStart
            ,H2.PE2_M1 AS IndPELatest
            ,G.Sibaseinfo8 AS IssuePrice
            ,G.Sibaseinfo9 AS TotalCapitalBeforeIPO
            ,G.Sibaseinfo10 AS ActualIssuedCapital
            ,SIAIPO78 AS ActualTransferredCapital
            ,SIAIPO2 AS PlannedIssueCapital
            ,SIAIPO77 AS PlannedTransferCapital
            ,Substr(F.IndustryChg4,1,4) AS Industry0
            ,SYMBOL_COMP_CODE AS PurchaseCodeOnNet
            ,C.BEGINDATE AS QuoteStart
            ,C.ENDDATE AS QuoteEnd
            ,C2.BEGINDATE AS PurchaseStart
            ,C2.ENDDATE AS PurchaseEnd
            ,C3.BEGINDATE AS PurchaseDate
            ,A.Listdate
            ,SIAIPO7 AS IssueVolumePlanOnNet
            ,SIAIPO4 AS IssueVolumePlanUnderNet
            ,SIAIPO39 AS DownLimitVolUnderNet
            ,SIAIPO57 AS ProgVolUnderNet
            ,SIAIPO40 AS UpLimitVolUnderNet
            ,Substr(D.EVENTPARTY5,1,4) AS PrimaryUnderwriter0
            ,D.EVENTPARTY5 AS PrimaryUnderwriter
            ,D.EVENTPARTY6 AS Contacts
            ,'见推介公告' AS Contacts0
            ,D.EVENTPARTY7 AS Phones
            ,'见推介公告' AS Phones0
            ,E.CNT AS OtherPrimaryUnderwriters
            ,SIAIPO42 AS UpLimitVolOnNet
            ,SIAIPO42*10000 AS UpLimitVolOnNet0
            FROM (SELECT * FROM SECURITYCODE WHERE STYPE='EQA') A 
            INNER JOIN
            (
              SELECT * 
              FROM IndustryChg A 
              WHERE PublishDate=
              (
                    SELECT MAX(PublishDate) 
                    FROM IndustryChg 
                    WHERE IndustryChg3='证监会行业分类(2012)'
                    AND A.CompanyCode=CompanyCode 
              ) AND IndustryChg3='证监会行业分类(2012)' 
            )F 
                ON A.COMPANYCODE=F.COMPANYCODE 
            LEFT JOIN (SELECT * FROM symbol_comp WHERE SYMBOL_COMP_TYPE='05')A2 
                  ON A.SYMBOL=A2.SYMBOL
            INNER JOIN SIAIPO B
                 ON A.COMPANYCODE=B.COMPANYCODE 
            LEFT JOIN (SELECT * FROM SIIDATE WHERE DATETYPECODE='0103')C
                 ON B.COMPANYCODE=C.COMPANYCODE AND B.SIBASEINFOID=C.SIBASEINFOID 
            LEFT JOIN (SELECT * FROM SIIDATE WHERE DATETYPECODE='0109')C2
                 ON B.COMPANYCODE=C2.COMPANYCODE AND B.SIBASEINFOID=C2.SIBASEINFOID 
            LEFT JOIN (SELECT * FROM SIIDATE WHERE DATETYPECODE='0117')C3
                 ON B.COMPANYCODE=C3.COMPANYCODE AND B.SIBASEINFOID=C3.SIBASEINFOID 
            LEFT JOIN (SELECT * FROM EVENTPARTY WHERE EVENTPARTY1='01' AND EVENTPARTY2 IN ('037'))D
                 ON D.COMPANYCODE=A.COMPANYCODE AND D.EVENTID=C.SIBASEINFOID
            LEFT JOIN (SELECT COMPANYCODE,EVENTID,COUNT(*) AS CNT FROM EVENTPARTY WHERE EVENTPARTY1='01' AND EVENTPARTY2 IN ('017') GROUP BY COMPANYCODE,EVENTID)E----副主承销商 
                 ON E.COMPANYCODE=A.COMPANYCODE AND E.EVENTID=C.SIBASEINFOID
            LEFT JOIN (SELECT * FROM SIBASEINFO WHERE FTYPE='01' AND STYPE='EQA') G
                 ON B.Companycode=G.Companycode AND B.Sibaseinfoid=G.Sibaseinfoid
            LEFT JOIN (SELECT * FROM CSI_PE_CSRC WHERE SType='1') H1
                 ON H1.IndustryCode = F.IndustryChg8 AND H1.TDATE = to_char(C.BEGINDATE,'yyyyMMdd')
            LEFT JOIN (SELECT * FROM CSI_PE_CSRC WHERE SType='1') H2
                 ON H2.IndustryCode = F.IndustryChg8 AND H2.TDATE = to_char(sysdate-1,'yyyyMMdd')
            WHERE 1=1
            " + sqlDateRestrict + @"
            ORDER BY Listdate DESC, C3.BEGINDATE DESC, A.Symbol";

        DataTable dtOutput = client.ExecuteSQL(sql).Tables[0];

        return dtOutput;
    }

    public DataTable GetUnderwriters(string codes)
    { 
        //codes=('000001','600000')

        CHServiceClient client = new CHServiceClient();
        string sql = @"SELECT A.Symbol
                    ,A.Sname
                    ,E.EVENTPARTY3 AS Type
                    ,E.EVENTPARTY5 AS Underwriter
                    FROM (SELECT * FROM SECURITYCODE WHERE STYPE='EQA') A 
                    INNER JOIN SIAIPO B
                         ON A.COMPANYCODE=B.COMPANYCODE 
                    LEFT JOIN (SELECT * FROM SIIDATE WHERE DATETYPECODE='0103')C
                         ON B.COMPANYCODE=C.COMPANYCODE AND B.SIBASEINFOID=C.SIBASEINFOID
                    LEFT JOIN (
                              SELECT * 
                                FROM EVENTPARTY 
                                WHERE EVENTPARTY1='01' 
                                AND EVENTPARTY2 IN ('017','037')
                              ) E
                         ON E.COMPANYCODE=A.COMPANYCODE AND E.EVENTID=C.SIBASEINFOID
                    WHERE 1=1
                    AND Symbol IN " + codes + @"
                    ORDER BY Listdate DESC,Symbol,EVENTPARTY3 DESC
                    ";

        DataTable dtOutput = client.ExecuteSQL(sql).Tables[0];
        return dtOutput;
    }

    public DataTable GetIPOUndernet()
    { 
        //获得网下参与新股申购的列表
        DBServiceClient client = new DBServiceClient();
        string sql = @"Select * 
                        From db6_t_ipo t
                        Where 1=1
                        AND (quotedate is null OR QuoteDate > sysdate-30)";

        DataSet ds = client.ExecuteSQL(sql);
        return ds.Tables[0];
    }

    public DataTable GetIPOCodes()
    {
        string sql = @"SELECT A.Symbol,A.SName,C3.BEGINDATE AS PurchaseDate
                ,A.Symbol || ' - ' || A.SName || ' | 询价:' || to_char(C.ENDDATE,'yyyy-MM-dd') || ' - 申购:' || to_char(C3.BEGINDATE,'yyyy-MM-dd') AS Name
            FROM (SELECT * FROM SECURITYCODE WHERE STYPE='EQA') A
            INNER JOIN SIAIPO B
                    ON A.COMPANYCODE=B.COMPANYCODE
            LEFT JOIN (SELECT * FROM SIIDATE WHERE DATETYPECODE='0103')C
                 ON B.COMPANYCODE=C.COMPANYCODE AND B.SIBASEINFOID=C.SIBASEINFOID 
            LEFT JOIN (SELECT * FROM SIIDATE WHERE DATETYPECODE='0117')C3
                    ON B.COMPANYCODE=C3.COMPANYCODE AND B.SIBASEINFOID=C3.SIBASEINFOID
            WHERE 1=1
            AND C3.BEGINDATE > (sysdate - 30) 
            ORDER BY C3.BEGINDATE DESC, A.Symbol
            ";

        CHServiceClient client = new CHServiceClient();
        DataTable dtOutput = client.ExecuteSQL(sql).Tables[0];
        return dtOutput;
    }

    public DataTable GetAnalyst()
    { 
        try
        {
            DataTable dtAnalyst;
            DBServiceClient client = new DBServiceClient();
            ProcedureParameter[] paraList = new ProcedureParameter[1];

            paraList[0] = new ProcedureParameter(); paraList[0].Name = "o_cursor"; paraList[0].Direction = ParameterDirection.Output; paraList[0].Type = ProcedureParameter.DBType.Cursor;

            DataSet ds = client.ExecuteStoredProcedure("db1_proc_selectanalyst", paraList);
            dtAnalyst = ds.Tables[0];
            return dtAnalyst;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable GetConvertables()
    {
        CHServiceClient client = new CHServiceClient();
        string sql = @"SELECT S.SYMBOL
                        ,S.SNAME
                        ,F.SYMBOL_COMP_CODE AS PurchaseCodeOnNet
                        ,BONDDT18 AS Stockcode
                        ,S2.Sname AS StockName
                        ,S.EXCHANGE
                        ,DECLAREDATE
                        ,BONDISSUE11 AS PaymentDate
                        ,BONDISSUE27 AS ListDate
                        ,BONDISSUE7 AS IssuePlan
                        ,BONDISSUE9 AS IssuePrice
                        ,BISSUE_BDC35 AS DownLimitOnNet
                        ,BISSUE_BDC36 AS UpLimitOnNet
                        ,BISSUE_BDC44 AS DownLimitUnderNet
                        ,BISSUE_BDC45 AS StepUnderNet
                        ,BISSUE_BDC46 AS UpLimitUnderNet
                        ,BISSUE_BDC10 AS ShareRight --每股优先配售金额
                        ,BISSUE_BDC39 AS WinRateOnNet
                        ,BISSUE_BDC47 AS DownpaymentRate
                        ,BISSUE_BDC47/100 AS DownpaymentRate0
                        ,BISSUE_BDC52 AS WinRateUnderNet
                        ,BISSUE_BDC52/100 AS WinRateUnderNet0
                        ,OEBondChange2 AS ConvertStart
                        ,OEBondChange4 AS StrikePrice
                        FROM (SELECT * FROM SECURITYCODE WHERE STYPE = 'BDC') S
                        INNER JOIN (SELECT * FROM BONDDT WHERE BONDDT2='04') A  
                             ON CASE WHEN EXCHANGE = 'CNSESH' THEN BONDDT16 ELSE BONDDT17 END = SYMBOL
                        INNER JOIN (SELECT * FROM SECURITYCODE WHERE STYPE = 'EQA') S2
                             ON S2.Symbol = BONDDT18
                        INNER JOIN BONDISSUE B
                             ON A.BCODE=B.BCODE
                        LEFT JOIN BISSUE_BDC C
                             ON B.BCODE=C.BCODE
                        LEFT JOIN (Select * FROM OEBondChange WHERE OEBondChange1 ='初始转股价') D
                             ON A.BCODE=D.BondCode
                        LEFT JOIN (SELECT * FROM SYMBOL_COMP WHERE SYMBOL_COMP_TYPE ='19') F
                             ON S.Symbol = F.Symbol
                        WHERE 1=1
                        AND DECLAREDATE > (sysdate - 365) 
                        ORDER BY DECLAREDATE DESC";

        DataTable dtOutput = client.ExecuteSQL(sql).Tables[0];

        return dtOutput;
    }

    public DataTable GetUnderNetProjects()
    {
        try
        {
            DBServiceClient client = new DBServiceClient();
            string sql = @"Select A.Fundcode
                            , B.Name AS FundName
                            , A.Stockcode
                            , A.Stockname
                            , A.QUOTEDATE
                            , A.Price
                            , A.Volume
                            , CASE A.Success WHEN '1' THEN '有效' ELSE '无效' END AS Success
                            , A.Getvolume 
                            From db6_t_ipo A
                            LEFT JOIN db5_t_portfoliolist B
                                 ON A.Fundcode = B.Code
                            WHERE A.IsActive = '1'
                            ORDER BY A.Fundcode, A.Quotedate DESC";
            DataSet ds = client.ExecuteSQL(sql);
            return ds.Tables[0];
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}