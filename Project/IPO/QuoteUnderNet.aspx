<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QuoteUnderNet.aspx.cs" Inherits="QuoteUnderNet" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>新股网下询价申请</title>
    <style>
        .table-a td{border:1px solid; font-size:small} 
        .focus
        {
            color: #FF0000;
        }
    </style>
    <!--media=print   这个属性可以在打印时有效-->   
    <style media="print">   
    .Noprint{display:none;}   
    .PageNext{page-break-after: always;}   
    </style>
    <script type="text/javascript">
        var gTotalAmt, gTotalPct, gEstTotalAmt, gEstTotalPct;
        function calcuAll() {
            var fundcodes = document.getElementById("hFundCodes").value;
            var symbols = document.getElementById("hIPOCodes").value;
            var aryFundcode = fundcodes.split(",");
            var arySymbol = symbols.split(",");

            for (var i = 0; i < aryFundcode.length; i++) {
                gTotalAmt = 0;
                gTotalPct = 0;
                gEstTotalAmt = 0;
                gEstTotalPct = 0;

                for (var j = 0; j < arySymbol.length; j++) {
                    calcuRow(aryFundcode[i], arySymbol[j]);
                }

                //Total
                document.getElementById("spTotalAmt" + aryFundcode[i]).innerText = gTotalAmt.toFixed(2);
                document.getElementById("spTotalPct" + aryFundcode[i]).innerText = (gTotalPct * 100).toFixed(2) + "%";
                document.getElementById("spEstTotalAmt" + aryFundcode[i]).innerText = gEstTotalAmt.toFixed(2);
                document.getElementById("spEstTotalPct" + aryFundcode[i]).innerText = (gEstTotalPct * 100).toFixed(2) + "%";

                //Frozen
                var today = new Date();
                var enddate = new Date((today.getYear() < 1900) ? (1900 + today.getYear()) : today.getYear(), today.getMonth() + 4, today.getDay());
                if (document.getElementById("chkFrozenYes" + aryFundcode[i]).checked) {
                    document.getElementById("txtFrozenStart" + aryFundcode[i]).value = dateFormat(today);
                    document.getElementById("txtFrozenEnd" + aryFundcode[i]).value = dateFormat(enddate);
                }
                else {
                    document.getElementById("txtFrozenStart" + aryFundcode[i]).value = "";
                    document.getElementById("txtFrozenEnd" + aryFundcode[i]).value = "";
                }
            }
        }
        function dateFormat(d) {
            var year = (d.getYear() < 1900) ? (1900 + d.getYear()) : d.getYear();
            return year + "-" + (d.getMonth() + 1) + "-" + d.getDate();
        }
        function calcuRow(fundcode, symbol) {
            var unicode = fundcode + "_" + symbol;
            var nav = document.getElementById("calnav" + fundcode).value;
            var px = document.getElementById("calPrice" + symbol).value;
            if (px == "")
                px = 0;
            var winrate = document.getElementById("calWinRate" + symbol).value;
            var vol = document.getElementById("calVolume" + unicode).value;
            var amt = px * vol;
            var pct = amt / nav;
            var estamt = amt * winrate;
            var estpct = pct * winrate;

            var spprice = document.getElementById("spPrice" + unicode);
            var hprice = document.getElementById("hPrice" + unicode);
            var spamt = document.getElementById("calAmount" + unicode);
            var sppct = document.getElementById("calPct" + unicode);
            var spestamt = document.getElementById("calEstAmt" + unicode);
            var spestpct = document.getElementById("calEstPct" + unicode);
            spprice.innerText = (px * 1.0).toFixed(2);
            hprice.value = px;
            spamt.innerText = amt.toFixed(2);
            sppct.innerText = (pct * 100).toFixed(2) + "%";
            spestamt.innerText = estamt.toFixed(2);
            spestpct.innerText = (estpct * 100).toFixed(2) + "%";

            //Summary
            document.getElementById("spSPrice1" + unicode).innerText = (px * 1.0).toFixed(2);
            document.getElementById("spSVolume1" + unicode).innerText = vol;
            document.getElementById("spSAmount1" + unicode).innerText = amt.toFixed(2);
            document.getElementById("spSPrice2" + unicode).innerText = (px * 1.0).toFixed(2);
            document.getElementById("spSVolume2" + unicode).innerText = vol;
            document.getElementById("spSAmount2" + unicode).innerText = amt.toFixed(2);

            //Total
            var chkJoin = document.getElementById("chkJoin" + unicode);
            if (chkJoin.checked) {
                gTotalAmt += amt;
                gTotalPct += pct;
                gEstTotalAmt += estamt;
                gEstTotalPct += estpct;
            }
        }

        function joinIn(chk, unicode) {
            var vol = document.getElementById("calVolume" + unicode);
            var td = chk.parentElement;
            var tr = td.parentElement;
            var trSummary1 = document.getElementById("trSummary1" + unicode);
            var trSummary2 = document.getElementById("trSummary2" + unicode);

            if (chk.checked) {
                vol.style.visibility = "visible";
                tr.style.color = "#000000";
                tr.style.textDecoration = "none";

                trSummary1.style.visibility = "visible";
                trSummary2.style.visibility = "visible";
            }
            else {
                vol.style.visibility = "hidden";
                tr.style.color = "#FFFFFF";
                tr.style.textDecoration = "line-through";

                trSummary1.style.visibility = "collapse";
                trSummary2.style.visibility = "collapse";
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h2 style="text-align: center">新股网下询价审批表</h2>
        <table width="100%" cellspacing="0" cellpadding="0" class="table-a">
            <tr>
                <td colspan="4">1.发行信息</td>
            </tr>
            <tr>
                <td>询价日期</td>
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
                    <asp:BoundField DataField="PurchaseCodeOnNet" HeaderText="发行" 
                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" > 
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
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
                    <asp:BoundField DataField="TotalCapitalBeforeIPO" HeaderText="原股本"  
                        DataFormatString="{0:N0}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" 
                        HeaderStyle-HorizontalAlign="Right"> 
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="PlannedIssueCapital" HeaderText="拟发行"  
                        DataFormatString="{0:N0}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" 
                        HeaderStyle-HorizontalAlign="Right"> 
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="PlannedTransferCapital" HeaderText="老股"  
                        DataFormatString="{0:N0}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" 
                        HeaderStyle-HorizontalAlign="Right"> 
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <%--<asp:BoundField DataField="PrimaryUnderwriter0" HeaderText="主承销商"  
                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" > 
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Contacts0" HeaderText="联系人"  
                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" > 
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Phones0" HeaderText="电话"  
                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" > 
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>--%>
                    <asp:TemplateField HeaderText = "德邦证券副主" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# "<input type='radio' id='' name='" + Eval("Symbol") + "' style='background-color: #FFFF00; font-size: large; font-weight: bold'>是</input> <input type='radio' id='' name='" + Eval("Symbol") + "' style='background-color: #FFFF00; font-size: large; font-weight: bold'>否</input> "%>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText = "对报告的评价" HeaderStyle-HorizontalAlign="Right"  ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <%# "客观性<input type='text' style='background-color: #FFFF00; font-size: large; font-weight: bold; width:30px;text-align:right;' /><br />"%>
                            <%# "合理性<input type='text' style='background-color: #FFFF00; font-size: large; font-weight: bold; width:30px;text-align:right;' /><br />"%>
                            <%# "总评分<input type='text' style='background-color: #FFFF00; font-size: large; font-weight: bold; width:30px;text-align:right;' />"%>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:TemplateField>
                </Columns> 

                </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                <asp:GridView ID="GridViewIPOQuote" runat="server" 
                    AutoGenerateColumns="False" CellPadding="4" 
                    ForeColor="#333333" GridLines="None"  Width="100%"
                    ShowHeaderWhenEmpty="True" CaptionAlign="Left" PageSize="20" 
                    AllowPaging="True" Caption="新股网下询价会议结论" >
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
                    <asp:BoundField DataField="SName" HeaderText="名称"  
                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"> 
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="QuoteEnd" HeaderText="询价截止日"  DataFormatString="{0:d}" 
                        HtmlEncode="False" ItemStyle-HorizontalAlign="Right" 
                        HeaderStyle-HorizontalAlign="Right">                     
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="PurchaseDate" HeaderText="预计申购日"  DataFormatString="{0:d}" 
                        HtmlEncode="False" ItemStyle-HorizontalAlign="Right" 
                        HeaderStyle-HorizontalAlign="Right">                     
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="DownLimitVolUnderNet" HeaderText="下限(万股)"  
                        DataFormatString="{0:N0}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" 
                        HeaderStyle-HorizontalAlign="Right"> 
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="ProgVolUnderNet" HeaderText="累进(万股)"  
                        DataFormatString="{0:N0}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" 
                        HeaderStyle-HorizontalAlign="Right"> 
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="UpLimitVolUnderNet" HeaderText="上限(万股)"  
                        DataFormatString="{0:N0}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right"
                        HeaderStyle-HorizontalAlign="Right" > 
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText = "统一申报价" HeaderStyle-HorizontalAlign="Right"  ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# "<input type='text' style='background-color: #FFFF00; font-size: large; font-weight: bold; width:60px;text-align:right' id='calPrice" + Eval("Symbol") + "' />"%>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText = "预估中签率" HeaderStyle-HorizontalAlign="Right"  ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# "<input type='text' value='0.1' style='background-color: #FFFF00; font-size: large; font-weight: bold; width:60px;text-align:right' id='calWinRate" + Eval("Symbol") + "' />"%>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                    </asp:TemplateField>
                    <asp:BoundField DataField="" HeaderText="研究员签字" ItemStyle-Width="150px"
                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" > 
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:BoundField>
                </Columns> 

                </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="2"><B>承销商列表：(联系方式见推介公告)</B><br /><br />
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
                <td colspan="2" valign="top">
                    定价依据：<br />
                    <input type="radio" name="greason" checked>主承销商的投资价值分析报告</input><br />
                    <input type="radio" name="greason">其他（请说明）</input><br />
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
                    <input type="button" value="计算全部询价数据" onclick="calcuAll();" class="Noprint" />
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
                                <td colspan="5"></td>
                            </tr>
                            <tr>
                                <td colspan="2">净资产</td>
                                <td colspan="2">1</td>
                                <td colspan="2">托管银行</td>
                                <td colspan="3"></td>
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
                                <td></td>
                            </tr>
                            <tr>
                                <td style="background-color: #FFFF00; font-weight: bold"><input type="checkbox" checked>参与</input></td>
                                <td>xxx</td>
                                <td align="right">1</td>
                                <td align="right">
                                    <input id="calVolume11" type="text" value="1000" style="width:80px;text-align:right; background-color: #FFFF00; font-size: large; font-weight: bold;" />
                                </td>
                                <td align="right"><span id="calAmount11" style="font-size: large; font-weight: bold">1.00</span></td>
                                <td align="right"><span id="calPct11">1.11%</span></td>
                                <td><input type="radio" name="group11">是</input><input type="radio" name="group11" checked >否</input></td>
                                <td><input type="radio" name="group21">是</input><input type="radio" name="group21" checked>否</input></td>
                                <td></td>
                            </tr>
                            <tr style="font-size: large; font-weight: bold" align="right">
                                <td></td>
                                <td></td>
                                <td></td>
                                <td>资金合计</td>
                                <td><span id="spTotalAmt">1</span></td>
                                <td><span id="spTotalPct">2</span></td>
                                <td><span id="spEstTotalAmt">3</span></td>
                                <td><span id="spEstTotalPct">4</span></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td colspan="4" valign="top">
                                    是否申请资金冻结：
                                    <input type="radio" name="group21" checked style='background-color: #FFFF00; font-size: large; font-weight: bold'>是</input>
                                    <input type="radio" name="group21" style='background-color: #FFFF00; font-size: large; font-weight: bold'>否</input>
                                    <ul>
                                        <li>冻结起始日：<input id="Text1" type="text" value="" style="width:80px;background-color: #FFFF00; font-size: large; font-weight: bold;" /></li>
                                        <li>冻结终止日：<input id="Text2" type="text" value="" style="width:80px;background-color: #FFFF00; font-size: large; font-weight: bold;" /></li>
                                        <li>冻结总金额：<input id="Text3" type="text" value="0" style="width:80px;background-color: #FFFF00; font-size: large; font-weight: bold;text-align:right;" />万元</li>
                                    </ul>
                                    备注：<br />
                                    冻结资金作为申购款划付时自动解冻；<br />
                                    未获得配售而退回的申购款不再冻结；
                                </td>
                                <td colspan="5" valign="top">
                                    不申请资金冻结请说明理由：<br/>
                                    <div style="width: 100%" align="center">
                                        <textarea rows="8" style="border-style: none; width: 90%; overflow: hidden;"></textarea>
                                    </div>
                                    <br/><br/>
                                    投资总监意见
                                </td>
                            </tr>
                            <tr style="height:60px">
                                <td colspan="9" valign="top">投资经理 签字/日期</td>
                            </tr>
                        </table>
                    </span>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <input type="button" value="计算全部询价数据" onclick="calcuAll();" class="Noprint" />
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
                        <li>申购后基金现金类资产比例不低于基金资产的5%（基金适用）</li>
                        <li>符合询价及推介公告要求</li>
                        <li>是否符合资管合同约定比例</li>
                    </ol>
                
                </td>
            </tr>
            <tr>
                <td colspan="4">4.审批区</td>
            </tr>
            <tr style="height:60px">
                <td valign="top">风控人员 签字/日期</td>
                <td valign="top">运营人员 签字/日期</td>
                <td colspan="2" valign="top">投资总监 签字/日期</td>
            </tr>
        </table>

        <div class="PageNext"></div>
        <div class="PageNext"></div>

        <h3>按投资组合汇总</h3>
        <span id="investmentsummary1" runat="server">
            <table width="100%" cellspacing="0" cellpadding="0">
            <tr>
                <td colspan="5">组合名称</td>
            </tr>
            <tr>
                <td width="10%"></td>
                <td width="20%">股票名称</td>
                <td width="20%">申购价格</td>
                <td width="20%">申购数量</td>
                <td>申购金额</td>
            </tr>
            <tr id="trSummary1" style="visibility: collapse;">
                <td></td>
                <td>xxxx</td>
                <td align="right"><span id="spSPrice1">1.00</span></td>
                <td align="right"><span id="spSVolume1">1000</span></td>
                <td align="right"><span id="spSAmount1">1000.00</span></td>
            </tr>
        </table>
        </span>
    
        <br />

        <h3>按投资标的汇总</h3>
        <span id="investmentsummary2" runat="server">
            <table width="100%" cellspacing="0" cellpadding="0">
            <tr>
                <td colspan="5">股票名称(交易代码,申购代码)</td>
            </tr>
            <tr>
                <td width="10%"></td>
                <td width="20%">组合名称</td>
                <td width="20%">申购价格</td>
                <td width="20%">申购数量</td>
                <td>申购金额</td>
            </tr>
            <tr id="trSummary2">
                <td></td>
                <td>xxxx</td>
                <td align="right"><span id="spSPrice2">1.00</span></td>
                <td align="right"><span id="spSVolume2">1000</span></td>
                <td align="right"><span id="spSAmount2">1000.00</span></td>
            </tr>
        </table>
        </span>
    </div>
    
    </form>
</body>
</html>
