<%@ Page Title="股票备选库" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="UpdateHistory.aspx.cs" Inherits="_Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .style1 {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        出入库变更记录
     </h2>


        <table class="style1">
            <tr>
                <td>证券代码</td>
                <td>
                    <asp:TextBox ID="TextBoxStockcode" runat="server">*</asp:TextBox>&nbsp;例如：600000.SH</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>证券类别</td>
                <td>
                    <asp:DropDownList ID="DropDownListSecurityType" runat="server"></asp:DropDownList>&nbsp;
                    <asp:DropDownList ID="DropDownListHedgefund" runat="server"></asp:DropDownList>&nbsp;
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>变更日期</td>
                <td>
                    <asp:TextBox ID="TextBoxStartDate" runat="server"></asp:TextBox>
                    至<asp:TextBox ID="TextBoxEndDate" runat="server"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td></td>
                <td><asp:Button ID="btnSearch" runat="server" Text="搜索" 
                        onclick="btnSearch_Click" />&nbsp; <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label></td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:GridView ID="GridViewFundEquityPool" runat="server" CellPadding="4" 
                        ForeColor="#333333" Width="100%"
                        GridLines="None" AutoGenerateColumns="False" AllowPaging="True" 
                        AllowSorting="True" PageSize="40" PagerSettings-PageButtonCount="10" 
                        EmptyDataText="(无)" 
                        onpageindexchanging="GridViewFundEquityPool_PageIndexChanging" PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" ShowHeaderWhenEmpty="True">
                        <AlternatingRowStyle BackColor="White" />
                        <EditRowStyle BackColor="#2461BF" />
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#EFF3FB" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                        <SortedDescendingHeaderStyle BackColor="#4870BE" />

                        <Columns> 
                            <asp:BoundField DataField="opdate" HeaderText="变更日期" Visible="true" HeaderStyle-HorizontalAlign="Left" DataFormatString="{0:yyyy-MM-dd}" /> 
                            <asp:BoundField DataField="s_info_windcode" HeaderText="代码" Visible="true" HeaderStyle-HorizontalAlign="Left" /> 
                            <asp:BoundField DataField="s_info_name" HeaderText="名称" Visible="true" HeaderStyle-HorizontalAlign="Left" /> 
                            <%--<asp:BoundField DataField="SW_IND_NAME1" HeaderText="一级行业" Visible="true" HeaderStyle-HorizontalAlign="Left" /> 
                            <asp:BoundField DataField="SW_IND_NAME2" HeaderText="二级行业" Visible="true" HeaderStyle-HorizontalAlign="Left" /> --%>
                            <asp:BoundField DataField="inbasepooltext" HeaderText="基础库" Visible="true"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" /> 
                            <asp:BoundField DataField="incorepooltext" HeaderText="核心库" Visible="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" /> 
                            <asp:BoundField DataField="inrestpooltext" HeaderText="限制库" Visible="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" /> 
                            <asp:BoundField DataField="inprohpooltext" HeaderText="禁止库" Visible="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" /> 
                            <asp:BoundField DataField="analystName" HeaderText="研究员" Visible="true" HeaderStyle-HorizontalAlign="Left" /> 
                            <asp:BoundField DataField="hedgefundname" HeaderText="公募/专户" Visible="true" HeaderStyle-HorizontalAlign="Left" /> 
                            <asp:BoundField DataField="submit" HeaderText="提交日期" Visible="true" HeaderStyle-HorizontalAlign="Left" DataFormatString="{0:yyyy-MM-dd}"  /> 
                            
                            <%--<asp:TemplateField HeaderText = "详细" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <%# "<a target=\"_blank\" href=\"" + Eval("URL") + "\">查看</a>"%>
                                    <%# "<a target=\"_blank\" href=\"" + Eval("URL") + "\">"+Eval("reportText")+"</a>"%>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            
                        </Columns> 
                    </asp:GridView>
                </td>
            </tr>
        </table>

</asp:Content>
