<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PerformanceReport.aspx.cs" Inherits="PerformanceReport" %>

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
                    <asp:BoundField DataField="Industry" HeaderText="行业" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" /> 
                    <asp:BoundField DataField="HoldingMV" HeaderText="持有市值(万元)" DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"/> 
                    <asp:BoundField DataField="PortfolioWeight" HeaderText="组合权重"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                    <asp:BoundField DataField="BenchmarkWeight" HeaderText="基准权重"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                    <asp:BoundField DataField="PortfolioReturnPct" HeaderText="组合回报"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                    <asp:BoundField DataField="BenchmarkReturnPct" HeaderText="基准回报"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                    <asp:BoundField DataField="PureSectorAllowcation" HeaderText="配置回报"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                    <asp:BoundField DataField="WithinSectorSelection" HeaderText="选股回报"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                    <asp:BoundField DataField="AllowcationSelectionInteraction" HeaderText="交叉回报"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                    <asp:BoundField DataField="TotalValueAdded" HeaderText="增加值"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                </Columns> 

                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td colspan="2">
            <ul>
            <li>行业配置回报 = (组合权重 - 基准权重) * (基准回报 - 基准的总体回报)，反应投资组合的行业配置效应；</li>
            <li>行业内选股回报 = 基准权重 * (组合回报 - 基准回报)，反应投资组合行业内的选股效应；</li>
            <li>交叉回报 = (组合权重 - 基准权重) * (组合回报 - 基准回报)，反应行业配置和行业内选股的联合效应；</li>
            <li>增加值 = 组合权重 * 组合回报 - 基准权重 * 基准回报，反应主动性管理给投资组合带来的超额收益效应；</li>
            </ul>
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
                    ForeColor="#333333" GridLines="None" Width="100%" PageSize="52" 
                    AutoGenerateColumns="False" AllowPaging="True" 
                    EmptyDataText="(无)" 
                    onpageindexchanging="GridViewPositionDetail_PageIndexChanging" onrowdatabound="GridViewEquityPositionDetail_RowDataBound" 
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
                        <asp:BoundField DataField="Code" HeaderText="股票代码"  HeaderStyle-HorizontalAlign="Left" /> 
                        <asp:BoundField DataField="Name" HeaderText="股票名称"  HeaderStyle-HorizontalAlign="Left" /> 
                        <asp:BoundField DataField="Industry" HeaderText="行业" HeaderStyle-HorizontalAlign="Left" />
                        <%--<asp:BoundField DataField="HoldingVolume" HeaderText="持有量(万股)"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />--%>
                        <asp:BoundField DataField="HoldingMV" HeaderText="市值(万元)"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />                        
                        <asp:BoundField DataField="pctofNAV" HeaderText="占净值(%)" DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"/> 
                        <asp:BoundField DataField="PortfolioWeight" SortExpression="PortfolioWeight" HeaderText="组合权重"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="BenchmarkWeight" SortExpression="BenchmarkWeight" HeaderText="基准权重"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <%--<asp:BoundField DataField="BMWghtDiff" HeaderText="权重差"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />--%> 
                        <asp:BoundField DataField="PortfolioReturnPct" HeaderText="收益率"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <%--<asp:BoundField DataField="pctofListed" HeaderText="占流通股本(%)"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />--%> 
                        <%--<asp:BoundField DataField="Turnover" HeaderText="换手率"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> --%>
                        <asp:BoundField DataField="PortfolioReturn" HeaderText="期间回报"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="LiqCycle" HeaderText="流动周期"  DataFormatString="{0:F2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />                          
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
                    ForeColor="#333333" GridLines="None" Width="100%" PageSize="22" 
                    AutoGenerateColumns="False" AllowPaging="True" 
                    EmptyDataText="(无)" 
                    onrowdatabound="GridViewBondPositionDetail_RowDataBound" 
                    Caption="纯债投资" ShowHeaderWhenEmpty="True"  CaptionAlign="Left"
                    onpageindexchanging="GridViewBondPositionDetail_PageIndexChanging">
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
                        <asp:BoundField DataField="Code" HeaderText="债券代码"  HeaderStyle-HorizontalAlign="Left" /> 
                        <asp:BoundField DataField="Name" HeaderText="债券名称"  HeaderStyle-HorizontalAlign="Left" /> 
                        <%--<asp:BoundField DataField="HoldingVolume" HeaderText="持有量(万份)"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />--%>
                        <asp:BoundField DataField="HoldingMV" HeaderText="市值(万元)"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="pctofNAV" HeaderText="占净值(%)" DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"/> 
                        <%--<asp:BoundField DataField="PortfolioWeight" SortExpression="PortfolioWeight" HeaderText="组合权重"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> --%>
                        <asp:BoundField DataField="PortfolioNetReturnPct" HeaderText="净价收益率"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="PortfolioReturnPct" HeaderText="全价收益率"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="PortfolioReturn" HeaderText="期间回报"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="AnnualYield" HeaderText="年化"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="MaturityDate" HeaderText="到期日期"  HeaderStyle-HorizontalAlign="Left" DataFormatString="{0:yyyy-MM-dd}"/> 
                        <%--<asp:BoundField DataField="pctofListed" HeaderText="占流通份额比例"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="Turnover" HeaderText="换手率"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="LiqCycle" HeaderText="流动周期"  DataFormatString="{0:F2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />--%>
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
            
            <asp:GridView ID="GridViewCBondPositionDetail" runat="server" CellPadding="4" 
                    ForeColor="#333333" GridLines="None" Width="100%" PageSize="22" 
                    AutoGenerateColumns="False" AllowPaging="True" 
                    EmptyDataText="(无)" 
                    Caption="可转债投资" ShowHeaderWhenEmpty="True"  CaptionAlign="Left" 
                    onpageindexchanging="GridViewCBondPositionDetail_PageIndexChanging" onrowdatabound="GridViewCBondPositionDetail_RowDataBound"
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
                        <asp:BoundField DataField="Code" HeaderText="转债代码"  HeaderStyle-HorizontalAlign="Left" /> 
                        <asp:BoundField DataField="Name" HeaderText="转债名称"  HeaderStyle-HorizontalAlign="Left" /> 
                        <%--<asp:BoundField DataField="HoldingVolume" HeaderText="持有量(万份)"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />--%>
                        <asp:BoundField DataField="HoldingMV" HeaderText="市值(万元)"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="pctofNAV" HeaderText="占净值(%)" DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"/> 
                        <%--<asp:BoundField DataField="PortfolioWeight" SortExpression="PortfolioWeight" HeaderText="组合权重"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> --%>
                        <asp:BoundField DataField="PortfolioNetReturnPct" HeaderText="净价收益率"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="PortfolioReturnPct" HeaderText="全价收益率"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="PortfolioReturn" HeaderText="期间回报"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="AnnualYield" HeaderText="年化"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="MaturityDate" HeaderText="到期日期"  HeaderStyle-HorizontalAlign="Left" DataFormatString="{0:yyyy-MM-dd}"/> 
                        <%--<asp:BoundField DataField="pctofListed" HeaderText="占流通份额比例"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="Turnover" HeaderText="换手率"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="LiqCycle" HeaderText="流动周期"  DataFormatString="{0:F2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />--%>
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
                    ForeColor="#333333" GridLines="None" Width="100%" PageSize="22" 
                    AutoGenerateColumns="False" AllowPaging="True" 
                    EmptyDataText="(无)" 
                    onrowdatabound="GridViewBondPositionDetail_RowDataBound" 
                    Caption="现金头寸" ShowHeaderWhenEmpty="True"  CaptionAlign="Left"
                    onpageindexchanging="GridViewBondPositionDetail_PageIndexChanging">
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
                        <asp:BoundField DataField="Name" HeaderText="现金科目"  HeaderStyle-HorizontalAlign="Left" /> 
                        <asp:BoundField DataField="HoldingMV" HeaderText="市值(万元)"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="pctofNAV" HeaderText="占净值(%)" DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"/> 
                        <asp:BoundField DataField="PortfolioReturnPct" HeaderText="收益率"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="PortfolioReturn" HeaderText="期间回报"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="AnnualYield" HeaderText="年化"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                    </Columns> 

                </asp:GridView>
                        
            </td>
            <td  valign="top">
                        
                <asp:GridView ID="GridViewOtherPositionDetail" runat="server" CellPadding="4" 
                    ForeColor="#333333" GridLines="None" Width="100%" PageSize="22" 
                    AutoGenerateColumns="False" AllowPaging="True" 
                    EmptyDataText="(无)" onrowdatabound="GridViewOtherPositionDetail_RowDataBound" 
                    Caption="其他投资" ShowHeaderWhenEmpty="True"  CaptionAlign="Left"
                    onpageindexchanging="GridViewOtherPositionDetail_PageIndexChanging">
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
                        <asp:BoundField DataField="Code" HeaderText="资产代码"  HeaderStyle-HorizontalAlign="Left" /> 
                        <asp:BoundField DataField="Name" HeaderText="资产名称"  HeaderStyle-HorizontalAlign="Left" /> 
                        <%--<asp:BoundField DataField="HoldingVolume" HeaderText="持有量(万单元)" DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />--%>
                        <asp:BoundField DataField="HoldingMV" HeaderText="市值(万元)"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="pctofNAV" HeaderText="占净值(%)" DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"/> 
                        <%--<asp:BoundField DataField="PortfolioWeight" SortExpression="PortfolioWeight" HeaderText="组合权重"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> --%>
                        <asp:BoundField DataField="PortfolioReturnPct" HeaderText="收益率"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="PortfolioReturn" HeaderText="期间回报"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
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
            <td>
                <h2>更多信息</h2>
                <ul>
                    <li><a id="aTradeDetail" runat="server" target="_blank" href="#">申赎与交易明细</a></li>
                </ul>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        
    </table>
    </div>
    </form>
</div>

</body>
</html>
