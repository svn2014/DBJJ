<%@ Page Title="搜索信息" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Search.aspx.cs" Inherits="About" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        搜索信息
    </h2>
    <table class="style1">
        <tr>
            <td width="20%">
                报告日期</td>
            <td width="70%">
                <asp:TextBox ID="TextBoxStartDate" runat="server"></asp:TextBox>
                至
                <asp:TextBox ID="TextBoxEndDate" runat="server"></asp:TextBox>
                
            </td>
            <td width="20%">
                
            </td>
        </tr>
        <tr>
            <td>
                关键词</td>
            <td>
                <asp:TextBox ID="TextBoxKeywords" runat="server"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <asp:Button ID="btnSearch" runat="server" Text="搜索" onclick="btnSearch_Click" />
            </td>
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
