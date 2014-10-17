using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

public partial class _SubmitPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataTable dtAnalyst = DBService.GetInstance().GetAnalyst();
            DataTable dtReportType1 = DBService.GetInstance().GetReportType(1);
            DataTable dtReportType2 = DBService.GetInstance().GetReportType(2);
            
            DropDownListAnalyst.DataSource = dtAnalyst;
            DropDownListAnalyst.DataValueField = "AnalystId";
            DropDownListAnalyst.DataTextField = "AnalystName";
            DropDownListAnalyst.DataBind();
            DropDownListAnalyst.Items.Insert(0, new ListItem("--请选择--", ""));

            DropDownListType1.DataSource = dtReportType1;
            DropDownListType1.DataValueField = "reportTypeId";
            DropDownListType1.DataTextField = "reportTypeName";
            DropDownListType1.DataBind();
            DropDownListType1.Items.Insert(0, new ListItem("--请选择--", ""));

            DropDownListType2.DataSource = dtReportType2;
            DropDownListType2.DataValueField = "reportTypeId";
            DropDownListType2.DataTextField = "reportTypeName";
            DropDownListType2.DataBind();
            DropDownListType2.Items.Insert(0, new ListItem("--请选择--", ""));

            DropDownListStockCode.Items.Add(new ListItem("--请选择--", ""));

            TextBoxReportDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            lblStatus.Text = "";

            //报告
            string reportName = "";
            if (TextBoxReportName.Text.Length == 0)
            {
                if (FileUploadAttachment.HasFile)
                {
                    TextBoxReportName.Text = FileUploadAttachment.FileName;
                    reportName = getReportName(FileUploadAttachment.FileName);
                }
            }
            else
                reportName = TextBoxReportName.Text;

            //审批
            string approvalFileExt = "";
            int idx = 0;
            if (FileUploadApproval.HasFile)
            {
                approvalFileExt = FileUploadApproval.FileName;
                idx = approvalFileExt.LastIndexOf(".");
                if (idx == -1)
                    approvalFileExt = "";
                else
                    approvalFileExt = approvalFileExt.Substring(idx);
            }

            string fileName = getFileName();
            string approvalfilename = fileName;
            idx = fileName.LastIndexOf(".");
            approvalfilename = fileName.Substring(0, idx) + "-研究总监意见" + approvalFileExt;
            fileName = fileName.Substring(0, idx) + "-" + DateTime.Now.ToString("hhmmss") + fileName.Substring(idx);

            int insertId = DBService.GetInstance().InsertEquityReport(
                    Convert.ToInt16(DropDownListAnalyst.SelectedValue),
                    DropDownListStockCode.SelectedValue.ToString(),
                    Convert.ToInt16(DropDownListType1.SelectedValue),
                    Convert.ToInt16(DropDownListType2.SelectedValue),
                    reportName,
                    TextBoxKeywords.Text,
                    TextBoxDesc.Text,
                    Convert.ToDateTime(TextBoxReportDate.Text),
                    fileName,
                    approvalfilename
                );

            
            if (FileUploadAttachment.HasFile && FileUploadApproval.HasFile)
            {
                string path = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["ReportFolderName"].ToString()) + @"\";
                DBService.GetInstance().InsertReportFile(FileUploadAttachment.PostedFile, path + fileName);
                DBService.GetInstance().InsertReportFile(FileUploadApproval.PostedFile, path + approvalfilename);
            }

            lblStatus.Text = "提交成功[" + insertId + "][" + fileName + "]";
            aFolder.Visible = true;
            aFolder.HRef = System.Configuration.ConfigurationManager.AppSettings["ReportFolderName"].ToString() + "/" + fileName;
            aApproval.Visible = true;
            aApproval.HRef = System.Configuration.ConfigurationManager.AppSettings["ReportFolderName"].ToString() + "/" + approvalfilename;

            TextBoxReportName.Text = "";
        }
        catch (Exception ex)
        {
            lblStatus.Text = ex.Message;
        }
        
    }

    private string getFileName()
    {
        //002484-江海股份-公司报告-调研简报-江海股份调研简报-20120223-叶翔.docx
        string stockCode = DropDownListStockCode.SelectedValue.ToString();
        string stockName = DropDownListStockCode.Items[DropDownListStockCode.SelectedIndex].Text;
        string industryName = "";

        int type1 = Convert.ToInt16(DropDownListType1.SelectedValue);
        string typeName1 = DropDownListType1.Items[DropDownListType1.SelectedIndex].Text;
        int type2 = Convert.ToInt16(DropDownListType2.SelectedValue);
        string typeName2 = DropDownListType2.Items[DropDownListType2.SelectedIndex].Text;

        string reportName = TextBoxReportName.Text.Replace("&", "_");   //去掉文件名中的非法字符
        string fileExt = "";

        if (reportName.Contains("."))
        {
            fileExt = reportName.Substring(reportName.LastIndexOf("."));
            reportName = reportName.Substring(0, reportName.LastIndexOf("."));
        }

        if (reportName.Contains("-"))
        {
            //300272-开能环保-公司研究-调研报告-开能环保300372)调研记录-20121228-程枫  Length = 7
            //房地产行业-行业研究-调研简报-新产品和新渠道支撑高增长-20120223-张亚辉     Length = 6
            string[] aryReportName = reportName.Split("-".ToCharArray());
            if (aryReportName.Length == 6)
            {
                reportName = aryReportName[3];
            }
            else if (aryReportName.Length == 7)
            {
                reportName = aryReportName[4];
            }
        }

        DateTime reportDate = Convert.ToDateTime(TextBoxReportDate.Text);

        int analystId = Convert.ToInt16(DropDownListAnalyst.SelectedValue);
        string analystName = DropDownListAnalyst.Items[DropDownListAnalyst.SelectedIndex].Text;

        string fileName = "";
        DataTable dtStockList;
        DataRow[] oRows;

        switch (type1)
        {
            case 12: //行业
                dtStockList = DBService.GetInstance().GetIndustryCodes();
                oRows = dtStockList.Select("s_info_windcode='" + stockCode + "'");
                if (oRows.Length > 0)
                {
                    stockName = oRows[0]["s_info_name"].ToString();
                    industryName = oRows[0]["industryname"].ToString();
                }

                fileName = stockName + "-"
                            + typeName1 + "-" + typeName2 + "-"
                            + reportName + "-" + reportDate.ToString("yyyyMMdd") + "-"
                            + analystName + fileExt;
                break;

            case 13: //公司
                dtStockList = DBService.GetInstance().GetStockCodes();
                oRows = dtStockList.Select("s_info_windcode='" + stockCode + "'");
                if(oRows.Length >0)
                {
                    stockName = oRows[0]["s_info_name"].ToString();
                    industryName = oRows[0]["industryname"].ToString();
                }

                fileName = stockCode.Substring(0,6) + "-" + stockName + "-"
                            + typeName1 + "-" + typeName2 + "-"
                            + reportName + "-" + reportDate.ToString("yyyyMMdd") + "-"
                            + analystName + fileExt;
                break;

            case 11: //宏观
            case 14: //债券
            case 15: //数量                
                dtStockList = DBService.GetInstance().GetStockCodes();
                oRows = dtStockList.Select("s_info_windcode='" + stockCode + "'");
                if(oRows.Length >0)
                {
                    stockName = oRows[0]["s_info_name"].ToString();
                    industryName = oRows[0]["industryname"].ToString();
                }

                fileName =  typeName1 + "-" + typeName2 + "-"
                            + reportName + "-" + reportDate.ToString("yyyyMMdd") + "-"
                            + analystName + fileExt;
                break;

            default:
                fileName = typeName1 + "-" + typeName2 + "-"
                            + reportName + "-" + reportDate.ToString("yyyyMMdd") + "-"
                            + analystName + fileExt;
                break;
        }

        return DBService.GetInstance().GetFolderName(stockCode) + @"\" + fileName;
    }

    private string getReportName(string fileName)
    {
        string reportName = fileName;

        if (reportName.Contains("."))
            reportName = reportName.Substring(0, reportName.LastIndexOf("."));
        
        if (reportName.Contains(@"\"))
            reportName = reportName.Substring(reportName.LastIndexOf(@"\") + 1);

        string[] fileNameArray = reportName.Split("-".ToCharArray());

        if (fileNameArray.Length == 6)
        {
            //行业命名法
            //房地产行业-行业研究-调研简报-新产品和新渠道支撑高增长-20120223-张亚辉     Length = 6
            reportName = fileNameArray[3];
        }
        else if (fileNameArray.Length == 7)
        {
            //公司命名法
            //300272-开能环保-公司研究-调研报告-开能环保300372)调研记录-20121228-程枫  Length = 7
            reportName = fileNameArray[4];
        }

        return reportName;
    }

    protected void DropDownListType1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DropDownListType1.SelectedValue.Length > 0)
        DBService.UpdateDDL(DropDownListStockCode, Convert.ToInt16(DropDownListType1.SelectedValue),2);
    }

    protected void btnParseFileName_Click(object sender, EventArgs e)
    {
        //if (FileUploadAttachment.HasFile)
        //{
        //    string fileName = FileUploadAttachment.FileName;

        //    300272-开能环保-公司研究-调研报告-开能环保300372)调研记录-20121228-程枫  Length = 7
        //    房地产行业-行业研究-调研简报-新产品和新渠道支撑高增长-20120223-张亚辉     Length = 6
        //    string[] fileNameArray = fileName.Split("-".ToCharArray());
        //    string stockCode = "";
        //    string analystName = "";
        //    string reportDate = DateTime.Today.ToString("yyyy-MM-dd");
        //    string reportName = "";
        //    string reportType1, reportType2;

        //    if (fileNameArray.Length == 6)
        //    {
        //        行业
        //        analystName = fileNameArray[5];
        //        reportDate = fileNameArray[4];
        //        reportName = fileNameArray[3];
        //        reportType2 = fileNameArray[2];
        //        reportType1 = fileNameArray[1];
        //    }
        //    else if (fileNameArray.Length > 7)
        //    {
        //        公司
        //        analystName = fileNameArray[6];
        //        reportDate = fileNameArray[5];
        //        reportName = fileNameArray[4];
        //        reportType2 = fileNameArray[3];
        //        reportType1 = fileNameArray[2];

        //        stockCode = fileNameArray[0];
        //    }
        //    else
        //        return;

        //    Analyst
        //    if (analystName.Length > 0)
        //    {
        //        for (int i = 0; i < DropDownListAnalyst.Items.Count; i++)
        //        {
        //            if (DropDownListAnalyst.Items[i].Text == analystName)
        //            {
        //                DropDownListAnalyst.SelectedIndex = i;
        //                break;
        //            }
        //        }
        //    }

        //    reportType1
        //    if (reportType1.Length > 0)
        //    {
        //        for (int i = 0; i < DropDownListType1.Items.Count; i++)
        //        {
        //            if (DropDownListType1.Items[i].Text == reportType1)
        //            {
        //                DropDownListType1.SelectedIndex = i;
        //                break;
        //            }
        //        }
        //    }

        //    reportType2
        //    if (reportType2.Length > 0)
        //    {
        //        for (int i = 0; i < DropDownListType2.Items.Count; i++)
        //        {
        //            if (DropDownListType2.Items[i].Text == reportType2)
        //            {
        //                DropDownListType2.SelectedIndex = i;
        //                break;
        //            }
        //        }
        //    }            

        //    reportDate 
        //    reportDate = reportDate.Substring(0, 4) + "-" + reportDate.Substring(4, 2) + "-" + reportDate.Substring(6, 2);
        //    TextBoxReportDate.Text = reportDate;

        //    reportName
        //    TextBoxReportName.Text = reportName;

        //    StockCode
        //    if (stockCode.Length > 0)
        //    {
        //        if (stockCode.Substring(0, 3) == "000" ||
        //        stockCode.Substring(0, 3) == "002" ||
        //        stockCode.Substring(0, 3) == "300")
        //            stockCode += ".SZ";
        //        else
        //            stockCode += ".SH";

        //        for (int i = 0; i < DropDownListStockCode.Items.Count; i++)
        //        {
        //            if (DropDownListStockCode.Items[i].Text == stockCode)
        //            {
        //                DropDownListStockCode.SelectedIndex = i;
        //                break;
        //            }
        //        }
        //    }

        //}
    }
}
