<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseUnderNet.aspx.cs" Inherits="PurchaseUnderNet" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>新股网下申购申请</title>
    <style>
        .table-a td{border:1px solid; font-size:small} 
        .focus
        {
            color: #FF0000;
        }
    </style>
    <script type="text/javascript">
        var gTotalAmt, gTotalPct;
        function calcuAll() {
            var fundcodes = document.getElementById("hFundCodes").value;
            var symbols = document.getElementById("hIPOCodes").value;
            var aryFundcode = fundcodes.split(",");
            var arySymbol = symbols.split(",");

            for (var i = 0; i < aryFundcode.length; i++) {
                gTotalAmt = 0;
                gTotalPct = 0;

                for (var j = 0; j < arySymbol.length; j++) {
                    calcuRow(aryFundcode[i], arySymbol[j]);
                }

                document.getElementById("spTotalAmt" + aryFundcode[i]).innerText = gTotalAmt.toFixed(2);
                document.getElementById("spTotalPct" + aryFundcode[i]).innerText = (gTotalPct * 100).toFixed(2) + "%";
            }
        }
        function calcuRow(fundcode, symbol) {
            var unicode = fundcode + "_" + symbol;
            var nav = document.getElementById("calnav" + fundcode).value;
            var px = document.getElementById("calPrice" + unicode).value;
            var vol = document.getElementById("calVolume" + unicode).value;
            var vol0 = document.getElementById("calVolume0" + unicode).value;
            var amt = px * vol;
            var pct = amt / nav;

            var spamt = document.getElementById("calAmount" + unicode);
            var sppct = document.getElementById("calPct" + unicode);
            spamt.innerText = amt.toFixed(2);
            sppct.innerText = (pct * 100).toFixed(2) + "%";

            //Total
            var chkJoin = document.getElementById("chkJoin" + unicode);
            if (chkJoin.checked) {
                gTotalAmt += amt;
                gTotalPct += pct;
            }

            if (vol != vol0)
                alert("询价股份数应当和申购股份数一致：" + unicode);
        }
        function joinIn(chk, unicode) {
            var vol = document.getElementById("calVolume" + unicode);
            var vol0 = document.getElementById("calVolume0" + unicode);
            var td = chk.parentElement;
            var tr = td.parentElement;

            if (chk.checked) {
                vol.style.visibility = "visible";
                vol0.style.visibility = "visible";
                tr.style.color = "#000000";
                tr.style.textDecoration = "none";
            }
            else {
                vol.style.visibility = "hidden";
                vol0.style.visibility = "hidden";
                tr.style.color = "#FFFFFF";
                tr.style.textDecoration = "line-through";
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h2 style="text-align: center">新股网下申购审批表</h2>
        <table width="100%" cellspacing="0" cellpadding="0" class="table-a">
            <tr>
                <td colspan="4">1.发行信息</td>
            </tr>
            <tr>
                <td>申购日期</td>
                <td>
                    <asp:TextBox ID="tbDate" runat="server" BorderStyle="None" 
                        style="background-color: #FFFF00" Font-Bold="True" Width="100px" 
                        Font-Size="Large"></asp:TextBox>
                </td>
                <td>经办人</td>
                <td>
                    <asp:DropDownList ID="ddlAnalyst" runat="server" BorderStyle="None" 
                        style="background-color: #FFFF00" AutoPostBack="True" Font-Bold="True"
                        onselectedindexchanged="ddlAnalyst_SelectedIndexChanged" Font-Size="Large"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>联系电话</td>
                <td><asp:Label ID="lblAnaPho" runat="server" Text=""></asp:Label></td>
                <td>Email</td>
                <td><asp:Label ID="lblEmail" runat="server" Text=""></asp:Label></td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:GridView ID="GridViewIPOInfo" runat="server" 
                    AutoGenerateColumns="False" CellPadding="4" 
                    ForeColor="#333333" GridLines="None"  Width="100%"
                    ShowHeaderWhenEmpty="True" CaptionAlign="Left" PageSize="20" 
                    AllowPaging="True" Caption="投资标的" >
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
                    <asp:BoundField DataField="PurchaseCodeOnNet" HeaderText="发行代码" 
                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" > 
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Symbol" HeaderText="交易代码" 
                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"> 
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="SName" HeaderText="名称"  
                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"> 
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="TotalCapitalBeforeIPO" HeaderText="原股本"  
                        DataFormatString="{0:N0}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" 
                        HeaderStyle-HorizontalAlign="Right"> 
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="ActualIssuedCapital" HeaderText="发行"  
                        DataFormatString="{0:N0}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" 
                        HeaderStyle-HorizontalAlign="Right"> 
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="ActualTransferredCapital" HeaderText="老股"  
                        DataFormatString="{0:N0}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" 
                        HeaderStyle-HorizontalAlign="Right"> 
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="IssuePrice" HeaderText="发行价"  
                        DataFormatString="{0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" 
                        HeaderStyle-HorizontalAlign="Right"> 
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="DownLimitVolUnderNet" HeaderText="下限"  
                        DataFormatString="{0:N0}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right"
                        HeaderStyle-HorizontalAlign="Right" > 
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="UpLimitVolUnderNet" HeaderText="上限"  
                        DataFormatString="{0:N0}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right"
                        HeaderStyle-HorizontalAlign="Right" > 
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <%--<asp:BoundField DataField="PrimaryUnderwriter0" HeaderText="主承销商"  
                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" > 
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>       --%>
                    <asp:TemplateField HeaderText = "德邦证券副主" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# "<input type='radio' id='' name='" + Eval("Symbol") + "' style='background-color: #FFFF00; font-size: large; font-weight: bold'>是</input> <input type='radio' id='' name='" + Eval("Symbol") + "' style='background-color: #FFFF00; font-size: large; font-weight: bold'>否</input> "%>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:BoundField DataField="" HeaderText="研究员签字" ItemStyle-Width="100px"
                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" > 
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                </Columns> 

                </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="4"><B>承销商列表：(联系方式见推介公告)</B><br /><br />
                    <div id="divUnderwriter" runat="server">
                        天华超净（300390）
                        <ul>
                            <li>1</li>
                            <li>2</li>
                        </ul>
                        艾比森（300389）
                        <ul>
                            <li>1</li>
                            <li>2</li>
                        </ul>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="4"><hr /></td>
            </tr>
            <tr>
                <td colspan="4">2.申购方案</td>
            </tr>
            <tr>
                <td colspan="4">
                    <input type="button" value="计算全部申购数据" onclick="calcuAll();"; />
                    <input type="hidden" value="" id="hFundCodes" runat="server" />
                    <input type="hidden" value="" id="hIPOCodes" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <span id="investmentdetail" runat="server">
                        <table width="100%">
                            <tr>
                                <td colspan="4"><strong style="font-size: large; font-weight: bold">优化配置</strong></td>
                                <td colspan="4"></td>
                            </tr>
                            <tr>
                                <td colspan="2">净资产</td>
                                <td colspan="2">1</td>
                                <td colspan="2">托管银行</td>
                                <td colspan="2"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>名称</td>
                                <td align="right">发行价</td>
                                <td align="right">申购量(股)</td>
                                <td align="right">申购金额(元)</td>
                                <td align="right">占比</td>
                                <td>是否参与过网下询价</td>
                                <td>托管行是否关联方</td>
                            </tr>
                            <tr style="text-decoration: none">
                                <td style="background-color: #FFFF00; font-weight: bold"><input type="checkbox" checked onclick="joinIn(this);">参与</input></td>
                                <td>xxx</td>
                                <td align="right">1</td>
                                <td align="right">
                                    <input id="calVolume11" type="text" value="1000" 
                                        style="width:80px;text-align:right; background-color: #FFFF00; font-size: large; font-weight: bold;" />
                                </td>
                                <td align="right"><span id="calAmount11">1.00</span></td>
                                <td align="right"><span id="calPct11">1.11%</span></td>
                                <td><input type="radio" name="group11">是</input><input type="radio" name="group11" checked >否</input></td>
                                <td><input type="radio" name="group21">是</input><input type="radio" name="group21" checked>否</input></td>
                            </tr>
                            <tr style="height:60px">
                                <td colspan="8" valign="top">投资经理 签字/日期</td>
                            </tr>
                        </table>
                    </span>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <input type="button" value="计算全部申购数据" onclick="calcuAll();"; />
                </td>
            </tr>
            <tr>
                <td colspan="4"><hr /></td>
            </tr>
            <tr>
                <td colspan="4">3.风险检查点</td>
            </tr>
            <tr>
                <td colspan="4">
                
                    <ol>
                        <li>是否存在关联交易</li>
                        <li>申购量不能超过申购上限量</li>
                        <li>申购金额不能超过组合资产的100%</li>
                        <li>申购后单只基金所持该股票市值不能超过基金资产的10%（基金适用）</li>
                        <li>申购后全部基金持有该证券不能超过证券发行总股本的10%（基金适用）</li>
                        <li>申购后基金现金类资产比例不低于基金资产的5%（基金适用）</li>
                        <li>已参与网下发行的组合不得参与网上申购</li>
                        <li>是否符合资管合同约定比例</li>
                        <li>网下申购符合新股询价时的申购承诺</li>
                    </ol>
                
                </td>
            </tr>
            <tr>
                <td colspan="4">4.审批区</td>
            </tr>
            <tr style="height:60px">
                <td colspan="2" valign="top">风控人员 签字/日期</td>
                <td colspan="2" valign="top">投资总监 签字/日期</td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
