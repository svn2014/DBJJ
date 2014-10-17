<%@ Page Title="新股发行" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        新股发行信息
    </h2>
    <p>
    <a target="_blank" href="http://money.finance.sina.com.cn/corp/go.php/vRPD_NewStockIssue/page/1.phtml">新股日历</a>
    </p>
    
        <table class="style1" width = "100%">
        <tr>
            <td colspan="3">
            <asp:Label ID="lblCode" runat="server" Text="股票代码"></asp:Label>
            <asp:TextBox ID="txtCode" runat="server"></asp:TextBox>
            <asp:Button ID="btnSearch" runat="server" Text="查询" onclick="btnSearch_Click" />
            &nbsp;代码为空则显示近期所有IPO项目
            </td>
        </tr>
        <tr><td colspan="3"><span style="color: #FF0000; font-weight: 700;">数据仅供参考，实际以相关公告为准，填报申请单请务必核对。</span></td></tr>
        <tr>
            <td colspan="3"><hr /></td>
        </tr>
        <tr>
            <td colspan="3">
            <asp:GridView ID="GridViewIPOInfo" runat="server" 
                    AutoGenerateColumns="False" CellPadding="4" 
                    ForeColor="#333333" GridLines="None" Width="100%" 
                    ShowHeaderWhenEmpty="True" CaptionAlign="Left" PageSize="30" 
                    AllowPaging="True" onrowdatabound="GridViewIPOInfo_RowDataBound" >
                <AlternatingRowStyle BackColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <PagerSettings Visible="False" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#EFF3FB" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                <SortedDescendingHeaderStyle BackColor="#4870BE" />

                <Columns> 
                    <asp:BoundField DataField="Symbol" HeaderText="代码" 
                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"> 
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="SName" HeaderText="名称"  
                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"> 
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Industry0" HeaderText="行业"  
                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"> 
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="QuoteStart" HeaderText="网下询价日"  
                        DataFormatString="{0:d}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" 
                        HeaderStyle-HorizontalAlign="Right" > 
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="QuoteEnd" HeaderText="截止日"  DataFormatString="{0:d}" 
                        HtmlEncode="False" ItemStyle-HorizontalAlign="Right" 
                        HeaderStyle-HorizontalAlign="Right" > 
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="IndPEOnStart" HeaderText="起始日PE"  
                        DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" 
                        HeaderStyle-HorizontalAlign="Right" > 
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="IndPELatest" HeaderText="最新PE"
                        DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" 
                        HeaderStyle-HorizontalAlign="Right" > 
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <%--<asp:BoundField DataField="PrimaryUnderwriter0" HeaderText="主承销商"  
                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" > 
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField> --%>                   
                    <%--<asp:BoundField DataField="OtherPrimaryUnderwriters" HeaderText="副主"  
                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" > 
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>--%>
                    <%--<asp:BoundField DataField="PurchaseStart" HeaderText="申购始日"  DataFormatString="{0:d}" 
                        HtmlEncode="False" ItemStyle-HorizontalAlign="Right" 
                        HeaderStyle-HorizontalAlign="Right" >                     
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="PurchaseEnd" HeaderText="截止日"  DataFormatString="{0:d}" 
                        HtmlEncode="False" ItemStyle-HorizontalAlign="Right" 
                        HeaderStyle-HorizontalAlign="Right" >                     
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>--%>
                    <asp:BoundField DataField="UpLimitVolUnderNet" HeaderText="申购上限"  
                        DataFormatString="{0:N0}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" 
                        HeaderStyle-HorizontalAlign="Right" > 
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="DownLimitVolUnderNet" HeaderText="下限"  
                        DataFormatString="{0:N0}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" 
                        HeaderStyle-HorizontalAlign="Right" > 
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="PurchaseCodeOnNet" HeaderText="网上发行" 
                        ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"  HeaderStyle-BackColor="#FF0000"> 
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="PurchaseDate" HeaderText="申购日"  DataFormatString="{0:d}" 
                        HtmlEncode="False" ItemStyle-HorizontalAlign="Right" 
                        HeaderStyle-HorizontalAlign="Right" HeaderStyle-BackColor="#FF0000">                     
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="UpLimitVolOnNet" HeaderText="申购上限"  
                        DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-BackColor="#FF0000"
                        HeaderStyle-HorizontalAlign="Right" > 
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="IssuePrice" HeaderText="发行价"  
                        DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" 
                        HeaderStyle-HorizontalAlign="Right"> 
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Listdate" HeaderText="上市日"  DataFormatString="{0:d}" 
                        HtmlEncode="False" ItemStyle-HorizontalAlign="Right" 
                        HeaderStyle-HorizontalAlign="Right">                     
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <%--<asp:TemplateField HeaderText = "操作" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <%# "<a target=\"_blank\" href=\"QuotePrint.aspx?scode=" + Eval("Symbol") + "\">询价</a>"%>
                            <%# "<a target=\"_blank\" href=\"PurchasePrint.aspx?scode=" + Eval("Symbol") + "\">申购</a>"%>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                </Columns> 

                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td colspan="3"><hr /></td>
        </tr>
        
        <tr>
            <td colspan="3">
            
            <asp:GridView ID="GridViewCBInfo" runat="server" 
                    AutoGenerateColumns="False" CellPadding="4" 
                    ForeColor="#333333" GridLines="None" Width="100%" 
                    ShowHeaderWhenEmpty="True" CaptionAlign="Left" PageSize="100" 
                    AllowPaging="True" onrowdatabound="GridViewIPOInfo_RowDataBound" >
                <AlternatingRowStyle BackColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <PagerSettings Visible="False" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#EFF3FB" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                <SortedDescendingHeaderStyle BackColor="#4870BE" />

                <Columns> 
                    <asp:BoundField DataField="SYMBOL" HeaderText="代码" 
                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" > 
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="SNAME" HeaderText="转债" 
                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"> 
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="StockName" HeaderText="正股"  
                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"> 
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <%--<asp:BoundField DataField="Industry0" HeaderText="行业"  
                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"> 
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>--%>
                    <asp:BoundField DataField="PaymentDate" HeaderText="申购日"  
                        DataFormatString="{0:d}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" 
                        HeaderStyle-HorizontalAlign="Right" > 
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="ListDate" HeaderText="上市日"  DataFormatString="{0:d}" 
                        HtmlEncode="False" ItemStyle-HorizontalAlign="Right" 
                        HeaderStyle-HorizontalAlign="Right" > 
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="IssuePlan" HeaderText="总额"  
                        DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" 
                        HeaderStyle-HorizontalAlign="Right" > 
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <%--<asp:BoundField DataField="" HeaderText="主承销商"  
                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" > 
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>--%>
                    <asp:BoundField DataField="DownLimitUnderNet" HeaderText="网下下限(手)"  
                        DataFormatString="{0:N0}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" 
                        HeaderStyle-HorizontalAlign="Right" > 
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="StepUnderNet" HeaderText="递增(手)"  
                        DataFormatString="{0:N0}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" 
                        HeaderStyle-HorizontalAlign="Right" > 
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="UpLimitUnderNet" HeaderText="上限(手)"  
                        DataFormatString="{0:N0}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" 
                        HeaderStyle-HorizontalAlign="Right" > 
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="DownpaymentRate0" HeaderText="定金"  
                        DataFormatString="{0:P0}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" 
                        HeaderStyle-HorizontalAlign="Right" > 
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>                    
                    <asp:BoundField DataField="WinRateUnderNet0" HeaderText="配售率"  
                        DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" 
                        HeaderStyle-HorizontalAlign="Right" > 
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="PurchaseCodeOnNet" HeaderText="网上发行" 
                        ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"  HeaderStyle-BackColor="#FF0000"> 
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="UpLimitOnNet" HeaderText="上限(手)"  
                        DataFormatString="{0:N0}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-BackColor="#FF0000"
                        HeaderStyle-HorizontalAlign="Right" > 
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="StrikePrice" HeaderText="行权价"
                        DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" 
                        HeaderStyle-HorizontalAlign="Right"> 
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <%--<asp:BoundField DataField="ConvertStart" HeaderText="行权始日"  DataFormatString="{0:d}" 
                        HtmlEncode="False" ItemStyle-HorizontalAlign="Right" 
                        HeaderStyle-HorizontalAlign="Right" > 
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>--%>
                </Columns> 

                </asp:GridView>

            </td>
        </tr>
        <tr>
            <td colspan="3"><hr /></td>
        </tr>
        <tr>
            <td colspan="3">
            重要公告对应的信息
            <ul>
            <li>询价推介公告:拟新发股份，拟老股转让，所属行业</li>
            <li>招股意向书:拟募集资金，发行相关费用</li>
            <li>询价结果公告:同发行公告</li>
            <li>发行公告:实际发行股份，实际老股转让，实际募集资金，发行市盈率和行业市盈率</li>
            </ul>
            </td>
        </tr>
    </table>
</asp:Content>
