<%@ Page Title="证券备选库" Language="C#" AutoEventWireup="true" CodeFile="Print.aspx.cs" Inherits="Print" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <span runat="server" id="spErrMsg"></span>

        <asp:GridView ID="GridViewPrint" runat="server" CellPadding="4" ForeColor="#333333" 
            GridLines="None" Width="100%" AutoGenerateColumns="False" 
            AllowSorting="True">
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
                <asp:BoundField DataField="s_info_windcode" HeaderText="代码" Visible="true" HeaderStyle-HorizontalAlign="Left" /> 
                <asp:BoundField DataField="s_info_name" HeaderText="名称" Visible="true" HeaderStyle-HorizontalAlign="Left" /> 
                <asp:BoundField DataField="SW_IND_NAME1" HeaderText="一级行业" Visible="true" HeaderStyle-HorizontalAlign="Left" /> 
                <asp:BoundField DataField="inbasepooltext" HeaderText="基础库" Visible="true"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" /> 
                <asp:BoundField DataField="incorepooltext" HeaderText="核心库" Visible="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" /> 
                <asp:BoundField DataField="inrestpooltext" HeaderText="限制库" Visible="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" /> 
                <asp:BoundField DataField="inprohpooltext" HeaderText="禁止库" Visible="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" /> 
                <asp:BoundField DataField="analystName" HeaderText="研究员" Visible="true" HeaderStyle-HorizontalAlign="Left" /> 
                <asp:BoundField DataField="hedgefundName" HeaderText="公募/专户" Visible="true" HeaderStyle-HorizontalAlign="Left" /> 
                <asp:BoundField DataField="opdate" HeaderText="变更日期" Visible="true" HeaderStyle-HorizontalAlign="Left" DataFormatString="{0:yyyy-MM-dd}" /> 
            </Columns> 

        </asp:GridView>

        <br />
        <div>
        请相关研究员认真确认以下事项：
        <ul>
        <li>准确性：以上所有在列的拟进行出入库变更的证券都已在投资研究联席会议中通过并签字生效</li>
        <li>完整性：所有投资研究联席会议中通过并签字生效的证券都已在此罗列拟进行出入库变更</li>
        </ul>
        研究员签字确认：_____________________
        </div>
    </div>
    </form>
</body>
</html>
