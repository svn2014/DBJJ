using System;
using System.Collections.Generic;
using System.Data;

namespace DBWcfService
{
    public class MeetingReportService : IMeetingReportService
    {

        public DataSet GetCategoryList()
        {
            try
            {
                List<ProcedureParameter> paraList = new List<ProcedureParameter>();
                paraList.Add(new ProcedureParameter("o_cursor", ProcedureParameter.DBType.Cursor, ParameterDirection.Output));

                return DBInstanceFactory.GetDBInstance().ExecuteStoredProcedure("db3_proc_selectcategory", paraList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetContentByCategory(DateTime reportDate, int categoryId)
        {
            try
            {
                List<ProcedureParameter> paraList = new List<ProcedureParameter>();
                paraList.Add(new ProcedureParameter("i_reportdate", reportDate.ToString("yyyyMMdd")));
                paraList.Add(new ProcedureParameter("i_categoryId", categoryId));
                paraList.Add(new ProcedureParameter("o_cursor", ProcedureParameter.DBType.Cursor, ParameterDirection.Output));

                return DBInstanceFactory.GetDBInstance().ExecuteStoredProcedure("db3_proc_selectcontent", paraList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetReportByDate(DateTime reportDate)
        {
            try
            {
                List<ProcedureParameter> paraList = new List<ProcedureParameter>();
                paraList.Add(new ProcedureParameter("i_reportdate", reportDate.ToString("yyyyMMdd")));
                paraList.Add(new ProcedureParameter("o_cursor", ProcedureParameter.DBType.Cursor, ParameterDirection.Output));

                return DBInstanceFactory.GetDBInstance().ExecuteStoredProcedure("db3_proc_selectmeetingreport", paraList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetSearchResult(DateTime startDate, DateTime endDate, string keyword)
        {
            try
            {
                List<ProcedureParameter> paraList = new List<ProcedureParameter>();
                paraList.Add(new ProcedureParameter("i_startdate", startDate.ToString("yyyyMMdd")));
                paraList.Add(new ProcedureParameter("i_enddate", endDate.ToString("yyyyMMdd")));
                paraList.Add(new ProcedureParameter("i_keyword", keyword));
                paraList.Add(new ProcedureParameter("o_cursor", ProcedureParameter.DBType.Cursor, ParameterDirection.Output));

                return DBInstanceFactory.GetDBInstance().ExecuteStoredProcedure("db3_proc_selectreportbykey", paraList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public long SubmitReport(DateTime submitDate, int categoryId, string keywords, string content)
        {
            try
            {
                Int64 newId = 0;

                List<ProcedureParameter> paraList = new List<ProcedureParameter>();
                paraList.Add(new ProcedureParameter("i_reportdate", submitDate.ToString("yyyyMMdd")));
                paraList.Add(new ProcedureParameter("i_categoryid", categoryId));
                paraList.Add(new ProcedureParameter("i_keywords", keywords));
                paraList.Add(new ProcedureParameter("i_content", ProcedureParameter.DBType.NClob, ParameterDirection.Input, content));
                paraList.Add(new ProcedureParameter("o_insertid", ProcedureParameter.DBType.BigInt, ParameterDirection.Output));

                newId = Convert.ToInt64(DBInstanceFactory.GetDBInstance().ExecuteNonQuery("db3_proc_updatemeetingreport", paraList, "o_insertid"));

                return newId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetReportTitle()
        {
            try
            {
                string title = "";

                List<ProcedureParameter> paraList = new List<ProcedureParameter>();
                ProcedureParameter para = new ProcedureParameter();
                para.Name = "o_title";
                para.Size = 100;
                para.Type = ProcedureParameter.DBType.NVarChar;
                para.Direction = ParameterDirection.Output;
                paraList.Add(para);

                title = DBInstanceFactory.GetDBInstance().ExecuteNonQuery("db3_proc_selecttitle", paraList, "o_title").ToString();

                return title;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
