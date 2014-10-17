<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AttributionReport.aspx.cs" Inherits="AttributionReport" %>

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
    </table>
    </div>
    </form>
</div>

</body>
</html>
