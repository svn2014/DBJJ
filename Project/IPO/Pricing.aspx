<%@ Page Title="定价参考" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Pricing.aspx.cs" Inherits="About" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        定价参考
    </h2>
    <p>
        <table width="100%">
            <tr>
                <td>
                    <asp:Label ID="lblIPOCode" runat="server" Text="请输入股票代码"></asp:Label> &nbsp;
                    <asp:TextBox ID="tbIPOCode" runat="server"></asp:TextBox> &nbsp;
                    <asp:Button ID="btnRun" runat="server" Text="运行" />
                </td>
            </tr>
            <tr>
                <td><hr /></td>
            </tr>
        </table>
    </p>
</asp:Content>
