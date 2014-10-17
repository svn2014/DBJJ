using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace PortfolioEvaluation
{
    public class HTMLOutput
    {
        private static Hashtable _InstanceList = new Hashtable();
        public static HTMLOutput GetActiveInstance(string code)
        {
            if (_InstanceList.ContainsKey(code))
            {
                return (HTMLOutput)_InstanceList[code];
            }
            else
            {
                return null;
            }
        }

        private PerformanceEvaluator _PerformanceObject;

        private string _tablestyle = "style=\"border:1px solid black;\"";
        private string _cellstyle = "bgcolor='#507CD1' style='color: #FFFFFF'";

        public HTMLOutput(PerformanceEvaluator perfEval)
        {
            this._PerformanceObject = perfEval;
            initReportTables();

            if (_InstanceList.ContainsKey(perfEval.Code))
            {
                _InstanceList[perfEval.Code] = this;
            }
            else
            {
                _InstanceList.Add(perfEval.Code, this);
            }
        }

        private DataTable _EquityAllowcationOutput;
        private DataTable _IndustryAllowcationOutput;
        private DataTable _BondAllowcationOutput;
        private DataTable _CBondAllowcationOutput;  //可转债
        private DataTable _CashAllowcationOutput;
        private DataTable _OtherAllowcationOutput;
        private DataTable _TransactionOutput;
        private DataTable _SubscribeRedeemOutput;
        private void initReportTables()
        {
            #region 现金头寸明细
            if (_CashAllowcationOutput == null)
            {
                _CashAllowcationOutput = new DataTable();
                _CashAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_Name, Type.GetType("System.String")));
                _CashAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_HoldingMV, Type.GetType("System.Double")));
                _CashAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_pctofNAV, Type.GetType("System.Double")));
                _CashAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_PortfolioReturnPct, Type.GetType("System.Double")));
                _CashAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_PortfolioReturn, Type.GetType("System.Double")));
                _CashAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_AnnualYield, Type.GetType("System.Double")));
            }
            #endregion

            #region 股票持仓明细
            if (_EquityAllowcationOutput == null)
            {
                _EquityAllowcationOutput = new DataTable();
                _EquityAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_Code, Type.GetType("System.String")));
                _EquityAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_Name, Type.GetType("System.String")));
                _EquityAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_HoldingVolume, Type.GetType("System.Double")));
                _EquityAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_HoldingMV, Type.GetType("System.Double")));
                _EquityAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_pctofNAV, Type.GetType("System.Double")));
                _EquityAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_PortfolioWeight, Type.GetType("System.Double")));
                _EquityAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_BenchmarkWeight, Type.GetType("System.Double")));
                _EquityAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_BMWghtDiff, Type.GetType("System.Double")));
                _EquityAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_PortfolioReturnPct, Type.GetType("System.Double")));
                _EquityAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_PortfolioReturn, Type.GetType("System.Double")));
                _EquityAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_pctofListed, Type.GetType("System.Double")));
                _EquityAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_Turnover, Type.GetType("System.Double")));
                _EquityAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_LiqCycle, Type.GetType("System.Double")));
                _EquityAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_Industry, Type.GetType("System.String")));
            }
            #endregion

            #region 债券持仓明细
            if (_BondAllowcationOutput == null)
            {
                //纯债
                _BondAllowcationOutput = new DataTable();
                _BondAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_Code, Type.GetType("System.String")));
                _BondAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_Name, Type.GetType("System.String")));
                _BondAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_HoldingVolume, Type.GetType("System.Double")));
                _BondAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_HoldingMV, Type.GetType("System.Double")));
                _BondAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_pctofNAV, Type.GetType("System.Double")));
                _BondAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_PortfolioWeight, Type.GetType("System.Double")));
                _BondAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_PortfolioReturnPct, Type.GetType("System.Double")));
                _BondAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_PortfolioNetReturnPct, Type.GetType("System.Double")));
                _BondAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_PortfolioReturn, Type.GetType("System.Double")));
                _BondAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_PortfolioNetReturn, Type.GetType("System.Double")));
                _BondAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_pctofListed, Type.GetType("System.Double")));
                _BondAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_Turnover, Type.GetType("System.Double")));
                _BondAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_LiqCycle, Type.GetType("System.Double")));
                _BondAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_AnnualYield, Type.GetType("System.Double")));
                _BondAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_MaturityDate, Type.GetType("System.DateTime")));

                //可转债
                _CBondAllowcationOutput = _BondAllowcationOutput.Clone();
            }
            #endregion

            #region 其他持仓明细
            if (_OtherAllowcationOutput == null)
            {
                _OtherAllowcationOutput = new DataTable();
                _OtherAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_Code, Type.GetType("System.String")));
                _OtherAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_Name, Type.GetType("System.String")));
                _OtherAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_HoldingVolume, Type.GetType("System.Double")));
                _OtherAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_HoldingMV, Type.GetType("System.Double")));
                _OtherAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_pctofNAV, Type.GetType("System.Double")));
                _OtherAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_PortfolioWeight, Type.GetType("System.Double")));
                _OtherAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_PortfolioReturnPct, Type.GetType("System.Double")));
                _OtherAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_PortfolioReturn, Type.GetType("System.Double")));
            }
            #endregion

            #region 行业配置
            if (_IndustryAllowcationOutput == null)
            {
                _IndustryAllowcationOutput = new DataTable();
                _IndustryAllowcationOutput.Columns.Add((new DataColumn(DBConsts.C_ColumnName_Industry, Type.GetType("System.String"))));
                _IndustryAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_HoldingMV, Type.GetType("System.Double")));
                _IndustryAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_PortfolioWeight, Type.GetType("System.Double")));
                _IndustryAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_BenchmarkWeight, Type.GetType("System.Double")));
                _IndustryAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_BMWghtDiff, Type.GetType("System.Double")));
                _IndustryAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_PortfolioReturnPct, Type.GetType("System.Double")));
                _IndustryAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_BenchmarkReturnPct, Type.GetType("System.Double")));
                _IndustryAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_PureSectorAllowcation, Type.GetType("System.Double")));
                _IndustryAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_AllowcationSelectionInteraction, Type.GetType("System.Double")));
                _IndustryAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_WithinSectorSelection, Type.GetType("System.Double")));
                _IndustryAllowcationOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_TotalValueAdded, Type.GetType("System.Double")));
            }
            #endregion

            #region 交易明细
            if (_TransactionOutput == null)
            {
                _TransactionOutput = new DataTable();
                _TransactionOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_TransactionDate, Type.GetType("System.DateTime")));
                _TransactionOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_TransactionType, Type.GetType("System.String")));
                _TransactionOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_Code, Type.GetType("System.String")));
                _TransactionOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_Name, Type.GetType("System.String")));
                _TransactionOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_pctofNAV, Type.GetType("System.Double")));
                _TransactionOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_PortfolioWeight, Type.GetType("System.Double")));
                _TransactionOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_TradingPrice, Type.GetType("System.Double")));
                _TransactionOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_TradingVolume, Type.GetType("System.Double")));
                _TransactionOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_HoldingMV, Type.GetType("System.Double")));
                _TransactionOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_MarketPrice, Type.GetType("System.Double")));                
                _TransactionOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_TransactionReturn, Type.GetType("System.Double")));
                _TransactionOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_Industry, Type.GetType("System.String")));
            }
            #endregion

            #region 申购赎回
            if (_SubscribeRedeemOutput == null)
            {
                _SubscribeRedeemOutput = new DataTable();
                _SubscribeRedeemOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_ReportDate, Type.GetType("System.DateTime")));
                _SubscribeRedeemOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_UnitCumNAV, Type.GetType("System.Double")));
                _SubscribeRedeemOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_PIC, Type.GetType("System.Double")));
                _SubscribeRedeemOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_Subscribe, Type.GetType("System.Double")));
                _SubscribeRedeemOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_Redeem, Type.GetType("System.Double")));
                _SubscribeRedeemOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_EquityWeight, Type.GetType("System.Double")));
                _SubscribeRedeemOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_BondWeight, Type.GetType("System.Double")));
                _SubscribeRedeemOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_YieldOnEquity, Type.GetType("System.Double")));
                _SubscribeRedeemOutput.Columns.Add(new DataColumn(DBConsts.C_ColumnName_NetYieldOnBond, Type.GetType("System.Double")));
            }
            #endregion
        }

        private double calculateYieldOnYear(double periodYield)
        {
            int dateSpan = (_PerformanceObject.EndDate - _PerformanceObject.StartDate).Days;
            return periodYield / dateSpan * 365;
        }

        public string GetFundInfo()
        {
            //benchmark
            string benchmark = "";
            List<BenchmarkDef> bmdList = this._PerformanceObject.PortfolioBenchmark.GetBenchmarkDef();
            for (int i = 0; i < bmdList.Count; i++)
            {
                benchmark += "[" + bmdList[i].Name + "]*[" + bmdList[i].Weight.ToString("P0") + "]+";
            }
            benchmark = benchmark.Substring(0, benchmark.Length - 1);

            //output table
            string html = "<table width=\"100%\" " + this._tablestyle + ">";
            html += @"
                <tr>
                    <td width='10%' " + _cellstyle + @">组合名称</td>
                    <td width='40%'>" + this._PerformanceObject.Name + "(" + this._PerformanceObject.Code + @")</td>
                    <td width='10%' " + _cellstyle + @">组合收益率</td>
                    <td width='10%' align='right' >" + this._PerformanceObject.PerformancePortfolio.YieldOnPortfolio.ToString("P2") + @"</td>
                    <td width='10%' " + _cellstyle + @">基准收益率</td>
                    <td  align='right'>" + this._PerformanceObject.PortfolioBenchmark.GetBenchmarkReturn().ToString("P2") + @"</td>
                </tr>
                <tr>
                    <td " + _cellstyle + @">业绩基准</td>
                    <td>" + benchmark + @"</td>
                    <td " + _cellstyle + @">股票仓位</td>
                    <td align='right'  style='background-color:Yellow' >" + this._PerformanceObject.PerformancePortfolio.EquityPositionPct.ToString("P2") + @"</td>
                    <td " + _cellstyle + @">股票收益率</td>
                    <td align='right' >" + this._PerformanceObject.PerformancePortfolio.YieldOnEquity.ToString("P2")
                                         //+ "\t[年化: " + calculateYieldOnYear(this._PerformanceObject.PerformancePortfolio.YieldOnEquity).ToString("P2") + "]"
                                         + @"</td>
                </tr>
                <tr>
                    <td " + _cellstyle + @">组合份额</td>
                    <td>" + (this._PerformanceObject.PerformancePortfolio.PIC / 10000).ToString("N2") + @"万份 / "
                          + (this._PerformanceObject.PerformancePortfolio.PIC * this._PerformanceObject.PerformancePortfolio.UnitNAV / 10000).ToString("N2") + @"万元 [份额/净值]"
                          + @"</td>
                    <td " + _cellstyle + @">债券仓位</td>
                    <td align='right' >" + this._PerformanceObject.PerformancePortfolio.BondPositionPct.ToString("P2") + @"</td>
                    <td " + _cellstyle + @">债券收益率</td>
                    <td align='right' >" + this._PerformanceObject.PerformancePortfolio.YieldOnBond.ToString("P2")
                                         //+ "\t[年化: " + calculateYieldOnYear(this._PerformanceObject.PerformancePortfolio.YieldOnBond).ToString("P2") + "]"
                                         + @"</td>
                </tr>
                <tr>
                    <td " + _cellstyle + @">组合净值</td>
                    <td>" + (this._PerformanceObject.PerformancePortfolio.UnitNAV).ToString("N4") + @"/"
                          + (this._PerformanceObject.PerformancePortfolio.UnitCumNAV).ToString("N4") + @" [单位净值/累计单位净值]"
                          +@"</td>
                    <td " + _cellstyle + @">其他仓位</td>
                    <td align='right' >" + (1 - this._PerformanceObject.PerformancePortfolio.BondPositionPct - this._PerformanceObject.PerformancePortfolio.EquityPositionPct - this._PerformanceObject.PerformancePortfolio.CashAllPositionPct).ToString("P2") + @"</td>
                    <td " + _cellstyle + @">其他收益率</td>
                    <td align='right' >" + this._PerformanceObject.PerformancePortfolio.YieldOnOther.ToString("P2")
                                         + @"</td>
                </tr>
                <tr>
                    <td " + _cellstyle + @">报告日期</td>
                    <td>" + this._PerformanceObject.StartDate.ToString("yyyy-MM-dd") + "至" + this._PerformanceObject.EndDate.ToString("yyyy-MM-dd") + @"</td>
                    <td " + _cellstyle + @">现金比例</td>
                    <td align='right'>" + this._PerformanceObject.PerformancePortfolio.CashAllPositionPct.ToString("P2") + @"</td>
                    <td " + _cellstyle + @">现金收益率</td>
                    <td align='right'>" + this._PerformanceObject.PerformancePortfolio.YieldOnCash.ToString("P2")
                                        + " [年化: " + calculateYieldOnYear(this._PerformanceObject.PerformancePortfolio.YieldOnCash).ToString("P2") + "]"
                                        + @"</td>
                </tr>
                ";

            html += "</table>";
            return html;
        }
        
        public DataTable GetEquityPositionTable()
        {
            if (this._EquityAllowcationOutput == null || this._EquityAllowcationOutput.Rows.Count == 0)
            {
                //Already sorted by position %
                List<AssetEvaluation> equityPositionList = this._PerformanceObject.EquityPerformanceList;

                DataRow oNewRow;
                double sumoftop10_pctofequity = 0, sumoftop20_pctofequity = 0;
                double sumoftop10_pctofnav = 0, sumoftop20_pctofnav = 0;

                int i = 0;
                for (; i < equityPositionList.Count; i++)
                {
                    if (i < 10)
                    {
                        sumoftop10_pctofequity += equityPositionList[i].PortfolioWeight;
                        sumoftop10_pctofnav += equityPositionList[i].pctofNAV;
                    }

                    if (i < 20)
                    {
                        sumoftop20_pctofequity += equityPositionList[i].PortfolioWeight;
                        sumoftop20_pctofnav += equityPositionList[i].pctofNAV;
                    }

                    oNewRow = _EquityAllowcationOutput.NewRow();
                    oNewRow[DBConsts.C_ColumnName_Code] = equityPositionList[i].Code;
                    oNewRow[DBConsts.C_ColumnName_Name] = equityPositionList[i].Name;
                    oNewRow[DBConsts.C_ColumnName_HoldingVolume] = equityPositionList[i].HoldingVolume / 10000;
                    oNewRow[DBConsts.C_ColumnName_HoldingMV] = equityPositionList[i].HoldingMarketValue / 10000;
                    oNewRow[DBConsts.C_ColumnName_pctofNAV] = equityPositionList[i].pctofNAV;
                    oNewRow[DBConsts.C_ColumnName_PortfolioWeight] = equityPositionList[i].PortfolioWeight;
                    oNewRow[DBConsts.C_ColumnName_BenchmarkWeight] = equityPositionList[i].SectorBenchmarkWeight;
                    oNewRow[DBConsts.C_ColumnName_BMWghtDiff] = equityPositionList[i].PortfolioWeight - equityPositionList[i].SectorBenchmarkWeight;
                    oNewRow[DBConsts.C_ColumnName_PortfolioReturnPct] = equityPositionList[i].PortfolioReturnPct;
                    oNewRow[DBConsts.C_ColumnName_PortfolioReturn] = equityPositionList[i].PortfolioReturn/10000;
                    oNewRow[DBConsts.C_ColumnName_pctofListed] = equityPositionList[i].pctofListedShare;
                    oNewRow[DBConsts.C_ColumnName_Turnover] = equityPositionList[i].TurnoverRate;
                    oNewRow[DBConsts.C_ColumnName_LiqCycle] = equityPositionList[i].LiquidityCycle;
                    oNewRow[DBConsts.C_ColumnName_Industry] = equityPositionList[i].EconomicSector;
                    this._EquityAllowcationOutput.Rows.Add(oNewRow);
                }

                if (equityPositionList.Count > 0)
                {
                    //Summary Top 10
                    oNewRow = _EquityAllowcationOutput.NewRow();
                    oNewRow[DBConsts.C_ColumnName_Code] = "前10名";
                    oNewRow[DBConsts.C_ColumnName_pctofNAV] = sumoftop10_pctofnav;
                    oNewRow[DBConsts.C_ColumnName_PortfolioWeight] = sumoftop10_pctofequity;
                    this._EquityAllowcationOutput.Rows.InsertAt(oNewRow, 0);

                    //Summary Top 20
                    oNewRow = _EquityAllowcationOutput.NewRow();
                    oNewRow[DBConsts.C_ColumnName_Code] = "前20名";
                    oNewRow[DBConsts.C_ColumnName_pctofNAV] = sumoftop20_pctofnav;
                    oNewRow[DBConsts.C_ColumnName_PortfolioWeight] = sumoftop20_pctofequity;
                    this._EquityAllowcationOutput.Rows.InsertAt(oNewRow, 0);
                }
            }

            return this._EquityAllowcationOutput;
        }

        public DataTable GetPureBondPositionTable()
        {
            if (this._BondAllowcationOutput == null || this._BondAllowcationOutput.Rows.Count == 0)
                GetBondPositionTable(AssetCategory.Bond);

            return this._BondAllowcationOutput;
        }

        public DataTable GetConvertableBondPositionTable()
        {
            if (this._CBondAllowcationOutput == null || this._CBondAllowcationOutput.Rows.Count == 0)
                GetBondPositionTable(AssetCategory.ConvertableBond);

            return this._CBondAllowcationOutput;
        }

        private void GetBondPositionTable(AssetCategory category)
        {            
            //Already sorted by position %
            List<AssetEvaluation> bondPositionList = this._PerformanceObject.BondPerformanceList;

            DataRow oNewRow;
            double sumoftop10_pctofequity = 0, sumoftop20_pctofequity = 0;
            double sumoftop10_pctofnav = 0, sumoftop20_pctofnav = 0;

            DataTable dtOutput = null;
            if (category == AssetCategory.Bond)
                dtOutput = _BondAllowcationOutput;
            else
                dtOutput = _CBondAllowcationOutput;

            int i = 0;
            for (; i < bondPositionList.Count; i++)
            {
                if (bondPositionList[i].Category == category)
                {
                    if (i < 10)
                    {
                        sumoftop10_pctofequity += bondPositionList[i].PortfolioWeight;
                        sumoftop10_pctofnav += bondPositionList[i].pctofNAV;
                    }

                    if (i < 20)
                    {
                        sumoftop20_pctofequity += bondPositionList[i].PortfolioWeight;
                        sumoftop20_pctofnav += bondPositionList[i].pctofNAV;
                    }

                    oNewRow = dtOutput.NewRow();
                    oNewRow[DBConsts.C_ColumnName_Code] = bondPositionList[i].Code;
                    oNewRow[DBConsts.C_ColumnName_Name] = bondPositionList[i].Name;
                    oNewRow[DBConsts.C_ColumnName_HoldingVolume] = bondPositionList[i].HoldingVolume / 10000;
                    oNewRow[DBConsts.C_ColumnName_HoldingMV] = bondPositionList[i].HoldingMarketValue / 10000;
                    oNewRow[DBConsts.C_ColumnName_pctofNAV] = bondPositionList[i].pctofNAV;
                    oNewRow[DBConsts.C_ColumnName_PortfolioWeight] = bondPositionList[i].PortfolioWeight;
                    oNewRow[DBConsts.C_ColumnName_PortfolioReturnPct] = bondPositionList[i].PortfolioReturnPct;
                    oNewRow[DBConsts.C_ColumnName_PortfolioNetReturnPct] = bondPositionList[i].PortfolioNetReturnPct;
                    oNewRow[DBConsts.C_ColumnName_PortfolioReturn] = bondPositionList[i].PortfolioReturn / 10000;
                    oNewRow[DBConsts.C_ColumnName_PortfolioNetReturn] = bondPositionList[i].PortfolioNetReturn / 10000;
                    oNewRow[DBConsts.C_ColumnName_pctofListed] = bondPositionList[i].pctofListedShare;
                    oNewRow[DBConsts.C_ColumnName_Turnover] = bondPositionList[i].TurnoverRate;
                    oNewRow[DBConsts.C_ColumnName_LiqCycle] = bondPositionList[i].LiquidityCycle;
                    oNewRow[DBConsts.C_ColumnName_AnnualYield] = bondPositionList[i].AnnualYield;
                    oNewRow[DBConsts.C_ColumnName_MaturityDate] = bondPositionList[i].BondMaturityDate;
                    dtOutput.Rows.Add(oNewRow);
                }
            }

            if (bondPositionList.Count > 0)
            {
                //Summary Top 10
                oNewRow = dtOutput.NewRow();
                oNewRow[DBConsts.C_ColumnName_Code] = "前10名";
                oNewRow[DBConsts.C_ColumnName_pctofNAV] = sumoftop10_pctofnav;
                oNewRow[DBConsts.C_ColumnName_PortfolioWeight] = sumoftop10_pctofequity;
                dtOutput.Rows.InsertAt(oNewRow, 0);

                //Summary Top 20
                oNewRow = dtOutput.NewRow();
                oNewRow[DBConsts.C_ColumnName_Code] = "前20名";
                oNewRow[DBConsts.C_ColumnName_pctofNAV] = sumoftop20_pctofnav;
                oNewRow[DBConsts.C_ColumnName_PortfolioWeight] = sumoftop20_pctofequity;
                dtOutput.Rows.InsertAt(oNewRow, 0);
            }
        }

        public DataTable GetOtherPositionTable()
        {
            if (this._OtherAllowcationOutput == null || this._OtherAllowcationOutput.Rows.Count == 0)
            {
                //Already sorted by position %
                List<AssetEvaluation> otherPositionList = this._PerformanceObject.OtherPerformanceList;

                DataRow oNewRow;
                double sumoftop10_pctofequity = 0, sumoftop20_pctofequity = 0;
                double sumoftop10_pctofnav = 0, sumoftop20_pctofnav = 0;

                int i = 0;
                for (; i < otherPositionList.Count; i++)
                {
                    if (i < 10)
                    {
                        sumoftop10_pctofequity += otherPositionList[i].PortfolioWeight;
                        sumoftop10_pctofnav += otherPositionList[i].pctofNAV;
                    }

                    if (i < 20)
                    {
                        sumoftop20_pctofequity += otherPositionList[i].PortfolioWeight;
                        sumoftop20_pctofnav += otherPositionList[i].pctofNAV;
                    }

                    oNewRow = _OtherAllowcationOutput.NewRow();
                    oNewRow[DBConsts.C_ColumnName_Code] = otherPositionList[i].Code;
                    oNewRow[DBConsts.C_ColumnName_Name] = otherPositionList[i].Name;
                    oNewRow[DBConsts.C_ColumnName_HoldingVolume] = otherPositionList[i].HoldingVolume / 10000;
                    oNewRow[DBConsts.C_ColumnName_HoldingMV] = otherPositionList[i].HoldingMarketValue / 10000;
                    oNewRow[DBConsts.C_ColumnName_pctofNAV] = otherPositionList[i].pctofNAV;
                    oNewRow[DBConsts.C_ColumnName_PortfolioWeight] = otherPositionList[i].PortfolioWeight;
                    oNewRow[DBConsts.C_ColumnName_PortfolioReturnPct] = otherPositionList[i].PortfolioReturnPct;
                    oNewRow[DBConsts.C_ColumnName_PortfolioReturn] = otherPositionList[i].PortfolioReturn / 10000;
                    this._OtherAllowcationOutput.Rows.Add(oNewRow);
                }

                if (otherPositionList.Count > 0)
                {
                    //Summary Top 10
                    oNewRow = _OtherAllowcationOutput.NewRow();
                    oNewRow[DBConsts.C_ColumnName_Code] = "前10名";
                    oNewRow[DBConsts.C_ColumnName_pctofNAV] = sumoftop10_pctofnav;
                    oNewRow[DBConsts.C_ColumnName_PortfolioWeight] = sumoftop10_pctofequity;
                    this._OtherAllowcationOutput.Rows.InsertAt(oNewRow, 0);

                    //Summary Top 20
                    oNewRow = _OtherAllowcationOutput.NewRow();
                    oNewRow[DBConsts.C_ColumnName_Code] = "前20名";
                    oNewRow[DBConsts.C_ColumnName_pctofNAV] = sumoftop20_pctofnav;
                    oNewRow[DBConsts.C_ColumnName_PortfolioWeight] = sumoftop20_pctofequity;
                    this._OtherAllowcationOutput.Rows.InsertAt(oNewRow, 0);
                }
            }

            return this._OtherAllowcationOutput;
        }

        public DataTable GetCashPositionTable()
        {
            if (this._CashAllowcationOutput == null || this._CashAllowcationOutput.Rows.Count == 0)
            {
                //Already sorted by position %
                List<AssetEvaluation> cashPositionList = this._PerformanceObject.CashPerformanceList;

                DataRow oNewRow;
                for (int i = 0; i < cashPositionList.Count; i++)
                {
                    oNewRow = _CashAllowcationOutput.NewRow();
                    oNewRow[DBConsts.C_ColumnName_Name] = cashPositionList[i].Name;
                    oNewRow[DBConsts.C_ColumnName_HoldingMV] = cashPositionList[i].HoldingMarketValue / 10000;
                    oNewRow[DBConsts.C_ColumnName_pctofNAV] = cashPositionList[i].pctofNAV;
                    oNewRow[DBConsts.C_ColumnName_PortfolioReturnPct] = cashPositionList[i].PortfolioReturnPct;
                    oNewRow[DBConsts.C_ColumnName_PortfolioReturn] = cashPositionList[i].PortfolioReturn / 10000;
                    oNewRow[DBConsts.C_ColumnName_AnnualYield] = cashPositionList[i].AnnualYield;
                    this._CashAllowcationOutput.Rows.Add(oNewRow);
                }
            }

            return this._CashAllowcationOutput;
        }

        public DataTable GetIndustryTable()
        {
            if (_IndustryAllowcationOutput == null || _IndustryAllowcationOutput.Rows.Count == 0)
            {
                List<AssetEvaluation> equityIndustryList = this._PerformanceObject.IndustryPerformanceList;

                foreach (AssetEvaluation industry in equityIndustryList)
                {
                    DataRow oNewRow = _IndustryAllowcationOutput.NewRow();
                    oNewRow[DBConsts.C_ColumnName_Industry] = industry.EconomicSector;
                    oNewRow[DBConsts.C_ColumnName_HoldingMV] = industry.HoldingMarketValue / 10000;
                    oNewRow[DBConsts.C_ColumnName_PortfolioWeight] = industry.PortfolioWeight;
                    oNewRow[DBConsts.C_ColumnName_BenchmarkWeight] = industry.SectorBenchmarkWeight;
                    oNewRow[DBConsts.C_ColumnName_BMWghtDiff] = industry.PortfolioWeight - industry.SectorBenchmarkWeight;
                    oNewRow[DBConsts.C_ColumnName_PortfolioReturnPct] = industry.PortfolioReturnPct;
                    oNewRow[DBConsts.C_ColumnName_BenchmarkReturnPct] = industry.SectorBenchmarkReturnPct;
                    oNewRow[DBConsts.C_ColumnName_PureSectorAllowcation] = industry.PureSectorAllocation;
                    oNewRow[DBConsts.C_ColumnName_AllowcationSelectionInteraction] = industry.AllocationSelectionInteraction;
                    oNewRow[DBConsts.C_ColumnName_WithinSectorSelection] = industry.WithinSectorSelection;
                    oNewRow[DBConsts.C_ColumnName_TotalValueAdded] = industry.TotalValueAdded;
                    this._IndustryAllowcationOutput.Rows.Add(oNewRow);
                }
            }

            return _IndustryAllowcationOutput;
        }

        public DataTable GetTransactionTable()
        {
            if (this._TransactionOutput == null || this._TransactionOutput.Rows.Count == 0)
            {
                //Already sorted
                List<Transaction> transactionList = this._PerformanceObject.TransactionList;

                DataRow oNewRow;

                for (int i = 0; i < transactionList.Count; i++)
                {
                    oNewRow = _TransactionOutput.NewRow();
                    oNewRow[DBConsts.C_ColumnName_TransactionDate] = transactionList[i].TransactionDate;
                    oNewRow[DBConsts.C_ColumnName_Code] = transactionList[i].Code;
                    oNewRow[DBConsts.C_ColumnName_Name] = transactionList[i].Name;
                    oNewRow[DBConsts.C_ColumnName_pctofNAV] = transactionList[i].Pctof_Nav;
                    oNewRow[DBConsts.C_ColumnName_PortfolioWeight] = transactionList[i].Pctof_Portfolio;
                    oNewRow[DBConsts.C_ColumnName_TradingPrice] = transactionList[i].TradingPrice;
                    oNewRow[DBConsts.C_ColumnName_TradingVolume] = transactionList[i].TradingVolume / 10000;
                    oNewRow[DBConsts.C_ColumnName_MarketPrice] = transactionList[i].MarketPrice;
                    oNewRow[DBConsts.C_ColumnName_Industry] = transactionList[i].IndustryName;
                    oNewRow[DBConsts.C_ColumnName_HoldingMV] = transactionList[i].MarketPrice * transactionList[i].TradingVolume / 10000;

                    if (transactionList[i].TradingPrice > 0)
                        oNewRow[DBConsts.C_ColumnName_TransactionReturn] = transactionList[i].MarketPrice / transactionList[i].TradingPrice - 1;
                    else
                        oNewRow[DBConsts.C_ColumnName_TransactionReturn] = 0;

                    if (transactionList[i].TransactionType == Transaction.TradingType.SellAll || transactionList[i].TransactionType == Transaction.TradingType.SellSome)
                    {
                        oNewRow[DBConsts.C_ColumnName_TransactionReturn] = -Convert.ToDouble(oNewRow[DBConsts.C_ColumnName_TransactionReturn]);
                    }

                    switch (transactionList[i].TransactionType)
                    {
                        case Transaction.TradingType.BuyNew:
                            oNewRow[DBConsts.C_ColumnName_TransactionType] = "买入";
                            break;
                        case Transaction.TradingType.BuyMore:
                            oNewRow[DBConsts.C_ColumnName_TransactionType] = "增持";
                            break;
                        case Transaction.TradingType.SellSome:
                            oNewRow[DBConsts.C_ColumnName_TransactionType] = "减持";
                            break;
                        case Transaction.TradingType.SellAll:
                            oNewRow[DBConsts.C_ColumnName_TransactionType] = "卖出";
                            break;
                        default:
                            oNewRow[DBConsts.C_ColumnName_TransactionType] = "未知";
                            break;
                    }

                    this._TransactionOutput.Rows.Add(oNewRow);
                }
            }

            return this._TransactionOutput;
        }

        public DataTable GetSubscribeRedeemTable()
        {
            if (this._SubscribeRedeemOutput == null || this._SubscribeRedeemOutput.Rows.Count == 0)
            {
                //Already sorted
                List<SubscribeRedemp> subscribeRedeemList = this._PerformanceObject.SubscribeRedempList;

                DataRow oNewRow;

                for (int i = 0; i < subscribeRedeemList.Count; i++)
                {
                    oNewRow = _SubscribeRedeemOutput.NewRow();
                    oNewRow[DBConsts.C_ColumnName_ReportDate] = subscribeRedeemList[i].ReportDate;
                    oNewRow[DBConsts.C_ColumnName_UnitCumNAV] = subscribeRedeemList[i].UnitCumNAV;
                    oNewRow[DBConsts.C_ColumnName_PIC] = subscribeRedeemList[i].CurrentBalance / 10000;
                    oNewRow[DBConsts.C_ColumnName_Subscribe] = subscribeRedeemList[i].SubscribeAmount / 10000;
                    oNewRow[DBConsts.C_ColumnName_Redeem] = subscribeRedeemList[i].RedeemAmount / 10000;
                    oNewRow[DBConsts.C_ColumnName_EquityWeight] = subscribeRedeemList[i].EquityWeight;
                    oNewRow[DBConsts.C_ColumnName_BondWeight] = subscribeRedeemList[i].BondWeight;
                    oNewRow[DBConsts.C_ColumnName_YieldOnEquity] = subscribeRedeemList[i].YieldOnEquity;
                    oNewRow[DBConsts.C_ColumnName_NetYieldOnBond] = subscribeRedeemList[i].NetYieldOnBond;
                    this._SubscribeRedeemOutput.Rows.Add(oNewRow);
                }
            }

            return this._SubscribeRedeemOutput;
        }
    }
}
