<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QuotePrint.aspx.cs" Inherits="QuotePrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>网下询价申请</title>
    <style>
        .table-a td{border:1px solid; font-size:small} 
        .focus
        {
            color: #FF0000;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div >
    <h2 style="text-align: center">新股询价审批表</h2>
        <table width="100%" cellspacing="0" cellpadding="0" class="table-a">
            <tr>
                <td colspan="4">1.发行信息（研究员填写）</td>
            </tr>
            <tr>
                <td style="width:20%">证券代码</td>
                <td style="width:30%">
                    <asp:TextBox ID="tbCode" runat="server" BorderStyle="None" 
                        style="background-color: #FFFF00" Columns="10" Font-Bold="True" 
                        Width="50px"></asp:TextBox>
                    <asp:Button ID="btnRefreshCode" runat="server" Text="刷新" 
                        onclick="btnRefreshCode_Click" />
                    <asp:Label ID="lblCode2" runat="server" Font-Bold="True"></asp:Label>
                </td>
                <td style="width:20%">证券简称</td>
                <td style="width:30%">
                    <asp:Label ID="lblSName" runat="server" Font-Bold="True" Font-Size="Large"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>初步询价起始日期</td>
                <td>
                    <asp:Label ID="lblQuoteStart" runat="server" Text=""></asp:Label>
                </td>
                <td>初步询价截止日期</td>
                <td>
                    <asp:Label ID="lblQuoteEnd" runat="server" Font-Bold="True" Font-Size="Large"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>主承销商</td>
                <td colspan="2">
                    <asp:Label ID="PrimaryUnderwriter" runat="server" Text=""></asp:Label>
                </td>
                <td>德邦证券
                    <asp:RadioButton ID="rbYes1" runat="server" Text="是" style="background-color: #FFFF00" GroupName="rbgPrimary"/>
                    <asp:RadioButton ID="rbNo1" runat="server" Text="否" style="background-color: #FFFF00" GroupName="rbgPrimary"  Checked="True" />
                    &nbsp;副主承销商/分销商</td>
            </tr>
            <tr>
                <td>联系人</td>
                <td>
                    <asp:Label ID="Contacts" runat="server" Text=""></asp:Label>
                </td>
                <td>联系电话</td>
                <td>
                    <asp:Label ID="Phone" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4">2.经办人信息（研究员填写）</td>
            </tr>
            <tr>
                <td>经办人</td>
                <td>
                    <asp:DropDownList ID="ddlAnalyst" runat="server" BorderStyle="None" 
                        style="background-color: #FFFF00" AutoPostBack="True" 
                        onselectedindexchanged="ddlAnalyst_SelectedIndexChanged"></asp:DropDownList>
                </td>
                <td>联系电话</td>
                <td>
                    <asp:Label ID="lblAnaPho" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>传真号码</td>
                <td>
                    <asp:Label ID="lblFax" runat="server" Text=""></asp:Label>
                </td>
                <td>Email</td>
                <td>
                    <asp:Label ID="lblEmail" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                对主承销商投资价值分析报告的评价
                <p>
                    <table>
                        <tr>
                            <td>1) 客观性评价(1-100分):</td>
                            <td><asp:TextBox ID="TextBox3" runat="server" BorderStyle="None" 
                                    style="background-color: #FFFF00" Columns="10" Font-Bold="True" 
                                    Font-Size="Large"></asp:TextBox>分</td>
                        </tr>
                        <tr>
                            <td>2) 估值合理性评价(1-100分):</td>
                            <td><asp:TextBox ID="TextBox4" runat="server" BorderStyle="None" 
                                    style="background-color: #FFFF00" Columns="10" Font-Bold="True" 
                                    Font-Size="Large"></asp:TextBox>分</td>
                        </tr>
                        <tr>
                            <td>3) 总体评价(1-100分):</td>
                            <td><asp:TextBox ID="TextBox5" runat="server" BorderStyle="None" 
                                    style="background-color: #FFFF00" Columns="10" Font-Bold="True" 
                                    Font-Size="Large"></asp:TextBox>分</td>
                        </tr>
                    </table>
                </p>
                </td>
            </tr>
            <tr>
                <td colspan="4">3.报价及申购意向信息（投资组合经理填写）
                </td>
            </tr>
            <tr>
                <td>组合名称</td>
                <td>
                    <asp:DropDownList ID="ddlFunds" runat="server" BorderStyle="None" 
                        style="background-color: #FFFF00" AutoPostBack="True" 
                        onselectedindexchanged="ddlFunds_SelectedIndexChanged" Font-Bold="True" 
                        Font-Size="Large"></asp:DropDownList>
                </td>
                <td>托管银行</td>
                <td>
                    <asp:Label ID="lblTrustee" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>净资产</td>
                <td>
                    <asp:Label ID="lblFundNAV" runat="server" Text="0"></asp:Label>
                    <asp:Label ID="lblFundNAVDate" runat="server" Text=""></asp:Label>
                    <asp:Label ID="lblFundNAVNum" runat="server" Text="0" Visible="False"></asp:Label>
                </td>
                <td colspan="2">托管银行与主承销商
                    <asp:RadioButton ID="rbYes2" runat="server" Text="是" style="background-color: #FFFF00" GroupName="rbgTrustee"/>
                    <asp:RadioButton ID="rbNo2" runat="server" Text="否" style="background-color: #FFFF00" GroupName="rbgTrustee"  Checked="True" />
                    &nbsp;关联方</td>
            </tr>
            <tr>
                <td>申报价格（元）</td>
                <td>申报数量（股）</td>
                <td>所需资金（万元）</td>
                <td>占组合净资产比例</td>
            </tr>
            <tr>
                <td><asp:TextBox ID="tbPrice" runat="server" BorderStyle="None" 
                        style="background-color: #FFFF00" Columns="10" Font-Bold="True" 
                        Font-Size="Large"></asp:TextBox></td>
                <td><asp:TextBox ID="tbVolume" runat="server" BorderStyle="None" 
                        style="background-color: #FFFF00" Columns="10" Font-Bold="True" 
                        Font-Size="Large"></asp:TextBox>
                    <asp:Button ID="btnRefreshAmount" runat="server" Text="刷新" 
                        onclick="btnRefreshAmount_Click"/>
                    <asp:Label ID="lblShares" runat="server" Text=""></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblAmount" runat="server" Font-Bold="True" Font-Size="Large"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblPercent" runat="server" Font-Bold="True" Font-Size="Large"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4">4.风险检查点（研究员、投资组合经理、风控人员填写）</td>
            </tr>
            <tr>
                <td>风险检查点</td>
                <td align="center">研究员</td>
                <td align="center">投资组合经理</td>
                <td align="center">风控人员</td>
            </tr>
            <tr>
                <td>1、是否存在关联交易</td>
                <td align="center">
                    <asp:RadioButton ID="RadioButton1" runat="server" Text="是" style="background-color: #FFFF00" GroupName="rbgCheck11"/>
                    <asp:RadioButton ID="RadioButton2" runat="server" Text="否" style="background-color: #FFFF00" GroupName="rbgCheck11"  Checked="True" />
                </td>
                <td align="center">——</td>
                <td align="center">
                    <asp:RadioButton ID="RadioButton3" runat="server" Text="是" style="background-color: #FFFF00" GroupName="rbgCheck31"/>
                    <asp:RadioButton ID="RadioButton4" runat="server" Text="否" style="background-color: #FFFF00" GroupName="rbgCheck31" Checked="True"/>
                </td>
            </tr>
            <tr>
                <td>2、网下拟申购数量不超过网下申购上限量</td>
                <td align="center">——</td>
                <td align="center">
                    <asp:RadioButton ID="RadioButton5" runat="server" Text="符合" style="background-color: #FFFF00" GroupName="rbgCheck22"  Checked="True" />
                    <asp:RadioButton ID="RadioButton6" runat="server" Text="不符合" style="background-color: #FFFF00" GroupName="rbgCheck22"/>
                </td>
                <td align="center">——</td>
            </tr>
            <tr>
                <td>3、申购后基金现金类资产比例不低于基金资产的5%（基金适用）</td>
                <td align="center">——</td>
                <td align="center">
                    <asp:RadioButton ID="RadioButton11" runat="server" Text="符合" style="background-color: #FFFF00" GroupName="rbgCheck23"  Checked="True" />
                    <asp:RadioButton ID="RadioButton12" runat="server" Text="不符合" style="background-color: #FFFF00" GroupName="rbgCheck23"/>
                </td>
                <td align="center">
                    <asp:RadioButton ID="RadioButton13" runat="server" Text="符合" style="background-color: #FFFF00" GroupName="rbgCheck33" Checked="True"/>
                    <asp:RadioButton ID="RadioButton14" runat="server" Text="不符合" style="background-color: #FFFF00" GroupName="rbgCheck33"/>
                </td>
            </tr>
            <tr>
                <td>4、符合询价及推介公告要求</td>
                <td align="center">
                    <asp:RadioButton ID="RadioButton7" runat="server" Text="符合"  Checked="True" 
                        style="background-color: #FFFF00" GroupName="rbgCheck14" />
                    <asp:RadioButton ID="RadioButton8" runat="server" Text="不符合" style="background-color: #FFFF00" GroupName="rbgCheck14"/>
                </td>
                <td align="center">
                    <asp:RadioButton ID="RadioButton9" runat="server" Text="符合" style="background-color: #FFFF00" GroupName="rbgCheck24"  Checked="True" />
                    <asp:RadioButton ID="RadioButton10" runat="server" Text="不符合" style="background-color: #FFFF00" GroupName="rbgCheck24"/>
                </td>
                <td align="center">——</td>
            </tr>
            <tr >
                <td colspan="4">
                    投资组合经理签字/日期：<br /><br /><br /><br /><br />
                    若为代签字，是否有授权（
                    <asp:CheckBox ID="RadioButton15" runat="server" Text="有" style="background-color: #FFFF00"  GroupName="rbgHasAuth"/>
                    <asp:CheckBox ID="RadioButton16" runat="server" Text="无" style="background-color: #FFFF00"  GroupName="rbgHasAuth"/>
                    ） 授权方式：（
                    <asp:CheckBox ID="RadioButton17" runat="server" Text="授权书" style="background-color: #FFFF00" GroupName="rbgAuthType" />
                    <asp:CheckBox ID="RadioButton18" runat="server" Text="邮件" style="background-color: #FFFF00"  GroupName="rbgAuthType"/>
                    <asp:CheckBox ID="RadioButton19" runat="server" Text="口头" style="background-color: #FFFF00"  GroupName="rbgAuthType"/>
                    ）
                    <br /><br />
                </td>
            </tr>
            <tr>
                <td colspan="4">5.签字区（研究员、风控人员、投资总监填写）</td>
            </tr>
            <tr>
                <td colspan="4">
                    <table width="100%" cellspacing="0" cellpadding="0" class="table-a">
                        <tr valign="top" style="height:100px">
                        <td style="width:30%">研究员/日期</td>
                        <td style="width:30%">风控人员/日期</td>
                        <td style="width:30%">投资总监/日期</td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
