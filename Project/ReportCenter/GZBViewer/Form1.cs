using System;
using System.Windows.Forms;
using System.Data;
using System.Collections.Generic;
using System.Management;
using ReportCenter.DBService;
using ReportCenter.GZBSvc;

namespace ReportCenter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            toolStripTextBoxStart.Text = "2012-10-29";
            toolStripTextBoxEnd.Text = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
            //toolStripTextBoxSelectDate.Text = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
            toolStripTextBoxFund.Text = "770001";
            toolStripTextBoxSort.Text = "FDate, FKmbm";

            dataGridView1.ReadOnly = true;
        }
        
        private void toolStripButtonShowData_Click(object sender, EventArgs e)
        {
            DateTime start = Convert.ToDateTime(toolStripTextBoxStart.Text);
            DateTime end = Convert.ToDateTime(toolStripTextBoxEnd.Text);
            string code = toolStripTextBoxFund.Text;

            ServiceSoapClient client = new ServiceSoapClient();

            DataSet ds = client.GetGzb("fadata", "fa*0926", start, end, code, "");
            DataTable dt = ds.Tables[0];
            dt.DefaultView.Sort = "FDate, FKmbm";

            dataGridView1.DataSource = dt;
        }

        private void toolStripButtonSelect_Click(object sender, EventArgs e)
        {
            filterResult();
        }

        private void toolStripButtonClear_Click(object sender, EventArgs e)
        {
            toolStripTextBoxSelectDate.Text = "";
            toolStripTextBoxSelectCode.Text = "";
            filterResult();
        }

        private void filterResult()
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            string ifCondition = "1=1";
            string fdate = toolStripTextBoxSelectDate.Text;
            string fcode = toolStripTextBoxSelectCode.Text;
            string sort = toolStripTextBoxSort.Text;

            if (fdate.Length > 0)
                ifCondition += "AND FDate='" + fdate + "'";

            if (fcode.Length > 0)
                ifCondition += "AND FKmbm like '" + fcode + "'";

            dt.DefaultView.RowFilter = ifCondition;
            dt.DefaultView.Sort = sort;
            dataGridView1.DataSource = dt;
        }
    }
 }