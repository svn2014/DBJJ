<%@ Page Title="股票备选库" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Statistic.aspx.cs" Inherits="_Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .style1 {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        统计数据
     </h2>


        <table class="style1">            
            <tr>
                <td colspan="3">
                    <asp:GridView ID="GridViewGroupByIndustry" runat="server" CellPadding="4" 
                        ForeColor="#333333" Width="100%"
                        GridLines="None" AutoGenerateColumns="False" AllowPaging="False" 
                        AllowSorting="False" EmptyDataText="(无)" 
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
                            <asp:BoundField DataField="SW_IND_NAME1" HeaderText="一级行业" Visible="true" HeaderStyle-HorizontalAlign="Left" /> 
                            <asp:BoundField DataField="totalcount" HeaderText="上市公司总数" Visible="true" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" /> 
                            <asp:BoundField DataField="sumofbasepool" HeaderText="入基础库数量" Visible="true" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" /> 
                            <asp:BoundField DataField="sumofcorepool" HeaderText="入核心库数量" Visible="true" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" /> 
                            <asp:BoundField DataField="sumofrestpool" HeaderText="入限制库数量" Visible="true" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" /> 
                            <asp:BoundField DataField="sumofprohpool" HeaderText="入禁止库数量" Visible="true" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />                             
                        </Columns> 
                    </asp:GridView>
                </td>
            </tr>

            <tr>
                <td colspan="3">
                    <asp:GridView ID="GridViewGroupByAnalyst" runat="server" CellPadding="4" 
                        ForeColor="#333333" Width="100%"
                        GridLines="None" AutoGenerateColumns="False" 
                        AllowSorting="True" 
                        EmptyDataText="(无)" 
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
                            <asp:BoundField DataField="analystName" HeaderText="研究员" Visible="true" HeaderStyle-HorizontalAlign="Left" />                             
                            <asp:BoundField DataField="sumofbasepool" HeaderText="入基础库数量" Visible="true" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" /> 
                            <asp:BoundField DataField="sumofcorepool" HeaderText="入核心库数量" Visible="true" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" /> 
                            <asp:BoundField DataField="sumofrestpool" HeaderText="入限制库数量" Visible="true" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" /> 
                            <asp:BoundField DataField="sumofprohpool" HeaderText="入禁止库数量" Visible="true" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />                        
                        </Columns> 
                    </asp:GridView>
                </td>
            </tr>
        </table>

</asp:Content>
