using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;

/// <summary>
///DBService 的摘要说明
/// </summary>
public class DBService
{
		private static DBService _Instance;

        public static DBService GetInstance()
        {
            if (_Instance == null)
                _Instance = new DBService();

            return _Instance;
        }

        private OracleConnection _conn;

        public DBService()
        {
            string _connString = "Data Source=fedb;user=feuser;password=feuser;";

            try
            {
                _conn = new OracleConnection(_connString);
                _conn.Open();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private string GetWebConfig(string project, string key)
        {
            try
            {
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = _conn;
                cmd.CommandText = "db0_proc_selectwebconfig";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new OracleParameter("i_project", project));
                cmd.Parameters.Add(new OracleParameter("i_key", key));
                cmd.Parameters.Add(new OracleParameter("o_cursor", OracleType.Cursor));
                cmd.Parameters["o_cursor"].Direction = ParameterDirection.Output;

                OracleDataAdapter oDA = new OracleDataAdapter(cmd);
                DataTable oDT = new DataTable();
                oDA.Fill(oDT);

                string ip = "";
                if (oDT.Rows.Count > 0)
                {
                    ip = oDT.Rows[0]["value"].ToString();
                }

                return ip;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetHomeSiteIP()
        {
            return GetWebConfig("POTL", "IP");
        }

        public string GetDynmicServerIP()
        {
            return GetWebConfig("EQTY", "IP");
        }

        private DataTable _dtReportType1, _dtReportType2;
        public DataTable GetReportType(int category)
        {
            try
            {
                if ((category == 1 && _dtReportType1 == null) || (category == 2 && _dtReportType2 == null))
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = _conn;
                    cmd.CommandText = "db1_proc_selecteqreporttype";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new OracleParameter("i_category",category));
                    cmd.Parameters.Add(new OracleParameter("o_cursor", OracleType.Cursor));
                    cmd.Parameters["o_cursor"].Direction = ParameterDirection.Output;

                    OracleDataAdapter oDA = new OracleDataAdapter(cmd);

                    if (category == 1)
                    {
                        _dtReportType1 = new DataTable();
                        oDA.Fill(_dtReportType1);                        
                    }
                    else
                    {
                        _dtReportType2 = new DataTable();
                        oDA.Fill(_dtReportType2);
                    }
                }

                if (category == 1)
                {
                    return _dtReportType1;
                }
                else
                {
                    return _dtReportType2;
                }
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
                DataTable _dtAnalyst = new DataTable();

                OracleCommand cmd = new OracleCommand();
                cmd.Connection = _conn;
                cmd.CommandText = "db1_proc_selectanalyst";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new OracleParameter("o_cursor", OracleType.Cursor));
                cmd.Parameters["o_cursor"].Direction = ParameterDirection.Output;

                OracleDataAdapter oDA = new OracleDataAdapter(cmd);
                _dtAnalyst = new DataTable();
                oDA.Fill(_dtAnalyst);

                return _dtAnalyst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertEquityReport(int analystid, string stockCode, int reportType1, int reportType2, string reportName, string keywords, string reportDesc, DateTime reportDate, string fileName, string approvalfilename)
        {
            try
            {
                int insertId = 0;

                OracleCommand cmd = new OracleCommand();
                cmd.Connection = _conn;
                cmd.CommandText = "db1_proc_updateequityreport";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new OracleParameter("i_reportid", DBNull.Value));
                cmd.Parameters.Add(new OracleParameter("i_analystid", analystid));
                cmd.Parameters.Add(new OracleParameter("i_stockcode", stockCode));
                cmd.Parameters.Add(new OracleParameter("i_reporttype1", reportType1));
                cmd.Parameters.Add(new OracleParameter("i_reporttype2", reportType2));
                cmd.Parameters.Add(new OracleParameter("i_reportname", reportName));
                cmd.Parameters.Add(new OracleParameter("i_keywords", keywords));
                cmd.Parameters.Add(new OracleParameter("i_reportdesc", reportDesc));
                cmd.Parameters.Add(new OracleParameter("i_fileName", fileName));
                cmd.Parameters.Add(new OracleParameter("i_apprfileName", approvalfilename));
                cmd.Parameters.Add(new OracleParameter("i_reportdate", reportDate.ToString("yyyyMMdd")));
                cmd.Parameters.Add(new OracleParameter("o_insertid", OracleType.Number,5)).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                insertId = Convert.ToInt32(cmd.Parameters["o_insertid"].Value);

                return insertId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertReportFile(HttpPostedFile uploadedFile, string filePathName)
        {
            try
            {
                uploadedFile.SaveAs(filePathName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetEquityReport(int reportId, int analystid, string stockCode, int reportType1, int reportType2, string reportName, string keywords, DateTime startDate, DateTime endDate)
        {
            try
            {
                reportName = "*" + reportName.Replace(" ","*") + "*";
                
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = _conn;
                cmd.CommandText = "db1_proc_selectequityreport";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new OracleParameter("i_reportid", reportId));
                cmd.Parameters.Add(new OracleParameter("i_analystid", analystid));
                cmd.Parameters.Add(new OracleParameter("i_stockcode", stockCode));
                cmd.Parameters.Add(new OracleParameter("i_reporttype1", reportType1));
                cmd.Parameters.Add(new OracleParameter("i_reporttype2", reportType2));
                cmd.Parameters.Add(new OracleParameter("i_reportname", reportName.Replace("*", "%")));
                cmd.Parameters.Add(new OracleParameter("i_keywords", keywords == "*" ? "*" : keywords.Replace("*", "%")));
                cmd.Parameters.Add(new OracleParameter("i_reportdate1", startDate.ToString("yyyyMMdd")));
                cmd.Parameters.Add(new OracleParameter("i_reportdate2", endDate.ToString("yyyyMMdd")));
                cmd.Parameters.Add(new OracleParameter("o_cursor", OracleType.Cursor)).Direction = ParameterDirection.Output;

                OracleDataAdapter oDA = new OracleDataAdapter(cmd);
                DataTable oDT = new DataTable();
                oDA.Fill(oDT);

                //build file folder
                foreach (DataRow oRow in oDT.Rows)
                {
                    string fileName = oRow["fileName"].ToString();
                    string apprfileName = oRow["apprfilename"].ToString();
                    string folderName = DBService.GetInstance().GetFolderName(oRow["stockcode"].ToString());

                    if (!(fileName.Contains("/") || fileName.Contains(@"\")))
                    {
                        fileName = folderName + "/" + fileName;
                    }

                    //if (oRow["reportName"].ToString().Trim().Length == 0)
                    //    oRow["reportName"] = fileName;
                    
                    oRow["URL"] = System.Configuration.ConfigurationSettings.AppSettings["ReportFolderName"].ToString() + "/" + fileName;
                    //oRow["fileURL"] = System.Configuration.ConfigurationSettings.AppSettings["ReportFileShare"].ToString() + "/" + fileName;
                    oRow["approvalURL"] = System.Configuration.ConfigurationSettings.AppSettings["ReportFolderName"].ToString() + "/" + apprfileName;
                }

                return oDT;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteEquityReport(int reportId)
        {
            try
            {
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = _conn;
                cmd.CommandText = "db1_proc_deleteequityreport";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new OracleParameter("i_reportid", reportId));

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DataTable _dtStockCode;
        public DataTable GetStockCodes()
        {
            try
            {
                if (_dtStockCode == null)
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = _conn;
                    cmd.CommandText = "db2_proc_selectstockcodes";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new OracleParameter("o_cursor", OracleType.Cursor)).Direction = ParameterDirection.Output;

                    OracleDataAdapter oDA = new OracleDataAdapter(cmd);
                    _dtStockCode = new DataTable();
                    oDA.Fill(_dtStockCode);

                    _dtStockCode.DefaultView.Sort = "s_info_windcode";
                    _dtStockCode = _dtStockCode.DefaultView.ToTable();
                }               

                return _dtStockCode;
            }
            catch (Exception ex)
            {
                throw ex;
            }  
        }

        private DataTable _dtIndustryCode;
        public DataTable GetIndustryCodes()
        {
            try
            {
                if (_dtIndustryCode == null)
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = _conn;
                    cmd.CommandText = "db2_proc_selectIndustrycodes";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new OracleParameter("o_cursor", OracleType.Cursor)).Direction = ParameterDirection.Output;

                    OracleDataAdapter oDA = new OracleDataAdapter(cmd);
                    _dtIndustryCode = new DataTable();
                    oDA.Fill(_dtIndustryCode);
                }

                return _dtIndustryCode;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DataTable _dtDBCode;
        public DataTable GetDBCodes(int reportTypeId)
        { 
            try
            {
                if (_dtDBCode == null)
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = _conn;
                    cmd.CommandText = "db1_proc_selectdbcodes";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new OracleParameter("o_cursor", OracleType.Cursor)).Direction = ParameterDirection.Output;

                    OracleDataAdapter oDA = new OracleDataAdapter(cmd);
                    _dtDBCode = new DataTable();
                    oDA.Fill(_dtDBCode);
                }

                DataRow[] oRows = _dtDBCode.Select("reportTypeId = " + reportTypeId);

                if (oRows.Length > 0)
                {
                    DataTable dtTmp = _dtDBCode.Copy();
                    dtTmp.Rows.Clear();

                    DataRow oRow = dtTmp.NewRow();
                    oRow[0] = oRows[0][0];
                    oRow[1] = oRows[0][1];
                    dtTmp.Rows.Add(oRow);

                    return dtTmp;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string GetFolderName(string stockCode)
        {
            string folder = "";

            if (_dtStockCode == null)
                this.GetStockCodes();

            if (_dtIndustryCode == null)
                this.GetIndustryCodes();

            if (_dtDBCode == null)
                this.GetDBCodes(11);

            DataRow[] oRows = _dtStockCode.Select("s_info_windcode = '" + stockCode + "'");

            if (oRows.Length == 0)
                oRows = _dtIndustryCode.Select("s_info_windcode = '" + stockCode + "'");

            if (oRows.Length == 0)
                oRows = _dtDBCode.Select("s_info_windcode = '" + stockCode + "'");

            if (oRows.Length > 0)
                folder = oRows[0]["industryname"].ToString();

            return folder;
        }

        public static void UpdateDDL(DropDownList ddl, int reportType, int usageId)
        {
            DataTable dtBind = null;

            switch (reportType)
            {                
                case 12: //行业研究
                    dtBind = DBService.GetInstance().GetIndustryCodes();
                    break;

                case 13: //公司研究
                    dtBind = DBService.GetInstance().GetStockCodes();
                    break;

                //case 11: //宏观策略
                //case 14: //债券研究                   
                //case 15: //数量研究
                //case 16: //月度策略
                //    dtBind = DBService.GetInstance().GetDBCodes(reportType);
                //    break;

                default:
                    dtBind = DBService.GetInstance().GetDBCodes(reportType);
                    break;
            }

            if (dtBind != null)
            {
                ddl.DataSource = dtBind;
                ddl.DataValueField = "s_info_windcode";
                ddl.DataTextField = "stockname";
                ddl.DataBind();

                if (reportType == 12 || reportType == 13)
                    ddl.Items.Insert(0, new ListItem("--请选择--", ""));
                else
                    ddl.SelectedIndex = 0;
            }
            else
            {
                ddl.DataSource = "";
                ddl.Items.Clear();
                if (usageId == 1)//Default
                    ddl.Items.Add(new ListItem("--任意--", "*"));
                else//Submit
                    ddl.Items.Add(new ListItem("--请选择--", ""));
            }
        }
}