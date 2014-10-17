<%@ Page Title="查询报告" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" %>

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
        查询晨会报告
    </h2>
    <table class="style1">
        <tr>
            <td width="20%">报告日期</td>
            <td width="70%">
                <asp:TextBox ID="TextBoxReportDate" runat="server"></asp:TextBox>
                &nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnRefresh" runat="server" onclick="btnRefresh_Click" 
                    Text="刷新" />
                &nbsp;&nbsp;&nbsp;
                <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
            </td>
            <td width="20%">
                <asp:Button ID="btnExportToWord" runat="server" Text="导出为Word" 
                    onclick="btnExportToWord_Click" />
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td colspan="3">
            <div id="reportContent" runat="server">

            </div>
            </td>
        </tr>
</table>
</asp:Content>
