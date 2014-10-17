<%@ Page Title="提交晨会报告" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="AddContent.aspx.cs" Inherits="About" %>

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
        填写晨会报告
    </h2>
    <table class="style1">
        <tr>
            <td width="20%">
                日期</td>
            <td width="70%">
                <asp:DropDownList ID="DropDownListSubmitDate" runat="server" 
                    AutoPostBack="True" 
                    onselectedindexchanged="DropDownListSubmitDate_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                类别</td>
            <td>
                <asp:DropDownList ID="DropDownListCategory" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="DropDownListCategory_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                关键词</td>
            <td>
                <asp:TextBox ID="TextBoxKeywords" runat="server" Columns="82"></asp:TextBox>
                <BR />
                <asp:Label ID="Label1" runat="server" Text="请输入涉及到的行业名称或股票名称" ForeColor="Blue"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td valign="top">
                内容</td>
            <td>
                <asp:TextBox ID="TextBoxContent" runat="server" Columns="80" Rows="20" 
                    TextMode="MultiLine"></asp:TextBox>
                <BR />
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorContent" runat="server" 
                    ControlToValidate="TextBoxContent" Display="Dynamic" 
                    ErrorMessage="请输入内容"
                    forecolor="Red"
                    >
                </asp:RequiredFieldValidator>
            </td>
            <td>
                &nbsp;</td>
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
            <td>
                &nbsp;</td>
            <td align="left">
                <asp:Button ID="btnSubmit" runat="server" Text="提交" onclick="btnSubmit_Click" 
                    Width="100px" /> &nbsp; &nbsp; &nbsp; &nbsp;
                <asp:Label ID="lblSubmitInfo" runat="server" ForeColor="Red"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
</table>
    
</asp:Content>
