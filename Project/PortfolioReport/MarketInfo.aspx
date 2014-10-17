<%@ Page Title="市场信息" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="MarketInfo.aspx.cs" Inherits="About" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
<script id="rs" language="javascript" type="text/javascript">
    function showMarketReport() {
        var equitycode = document.getElementById("<%=TextBoxCode.ClientID%>").value;
        var startdate = document.getElementById("<%=TextBoxStartDate.ClientID%>").value;
        var enddate = document.getElementById("<%=TextBoxEndDate.ClientID%>").value;

        var url = "MarketReport.aspx?equitycode=" + equitycode
                    + "&startdate=" + startdate
                    + "&enddate=" + enddate
                    ;
        window.open(url, '_blank');
    }

    function showMarketReportInPool() {
        var equitycode = "pool";
        var startdate = document.getElementById("<%=TextBoxStartDate.ClientID%>").value;
        var enddate = document.getElementById("<%=TextBoxEndDate.ClientID%>").value;

        var url = "MarketReport.aspx?equitycode=" + equitycode
                    + "&startdate=" + startdate
                    + "&enddate=" + enddate
                    ;
        window.open(url, '_blank');
    }

</script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        市场消息
    </h2>
    <div class="main">
    <table class="style1" border="0" style="background-color:White; width:100%">
        <tr>            
            <td width="12%" class="style2">报告期初</td>
            <td width="10%" class="style2"><asp:TextBox ID="TextBoxStartDate" runat="server"></asp:TextBox></td>
            <td width="12%" class="style2">报告期末</td>
            <td width="10%" class="style2"><asp:TextBox ID="TextBoxEndDate" runat="server"></asp:TextBox></td>
            <td width="12%" class="style2">股票代码</td>
            <td class="style2"><asp:TextBox ID="TextBoxCode" runat="server"></asp:TextBox> e.g. 600000.SH,000001.SZ
            </td>            
            <td></td>
        </tr>
        <tr>
            <td colspan="6">
                <asp:Button ID="btnRunMarketReport" runat="server" Text="市场提示" OnClientClick="showMarketReport();" Width="100px" />
                <asp:Button ID="btnRunMarketReport4EquityPool" runat="server" Text="入库股票相关消息" OnClientClick="showMarketReportInPool();" Width="100px" Visible="False" />
            </td>
        </tr>
        <tr>
            <td colspan="6">
            可用的市场公开信息：
                <ul>
                    <li>限售解禁</li>
                    <li>大宗交易</li>
                    <li>异常交易</li>
                    <li>融资融券</li>
                    <li>首发增发</li>
                    <li>新闻资讯</li>
                </ul>
            </td>
        </tr>
        </table>
    </div>

</asp:Content>
