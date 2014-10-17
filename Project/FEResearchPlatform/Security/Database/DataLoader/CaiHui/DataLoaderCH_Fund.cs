using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Diagnostics;

namespace Security
{
    public partial class DataLoaderCH
    {
        #region 基金信息
        public override MutualFundGroup GetMutualFunds(AFundCategory category)
        {
            try
            {
                //读取数据库
                this.LoadMutualFundInfo();

                //以Group输出
                MutualFundGroup mfg = new MutualFundGroup();

                foreach (DataRow oRow in _FundInfo.Tables[0].Rows)
                {
                    MutualFund f = new MutualFund(oRow[C_ColName_Code].ToString());
                    this.LoadMutualFundInfo(f);

                    if (category == null ||
                        (
                            (f.Category.AssetCategory == category.AssetCategory || category.AssetCategory == FundAssetCategory.Undefined) &&
                            (f.Category.InvestmentCategory == category.InvestmentCategory || category.InvestmentCategory == FundInvestmentCategory.Undefined) && 
                            (f.Category.OperationCategory == category.OperationCategory || category.OperationCategory == FundOperationCategory.Undefined) &&
                            (f.Category.StructureCategory == category.StructureCategory || category.StructureCategory == FundStructureCategory.Undefined)
                         )
                       )
                        mfg.Add(f);
                }

                return mfg;
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        public override void LoadMutualFundInfo()
        {
            try
            {
                if (_FundInfo == null)
                {
                    string sql = @"SELECT A.Symbol, A.Name as SNAME, A.listdate, B.listdate as relistdate, A.Delistdate, B.GSFundType
                                            ,B.Tdate, B.CompanyName, C.Symbol_Comp, C.Sname_Comp
                                        FROM(
                                          SELECT Symbol, Ofprofile3 as name, OFProfile8 as listdate, 'OF' as FundType, null as Delistdate
                                          FROM OFProfile
                                          UNION
                                          SELECT Symbol, Cfprofile3 as name, CFProfile8 as listdate, 'CF' as FundType, CFProfile10 as Delistdate
                                          FROM CFProfile
                                        ) A
                                        LEFT JOIN (
                                              SELECT AA.Symbol, AA.listdate, AA.Operation, AA.Tdate, AA.CompanyName
                                              , BB.Columnvalue as GSFundType
                                              FROM GSFRating AA 
                                              LEFT JOIN DATADICT_COLUMNVALUE BB
                                                   ON AA.Stype=BB.Columncode AND BB.Cid = '25672'
                                              WHERE TDATE=(SELECT MAX(TDATE) FROM GSFRating WHERE AA.SYMBOL=SYMBOL)
                                        ) B
                                          ON A.Symbol = B.SYMBOL
                                        LEFT JOIN CURFSCODE C
                                          ON A.Symbol = C.SYMBOL
                                        ";

                    //获得所有基金的信息做缓存
                    _FundInfo = base.DBInstance.ExecuteSQL(sql);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override void LoadMutualFundInfo(MutualFund f)
        {
            try
            {
                //读取数据库
                this.LoadMutualFundInfo();

                //更新数据
                DataRow[] rows = _FundInfo.Tables[0].Select("SYMBOL='" + f.Code + "'");
                if (rows.Length >= 1)
                {
                    f.DataSource = this.DataSource;
                    f.Category.DataSource = this.DataSource;

                    DataRow row = rows[0];
                    f.Name = row[C_ColName_Name].ToString();
                    f.ListedDate = DataManager.ConvertToDate(row[C_ColName_ListedDate]);
                    f.DelistedDate = DataManager.ConvertToDate(row[C_ColName_DelistedDate]);
                    f.Category.SetupCategory(row[C_ColName_GSFundType].ToString(), 3);

                    if (row[C_ColName_ParentCode] == DBNull.Value)
                    {
                        //非分级基金
                        f.IsStructured = false;
                        f.Category.StructureCategory = FundStructureCategory.Parent;
                    }
                    else
                    {
                        //分级基金
                        f.IsStructured = true;
                        f.ParentFundCode = row[C_ColName_ParentCode].ToString();

                        if (row[C_ColName_ParentCode].ToString() == row[C_ColName_Code].ToString())
                        {
                            f.Category.StructureCategory = FundStructureCategory.Parent;

                            //查找子基金
                            string sql = @"Select SYMBOL From CURFSCODE WHERE Symbol_Comp = '" + f.Code + "' AND Symbol <> Symbol_Comp";
                            DataSet ds = base.DBInstance.ExecuteSQL(sql);

                            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                            {
                                MessageManager.GetInstance().AddMessage(MessageType.Warning, Message.C_Msg_MF8, f.Code);
                            }
                            else
                            {
                                if (f.SubFundCodes == null)
                                    f.SubFundCodes = new List<string>();

                                foreach (DataRow rowsub in ds.Tables[0].Rows)
                                {
                                    string code = rowsub[C_ColName_Code].ToString();
                                    f.SubFundCodes.Add(code);
                                }
                            }
                        }
                        else
                        {
                            f.Category.StructureCategory = FundStructureCategory.Child;
                        }

                    }
                }
                else
                {
                    MessageManager.GetInstance().AddMessage(MessageType.Warning, Message.C_Msg_MF5, f.Code);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 基金净值
        private const string C_SQL_GetFundNAV = "SELECT * FROM NAV WHERE 1=1 ";
        public override void LoadMutualFundNAV(SeriesNetAssetValue navs)
        {
            try
            {
                //读数据
                string sql = C_SQL_GetFundNAV;
                sql += " AND " + base.BuildSQLClauseIn(navs.Code, "SYMBOL");
                sql += " AND Publishdate >= " + base.DBInstance.ConvertToSQLDate(navs.TimeSeriesStartExtended)
                     + " AND Publishdate <= " + base.DBInstance.ConvertToSQLDate(navs.TimeSeriesEnd);
                sql += " ORDER BY Symbol, Publishdate Desc";
                DataSet ds = base.DBInstance.ExecuteSQL(sql);

                //更新数据
                this.updateFundNAV(ds, navs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override void LoadMutualFundNAV(ASecurityGroup g)
        {
            //===============================
            //批量读取多个股票的数据，速度更快
            //===============================
            try
            {
                //如果没有持仓则退出
                if (g.SecurityHoldings == null || g.SecurityCodes.Count == 0)
                    return;

                //读数据: 按时间降序排列
                string sql = C_SQL_GetFundNAV;
                sql += " AND " + base.BuildSQLClauseIn(g.SecurityCodes, "SYMBOL");
                sql += " AND Publishdate >= " + base.DBInstance.ConvertToSQLDate(g.TimeSeriesStartExtended)
                     + " AND Publishdate <= " + base.DBInstance.ConvertToSQLDate(g.TimeSeriesEnd);
                sql += " ORDER BY Symbol, Publishdate Desc";
                DataSet ds = base.DBInstance.ExecuteSQL(sql);

                //更新数据
                foreach (MutualFund f in g.SecurityHoldings)
                {
                    this.updateFundNAV(ds, f.TradingNAV);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void updateFundNAV(DataSet ds, SeriesNetAssetValue navs)
        {
            DataRow[] rows = ds.Tables[0].Select("SYMBOL='" + navs.Code + "'");
            if (rows.Length > 0)
            {
                //基本信息
                navs.DataSource = this.DataSource;
                navs.OriginalTimeSeries.Clear();

                foreach (DataRow row in rows)
                {
                    NetAssetValue nav = new NetAssetValue();
                    nav.TradeDate = DataManager.ConvertToDate(row[C_ColName_PublishDate]);
                    nav.UnitNAV = DataManager.ConvertToDouble(row[C_ColName_UnitNAV]);
                    nav.AccumUnitNAV = DataManager.ConvertToDouble(row[C_ColName_AccumUnitNAV]);
                    //复权系数
                    navs.OriginalTimeSeries.Add(nav);
                }

                //交易日校验,复权并计算涨跌幅
                navs.Adjust();
            }
            else
            {
                MessageManager.GetInstance().AddMessage(MessageType.Warning, Message.C_Msg_MF4, navs.Code);
            }
        }
        #endregion

        #region 基金价格
        private string C_SQL_GetFundPrice = @"SELECT A.*, C.LTDR*B.TClose as Price2
                                                FROM FHDQUOTE A
                                                LEFT JOIN (
	                                                SELECT * FROM FHDQUOTE AA WHERE TDATE=(SELECT MIN(TDATE) FROM FHDQUOTE WHERE AA.SYMBOL=SYMBOL AND TOPEN>0)
                                                )B
                                                     ON A.Symbol = B.Symbol
                                                LEFT JOIN Se_dreturn C
                                                     ON A.Symbol = C.Symbol AND A.Tdate=C.Tdate AND C.dtype=6
                                                WHERE 1=1 ";

        public override void LoadMutualFundPrice(SeriesFundPrice pxs)
        {
            try
            {
                //读数据
                string sql = C_SQL_GetFundPrice;
                sql += " AND " + base.BuildSQLClauseIn(pxs.Code, "A.SYMBOL");
                sql += " AND A.Tdate >= '" + pxs.TimeSeriesStartExtended.ToString("yyyyMMdd") + "' AND A.Tdate <= '" + pxs.TimeSeriesEnd.ToString("yyyyMMdd") + "'";
                sql += " ORDER BY A.Symbol, A.Tdate Desc";
                DataSet ds = base.DBInstance.ExecuteSQL(sql);

                //更新数据
                this.updateFundPrice(ds, pxs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override void LoadMutualFundPrice(ASecurityGroup g)
        {
            try
            {
                //如果没有持仓则退出
                if (g.SecurityHoldings == null || g.SecurityCodes.Count == 0)
                    return;

                //读数据: 按时间降序排列
                string sql = C_SQL_GetFundPrice;
                sql += " AND " + base.BuildSQLClauseIn(g.SecurityCodes, "A.SYMBOL");
                sql += " AND A.Tdate >= '" + g.TimeSeriesStartExtended.ToString("yyyyMMdd") + "' AND A.Tdate <= '" + g.TimeSeriesEnd.ToString("yyyyMMdd") + "'";
                sql += " ORDER BY A.Symbol, Tdate Desc";
                DataSet ds = base.DBInstance.ExecuteSQL(sql);

                //更新数据
                foreach (MutualFund f in g.SecurityHoldings)
                {
                    this.updateFundPrice(ds, (SeriesFundPrice)f.TradingPrice);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void updateFundPrice(DataSet ds, SeriesFundPrice pxs)
        {
            DataRow[] rows = ds.Tables[0].Select("SYMBOL='" + pxs.Code + "'");
            if (rows.Length > 0)
            {
                //基本信息
                pxs.DataSource = this.DataSource;
                pxs.OriginalTimeSeries.Clear();

                foreach (DataRow row in rows)
                {
                    ExchangeTradingPrice px = new ExchangeTradingPrice();
                    px.TradeDate = DataManager.ConvertToDate(row[C_ColName_TradeDate]);
                    px.PreClose = DataManager.ConvertToDouble(row[C_ColName_PreClose]);
                    px.Close = DataManager.ConvertToDouble(row[C_ColName_Close]);
                    px.High = DataManager.ConvertToDouble(row[C_ColName_High]);
                    px.Low = DataManager.ConvertToDouble(row[C_ColName_Low]);
                    px.Open = DataManager.ConvertToDouble(row[C_ColName_Open]);
                    px.Volume = DataManager.ConvertToDouble(row[C_ColName_Volume]);
                    px.Amount = DataManager.ConvertToDouble(row[C_ColName_Amount]);
                    px.Average = (px.Volume == 0) ? 0 : px.Amount / px.Volume;

                    //判断停牌
                    if (px.Close == 0)
                    {
                        px.IsTrading = false;
                        px.Close = px.PreClose;
                        px.Open = px.PreClose;
                        px.High = px.PreClose;
                        px.Low = px.PreClose;
                    }

                    //复权系数
                    double adjustedClose = DataManager.ConvertToDouble(row[C_ColName_AdjustedClose]);
                    px.AdjustCoefficient = (px.Close == 0 || adjustedClose == 0) ? 1 : adjustedClose / px.Close;
                    pxs.OriginalTimeSeries.Add(px);
                }

                //复权并计算涨跌幅
                pxs.Adjust();
            }
            else
            {
                MessageManager.GetInstance().AddMessage(MessageType.Warning, Message.C_Msg_MF3, pxs.Code);
            }
        }
        #endregion

        #region 基金报表
        private string C_SQL_GetFundShareReport = "SELECT * FROM FShare  WHERE 1=1 ";
        private string C_SQL_GetFundAssetReport = "SELECT * FROM AssetAL WHERE 1=1 ";
        public override void LoadMutualFundReport(SeriesFundReport r)
        {
            try
            {
                //读数据
                string sqlShare = C_SQL_GetFundShareReport
                    + " AND " + base.BuildSQLClauseIn(r.Code, "SYMBOL")
                    + " AND Declaredate >= " + base.DBInstance.ConvertToSQLDate(r.TimeSeriesStartExtended.AddDays(-100))
                    + " AND Declaredate <= " + base.DBInstance.ConvertToSQLDate(r.TimeSeriesEnd)
                    + " ORDER BY Symbol, PublishDate Desc ";

                string sqlAsset = C_SQL_GetFundAssetReport
                    + " AND " + base.BuildSQLClauseIn(r.Code, "SYMBOL")
                    + " AND PublishDate >= " + base.DBInstance.ConvertToSQLDate(r.TimeSeriesStartExtended.AddDays(-100))
                    + " AND PublishDate <= " + base.DBInstance.ConvertToSQLDate(r.TimeSeriesEnd)
                    + " ORDER BY Symbol, Reportdate Desc ";

                DataSet dsShare = base.DBInstance.ExecuteSQL(sqlShare);
                DataSet dsAsset = base.DBInstance.ExecuteSQL(sqlAsset);

                //更新数据
                this.updateFundReport(dsShare, dsAsset, r);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override void LoadMutualFundReport(ASecurityGroup g)
        {
            try
            {
                //如果没有持仓则退出
                if (g.SecurityHoldings == null || g.SecurityCodes.Count == 0)
                    return;

                //读数据: 
                string sqlShare = C_SQL_GetFundShareReport
                    + " AND " + base.BuildSQLClauseIn(g.SecurityCodes, "SYMBOL")
                    + " AND Declaredate >= " + base.DBInstance.ConvertToSQLDate(g.TimeSeriesStartExtended.AddDays(-100))
                    + " AND Declaredate <= " + base.DBInstance.ConvertToSQLDate(g.TimeSeriesEnd)
                    + " ORDER BY Symbol, PublishDate Desc ";

                string sqlAsset = C_SQL_GetFundAssetReport
                    + " AND " + base.BuildSQLClauseIn(g.SecurityCodes, "SYMBOL")
                    + " AND PublishDate >= " + base.DBInstance.ConvertToSQLDate(g.TimeSeriesStartExtended.AddDays(-100))
                    + " AND PublishDate <= " + base.DBInstance.ConvertToSQLDate(g.TimeSeriesEnd)
                    + " ORDER BY Symbol, Reportdate Desc ";

                DataSet dsShare = base.DBInstance.ExecuteSQL(sqlShare);
                DataSet dsAsset = base.DBInstance.ExecuteSQL(sqlAsset);

                //更新数据
                foreach (MutualFund f in g.SecurityHoldings)
                {
                    this.updateFundReport(dsShare, dsAsset, f.FundReport);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void updateFundReport(DataRow rowShare, DataRow rowAsset, SeriesFundReport r, bool useAssetReportDate)
        {
            MutualFundReport rpt = new MutualFundReport();
            if (rowShare != null)
            {
                rpt.PublishDate = DataManager.ConvertToDate(rowShare[C_ColName_DeclareDate]);
                rpt.ReportDate = DataManager.ConvertToDate(rowShare[C_ColName_PublishDate]);

                rpt.TotalShare = DataManager.ConvertToDouble(rowShare[C_ColName_FundShare]) * 10000;        //单位：份
            }

            if (rowAsset != null)
            {
                if (useAssetReportDate)
                {
                    rpt.PublishDate = DataManager.ConvertToDate(rowAsset[C_ColName_PublishDate]);
                    rpt.ReportDate = DataManager.ConvertToDate(rowAsset[C_ColName_ReportDate]);
                }

                rpt.TotalNetAsset = DataManager.ConvertToDouble(rowAsset[C_ColName_FundNetAsset]);          //单位：元
                rpt.TotalEquityAsset = DataManager.ConvertToDouble(rowAsset[C_ColName_FundEquityAsset]);    //单位：元
                rpt.TotalBondAsset = DataManager.ConvertToDouble(rowAsset[C_ColName_FundBondAsset]);        //单位：元

                rpt.ConvertableBondAsset = DataManager.ConvertToDouble(rowAsset[C_ColName_FundCBAsset]);    //单位：元
                rpt.PureBondAsset = rpt.TotalBondAsset - rpt.ConvertableBondAsset;                          //单位：元
            }

            rpt.TradeDate = rpt.PublishDate;
            r.OriginalTimeSeries.Add(rpt);
        }
        private void updateFundReport(DataSet dsShare, DataSet dsAsset, SeriesFundReport rpt)
        {
            //基本信息
            rpt.DataSource = this.DataSource;
            rpt.OriginalTimeSeries.Clear();

            string strSymbol = "SYMBOL='" + rpt.Code + "'";
            DataRow[] rowsShare = dsShare.Tables[0].Select(strSymbol);
            DataRow[] rowsAsset = dsAsset.Tables[0].Select(strSymbol);

            //注意: 数据问题：
            //  1) 年报数据公布的较晚，在年报公布之前已有相关信息被披露，那么过时的数据将丢弃
            //  e.g. 166802.OF, 
            //      2013/1/8  公布了2013/1/4  的基金份额数据；[因折算]
            //      2013/1/22 公布了2012/12/31的基金份额数据；[因年报]    --这条因数据过期被丢弃
            //  2) 通常份额的更新数据多于资产配置的
            //  3) 所有序列按照报告日排序，非公告日

            DateTime dShare1, dAsset1;
            DateTime dShare0, dAsset0;
            DataRow rShare, rAsset;
            int iShare = 0, iAsset = 0;
            //按信息公布日期合并数据
            while (true)
            {
                //=======================
                //  准备数据
                //=======================
                //1.基金份额
                if (rowsShare.Length == 0)
                    rShare = null;
                else if (iShare >= rowsShare.Length)
                    rShare = rowsShare[rowsShare.Length - 1];
                else
                    rShare = rowsShare[iShare];

                if (rShare == null)
                    dShare1 = DateTime.Today.AddDays(1);
                else
                    dShare1 = DataManager.ConvertToDate(rShare[C_ColName_DeclareDate]);//本条数据的公告日

                if (iShare >= 1 && iShare < rowsShare.Length)
                {
                    //前一条数据的公告日（报告日更晚的）
                    dShare0 = DataManager.ConvertToDate(rowsShare[iShare - 1][C_ColName_DeclareDate]);
                    if (dShare1 >= dShare0)
                    {
                        //丢弃过期数据
                        iShare++;
                        continue;
                    }
                }

                //2.基金资产
                if (rowsAsset.Length == 0)
                    rAsset = null;
                else if (iAsset >= rowsAsset.Length)
                    rAsset = rowsAsset[rowsAsset.Length - 1];
                else
                    rAsset = rowsAsset[iAsset];

                if (rAsset == null)
                    dAsset1 = DateTime.Today.AddDays(1);
                else
                    dAsset1 = DataManager.ConvertToDate(rAsset[C_ColName_PublishDate]);

                if (iAsset >= 1 && iAsset < rowsAsset.Length)
                {
                    //前一条数据的公告日（报告日更晚的）
                    dAsset0 = DataManager.ConvertToDate(rowsAsset[iAsset - 1][C_ColName_PublishDate]);
                    if (dAsset1 >= dAsset0)
                    {
                        //丢弃过期数据
                        iAsset++;
                        continue;
                    }
                }

                //=======================
                //  合并数据
                //=======================
                if (rShare == null && rAsset == null)
                    break;

                if (dShare1 > dAsset1)
                {
                    if (iShare >= rowsShare.Length)
                    {
                        updateFundReport(null, rAsset, rpt, true);
                        iAsset++;
                    }
                    else
                        updateFundReport(rShare, rAsset, rpt, false);

                    iShare++;
                }
                else if (dShare1 < dAsset1)
                {
                    if (iAsset >= rowsAsset.Length)
                    {
                        updateFundReport(rShare, null, rpt, false);
                        iShare++;
                    }
                    else
                        updateFundReport(rShare, rAsset, rpt, true);

                    iAsset++;
                }
                else
                {
                    updateFundReport(rShare, rAsset, rpt, false);
                    iShare++;
                    iAsset++;
                }

                //=======================
                //  退出条件
                //=======================
                if (iShare >= rowsShare.Length && iAsset >= rowsAsset.Length)
                    break;
            }

            //交易日校验,复权
            rpt.Adjust();
        }
        #endregion
    }
}
