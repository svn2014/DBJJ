<%@ Page Title="备选库变更" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="UpdatePool.aspx.cs" Inherits="About" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        变更
    </h2>

    <table class="style1">
        <tr>
            <td width="10%">
                库类别</td>
            <td width="50%">
                <asp:DropDownList ID="DropDownListSecurityType" runat="server"></asp:DropDownList>&nbsp;
                <asp:DropDownList ID="DropDownListPoolType" runat="server"></asp:DropDownList>&nbsp;                
                <asp:DropDownList ID="DropDownListHedgefund" runat="server"></asp:DropDownList>&nbsp;
                </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td width="10%">
                研究员</td>
            <td width="50%">
                <asp:DropDownList ID="DropDownListAnalyst" runat="server"></asp:DropDownList>
                </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td valign="top">
                证券代码</td>
            <td>
                <asp:TextBox ID="TextBoxCodes" runat="server" Rows="10" TextMode="MultiLine" 
                    Width="100%"></asp:TextBox>
            </td>
            <td valign="top">
                例：<br />
                000001.SZ<br />
                600000.SH</td>
        </tr>
        <tr>
            <td valign="top">
                入库原因</td>
            <td>
                <asp:TextBox ID="TextBoxReason" runat="server" Rows="10" TextMode="MultiLine" 
                    Width="100%"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                入库检查</td>
            <td colspan="2">
                &nbsp;如果入库证券已经存在于下列库中，则会提示出错<br />
                <asp:CheckBox ID="CheckBoxCheckOnBase" runat="server" Checked="True" Text="基础库复核" />&nbsp;
                <asp:CheckBox ID="CheckBoxCheckOnCore" runat="server" Checked="True" Text="核心库复核" />&nbsp;
                <asp:CheckBox ID="CheckBoxCheckOnRestrict" runat="server" Checked="True" Text="限制库复核" />&nbsp;
                <asp:CheckBox ID="CheckBoxCheckOnProhibited" runat="server" Checked="True" Text="禁止库复核" />&nbsp;
                <asp:CheckBox ID="CheckBoxCheckCode" runat="server" Checked="True" Text="证券代码复核" />&nbsp;
            </td>
        </tr>
        <tr>
            <td>
                授权码</td>
            <td>
                <asp:TextBox ID="TextBoxAuthCode" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="Label1" runat="server" ForeColor="White" Text="2012080715T"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="提交" onclick="btnSubmit_Click" /> &nbsp;
                <asp:Label ID="lblStatus" runat="server" ForeColor="Red"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <asp:TextBox ID="TextBoxMsg" runat="server" BorderStyle="None" Rows="10" 
                    TextMode="MultiLine" Visible="False" Width="100%"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
        </tr>
    </table>

</asp:Content>
