using System;
using System.Data;
using System.Collections.Generic;

namespace Security
{
    public partial class DataLoaderCH : ADataLoader
    {
        #region 常量
        protected const string C_ColName_Code = "SYMBOL";
        protected const string C_ColName_Name = "SNAME";
        protected const string C_ColName_Exchange = "Exchange";
        protected const string C_ColName_ListedDate = "LISTDATE";
        protected const string C_ColName_DelistedDate = "DELISTDATE";
        protected const string C_ColName_EndDate = "ENDDATE";
        protected const string C_ColName_TradeDate = "Tdate";
        protected const string C_ColName_PreClose = "LCLOSE";
        protected const string C_ColName_Open = "TOPEN";
        protected const string C_ColName_High = "HIGH";
        protected const string C_ColName_Low = "Low";
        protected const string C_ColName_Close = "TCLOSE";
        protected const string C_ColName_AdjustedClose = "Price2";
        protected const string C_ColName_Volume = "VOTURNOVER";
        protected const string C_ColName_Amount = "VATURNOVER";
        protected const string C_ColName_Average = "AVGPRICE";
        protected const string C_ColName_GSFundType = "GSFundType"; //银河基金分类
        protected const string C_ColName_PublishDate = "Publishdate";
        protected const string C_ColName_ReportDate = "ReportDate";
        protected const string C_ColName_DeclareDate = "DECLAREDATE";        
        protected const string C_ColName_UnitNAV = "NAV1";
        protected const string C_ColName_AccumUnitNAV = "NAV2";
        protected const string C_ColName_ParentCode = "Symbol_Comp";
        protected const string C_ColName_IndexName = "IName";
        protected const string C_ColName_IndexBaseDate = "Bdate";
        protected const string C_ColName_IndexStopDate = "Stopdate";
        protected const string C_ColName_IndexCategory = "Iprofile7";
        protected const string C_ColName_FundShare = "FShare2";
        protected const string C_ColName_FundNetAsset = "AssetAL1";
        protected const string C_ColName_FundEquityAsset = "AssetAL2";
        protected const string C_ColName_FundBondAsset = "AssetAL33";
        protected const string C_ColName_FundCashAsset = "AssetAL10";
        protected const string C_ColName_FundCBAsset = "AssetAL6";
        #endregion

        #region 基础方法
        public override void LoadTradingDate(ATimeSeries ts)
        {
            try
            {
                //数据已经存在
                if (_TradingDays == null || _TimeSeriesStart != ts.TimeSeriesStartExtended || _TimeSeriesEnd != ts.TimeSeriesEnd)
                {
                    _TimeSeriesStart = ts.TimeSeriesStartExtended;
                    _TimeSeriesEnd = ts.TimeSeriesEnd;

                    //读数据: 这里不需要TimeSeriesStartExtended
                    string sql = @"SELECT Tdate
                                FROM TRADEDATE 
                                WHERE Exchange = 'CNSESH' 
                                AND TDate>='" + _TimeSeriesStart.ToString("yyyyMMdd")
                               + @"' AND TDate<='" + _TimeSeriesEnd.ToString("yyyyMMdd") + "' ORDER BY TDate Desc";

                    _TradingDays = base.DBInstance.ExecuteSQL(sql);
                }

                //更新数据
                ts.TradingDates.Clear();

                if (_TradingDays == null || _TradingDays.Tables.Count == 0 || _TradingDays.Tables[0].Rows.Count == 0)
                    return;

                foreach (DataRow row in _TradingDays.Tables[0].Rows)
                {
                    DateTime tradedate = DataManager.ConvertToDate(row[C_ColName_TradeDate]);
                    ts.TradingDates.Add(tradedate);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 扩展方法
        public DataLoaderCH(ADBInstance instance)
        {
            base.DBInstance = instance;
            base.DataSource = "财汇";
        }
        private ExchangeType GetExchange(string strEx)
        {
            switch (strEx)
            {
                case "CNSESH":
                    return ExchangeType.SSE;
                case "CNSESZ":
                    return ExchangeType.SZSE;
                case "CNIBEX":
                    return ExchangeType.IBM;
                case "CNFF00":
                    return ExchangeType.IFE;
                default:
                    return ExchangeType.OTC;
            }
        }
        #endregion
    }
}
