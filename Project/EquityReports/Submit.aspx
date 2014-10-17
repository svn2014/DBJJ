<%@ Page Title="上传报告" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Submit.aspx.cs" Inherits="_SubmitPage" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            color: #FF0000;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        上传报告
    </h2>
    
    <table class="style1">
        <tr>
            <td style="width:10%">
                报告日期<span class="style2">*</span></td>
            <td>
                <asp:TextBox ID="TextBoxReportDate" runat="server"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td style="width:10%">
                报告类型<span class="style2">*</span></td>
            <td>
                <asp:DropDownList ID="DropDownListType1" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="DropDownListType1_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorReportType1" 
                    runat="server" ControlToValidate="DropDownListType1" Display="Dynamic" 
                    ErrorMessage="请选择报告类型" ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                研究深度<span class="style2">*</span></td>
            <td>
                <asp:DropDownList ID="DropDownListType2" runat="server">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorReportType2" 
                    runat="server" ControlToValidate="DropDownListType2" Display="Dynamic" 
                    ErrorMessage="请选择研究深度" ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                股票代码<span class="style2">*</span></td>
            <td>
                <asp:DropDownList ID="DropDownListStockCode" runat="server"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorReportType3" 
                    runat="server" ControlToValidate="DropDownListStockCode" Display="Dynamic" 
                    ErrorMessage="请选择股票代码" ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                研究员<span class="style2">*</span></td>
            <td>
                <asp:DropDownList ID="DropDownListAnalyst" runat="server">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorReportType4" 
                    runat="server" ControlToValidate="DropDownListAnalyst" Display="Dynamic" 
                    ErrorMessage="请选择研究员" ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr style="visibility: hidden; display: none;">
            <td>
                报告名称</td>
            <td>
                <asp:TextBox ID="TextBoxReportName" runat="server" Columns="50" Visible="false"></asp:TextBox>
                (自动识别)
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td style="width:10%">
                选择研究报告<span class="style2">*</span></td>
            <td>
                <asp:FileUpload ID="FileUploadAttachment" runat="server" Width="400px" />推荐使用pdf格式
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" 
                    runat="server" ControlToValidate="FileUploadAttachment" Display="Dynamic" 
                    ErrorMessage="请选择研究报告" ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td style="width:10%">
                研究总监意见<span class="style2">*</span></td>
            <td>
                <asp:FileUpload ID="FileUploadApproval" runat="server" Width="400px" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" 
                    runat="server" ControlToValidate="FileUploadApproval" Display="Dynamic" 
                    ErrorMessage="请选择研究总监意见" ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr style="visibility: hidden">
            <td>
                关键词</td>
            <td>
                <asp:TextBox ID="TextBoxKeywords" runat="server" Columns="50"></asp:TextBox>
                <asp:TextBox ID="TextBoxDesc" runat="server" Columns="50" Visible= "false"></asp:TextBox>
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
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="提交" onclick="btnSubmit_Click" />&nbsp;&nbsp;&nbsp;
                <asp:Label ID="lblStatus" runat="server" ForeColor="Red"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <a id="aFolder" runat="server" visible="false">查看研究报告</a><br/>
                <a id="aApproval" runat="server" visible="false">查看研究总监意见</a>
            </td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
    
</asp:Content>
