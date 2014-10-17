using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class _Default : System.Web.UI.Page
{
    private void LoadIPOTable()
    {
        string code = txtCode.Text;
        DataTable dt = DataService.GetInstance().GetIPOs(code);
        GridViewIPOInfo.DataSource = dt;
        GridViewIPOInfo.DataBind();
    }

    private void LoadCBTable()
    {
        DataTable dt = DataService.GetInstance().GetConvertables();
        GridViewCBInfo.DataSource = dt;
        GridViewCBInfo.DataBind();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //LoadIPOTable();
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        LoadIPOTable();
        LoadCBTable();
    }

    private void formatGridValue(object sender, GridViewRowEventArgs e)
    {
        GridView gv = ((GridView)sender);

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                string colName = gv.Columns[i].HeaderText;
                string text = e.Row.Cells[i].Text.Trim().Replace("&nbsp;", "");

                if (colName.Substring(colName.Length - 1, 1) == "日")
                {
                    try
                    {
                        DateTime d = Convert.ToDateTime(text);
                        if (d > DateTime.Today.AddDays(7))
                        {
                            e.Row.Cells[i].BackColor = System.Drawing.Color.Aqua;
                        }
                        else if (d > DateTime.Today && d<=DateTime.Today.AddDays(7))
                        {
                            e.Row.Cells[i].BackColor = System.Drawing.Color.Yellow;
                        }
                        else if (d == DateTime.Today)
                        {
                            e.Row.Cells[i].BackColor = System.Drawing.Color.Red;
                        }
                    }
                    catch (Exception)
                    {

                    }
                    
                }
            }
        }
    }
    protected void GridViewIPOInfo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        formatGridValue(sender, e);
    }
}
