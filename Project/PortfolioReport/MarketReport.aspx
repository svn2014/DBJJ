<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MarketReport.aspx.cs" Inherits="MarketReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>市场信息提示</title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />   
</head>
<body class="main">

<div class="main">
    <form id="form1" runat="server">
    <asp:label ID="LabelStatus" runat="server" text="" Visible="false" ForeColor="Red"></asp:label>

    <div>
    

        <table style="background-color:White; width:100%">
        <tr>
            <td colspan="2" align="center"><h1>市场信息提示</h1> <hr /></td>
        </tr>
        <tr>
        <td colspan="2">
            <h2>限售解禁</h2>
            <asp:GridView ID="GridViewRestrictStock" runat="server" CellPadding="4"
                    ForeColor="#333333" GridLines="None" Width="100%" PageSize="30" 
                    AutoGenerateColumns="False" AllowPaging="True" 
                    EmptyDataText="(无)"                     
                    ShowHeaderWhenEmpty="True" 
                    onpageindexchanging="GridViewRestrictStock_PageIndexChanging" onrowdatabound="GridViewRestrictStock_RowDataBound" 
                    
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
                        <asp:BoundField DataField="s_info_listdated" HeaderText="解禁日期"  HeaderStyle-HorizontalAlign="Left" DataFormatString="{0:yyyy-MM-dd}"/> 
                        <asp:BoundField DataField="s_info_windcode" HeaderText="股票代码"/> 
                        <asp:BoundField DataField="s_info_name" HeaderText="股票名称"/>
                        <asp:BoundField DataField="s_share_lst" HeaderText="解禁股份(万股)"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="s_share_lst_amount" HeaderText="解禁金额(万元)"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="s_share_nonlst" HeaderText="剩余未解禁股份(万股)"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="s_share_nonlst_amount" HeaderText="剩余未解禁金额(万元)"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="s_share_lstsym" HeaderText="方案"/> 
                        <asp:BoundField DataField="s_share_lsttypetext" HeaderText="类型"/>                         
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
            <h2>大宗交易</h2>
            <asp:GridView ID="GridViewBlockTrade" runat="server" CellPadding="4"
                    ForeColor="#333333" GridLines="None" Width="100%" PageSize="30" 
                    AutoGenerateColumns="False" AllowPaging="True" 
                    EmptyDataText="(无)"                     
                    ShowHeaderWhenEmpty="True" 
                    onpageindexchanging="GridViewBlockTrade_PageIndexChanging" onrowdatabound="GridViewBlockTrade_RowDataBound"
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
                        <asp:BoundField DataField="Trade_dtd" HeaderText="日期"  HeaderStyle-HorizontalAlign="Left" DataFormatString="{0:yyyy-MM-dd}"/> 
                        <asp:BoundField DataField="s_info_windcode" HeaderText="股票代码"/> 
                        <asp:BoundField DataField="s_info_name" HeaderText="股票名称"/>
                        <asp:BoundField DataField="s_block_price" HeaderText="成交价"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="s_block_amount" HeaderText="成交额(万元)"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="s_block_buyername" HeaderText="买方"/> 
                        <asp:BoundField DataField="s_block_sellername" HeaderText="卖方"/> 
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
            <h2>异常交易</h2>
            <asp:GridView ID="GridViewStrangeTrade" runat="server" CellPadding="4"
                    ForeColor="#333333" GridLines="None" Width="100%" PageSize="30" 
                    AutoGenerateColumns="False" AllowPaging="True" 
                    EmptyDataText="(无)"                     
                    ShowHeaderWhenEmpty="True" 
                    onpageindexchanging="GridViewStrangeTrade_PageIndexChanging" onrowdatabound="GridViewStrangeTrade_RowDataBound"                     
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
                        <asp:BoundField DataField="Trade_dtd" HeaderText="日期"  HeaderStyle-HorizontalAlign="Left" DataFormatString="{0:yyyy-MM-dd}"/> 
                        <asp:BoundField DataField="s_info_windcode" HeaderText="股票代码"/> 
                        <asp:BoundField DataField="s_info_name" HeaderText="股票名称"/>
                        <asp:BoundField DataField="s_dq_avgprice" HeaderText="均价"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="s_strange_range" HeaderText="涨跌幅"  DataFormatString="{0:P2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="s_strange_type" HeaderText="类型"/> 
                        <asp:BoundField DataField="s_strange_tradername" HeaderText="营业部名称"/> 
                        <asp:BoundField DataField="s_strange_buyamount" HeaderText="买入金额(万元)"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="s_strange_sellamount" HeaderText="卖出金额(万元)"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
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
            <h2>融资融券</h2>
            <asp:GridView ID="GridViewMarginTrade" runat="server" CellPadding="4"
                    ForeColor="#333333" GridLines="None" Width="100%" PageSize="30" 
                    AutoGenerateColumns="False" AllowPaging="True" 
                    EmptyDataText="(无)"                     
                    ShowHeaderWhenEmpty="True" 
                    onpageindexchanging="GridViewMarginTrade_PageIndexChanging" onrowdatabound="GridViewMarginTrade_RowDataBound"
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
                        <asp:BoundField DataField="Trade_dtd" HeaderText="日期"  HeaderStyle-HorizontalAlign="Left" DataFormatString="{0:yyyy-MM-dd}"/> 
                        <asp:BoundField DataField="s_info_windcode" HeaderText="股票代码"/> 
                        <asp:BoundField DataField="s_info_name" HeaderText="股票名称"/>
                        <asp:BoundField DataField="s_margin_tradingbalance" HeaderText="融资余额(万元)"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="s_margin_purchwithborrowmoney" HeaderText="融资买入额(万元)"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="s_margin_repaymenttobroker" HeaderText="融资偿还额(万元)"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="s_margin_seclendingbalance" HeaderText="融券余量金额(万元)"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="s_margin_salesofborrowedamt" HeaderText="融券卖出额(万元)"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="s_margin_repaymentofborrowamt" HeaderText="融券偿还额(万元)"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
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
            <h2>首发增发</h2>
            <asp:GridView ID="GridViewIPOSEO" runat="server" CellPadding="4"
                    ForeColor="#333333" GridLines="None" Width="100%" PageSize="30" 
                    AutoGenerateColumns="False" AllowPaging="True" 
                    EmptyDataText="(无)"                     
                    ShowHeaderWhenEmpty="True" 
                    onpageindexchanging="GridViewIPOSEO_PageIndexChanging" onrowdatabound="GridViewIPOSEO_RowDataBound"                     
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
                        <asp:BoundField DataField="s_fellow_offeringdated" HeaderText="公告日期"  HeaderStyle-HorizontalAlign="Left" DataFormatString="{0:yyyy-MM-dd}"/> 
                        <asp:BoundField DataField="s_info_windcode" HeaderText="股票代码"/> 
                        <asp:BoundField DataField="s_info_name" HeaderText="股票名称"/>
                        <asp:BoundField DataField="s_fellow_price" HeaderText="发行价"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="s_fellow_amount" HeaderText="发行量(万股)"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="s_fellow_collection" HeaderText="募集金额(万元)"  DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" /> 
                        <asp:BoundField DataField="s_fellow_issuetypetext" HeaderText="备注"/>
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
            <h2>新闻资讯</h2>
            <asp:GridView ID="GridViewNews" runat="server" CellPadding="4"
                    ForeColor="#333333" GridLines="None" Width="100%" PageSize="30" 
                    AutoGenerateColumns="False" AllowPaging="True" 
                    EmptyDataText="(无)"                     
                    ShowHeaderWhenEmpty="True" 
                onpageindexchanging="GridViewNews_PageIndexChanging" onrowdatabound="GridViewNews_RowDataBound" 
                    
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
                        <asp:BoundField DataField="Trade_dtd" HeaderText="日期"  HeaderStyle-HorizontalAlign="Left" DataFormatString="{0:yyyy-MM-dd}"/> 
                        <asp:BoundField DataField="s_info_windcode" HeaderText="股票代码"/> 
                        <asp:BoundField DataField="s_info_name" HeaderText="股票名称"/>                        
                        <asp:TemplateField HeaderText = "新闻标题" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <%# "<a target=\"_blank\" href=\"MarketNewsContent.aspx?newsid=" + Eval("Newstextid") + "\">" + Eval("TITLE") + "</a>"%>
                                </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="Mediumname" HeaderText="媒体名称"/>

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
