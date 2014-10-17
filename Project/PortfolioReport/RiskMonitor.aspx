<%@ Page Title="风险预警" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="RiskMonitor.aspx.cs" Inherits="About" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
<script id="rs" language="javascript" type="text/javascript">
    function showMarketReport() {
        var selectObj = document.getElementById("<%=DropDownListFund.ClientID%>");
        var idx = document.getElementById("<%=DropDownListFund.ClientID%>").selectedIndex
        var code = selectObj.options[idx].value;
        var authcode = document.getElementById("<%=TextBoxAuthCode.ClientID%>").value;
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
                    + "&startdate=" + enddate
                    + "&enddate=" + enddate;

        window.open(url, '_blank');
    }
    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        风险预警
    </h2>
    
    <table class="style1" border="0">
        <tr>
            <td width="10%" class="style2">授权码</td>
            <td width="15%" class="style2"><asp:TextBox ID="TextBoxAuthCode" runat="server"></asp:TextBox></td>
            <td width="10%" class="style2">投资组合</td>
            <td width="10%"><asp:DropDownList ID="DropDownListFund" runat="server"></asp:DropDownList></td>
            <td width="10%" class="style2">日期</td>
            <td class="style2"><asp:TextBox ID="TextBoxEndDate" runat="server"></asp:TextBox></td>            
            <td class="style2"><asp:Button ID="btnRiskMonitor" runat="server" Text="运行" Width="100px" onclick="btnRiskMonitor_Click" /></td>
            <td class="style2"><asp:Button ID="btnRunMarketReport" runat="server" Text="市场" OnClientClick="showMarketReport();" Width="100px" /></td>
        </tr>
        <tr>
            <td colspan="8"><hr /></td>
        </tr>
        <tr>
            <td colspan="8"><asp:label ID="LabelStatus" runat="server" text="" Visible="false" ForeColor="Red"></asp:label><asp:label ID="lblcode" runat="server" text="" ForeColor="White"></asp:label></td>
        </tr>
        <tr>
            <td colspan="8" align="center"><asp:label ID="LabelFundName" runat="server" ></asp:label></td>
        </tr>
        <tr>
            <td colspan="8">
            
                <asp:GridView ID="GridViewRiskMonitor_Failed" runat="server" 
                    AllowSorting="True" AutoGenerateColumns="False" CellPadding="4" 
                    ForeColor="#333333" GridLines="None" Width="100%" 
                    Caption="警告" ShowHeaderWhenEmpty="true" CaptionAlign="Left" 
                    EmptyDataText="(无)">
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
                    <asp:BoundField DataField="TITLE" HeaderText="标题" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="70" /> 
                    <asp:BoundField DataField="CODE" HeaderText="代码" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="70"  />
                    <asp:BoundField DataField="NAME" HeaderText="名称" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100"  />
                    <asp:BoundField DataField="DESC" HeaderText="描述" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" /> 
                </Columns> 

                </asp:GridView>

                </td>
        </tr>
        <tr>
            <td colspan="8"></td>
        </tr>
        <tr>
            <td colspan="8">
                <asp:GridView ID="GridViewRiskMonitor_Warning" runat="server" 
                    AllowSorting="True" AutoGenerateColumns="False" CellPadding="4" 
                    ForeColor="#333333" GridLines="None" Width="100%" 
                    Caption="警告" ShowHeaderWhenEmpty="true" CaptionAlign="Left" 
                    EmptyDataText="(无)">
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
                    <asp:BoundField DataField="TITLE" HeaderText="标题" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="70" /> 
                    <asp:BoundField DataField="CODE" HeaderText="代码" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="70"  /> 
                    <asp:BoundField DataField="NAME" HeaderText="名称" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100"  />
                    <asp:BoundField DataField="DESC" HeaderText="描述" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" /> 
                </Columns> 

                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td colspan="8"><hr /></td>
        </tr>
        <tr>
            <td colspan="4" valign="top">
                <asp:GridView ID="GridViewPortfolio" runat="server" 
                    AllowSorting="True" AutoGenerateColumns="False" CellPadding="4" 
                    ForeColor="#333333" GridLines="None" Width="100%" 
                    Caption="组合指标" ShowHeaderWhenEmpty="true" CaptionAlign="Left" 
                    EmptyDataText="(无)">
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
                    <asp:BoundField DataField="NAME" HeaderText="名称" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100"  />                    
                    <asp:BoundField DataField="MARKETVALUE" HeaderText="值" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" /> 
                </Columns> 

                </asp:GridView>
            </td>
            <td colspan="4" valign="top">
                <asp:GridView ID="GridViewDepositPosition" runat="server" 
                    AllowSorting="True" AutoGenerateColumns="False" CellPadding="4" 
                    ForeColor="#333333" GridLines="None" Width="100%" 
                    Caption="存款" ShowHeaderWhenEmpty="true" CaptionAlign="Left" 
                    EmptyDataText="(无)">
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
                    <asp:BoundField DataField="CODE" HeaderText="代码" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="70"  /> 
                    <asp:BoundField DataField="NAME" HeaderText="名称" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100"  />                    
                    <asp:BoundField DataField="MARKETVALUE" HeaderText="市值(万元)" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" /> 
                    <asp:BoundField DataField="MARKETVALUEPCT" HeaderText="占比" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:P2}" />  
                </Columns> 

                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td colspan="8"><hr /></td>
        </tr>
        <tr>
            <td colspan="8">

                <asp:GridView ID="GridViewEquityPosition" runat="server" 
                    AllowSorting="True" AutoGenerateColumns="False" CellPadding="4" 
                    ForeColor="#333333" GridLines="None" Width="100%" 
                    Caption="股票持仓" ShowHeaderWhenEmpty="true" CaptionAlign="Left" 
                    EmptyDataText="(无)">
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
                    <asp:BoundField DataField="CODE" HeaderText="代码" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="70"  /> 
                    <asp:BoundField DataField="NAME" HeaderText="名称" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100"  />
                    <asp:BoundField DataField="INDUSTRY1" HeaderText="行业" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100"  />
                    <%--<asp:BoundField DataField="INDUSTRY2" HeaderText="行业" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100"  />--%>
                    <asp:BoundField DataField="PRECLOSE" HeaderText="价格" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" /> 
                    <asp:BoundField DataField="QUANTITY" HeaderText="数量(万股)" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" /> 
                    <asp:BoundField DataField="MARKETVALUE" HeaderText="市值(万元)" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" /> 
                    <asp:BoundField DataField="MARKETVALUEPCT" HeaderText="占比" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:P2}" /> 
                </Columns> 

                </asp:GridView>

            </td>
        </tr>
        <tr>
            <td colspan="8">
                <asp:GridView ID="GridViewBondPosition" runat="server" 
                    AllowSorting="True" AutoGenerateColumns="False" CellPadding="4" 
                    ForeColor="#333333" GridLines="None" Width="100%" 
                    Caption="债券持仓" ShowHeaderWhenEmpty="true" CaptionAlign="Left" 
                    EmptyDataText="(无)">
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
                    <asp:BoundField DataField="CODE" HeaderText="代码" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="70"  /> 
                    <asp:BoundField DataField="NAME" HeaderText="名称" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100"  />                    
                    <asp:BoundField DataField="INDUSTRY3" HeaderText="行业" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100"  />
                    <asp:BoundField DataField="PRECLOSE" HeaderText="价格" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" /> 
                    <asp:BoundField DataField="QUANTITY" HeaderText="数量(万份)" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" /> 
                    <asp:BoundField DataField="MARKETVALUE" HeaderText="市值(万元)" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" /> 
                    <asp:BoundField DataField="MARKETVALUEPCT" HeaderText="占比" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:P2}" /> 
                    <asp:BoundField DataField="NORMINALINTRATE" HeaderText="名义利率" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:P2}" /> 
                    <asp:BoundField DataField="BONDCREDITRATE" HeaderText="评级" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" /> 
                    <asp:BoundField DataField="INTPAYMENTDATE" HeaderText="付息日" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:yyyy-MM-dd}" /> 
                    <%--<asp:BoundField DataField="DELISTEDDATE" HeaderText="到期日" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:yyyy-MM-dd}" />--%> 
                    <asp:BoundField DataField="TERMTOMATURITY" HeaderText="剩余期限(年)" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" /> 
                    <%--<asp:BoundField DataField="EMBEDEDOPTION" HeaderText="是否含权" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />--%> 
                    <%--<asp:BoundField DataField="ISSUERSTOCKCODE" HeaderText="相关股票" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />--%> 
                    <%--<asp:BoundField DataField="VALUEDEVIATION" HeaderText="估值偏离" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:P2}" />--%> 
                </Columns> 

                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td colspan="8">
                <asp:GridView ID="GridViewFundPosition" runat="server" 
                    AllowSorting="True" AutoGenerateColumns="False" CellPadding="4" 
                    ForeColor="#333333" GridLines="None" Width="100%" 
                    Caption="基金持仓" ShowHeaderWhenEmpty="true" CaptionAlign="Left" 
                    EmptyDataText="(无)">
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
                    <asp:BoundField DataField="CODE" HeaderText="代码" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="70"  /> 
                    <asp:BoundField DataField="NAME" HeaderText="名称" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100"  />                    
                    <asp:BoundField DataField="UNITNAV" HeaderText="净值" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:N3}" /> 
                    <asp:BoundField DataField="PRECLOSE" HeaderText="价格" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:N3}" /> 
                    <asp:BoundField DataField="QUANTITY" HeaderText="数量(万份)" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" /> 
                    <asp:BoundField DataField="COST" HeaderText="成本(万元)" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" /> 
                    <asp:BoundField DataField="MARKETVALUE" HeaderText="市值(万元)" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" /> 
                    <asp:BoundField DataField="MARKETVALUEPCT" HeaderText="占比" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:P2}" /> 
                </Columns> 

                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td colspan="4" valign="top">
                <asp:GridView ID="GridViewRevRepoPosition" runat="server" 
                    AllowSorting="True" AutoGenerateColumns="False" CellPadding="4" 
                    ForeColor="#333333" GridLines="None" Width="100%" 
                    Caption="逆回购" ShowHeaderWhenEmpty="true" CaptionAlign="Left" 
                    EmptyDataText="(无)">
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
                    <asp:BoundField DataField="CODE" HeaderText="代码" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="70"  /> 
                    <asp:BoundField DataField="NAME" HeaderText="名称" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100"  />                    
                    <asp:BoundField DataField="MARKETVALUE" HeaderText="市值(万元)" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" /> 
                    <asp:BoundField DataField="MARKETVALUEPCT" HeaderText="占比" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:P2}" /> 
                </Columns> 

                </asp:GridView>
            </td>
            <td colspan="4" valign="top">
                <asp:GridView ID="GridViewTheRepoPosition" runat="server" 
                    AllowSorting="True" AutoGenerateColumns="False" CellPadding="4" 
                    ForeColor="#333333" GridLines="None" Width="100%" 
                    Caption="正回购" ShowHeaderWhenEmpty="true" CaptionAlign="Left" 
                    EmptyDataText="(无)">
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
                    <asp:BoundField DataField="CODE" HeaderText="代码" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="70"  /> 
                    <asp:BoundField DataField="NAME" HeaderText="名称" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100"  />                    
                    <asp:BoundField DataField="MARKETVALUE" HeaderText="市值(万元)" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" /> 
                    <asp:BoundField DataField="MARKETVALUEPCT" HeaderText="占比" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:P2}" /> 
                </Columns> 

                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>
