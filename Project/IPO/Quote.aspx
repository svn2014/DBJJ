<%@ Page Title="询价与申购申请表" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Quote.aspx.cs" Inherits="Quote" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        询价与申购申请表</h2>
    <p>
        <table width="100%">
            <tr>
                <td colspan="4"><hr /></td>
            </tr>
            <tr>
                <td width="100px">
                    经办人
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddlAnalyst" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="4"><hr /></td>
            </tr>
            <tr>
                <td valign="top">
                    投资标的
                </td>
                <td colspan="3">
                    <asp:CheckBoxList ID="cblIPOs" runat="server"></asp:CheckBoxList>
                    <BR />
                    <%--自动钩选: 
                    <asp:Button ID="btnQuoteToday" runat="server" Text="今日询价" />&nbsp;
                    <asp:Button ID="btnPurchaseToday" runat="server" Text="今日申购" />--%>
                </td>
            </tr>
            <tr>
                <td colspan="4"><hr /></td>
            </tr>
            <tr>
                <td valign="top">
                    投资组合                    
                </td>
                <td colspan="3">
                    <asp:CheckBoxList ID="cblFunds" runat="server"></asp:CheckBoxList>
                </td>
            </tr>
            <tr>
                <td colspan="4"><hr /></td>
            </tr>
            <tr>
                <td>网下</td>
                <td><asp:Button ID="btnQuote" runat="server" Text="询价" onclick="btnQuote_Click"/>&nbsp;
                    <asp:Button ID="btnPurchase1" runat="server" Text="申购" onclick="btnPurchase1_Click"/>
                </td>
            </tr>
            <tr>
                <td>网上</td>
                <td colspan="3">
                    <asp:Button ID="btnPurchase2" runat="server" Text="申购" onclick="btnPurchase2_Click" />
                </td>
            </tr>
        </table>
    </p>
</asp:Content>
