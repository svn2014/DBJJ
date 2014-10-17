<%@ Page Title="检索报告" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
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
        检索报告
    </h2>
    
    <table class="style1">
        <tr>
            <td style="width:10%">
                报告类型</td>
            <td>
                <asp:DropDownList ID="DropDownListType1" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="DropDownListType1_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td>
                开始日期</td>
            <td>
                <asp:TextBox ID="TextBoxStartDate" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                研究深度</td>
            <td>
                <asp:DropDownList ID="DropDownListType2" runat="server">
                </asp:DropDownList>
            </td>
            <td>
                结束日期</td>
            <td>
                <asp:TextBox ID="TextBoxEndDate" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                股票代码</td>
            <td>
                <asp:DropDownList ID="DropDownListStockCode" runat="server">
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                研究员</td>
            <td>
                <asp:DropDownList ID="DropDownListAnalyst" runat="server">
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                报告名称</td>
            <td>
                <asp:TextBox ID="TextBoxReportName" runat="server" Columns="50">*</asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr style="visibility: hidden">
            <td>
                关键词</td>
            <td>
                <asp:TextBox ID="TextBoxKeywords" runat="server" Columns="50">*</asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>
                <asp:Button ID="btnSearch" runat="server" Text="搜索" onclick="btnSearch_Click" />
            &nbsp;&nbsp;
                <asp:Label ID="lblStatus" runat="server" ForeColor="Red"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:GridView ID="GridViewReportList" runat="server" CellPadding="4" ForeColor="#333333" DataKeyNames="reportId"
                    GridLines="None" Width="100%" AutoGenerateColumns="False"
                    onrowdeleting="GridViewReportList_RowDeleting" 
                    onrowcommand="GridViewReportList_RowCommand" >
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <EditRowStyle BackColor="#999999" />
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />

                    <Columns> 
                        <asp:BoundField DataField="ReportId" HeaderText="编号" Visible="false" /> 
                        <asp:BoundField DataField="reportDate" HeaderText="日期" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"/> 
                        <asp:BoundField DataField="analystname" HeaderText="研究员" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"/> 
                        <asp:BoundField DataField="ReportType1" HeaderText="报告类型" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"/> 
                        <asp:BoundField DataField="ReportType2" HeaderText="报告深度" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"/> 
                        <asp:BoundField DataField="StockCode" HeaderText="股票代码" Visible="false"/> 
                        <asp:BoundField DataField="S_info_name" HeaderText="股票名称" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"/> 
                        <asp:BoundField DataField="industryname" HeaderText="行业" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"/> 
                        <asp:BoundField DataField="Keywords" HeaderText="关键词" Visible="false"/> 
                        <asp:BoundField DataField="reportName" HeaderText="报告名称" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" Visible="false"/> 
                        <asp:BoundField DataField="reportDesc" HeaderText="报告摘要" Visible="false"/> 
                        <asp:TemplateField HeaderText = "报告名称">
                            <ItemTemplate>
                                <%# "<a target=\"_blank\" href=\"" + Eval("URL") + "\">" + Eval("reportName") + "</a>"%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText = "审批">
                            <ItemTemplate>
                                <%# "<a target=\"_blank\" href=\"" + Eval("approvalURL") + "\">" + Eval("approvalText") + "</a>"%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="操作" ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButtonAction" runat="server" CausesValidation="False" CommandName="Delete" CommandArgument="ReportId" Text="删除" OnClientClick="return confirm('确认删除？');" ></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns> 
                </asp:GridView>
            </td>
        </tr>
    </table>
    
</asp:Content>
