<%@ Page Title="查询详情" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Detail.aspx.cs" Inherits="About" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        查询详情
    </h2>
    
    <table class="style1" width="100%">
            <tr>
                <td width="10%">代码</td>
                <td width="15%">
                    <asp:Label ID="LabelCode" runat="server" Text=""></asp:Label>
                </td>
                <td width="10%"><span id="spIndustryOrRating" runat="server">行业</span></td>
                <td width="15%">
                    <asp:Label ID="LabelIndustry" runat="server" Text=""></asp:Label>
                </td>
                <td width="10%">研究员</td>
                <td width="15%">
                    <asp:Label ID="LabelAnalyst" runat="server"></asp:Label>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    名称</td>
                <td>
                    <asp:Label ID="LabelName" runat="server" Text=""></asp:Label>
                </td>
                <td>
                    所在库</td>
                <td>
                    <asp:Label ID="LabelPoolName" runat="server" Text="(未分配)"></asp:Label>
                </td>
                <td></td>
                    <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td valign="top">
                    入库理由</td>
                <td colspan="5">
                    <asp:TextBox ID="TextBoxReason" runat="server" BorderStyle="None" 
                        ReadOnly="True" Rows="5" TextMode="MultiLine" Width="100%"></asp:TextBox>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td colspan="7">
                    <asp:GridView ID="GridViewReports" runat="server" CellPadding="4" Width="100%"
                        ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" 
                        Caption="研究报告" CaptionAlign="Left" EmptyDataText="(无)" 
                        ShowHeaderWhenEmpty="True">
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
                            <asp:BoundField DataField="reportdate" HeaderText="日期" Visible="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" /> 
                            <asp:BoundField DataField="analystname" HeaderText="研究员" Visible="true" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="10%" /> 
                            <asp:BoundField DataField="ReportType1" HeaderText="报告类型" Visible="true" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="10%" /> 
                            <asp:BoundField DataField="ReportType2" HeaderText="报告深度" Visible="true" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="10%" /> 
                            <asp:BoundField DataField="s_info_name" HeaderText="股票名称" Visible="true" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="10%" /> 
                            
                            <asp:TemplateField HeaderText = "报告">
                                <ItemTemplate>
                                    <%# "<a target=\"_blank\" href=\"" + Eval("URL") + "\">" + Eval("reportname") + "</a>"%>
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns> 

                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td colspan="7">
                <asp:GridView ID="GridViewHistory" runat="server" CellPadding="4" 
                        ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" 
                        Width="100%" Caption="变更记录" CaptionAlign="Left" EmptyDataText="(无)" 
                        ShowHeaderWhenEmpty="True">
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
                            <asp:BoundField DataField="opdate2" HeaderText="变更日期" Visible="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  HeaderStyle-Width="10%" /> 
                            <asp:BoundField DataField="inbasepooltext" HeaderText="基础库" Visible="true"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" /> 
                            <asp:BoundField DataField="incorepooltext" HeaderText="核心库" Visible="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  HeaderStyle-Width="10%"/> 
                            <asp:BoundField DataField="inrestpooltext" HeaderText="限制库" Visible="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  HeaderStyle-Width="10%"/> 
                            <asp:BoundField DataField="inprohpooltext" HeaderText="禁止库" Visible="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  HeaderStyle-Width="10%"/> 
                            <asp:BoundField DataField="analystname" HeaderText="研究员" Visible="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" /> 
                            <asp:BoundField DataField="hedgefundname" HeaderText="公募/专户" Visible="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" /> 
                            <asp:BoundField DataField="reason" HeaderText="理由" Visible="true" /> 
                        </Columns> 

                    </asp:GridView>
                </td>
            </tr>
        </table>

</asp:Content>
