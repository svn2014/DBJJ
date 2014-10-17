<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PositionReport.aspx.cs" Inherits="PositionReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>持仓与归因分析</title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />   
</head>
<body class="main">

<div class="main">
    <form id="form1" runat="server">
    <asp:label ID="LabelStatus" runat="server" text="" Visible="false" ForeColor="White"></asp:label>
    <div id="reportDIV" runat="server" visible="false">    
    
    <table style="background-color:White; width:100%">
        <tr>
            <td colspan="2" align="center"><h1>持仓与归因分析</h1> <hr /></td>
        </tr>
        <tr>
            <td colspan="2"><h2>基本信息</h2><span runat="server" id="spFundInfo"/></td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td colspan="2">
                <h2>归因分析</h2>
                <asp:GridView ID="GridViewIndustry" runat="server" 
                    AllowSorting="True" AutoGenerateColumns="False" CellPadding="4" 
                    ForeColor="#333333" GridLines="None" Width="100%" 
                    onrowdatabound="GridViewIndustry_RowDataBound"
                    Caption="股票业绩归因" ShowHeaderWhenEmpty="True" CaptionAlign="Left">
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
                    <asp:BoundField DataField="INDUSTRY" HeaderText="行业" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" /> 
                    <asp:BoundField DataField="MARKETVALUE" HeaderText="持有市值(万元)" DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"/> 
                    <asp:BoundField DataField="PORTFWEIGHT" HeaderText="组合权重"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                    <asp:BoundField DataField="BENCHWEIGHT" HeaderText="基准权重"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                    <asp:BoundField DataField="PORTFRET" HeaderText="组合回报"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                    <asp:BoundField DataField="BENCHRET" HeaderText="基准回报"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                    <asp:BoundField DataField="INDUSTRYATTR" HeaderText="配置回报"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                    <asp:BoundField DataField="SECURITYATTR" HeaderText="选股回报"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                    <asp:BoundField DataField="CROSSATTR" HeaderText="交叉回报"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                    <asp:BoundField DataField="VALUEADDED" HeaderText="增加值"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
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
                <h2>持仓分析</h2>
                <asp:GridView ID="GridViewEquityPositionDetail" runat="server" CellPadding="4" 
                    ForeColor="#333333" GridLines="None" Width="100%"
                    AutoGenerateColumns="False"
                    EmptyDataText="(无)" 
                    onrowdatabound="GridViewEquityPositionDetail_RowDataBound" 
                    Caption="股票投资" ShowHeaderWhenEmpty="True" CaptionAlign="Left">
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
                        <asp:BoundField DataField="CODE" HeaderText="股票代码"  HeaderStyle-HorizontalAlign="Left" /> 
                        <asp:BoundField DataField="NAME" HeaderText="股票名称"  HeaderStyle-HorizontalAlign="Left" /> 
                        <asp:BoundField DataField="INDUSTRY1" HeaderText="行业" HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="QUANTITY" HeaderText="数量(万股)"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="COST" HeaderText="原始成本(万元)"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="MARKETVALUE" HeaderText="市值(万元)"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="MARKETVALUEPCT" HeaderText="占净值(%)" DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"/> 
                        <asp:BoundField DataField="HOLDPERIODYIELD" HeaderText="收益率(%)"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="HOLDPERIODRET" HeaderText="期间回报"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="DATEIN" HeaderText="买入日期" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"/>
                        <asp:BoundField DataField="DATEOUT" HeaderText="卖出日期" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"/>
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
            
                <asp:GridView ID="GridViewBondPositionDetail" runat="server" CellPadding="4" 
                    ForeColor="#333333" GridLines="None" Width="100%"
                    AutoGenerateColumns="False"
                    EmptyDataText="(无)" 
                    Caption="债券投资" ShowHeaderWhenEmpty="True"  CaptionAlign="Left"
                    onrowdatabound="GridViewBondPositionDetail_RowDataBound" 
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
                        <asp:BoundField DataField="CODE" HeaderText="债券代码"  HeaderStyle-HorizontalAlign="Left" /> 
                        <asp:BoundField DataField="NAME" HeaderText="债券名称"  HeaderStyle-HorizontalAlign="Left" /> 
                        <asp:BoundField DataField="QUANTITY" HeaderText="持有量(万份)"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="COST" HeaderText="原始成本(万元)"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="MARKETVALUE" HeaderText="市值(万元)"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="MARKETVALUEPCT" HeaderText="占净值(%)" DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"/> 
                        <asp:BoundField DataField="BONDCREDITRATE" HeaderText="评级" HeaderStyle-HorizontalAlign="Left"  /> 
                        <asp:BoundField DataField="NORMINALINTRATE" HeaderText="票面利率"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="INTPAYMENTDATE" HeaderText="付息日"  HeaderStyle-HorizontalAlign="Left" DataFormatString="{0:yyyy-MM-dd}"/>
                        <asp:BoundField DataField="DELISTEDDATE" HeaderText="到期日"  HeaderStyle-HorizontalAlign="Left" DataFormatString="{0:yyyy-MM-dd}"/> 
                        <asp:BoundField DataField="HOLDPERIODYIELD" HeaderText="全价收益率(%)" DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"/> 
                        <asp:BoundField DataField="HOLDPERIODRET" HeaderText="期间回报"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />                         
                        <asp:BoundField DataField="DATEIN" HeaderText="买入日期" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="DATEOUT" HeaderText="卖出日期" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
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
            
                <asp:GridView ID="GridViewFundPositionDetail" runat="server" CellPadding="4" 
                    ForeColor="#333333" GridLines="None" Width="100%"
                    AutoGenerateColumns="False"
                    EmptyDataText="(无)" 
                    Caption="基金投资" ShowHeaderWhenEmpty="True"  CaptionAlign="Left"
                    onrowdatabound="GridViewFundPositionDetail_RowDataBound" 
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
                        <asp:BoundField DataField="CODE" HeaderText="基金代码"  HeaderStyle-HorizontalAlign="Left" /> 
                        <asp:BoundField DataField="NAME" HeaderText="基金名称"  HeaderStyle-HorizontalAlign="Left" /> 
                        <asp:BoundField DataField="QUANTITY" HeaderText="持有量(万份)"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="COST" HeaderText="原始成本(万元)"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="MARKETVALUE" HeaderText="市值(万元)"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="MARKETVALUEPCT" HeaderText="占净值(%)" DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"/> 
                        
                        <%--
                        <asp:BoundField DataField="HOLDPERIODYIELD" HeaderText="收益率(%)" DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"/> 
                        <asp:BoundField DataField="HOLDPERIODRET" HeaderText="期间回报"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                        --%>
                        <asp:BoundField DataField="DATEIN" HeaderText="买入日期" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="DATEOUT" HeaderText="卖出日期" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
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
            <td valign="top" style="width:50%;">                        
                <asp:GridView ID="GridViewRevRepoPositionDetail" runat="server" CellPadding="4" 
                    ForeColor="#333333" GridLines="None" Width="100%"
                    AutoGenerateColumns="False" 
                    EmptyDataText="(无)" 
                    onrowdatabound="GridViewBondPositionDetail_RowDataBound" 
                    Caption="逆回购（买入返售金融资产）" ShowHeaderWhenEmpty="True"  CaptionAlign="Left">
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
                        <asp:BoundField DataField="CODE" HeaderText="代码"  HeaderStyle-HorizontalAlign="Left" /> 
                        <asp:BoundField DataField="NAME" HeaderText="名称"  HeaderStyle-HorizontalAlign="Left" /> 
                        <asp:BoundField DataField="MARKETVALUE" HeaderText="市值(万元)"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="MARKETVALUEPCT" HeaderText="占净值(%)" DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"/> 
                        <asp:BoundField DataField="DATEIN" HeaderText="融出日期" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="DATEOUT" HeaderText="购回日期" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                    </Columns> 

                </asp:GridView>
                        
            </td>
            <td  valign="top">
            <asp:GridView ID="GridViewTheRepoPositionDetail" runat="server" CellPadding="4" 
                    ForeColor="#333333" GridLines="None" Width="100%"
                    AutoGenerateColumns="False" 
                    EmptyDataText="(无)" 
                    onrowdatabound="GridViewBondPositionDetail_RowDataBound" 
                    Caption="正回购（卖出返售金融资产）" ShowHeaderWhenEmpty="True"  CaptionAlign="Left">
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
                        <asp:BoundField DataField="CODE" HeaderText="代码"  HeaderStyle-HorizontalAlign="Left" /> 
                        <asp:BoundField DataField="NAME" HeaderText="名称"  HeaderStyle-HorizontalAlign="Left" /> 
                        <asp:BoundField DataField="MARKETVALUE" HeaderText="市值(万元)"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="MARKETVALUEPCT" HeaderText="占净值(%)" DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"/> 
                        <asp:BoundField DataField="DATEIN" HeaderText="融入日期" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="DATEOUT" HeaderText="购回日期" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
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
            <td valign="top" style="width:50%;">                        
                <asp:GridView ID="GridViewCashPositionDetail" runat="server" CellPadding="4" 
                    ForeColor="#333333" GridLines="None" Width="100%"
                    AutoGenerateColumns="False" 
                    EmptyDataText="(无)" 
                    onrowdatabound="GridViewBondPositionDetail_RowDataBound" 
                    Caption="现金与存款" ShowHeaderWhenEmpty="True"  CaptionAlign="Left">
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
                        <asp:BoundField DataField="CODE" HeaderText="代码"  HeaderStyle-HorizontalAlign="Left" /> 
                        <asp:BoundField DataField="NAME" HeaderText="名称"  HeaderStyle-HorizontalAlign="Left" /> 
                        <asp:BoundField DataField="MARKETVALUE" HeaderText="市值(万元)"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="MARKETVALUEPCT" HeaderText="占净值(%)" DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"/> 
                    </Columns> 

                </asp:GridView>
                        
            </td>
            <td  valign="top">
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td colspan="2">
            
                <asp:GridView ID="GridViewTransactionDetail" runat="server" CellPadding="4" 
                    ForeColor="#333333" GridLines="None" Width="100%"
                    AutoGenerateColumns="False"
                    EmptyDataText="(无)" 
                    Caption="交易明细" ShowHeaderWhenEmpty="True"  CaptionAlign="Left"
                    onrowdatabound="GridViewTransactionDetail_RowDataBound" 
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
                        <asp:BoundField DataField="TRADEDATE" HeaderText="交易日"  HeaderStyle-HorizontalAlign="Left" DataFormatString="{0:yyyy-MM-dd}"/>
                        <asp:BoundField DataField="TRADEACTION" HeaderText="操作"  HeaderStyle-HorizontalAlign="Left" /> 
                        <asp:BoundField DataField="CODE" HeaderText="代码"  HeaderStyle-HorizontalAlign="Left" /> 
                        <asp:BoundField DataField="NAME" HeaderText="名称"  HeaderStyle-HorizontalAlign="Left" /> 
                        <asp:BoundField DataField="SECURITYTYPE" HeaderText="类别"  HeaderStyle-HorizontalAlign="Left" /> 
                        <asp:BoundField DataField="PRICE" HeaderText="交易价格"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="QUANTITY" HeaderText="交易量(万份)"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="MARKETVALUE" HeaderText="交易额(万元)"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
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
            
                <asp:GridView ID="GridViewMessages" runat="server" CellPadding="4" 
                    ForeColor="#333333" GridLines="None" Width="100%"
                    AutoGenerateColumns="False"
                    EmptyDataText="(无)" 
                    Caption="" ShowHeaderWhenEmpty="True"  CaptionAlign="Left"
                    onrowdatabound="GridViewMessagesDetail_RowDataBound" 
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
                        <asp:BoundField DataField="TYPE" HeaderText="类型"  HeaderStyle-HorizontalAlign="Left" /> 
                        <asp:BoundField DataField="CODE" HeaderText="关键词"  HeaderStyle-HorizontalAlign="Left" /> 
                        <asp:BoundField DataField="NAME" HeaderText="消息"  HeaderStyle-HorizontalAlign="Left" /> 
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
