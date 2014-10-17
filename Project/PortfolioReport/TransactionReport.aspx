<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TransactionReport.aspx.cs" Inherits="TransactionReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>申赎与交易明细</title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />   
</head>
<body class="main">

<div class="main">
    <form id="form1" runat="server">
    <asp:label ID="LabelStatus" runat="server" text="" Visible="false" ForeColor="Red"></asp:label>

    <div>
    

        <table style="background-color:White; width:100%">
        <tr>
            <td colspan="2" align="center"><h1>申赎与交易明细</h1> <hr /></td>
        </tr>
        <tr>
            <td colspan="2"><h2>基本信息</h2><span runat="server" id="spFundInfo"/></td>
        </tr><tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
        <td colspan="2">
            <h2>申赎明细</h2>
            <asp:GridView ID="GridViewSubscribeRedeem" runat="server" CellPadding="4"
                    ForeColor="#333333" GridLines="None" Width="100%" PageSize="30" 
                    AutoGenerateColumns="False" AllowPaging="True" 
                    EmptyDataText="(无)"                     
                    ShowHeaderWhenEmpty="True" 
                onpageindexchanging="GridViewSubscribeRedeem_PageIndexChanging" onrowdatabound="GridViewSubscribeRedeem_RowDataBound"
                    >
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
                        <asp:BoundField DataField="ReportDate" HeaderText="日期"  HeaderStyle-HorizontalAlign="Left" DataFormatString="{0:yyyy-MM-dd}"/> 
                        <asp:BoundField DataField="UnitCumNAV" HeaderText="累计单位净值"  DataFormatString="{0:N4}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="PIC" HeaderText="剩余份额(万份)"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="Subscribe" HeaderText="申购金额(万元)"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />                    
                        <asp:BoundField DataField="Redeem" HeaderText="连续两日赎回金额(万元)"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="EquityWeight" HeaderText="股票仓位"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="YieldOnEquity" HeaderText="收益率"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="BondWeight" HeaderText="债券仓位"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="NetYieldOnBond" HeaderText="净价收益率"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                    </Columns> 

                </asp:GridView>
        </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
        <td colspan="2">
            <h2>交易明细</h2>
            <asp:GridView ID="GridViewTradingDetail" runat="server" CellPadding="4"
                    ForeColor="#333333" GridLines="None" Width="100%" PageSize="30" 
                    AutoGenerateColumns="False" AllowPaging="True" 
                    EmptyDataText="(无)"                     
                    ShowHeaderWhenEmpty="True" 
                    onpageindexchanging="GridViewTradingDetail_PageIndexChanging" 
                    onrowdatabound="GridViewTradingDetail_RowDataBound">
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
                        <asp:BoundField DataField="TransactionDate" HeaderText="交易日期"  HeaderStyle-HorizontalAlign="Left" DataFormatString="{0:yyyy-MM-dd}"/> 
                        <asp:BoundField DataField="TransactionType" HeaderText="交易类型"  HeaderStyle-HorizontalAlign="Left" /> 
                        <asp:BoundField DataField="Code" HeaderText="代码"  HeaderStyle-HorizontalAlign="Left" /> 
                        <asp:BoundField DataField="Name" HeaderText="名称"  HeaderStyle-HorizontalAlign="Left" /> 
                        <asp:BoundField DataField="Industry" HeaderText="行业" HeaderStyle-HorizontalAlign="Left" /> 
                        <asp:BoundField DataField="TradingPrice" HeaderText="交易价(估)"  DataFormatString="{0:F2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="MarketPrice" HeaderText="结算价"  DataFormatString="{0:F2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="TradingVolume" HeaderText="交易量(万股)"  DataFormatString="{0:F2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="HoldingMV" HeaderText="市值(万元)"  DataFormatString="{0:F2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="TransactionReturn" HeaderText="收益率"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="pctofNAV" HeaderText="占净值(%)" DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"/> 
                        <asp:BoundField DataField="PortfolioWeight" SortExpression="PortfolioWeight" HeaderText="组合权重"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />                         
                    </Columns> 

                </asp:GridView>
        </td>
        </tr>
        
        </table>


    </div>
    </form>
</div>

</body>
</html>
