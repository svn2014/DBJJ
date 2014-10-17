using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class About : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            DropDownListPoolType.Items.Add(new ListItem("基础库", "1000"));
            DropDownListPoolType.Items.Add(new ListItem("核心库", "1100"));
            DropDownListPoolType.Items.Add(new ListItem("限制库", "0010"));
            DropDownListPoolType.Items.Add(new ListItem("禁止库", "0001"));
            DropDownListPoolType.Items.Add(new ListItem("出库", "*"));

            DropDownListSecurityType.Items.Add(new ListItem("--请选择--", ""));
            DropDownListSecurityType.Items.Add(new ListItem("股票", "E"));
            DropDownListSecurityType.Items.Add(new ListItem("债券", "B"));

            DataTable dtAnalyst = DataService.GetInstance().GetAnalyst();
            DropDownListAnalyst.DataSource = dtAnalyst;
            DropDownListAnalyst.DataValueField = "AnalystId";
            DropDownListAnalyst.DataTextField = "AnalystName";
            DropDownListAnalyst.DataBind();
            DropDownListAnalyst.Items.Insert(0, new ListItem("--任意--", "0"));

            DataTable dtHedgeFund = DataService.GetInstance().GetHedgefundList();
            DropDownListHedgefund.DataSource = dtHedgeFund;
            DropDownListHedgefund.DataValueField = "HedgeFundId";
            DropDownListHedgefund.DataTextField = "HedgeFundName";
            DropDownListHedgefund.DataBind();
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            string msg = "";
            string authCode = TextBoxAuthCode.Text.Trim();
            string compareCode = DataService.GetInstance().GetAuthorizationCode();

            if (authCode != compareCode)
            {
                lblStatus.Text = "授权码错误！";
                return;
            }

            int analystid = Convert.ToInt16(DropDownListAnalyst.SelectedValue);
            if (analystid == 0)
            {
                lblStatus.Text = "未选择研究员！";
                return;
            }

            int hedgefundid = Convert.ToInt16(DropDownListHedgefund.SelectedValue);

            string secType = DropDownListSecurityType.SelectedValue;
            if (secType.Length == 0)
            {
                lblStatus.Text = "未选择证券类别！";
                return;
            }

            string poolType = DropDownListPoolType.SelectedValue;
            string codes = TextBoxCodes.Text;
            string reason = TextBoxReason.Text;
            string[] aryCodes = codes.Split("\r".ToCharArray());
            bool isEquity = (secType == "E") ? true : false;

            DataTable dtSecurityList =null;
            DataTable dtProhibitedList = null;
            DataTable dtRestrictList = null;
            DataTable dtBaseList = null;
            DataTable dtCoreList = null;

            if (isEquity)
            {
                dtSecurityList = DataService.GetInstance().GetActiveStockList();
                dtProhibitedList = DataService.GetInstance().GetFundSecurityPool(null, null, false, false, false, true, 0, true, new DateTime(1900, 1, 1), DateTime.Today,-1);
                dtRestrictList = DataService.GetInstance().GetFundSecurityPool(null, null, false, false, true, false, 0, true, new DateTime(1900, 1, 1), DateTime.Today, -1);
                dtBaseList = DataService.GetInstance().GetFundSecurityPool(null, null, true, false, false, false, 0, true, new DateTime(1900, 1, 1), DateTime.Today, -1);
                dtCoreList = DataService.GetInstance().GetFundSecurityPool(null, null, false, true, false, false, 0, true, new DateTime(1900, 1, 1), DateTime.Today, -1);
            }
            else
            {
                dtSecurityList = DataService.GetInstance().GetActiveBondList(); //不包含到期的
                dtProhibitedList = DataService.GetInstance().GetFundSecurityPool(null, null, false, false, false, true, 0, false, new DateTime(1900, 1, 1), DateTime.Today, -1);
                dtRestrictList = DataService.GetInstance().GetFundSecurityPool(null, null, false, false, true, false, 0, false, new DateTime(1900, 1, 1), DateTime.Today, -1);
                dtBaseList = DataService.GetInstance().GetFundSecurityPool(null, null, true, false, false, false, 0, false, new DateTime(1900, 1, 1), DateTime.Today, -1);
                dtCoreList = DataService.GetInstance().GetFundSecurityPool(null, null, false, true, false, false, 0, false, new DateTime(1900, 1, 1), DateTime.Today, -1);
            }

            string currCode = "";
            foreach (string tmpCode in aryCodes)
            {
                currCode = tmpCode.Replace("\n", "").Trim().ToUpper();

                if (currCode.Length == 0)
                    continue;

                //Check code valid
                DataRow[] selectedRows = dtSecurityList.Select("s_info_windcode='" + currCode + "'");
                if (selectedRows.Length == 0 && CheckBoxCheckCode.Checked)
                {
                    msg += currCode + ": 无效的代码;\r\n";
                    continue;
                }

                //检查是否已经入库
                if (CheckBoxCheckOnBase.Checked)
                {
                    selectedRows = dtBaseList.Select("s_info_windcode='" + currCode + "'");
                    if (selectedRows.Length > 0)
                    {
                        msg += currCode + ": 已列入基础库;\r\n";
                        continue;
                    }
                }

                if (CheckBoxCheckOnCore.Checked)
                {
                    selectedRows = dtCoreList.Select("s_info_windcode='" + currCode + "'");
                    if (selectedRows.Length > 0)
                    {
                        msg += currCode + ": 已列入核心库;\r\n";
                        continue;
                    }
                }

                if (CheckBoxCheckOnRestrict.Checked)
                {
                    selectedRows = dtRestrictList.Select("s_info_windcode='" + currCode + "'");
                    if (selectedRows.Length > 0)
                    {
                        msg += currCode + ": 已列入限制库;\r\n";
                        continue;
                    }
                }

                if (CheckBoxCheckOnProhibited.Checked)
                {
                    selectedRows = dtProhibitedList.Select("s_info_windcode='" + currCode + "'");
                    if (selectedRows.Length > 0)
                    {
                        msg += currCode + ": 已列入禁止库;\r\n";
                        continue;
                    }
                }

                try
                {
                    DataService.GetInstance().UpdateFundEquityPool(currCode, poolType, reason, analystid, isEquity, hedgefundid);
                }
                catch (Exception ex)
                {
                    msg += currCode + ":" + ex.Message + "\r\n";
                }                
            }

            TextBoxAuthCode.Text = "";

            if (msg.Length > 0)
            {
                TextBoxMsg.Text = msg;
                TextBoxMsg.Visible = true;
                lblStatus.Text = "错误！";
            }
            else 
            {
                TextBoxMsg.Visible = false;
                lblStatus.Text = "提交成功！";
            }
        }
        catch (Exception ex)
        {
            lblStatus.Text = ex.Message;
        }
    }
}
