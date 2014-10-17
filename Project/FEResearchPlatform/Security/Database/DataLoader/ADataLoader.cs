using System;
using System.Collections.Generic;
using System.Data;

namespace Security
{
    public abstract class ADataLoader
    {
        protected ADBInstance DBInstance;
        protected string DataSource = "未知数据源";

        #region 数据缓存
        protected static DataSet _EquityInfo;
        protected static DataSet _FundInfo;
        protected static DataSet _IndexInfo;
        protected static DataSet _TradingDays;
        protected static DateTime _TimeSeriesStart, _TimeSeriesEnd;
        #endregion

        #region 辅助方法
        public abstract void LoadTradingDate(ATimeSeries ts);
        #endregion

        #region 股票方法
        public abstract void LoadEquityInfo(Equity e);
        public abstract void LoadEquityPrice(SeriesEquityPrice pxs);
        public abstract void LoadEquityPrice(ASecurityGroup g);
        #endregion

        #region 基金方法
        public abstract MutualFundGroup GetMutualFunds(AFundCategory category);
        public abstract void LoadMutualFundInfo();
        public abstract void LoadMutualFundInfo(MutualFund f);
        public abstract void LoadMutualFundNAV(SeriesNetAssetValue navs);
        public abstract void LoadMutualFundNAV(ASecurityGroup g);
        public abstract void LoadMutualFundPrice(SeriesFundPrice pxs);
        public abstract void LoadMutualFundPrice(ASecurityGroup g);
        public abstract void LoadMutualFundReport(SeriesFundReport r);
        public abstract void LoadMutualFundReport(ASecurityGroup g);
        #endregion

        #region 指数方法
        public abstract void LoadIndexInfo(Index i);
        public abstract void LoadIndexPrice(SeriesIndexPrice pxs);
        public abstract void LoadIndexPrice(ASecurityGroup g);
        #endregion

        #region 通用方法
        protected string BuildSQLClauseIn(string code, string inFieldName)
        {
            List<string> codeList = new List<string>();
            codeList.Add(code);
            return BuildSQLClauseIn(codeList, inFieldName);
        }
        protected string BuildSQLClauseIn(List<string> codelist, string inFieldName)
        {
            string codestring="";
            if (codelist != null && codelist.Count > 0)
            {
                codestring = inFieldName + " IN (";
                foreach (string code in codelist)
                {
                    codestring += "'" + code + "',";
                }
                codestring = codestring.Substring(0, codestring.Length - 1) + ")";
            }

            return codestring;
        }
        #endregion
    }
}
