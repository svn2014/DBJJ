<%@ Page Title="投资研究部主页" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        目录
    </h2>
    <table width="50%">
        <tr>
            <td width="50%" valign="top">
            投研平台
                <ol>
                    <li><a href="/meetingreport/">晨会纪要</a></li>
                    <li><a href="/equityreport/">研究报告</a></li>
                    <li><a href="/fundequitypool/">证券备选库</a></li>
                    <li><a href="/reportcenter/MarketInfo.aspx">市场信息</a></li>
                    <li><a href="/ipo/">IPO</a></li>
                </ol>
            </td>
            <td valign="top">
            金融工程
                <ol>
                    <li><a href="/reportcenter/Default.aspx">归因分析</a></li>
                    <li><a href="/reportcenter/RiskMonitor.aspx">风险预警</a></li>
                    <li><a href="/reportcenter/AllFundsReport.aspx">基金汇总</a></li>
                </ol>
            </td>
        </tr>
    </table>

    
    <p id="tst" runat="server">        
    </p>
</asp:Content>
