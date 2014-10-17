using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OracleClient;
using System.Data;
using MRService;

/// <summary>
///DBService 的摘要说明
/// </summary>
public class DBService
{
    public static DataTable GetCategory()
    {
        try
        {
            MeetingReportServiceClient client = new MeetingReportServiceClient();
            DataSet ds = client.GetCategoryList();
            return ds.Tables[0];
        }
        catch (Exception ex)
        {            
            throw ex;
        }
    }
    
    public static DataTable GetSubmitDates()
    {
        try
        {
            DataTable oDT = new DataTable();
            oDT.Columns.Add("SubmitDate", Type.GetType("System.DateTime"));
            oDT.Columns.Add("SubmitDateText", Type.GetType("System.String"));

            DataRow oRow;
            oRow = oDT.NewRow();
            oRow["SubmitDate"] = DateTime.Today.Date;
            oRow["SubmitDateText"] = DateTime.Today.Date.ToString("yyyy-MM-dd");
            oDT.Rows.Add(oRow);

            oRow = oDT.NewRow();
            oRow["SubmitDate"] = DateTime.Today.AddDays(-1);
            oRow["SubmitDateText"] = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
            oDT.Rows.Add(oRow);

            oRow = oDT.NewRow();
            oRow["SubmitDate"] = DateTime.Today.AddDays(-2);
            oRow["SubmitDateText"] = DateTime.Today.AddDays(-2).ToString("yyyy-MM-dd");
            oDT.Rows.Add(oRow);

            oRow = oDT.NewRow();
            oRow["SubmitDate"] = DateTime.Today.AddDays(-3);
            oRow["SubmitDateText"] = DateTime.Today.AddDays(-3).ToString("yyyy-MM-dd");
            oDT.Rows.Add(oRow);

            oRow = oDT.NewRow();
            oRow["SubmitDate"] = DateTime.Today.AddDays(-4);
            oRow["SubmitDateText"] = DateTime.Today.AddDays(-4).ToString("yyyy-MM-dd");
            oDT.Rows.Add(oRow);

            oRow = oDT.NewRow();
            oRow["SubmitDate"] = DateTime.Today.AddDays(-5);
            oRow["SubmitDateText"] = DateTime.Today.AddDays(-5).ToString("yyyy-MM-dd");
            oDT.Rows.Add(oRow); 

            return oDT;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static long SubmitReport(DateTime submitDate, int categoryId, string keywords, string content)
    {
        try
        {            
            MeetingReportServiceClient client = new MeetingReportServiceClient();
            long newId = client.SubmitReport(submitDate, categoryId, keywords, content);
            return newId;            
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static DataTable GetReport(DateTime reportDate)
    {
        try
        {
            MeetingReportServiceClient client = new MeetingReportServiceClient();
            DataSet ds = client.GetReportByDate(reportDate);
            return ds.Tables[0];
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static DataTable GetSearchResult(DateTime startDate, DateTime endDate, string keyword)
    {
        try
        {
            MeetingReportServiceClient client = new MeetingReportServiceClient();
            DataSet ds = client.GetSearchResult(startDate, endDate, keyword);
            return ds.Tables[0];
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static DataTable GetContent(DateTime reportDate, int categoryId)
    {
        try
        {
            MeetingReportServiceClient client = new MeetingReportServiceClient();
            DataSet ds = client.GetContentByCategory(reportDate, categoryId);
            return ds.Tables[0];
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static string GetTitle()
    {
        try
        {
            MeetingReportServiceClient client = new MeetingReportServiceClient();
            string title = client.GetReportTitle();
            return title;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}