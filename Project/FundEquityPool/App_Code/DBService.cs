using System;
using System.Data;
using DBService;
using CHService;

public class DataService
{
    private static DataService _Instance;

    public static DataService GetInstance()
    {
        if (_Instance == null)
            _Instance = new DataService();

        return _Instance;
    }

    public DataTable GetFundSecurityPool(string stockCode, string industryOrRating, bool inBasePool, bool inCorePool, bool inRestPool, bool inProhPool, int analystId, bool isEquity, DateTime startdate, DateTime enddate, int hedgefundId)
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

    public DataTable GetFundSecurityPoolHistory(string stockCode, DateTime startdate, DateTime enddate, bool? isEquity, int? hedgefundid)
    {
        try
        {
            if (stockCode == null || stockCode.Length == 0)
                stockCode = "%";

            stockCode = stockCode.Replace("*","%");

            DBServiceClient client = new DBServiceClient();
            ProcedureParameter[] paraList = new ProcedureParameter[6];

            paraList[0] = new ProcedureParameter(); paraList[0].Name = "i_stockcode"; paraList[0].Value = stockCode; paraList[0].Direction = ParameterDirection.Input; paraList[0].Type = ProcedureParameter.DBType.NVarChar;
            paraList[1] = new ProcedureParameter(); paraList[1].Name = "o_cursor"; paraList[1].Type = ProcedureParameter.DBType.Cursor; paraList[1].Direction = ParameterDirection.Output;
            paraList[2] = new ProcedureParameter(); paraList[2].Name = "i_startdate"; paraList[2].Value = startdate.ToString("yyyyMMdd"); paraList[2].Direction = ParameterDirection.Input; paraList[2].Type = ProcedureParameter.DBType.NVarChar;
            paraList[3] = new ProcedureParameter(); paraList[3].Name = "i_enddate"; paraList[3].Value = enddate.ToString("yyyyMMdd"); paraList[3].Direction = ParameterDirection.Input; paraList[3].Type = ProcedureParameter.DBType.NVarChar;

            paraList[4] = new ProcedureParameter(); paraList[4].Name = "i_sectype"; paraList[4].Value = (isEquity==null || isEquity.Value ==false) ? "B" : "E"; paraList[4].Direction = ParameterDirection.Input; paraList[4].Type = ProcedureParameter.DBType.Char;
            paraList[5] = new ProcedureParameter(); paraList[5].Name = "i_hedgefundid"; paraList[5].Value = hedgefundid;  paraList[5].Direction = ParameterDirection.Input; paraList[5].Type = ProcedureParameter.DBType.Int;

            if (isEquity == null)
                paraList[4].Value = null;
            else
                paraList[4].Value = (isEquity.Value == true ? "E" : "B");

            DataSet ds = client.ExecuteStoredProcedure("db2_proc_selectfundequityhist", paraList);
            return ds.Tables[0];
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable GetReportList(string stockCode)
    {
        try
        {
            DBServiceClient client = new DBServiceClient();
            ProcedureParameter[] paraList = new ProcedureParameter[10];

            if (stockCode == null || stockCode.Length == 0 || stockCode == "*")
                return null;

            paraList[0] = new ProcedureParameter(); paraList[0].Name = "i_reportid"; paraList[0].Value = null; paraList[0].Direction = ParameterDirection.Input; paraList[0].Type = ProcedureParameter.DBType.BigInt;
            paraList[1] = new ProcedureParameter(); paraList[1].Name = "i_analystid"; paraList[1].Value = null; paraList[1].Direction = ParameterDirection.Input; paraList[1].Type = ProcedureParameter.DBType.BigInt;
            paraList[2] = new ProcedureParameter(); paraList[2].Name = "i_stockcode"; paraList[2].Value = stockCode; paraList[2].Direction = ParameterDirection.Input; paraList[2].Type = ProcedureParameter.DBType.NVarChar;
            paraList[3] = new ProcedureParameter(); paraList[3].Name = "i_reporttype1"; paraList[3].Value = null; paraList[3].Direction = ParameterDirection.Input; paraList[3].Type = ProcedureParameter.DBType.BigInt;
            paraList[4] = new ProcedureParameter(); paraList[4].Name = "i_reporttype2"; paraList[4].Value = null; paraList[4].Direction = ParameterDirection.Input; paraList[4].Type = ProcedureParameter.DBType.BigInt;
            paraList[5] = new ProcedureParameter(); paraList[5].Name = "i_reportname"; paraList[5].Value = null; paraList[5].Direction = ParameterDirection.Input; paraList[5].Type = ProcedureParameter.DBType.NVarChar;
            paraList[6] = new ProcedureParameter(); paraList[6].Name = "i_keywords"; paraList[6].Value = null; paraList[6].Direction = ParameterDirection.Input; paraList[6].Type = ProcedureParameter.DBType.NVarChar;
            paraList[7] = new ProcedureParameter(); paraList[7].Name = "i_reportdate1"; paraList[7].Value = null; paraList[7].Direction = ParameterDirection.Input; paraList[7].Type = ProcedureParameter.DBType.Date;
            paraList[8] = new ProcedureParameter(); paraList[8].Name = "i_reportdate2"; paraList[8].Value = null; paraList[8].Direction = ParameterDirection.Input; paraList[8].Type = ProcedureParameter.DBType.Date;
            
            paraList[9] = new ProcedureParameter(); paraList[9].Name = "o_cursor"; paraList[9].Direction = ParameterDirection.Output; paraList[9].Type = ProcedureParameter.DBType.Cursor;

            DataSet ds = client.ExecuteStoredProcedure("db1_proc_selectequityreport", paraList);
            return ds.Tables[0];
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable GetStatisticData(bool isGroupByIndustry)
    {
        string sql = "";

        if (isGroupByIndustry)
        {
            sql = "select * from db2_v_fundequitypoolstat1";
        }
        else
        {
            sql = "select * from db2_v_fundequitypoolstat2";
        }

        DBServiceClient client = new DBServiceClient();
        DataSet dt = client.ExecuteSQL(sql);

        return dt.Tables[0];
    }

    public void UpdateFundEquityPool(string stockCode, string poolFlag, string reason, int analystId, bool isEquity, int hedgeFundId)
    {
        try
        {
            if (stockCode == null || stockCode.Length < 9)
                return;

            string inBase, inCore, inRest, inProh;
            inBase = inCore = inRest = inProh = null;

            switch (poolFlag)
            {
                case "0001": //禁止
                    inProh = "1";
                    break;
                case "0010": //限制
                    inRest = "1";
                    break;
                case "1000": //基础
                    inBase = "1";
                    break;

                case "1100": //核心
                    inBase = "1";
                    inCore = "1";
                    break;

                case "0000": //不分配
                    break;
                    
                default:
                    break;
            }

            DBServiceClient client = new DBServiceClient();
            ProcedureParameter[] paraList;

            paraList = new ProcedureParameter[10];
            paraList[0] = new ProcedureParameter(); paraList[0].Name = "i_stockcode"; paraList[0].Value = stockCode; paraList[0].Direction = ParameterDirection.Input; paraList[0].Type = ProcedureParameter.DBType.NVarChar;
            paraList[1] = new ProcedureParameter(); paraList[1].Name = "i_inbasepool"; paraList[1].Value = inBase; paraList[1].Direction = ParameterDirection.Input; paraList[1].Type = ProcedureParameter.DBType.Char;
            paraList[2] = new ProcedureParameter(); paraList[2].Name = "i_incorepool"; paraList[2].Value = inCore; paraList[2].Direction = ParameterDirection.Input; paraList[2].Type = ProcedureParameter.DBType.Char;
            paraList[3] = new ProcedureParameter(); paraList[3].Name = "i_inprohpool"; paraList[3].Value = inProh; paraList[3].Direction = ParameterDirection.Input; paraList[3].Type = ProcedureParameter.DBType.Char;
            paraList[4] = new ProcedureParameter(); paraList[4].Name = "i_opuser"; paraList[4].Value = "system"; paraList[4].Direction = ParameterDirection.Input; paraList[4].Type = ProcedureParameter.DBType.NVarChar;
            paraList[5] = new ProcedureParameter(); paraList[5].Name = "i_reason"; paraList[5].Value = reason; paraList[5].Direction = ParameterDirection.Input; paraList[5].Type = ProcedureParameter.DBType.NVarChar;
            paraList[6] = new ProcedureParameter(); paraList[6].Name = "i_analystid"; paraList[6].Value = analystId; paraList[6].Direction = ParameterDirection.Input; paraList[6].Type = ProcedureParameter.DBType.Int;
            paraList[7] = new ProcedureParameter(); paraList[7].Name = "i_sectype"; paraList[7].Value = (isEquity?"E":"B"); paraList[7].Direction = ParameterDirection.Input; paraList[7].Type = ProcedureParameter.DBType.Char;
            paraList[8] = new ProcedureParameter(); paraList[8].Name = "i_inrestpool"; paraList[8].Value = inRest; paraList[8].Direction = ParameterDirection.Input; paraList[8].Type = ProcedureParameter.DBType.Char;
            paraList[9] = new ProcedureParameter(); paraList[9].Name = "i_hedgefundid"; paraList[9].Value = hedgeFundId; paraList[9].Direction = ParameterDirection.Input; paraList[9].Type = ProcedureParameter.DBType.Int;

            if (hedgeFundId == 0 || hedgeFundId == 1)
                client.ExecuteNonQuery("db2_proc_updatefundpool", paraList, null);    //公募 | 专户
            else if (hedgeFundId > 10)
                client.ExecuteNonQuery("db2_proc_updatefundpool_hf", paraList, null); //专户: 限制|禁止
            else
                throw new Exception("公募/专户类别错误");

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    private string _reportURL = "";
    public string GetReportServerURL()
    {
        if (_reportURL.Length == 0)
        {
            DBServiceClient client = new DBServiceClient();
            string ip = client.GetAlternativeSiteIP();

            _reportURL = @"http://" + ip + @"/equityreport/default.aspx";
        }

        return _reportURL;
    }

    private DataTable _dtIndustryCode;
    public DataTable GetIndustryCodes()
    {
        try
        {
            if (_dtIndustryCode == null)
            {
                DBServiceClient client = new DBServiceClient();
                ProcedureParameter[] paraList = new ProcedureParameter[1];

                paraList[0] = new ProcedureParameter(); paraList[0].Name = "o_cursor"; paraList[0].Direction = ParameterDirection.Output; paraList[0].Type = ProcedureParameter.DBType.Cursor;

                DataSet ds = client.ExecuteStoredProcedure("db2_proc_selectIndustrycodes", paraList);
                _dtIndustryCode = ds.Tables[0];
            }

            return _dtIndustryCode;
        }
        catch (Exception ex)
        {
            throw ex;
        }
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

    public DataTable GetHedgefundList()
    {
        try
        {
            DataTable dtHedgefundList;
            DBServiceClient client = new DBServiceClient();
            ProcedureParameter[] paraList = new ProcedureParameter[1];

            paraList[0] = new ProcedureParameter(); paraList[0].Name = "o_cursor"; paraList[0].Direction = ParameterDirection.Output; paraList[0].Type = ProcedureParameter.DBType.Cursor;

            DataSet ds = client.ExecuteStoredProcedure("db2_proc_selecthedgefundlist", paraList);
            dtHedgefundList = ds.Tables[0];
            return dtHedgefundList;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable GetActiveStockList()
    {
        try
        {
            DataTable dtStockList;
            CHServiceClient client = new CHServiceClient();
            dtStockList = client.GetAShareStockList(null,null,null,"1").Tables[0];
            return dtStockList;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable GetActiveBondList()
    {
        try
        {
            DataTable dtBondList;
            CHServiceClient client = new CHServiceClient();
            dtBondList = client.GetChinaBondList(null, null).Tables[0];
            dtBondList.DefaultView.RowFilter = "b_info_maturitydate >='" + DateTime.Today.ToString("yyyyMMdd") + "'";
            dtBondList = dtBondList.DefaultView.ToTable();
            return dtBondList;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public string GetAuthorizationCode()
    {
        DBServiceClient client = new DBServiceClient();
        return client.GetAuthrizationCode();
    }
}