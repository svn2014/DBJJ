<%@ Page Title="归因分析" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            height: 23px;
        }
        </style>

    <script id="rs" language="javascript" type="text/javascript">
        function showPositionReport() {
            var startdate = document.getElementById("<%=TextBoxStartDate.ClientID%>").value;
            var enddate = document.getElementById("<%=TextBoxEndDate.ClientID%>").value;
            var authcode = document.getElementById("<%=TextBoxAuthCode.ClientID%>").value;

            var selectObj = document.getElementById("<%=DropDownListFund.ClientID%>");
            var idx = document.getElementById("<%=DropDownListFund.ClientID%>").selectedIndex
            var code = selectObj.options[idx].value;

            if (authcode == "") {
                alert("缺少授权码！");
                return;
            }

            if (code == "" || startdate == "" || enddate == "") {
                alert("请选择一个投资组合和合适的报告日期");
                return;
            }

            var url = "PositionReport.aspx?startdate=" + startdate
                        + "&enddate=" + enddate
                        + "&code=" + code
                        + "&authcode=" + authcode;

            window.open(url, '_blank');
        }
        function showPerformanceReport() {
            var startdate = document.getElementById("<%=TextBoxStartDate.ClientID%>").value;
            var enddate = document.getElementById("<%=TextBoxEndDate.ClientID%>").value;
            var authcode = document.getElementById("<%=TextBoxAuthCode.ClientID%>").value;

            var selectObj = document.getElementById("<%=DropDownListFund.ClientID%>");
            var idx = document.getElementById("<%=DropDownListFund.ClientID%>").selectedIndex
            var code = selectObj.options[idx].value;

            if (authcode == "") {
                alert("缺少授权码！");
                return;
            }

            if (code == "" || startdate=="" || enddate=="") {
                alert("请选择一个投资组合和合适的报告日期");
                return;
            }    
            
            var url = "AttributionReport.aspx?startdate=" + startdate
                        + "&enddate=" + enddate
                        + "&code=" + code
                        + "&authcode=" + authcode;

            window.open(url, '_blank');
        }

        function showAllPerformanceReport() {
            var startdate = document.getElementById("<%=TextBoxStartDate.ClientID%>").value;
            var enddate = document.getElementById("<%=TextBoxEndDate.ClientID%>").value;
            var authcode = document.getElementById("<%=TextBoxAuthCode.ClientID%>").value;

            if (authcode == "") {
                alert("缺少授权码！");
                return;
            }

            if (startdate == "" || enddate == "") {
                alert("请选择一个投资组合和合适的报告日期");
                return;
            }

            var selectObj = document.getElementById("<%=DropDownListFund.ClientID%>");
            var cnt = selectObj.options.length;
            var code = "";
            var url = "";

            for (var i = 0; i < cnt; i++) {
                code = selectObj.options[i].value;
                if (code == "") {
                    continue;
                }

                url = "AttributionReport.aspx?startdate=" + startdate
                + "&enddate=" + enddate
                + "&code=" + code
                + "&authcode=" + authcode;

                window.open(url, '_blank');
            }            
        }

        function showMarketReport() {
            var selectObj = document.getElementById("<%=DropDownListFund.ClientID%>");
            var idx = document.getElementById("<%=DropDownListFund.ClientID%>").selectedIndex
            var code = selectObj.options[idx].value;
            var authcode = document.getElementById("<%=TextBoxAuthCode.ClientID%>").value;
            var startdate = document.getElementById("<%=TextBoxStartDate.ClientID%>").value;
            var enddate = document.getElementById("<%=TextBoxEndDate.ClientID%>").value;

            if (authcode == "") {
                alert("缺少授权码！");
                return;
            }

            if (code == "") {
                alert("请选择一个投资组合");
                return;
            }

            var url = "MarketReport.aspx?code=" + code
                    + "&authcode=" + authcode
                    + "&startdate=" + startdate
                    + "&enddate=" + enddate;

            window.open(url, '_blank');
        }
    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        归因分析
    </h2>
    <div class="main">
    <table class="style1" border="0">
        <tr>
            <td width="10%" class="style2">授权码</td>
            <td width="15%" class="style2"><asp:TextBox ID="TextBoxAuthCode" runat="server"></asp:TextBox></td>
            <td width="10%" class="style2">投资组合</td>
            <td width="10%"><asp:DropDownList ID="DropDownListFund" runat="server"></asp:DropDownList></td>
            <td width="10%" class="style2">报告期初</td>
            <td width="20%" class="style2"><asp:TextBox ID="TextBoxStartDate" runat="server"></asp:TextBox></td>
            <td width="10%" class="style2">报告期末</td>
            <td class="style2"><asp:TextBox ID="TextBoxEndDate" runat="server"></asp:TextBox></td>            
        </tr>
        <tr>
            <td colspan="8">
                <asp:Button ID="btnRunPerformanceReport" runat="server" Text="归因分析" 
                    OnClientClick="showPerformanceReport();" Width="100px" Visible="false" />
                    &nbsp;
                <asp:Button ID="btnRunPositionReport" runat="server" Text="持仓分析" 
                    OnClientClick="showPositionReport();" Width="100px" />
                    &nbsp;
                <asp:Button ID="btnRunMarketReport" runat="server" Text="市场提示" 
                    OnClientClick="showMarketReport();" Width="100px" />
                    
            </td>
        </tr>
        </table>
    </div>

</asp:Content>
