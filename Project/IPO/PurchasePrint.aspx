<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchasePrint.aspx.cs" Inherits="QuotePrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>新股申购申请</title>
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
    <h2 style="text-align: center">新股申购审批表</h2>
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
                    <asp:Button ID="btnRefreshCode" runat="server" Text="刷新" onclick="btnRefreshCode_Click" />
                    <asp:Label ID="lblCode2" runat="server" Font-Bold="True"></asp:Label>
                </td>
                <td style="width:20%">证券简称</td>
                <td style="width:30%">
                    <asp:Label ID="lblSName" runat="server" Font-Bold="True" Font-Size="Large"></asp:Label>
                </td>
            </tr>
            
            <tr>
                <td>申购截止日</td>
                <td>
                    <asp:Label ID="lblPurchaseDate" runat="server" Font-Bold="False"></asp:Label>
                </td>
                <td>网上申购上限(万股)</td>
                <td>
                    <asp:Label ID="lblOnNetUpLimit" runat="server" Font-Bold="True" Font-Size="Large"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>网下申购下限(万股)</td>
                <td>
                    <asp:Label ID="lblUnderNetDownLimit" runat="server" Font-Bold="True" 
                        Font-Size="Large"></asp:Label>
                </td>
                <td>网下申购上限(万股)</td>
                <td>
                    <asp:Label ID="lblUnderNetUpLimit" runat="server" Font-Bold="True" 
                        Font-Size="Large"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>主承销商</td>
                <td colspan="2">
                    <asp:Label ID="PrimaryUnderwriter" runat="server" Text=""></asp:Label>
                </td>
                <td>德邦证券
                    <asp:RadioButton ID="rbYes1" runat="server" Text="是" style="background-color: #FFFF00" GroupName="rbgPrimary"/>
                    <asp:RadioButton ID="rbNo1" runat="server" Text="否" style="background-color: #FFFF00" GroupName="rbgPrimary" Checked="True" />
                    &nbsp;副主承销商/分销商</td>
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
            <tr style="height:80px">
                <td valign="top" colspan="4">研究员签字/日期</td>
            </tr>
            <tr>
                <td colspan="4">2.申购方案（投资组合经理填写）
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
                <td><asp:Label ID="lblFundNAV" runat="server" Text=""></asp:Label>
                    <asp:Label ID="lblFundNAVDate" runat="server" Text=""></asp:Label>
                    <asp:Label ID="lblFundNAVNum" runat="server" Text="0" Visible="False"></asp:Label>
                </td>
                <td colspan="2">托管银行与主承销商
                    <asp:RadioButton ID="rbYes2" runat="server" Text="是" style="background-color: #FFFF00" GroupName="rbgTrustee"/>
                    <asp:RadioButton ID="rbNo2" runat="server" Text="否" style="background-color: #FFFF00" GroupName="rbgTrustee"  Checked="True" />
                    &nbsp;关联方</td>
            </tr>
            <tr>
                <td>是否已参与新股网下询价</td>
                <td>
                    <asp:RadioButton ID="rbYes4" runat="server" Text="是" 
                        style="background-color: #FFFF00" GroupName="rbgHasQuoted" 
                        Font-Bold="True" Font-Size="Large"/>
                    <asp:RadioButton ID="rbNo4" runat="server" Text="否" 
                        style="background-color: #FFFF00" GroupName="rbgHasQuoted" 
                        Font-Bold="True" Font-Size="Large"/>
                </td>
                <td>申购方式</td>
                <td>
                    <asp:RadioButton ID="RadioButton3" runat="server" Text="网下" 
                        style="background-color: #FFFF00" GroupName="rbgPurchaseType" 
                        Font-Bold="True" Font-Size="Large"/>
                    <asp:RadioButton ID="RadioButton4" runat="server" Text="网上" 
                        style="background-color: #FFFF00" GroupName="rbgPurchaseType" 
                        Font-Bold="True" Font-Size="Large"/>
                </td>
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
            <tr >
                <td colspan="4">
                    投资组合经理签字/日期：<br /><br /><br />
                    若为代签字，是否有授权（
                    <asp:CheckBox ID="RadioButton15" runat="server" Text="有" style="background-color: #FFFF00"  GroupName="rbgHasAuth"/>
                    <asp:CheckBox ID="RadioButton16" runat="server" Text="无" style="background-color: #FFFF00"  GroupName="rbgHasAuth"/>
                    ） 授权方式：（
                    <asp:CheckBox ID="RadioButton17" runat="server" Text="授权书" style="background-color: #FFFF00" GroupName="rbgAuthType" />
                    <asp:CheckBox ID="RadioButton18" runat="server" Text="邮件" style="background-color: #FFFF00"  GroupName="rbgAuthType"/>
                    <asp:CheckBox ID="RadioButton19" runat="server" Text="口头" style="background-color: #FFFF00"  GroupName="rbgAuthType"/>
                    ）
                </td>
            </tr>
            <tr>
                <td colspan="4">3.风险检查点（投资组合经理、风控人员填写）</td>
            </tr>
            <tr>
                <td>风险检查点</td>
                <td align="center">投资组合经理</td>
                <td align="center">风控人员</td>
                <td align="center">备注</td>
            </tr>
            <tr>
                <td>1、是否存在关联交易</td>
                <td align="center">——</td>
                <td align="center">
                    <asp:RadioButton ID="RadioButton1" runat="server" GroupName="rbgCheck12" 
                        style="background-color: #FFFF00" Text="是" />
                    <asp:RadioButton ID="RadioButton2" runat="server" GroupName="rbgCheck12"  Checked="True"
                        style="background-color: #FFFF00" Text="否" />
                </td>
                <td align="center">
                    &nbsp;</td>
            </tr>
            <tr>
                <td>2、申购量不能超过申购上限量</td>
                <td align="center">
                    <asp:RadioButton ID="RadioButton20" runat="server" Text="符合"  Checked="True" 
                        style="background-color: #FFFF00" GroupName="rbgCheck21" />
                    <asp:RadioButton ID="RadioButton21" runat="server" Text="不符合" 
                        style="background-color: #FFFF00" GroupName="rbgCheck21"/>
                </td>
                <td align="center">
                    <asp:RadioButton ID="RadioButton5" runat="server" Text="符合" style="background-color: #FFFF00" GroupName="rbgCheck22" Checked="True"/>
                    <asp:RadioButton ID="RadioButton6" runat="server" Text="不符合" style="background-color: #FFFF00" GroupName="rbgCheck22" />
                </td>
                <td align="center">&nbsp;</td>
            </tr>
            <tr>
                <td>3、申购金额不能超过组合资产的100%</td>
                <td align="center">
                    <asp:RadioButton ID="RadioButton22" runat="server" Text="符合"  Checked="True" 
                        style="background-color: #FFFF00" GroupName="rbgCheck31" />
                    <asp:RadioButton ID="RadioButton23" runat="server" Text="不符合" 
                        style="background-color: #FFFF00" GroupName="rbgCheck31"/>
                </td>
                <td align="center">
                    <asp:RadioButton ID="RadioButton11" runat="server" Text="符合" style="background-color: #FFFF00" GroupName="rbgCheck32" Checked="True"/>
                    <asp:RadioButton ID="RadioButton12" runat="server" Text="不符合" style="background-color: #FFFF00" GroupName="rbgCheck32" />
                </td>
                <td align="center">
                    &nbsp;</td>
            </tr>
            <tr>
                <td>4、申购后单只基金所持该股票市值不能超过基金资产的10%（基金适用）</td>
                <td align="center">
                    <asp:RadioButton ID="RadioButton7" runat="server" Text="符合"  Checked="True" 
                        style="background-color: #FFFF00" GroupName="rbgCheck41" />
                    <asp:RadioButton ID="RadioButton8" runat="server" Text="不符合" style="background-color: #FFFF00" GroupName="rbgCheck41"/>
                </td>
                <td align="center">
                    <asp:RadioButton ID="RadioButton9" runat="server" Text="符合" style="background-color: #FFFF00" GroupName="rbgCheck42" Checked="True"/>
                    <asp:RadioButton ID="RadioButton10" runat="server" Text="不符合" style="background-color: #FFFF00" GroupName="rbgCheck42"/>
                </td>
                <td align="center">&nbsp;</td>
            </tr>
            
            <tr>
                <td>5、申购后全部基金持有该证券不能超过证券发行总股本的10%（基金适用）</td>
                <td align="center">
                    ——</td>
                <td align="center">
                    <asp:RadioButton ID="RadioButton30" runat="server" Text="符合"  Checked="True"
                        style="background-color: #FFFF00" GroupName="rbgCheck52"/>
                    <asp:RadioButton ID="RadioButton31" runat="server" Text="不符合" 
                        style="background-color: #FFFF00" GroupName="rbgCheck52"/>
                </td>
                <td align="center">
                    <asp:Label ID="lblTotalCapital" runat="server"></asp:Label>
                </td>
            </tr>
            
            <tr>
                <td>6、申购后基金现金类资产比例不低于基金资产的5% （基金适用）</td>
                <td align="center">
                    <asp:RadioButton ID="RadioButton24" runat="server" Text="符合"  Checked="True" 
                        style="background-color: #FFFF00" GroupName="rbgCheck61" />
                    <asp:RadioButton ID="RadioButton25" runat="server" Text="不符合" 
                        style="background-color: #FFFF00" GroupName="rbgCheck61"/>
                </td>
                <td align="center">
                    <asp:RadioButton ID="RadioButton32" runat="server" Text="符合"  Checked="True"
                        style="background-color: #FFFF00" GroupName="rbgCheck62"/>
                    <asp:RadioButton ID="RadioButton33" runat="server" Text="不符合" 
                        style="background-color: #FFFF00" GroupName="rbgCheck62"/>
                </td>
                <td align="center">&nbsp;</td>
            </tr>
            
            <tr>
                <td>7、网上申购上限不超过相关规定上限</td>
                <td align="center">
                    <asp:RadioButton ID="RadioButton26" runat="server" Text="符合"  Checked="True" 
                        style="background-color: #FFFF00" GroupName="rbgCheck71" />
                    <asp:RadioButton ID="RadioButton27" runat="server" Text="不符合" 
                        style="background-color: #FFFF00" GroupName="rbgCheck71"/>
                </td>
                <td align="center">
                    <asp:RadioButton ID="RadioButton34" runat="server" Text="符合"  Checked="True"
                        style="background-color: #FFFF00" GroupName="rbgCheck72"/>
                    <asp:RadioButton ID="RadioButton35" runat="server" Text="不符合" 
                        style="background-color: #FFFF00" GroupName="rbgCheck72"/>
                </td>
                <td align="center">&nbsp;</td>
            </tr>
            
            <tr>
                <td>8、参与网上申购的组合只能使用一个证券账户</td>
                <td align="center">
                    ——</td>
                <td align="center">
                    ——</td>
                <td align="center">&nbsp;</td>
            </tr>
            
            <tr>
                <td>9、已参与网下发行的组合不得参与网上申购</td>
                <td align="center">
                    <asp:RadioButton ID="RadioButton28" runat="server" Text="符合"  Checked="True" 
                        style="background-color: #FFFF00" GroupName="rbgCheck91" />
                    <asp:RadioButton ID="RadioButton29" runat="server" Text="不符合" 
                        style="background-color: #FFFF00" GroupName="rbgCheck91"/>
                </td>
                <td align="center">
                    <asp:RadioButton ID="RadioButton36" runat="server" Text="符合"  Checked="True"
                        style="background-color: #FFFF00" GroupName="rbgCheck92"/>
                    <asp:RadioButton ID="RadioButton37" runat="server" Text="不符合" 
                        style="background-color: #FFFF00" GroupName="rbgCheck92"/>
                </td>
                <td align="center">&nbsp;</td>
            </tr>
            
            <tr>
                <td>10、网下申购符合新股询价时的申购承诺</td>
                <td align="center">
                    <asp:RadioButton ID="RadioButton38" runat="server" Text="有承诺" 
                        style="background-color: #FFFF00" GroupName="rbgCheck101" />
                    <asp:RadioButton ID="RadioButton39" runat="server" Text="无"  Checked="True" 
                        style="background-color: #FFFF00" GroupName="rbgCheck101"/>
                </td>
                <td align="center">
                    <asp:RadioButton ID="RadioButton40" runat="server" Text="有承诺" 
                        style="background-color: #FFFF00" GroupName="rbgCheck102" />
                    <asp:RadioButton ID="RadioButton41" runat="server" Text="无"  Checked="True"
                        style="background-color: #FFFF00" GroupName="rbgCheck102"/>
                </td>
                <td align="center">&nbsp;</td>
            </tr>
            
            <tr>
                <td colspan="4">4.审批签字区（投资风控人员、投资总监填写）</td>
            </tr>
            <tr style="height:80px">
                <td valign="top" colspan="2">风控人员/日期</td>
                <td valign="top" colspan="2">投资总监/日期</td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
