using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Security
{
    public partial class DataLoaderCH
    {

        #region 指数信息
        public override void LoadIndexInfo(Index i)
        {
            try
            {
                if (_IndexInfo == null)
                {
                    string sql = @"SELECT Symbol, Iname, Ianame, Bdate, Bpoint, Stopdate, Iprofile6, Iprofile7, stocknum, Exchange
                                    FROM IPROFILE A
                                    WHERE Status = -1 AND currency='CNY'
                                    AND IPROFILE7 in ('申万一级行业指数', '中信一级行业指数','中证规模指数','申万风格指数',
                                        '巨潮规模指数','巨潮风格指数','巨潮行业指数','中证债券指数','上证综合指数','深证规模指数')";

                    //获得所有指数的信息做缓存
                    _IndexInfo = base.DBInstance.ExecuteSQL(sql);
                }

                //更新数据
                DataRow[] rows = _IndexInfo.Tables[0].Select("SYMBOL='" + i.Code + "'");
                if (rows.Length >= 1)
                {
                    DataRow row = rows[0];
                    i.Name = row[C_ColName_IndexName].ToString();
                    i.ListedDate = DataManager.ConvertToDate(row[C_ColName_IndexBaseDate]);
                    i.DelistedDate = DataManager.ConvertToDate(row[C_ColName_IndexStopDate]);
                    i.Category = row[C_ColName_IndexCategory].ToString();
                    i.Exchange = this.GetExchange(row[C_ColName_Exchange].ToString());
                    i.DataSource = this.DataSource;
                }
                else
                {
                    MessageManager.GetInstance().AddMessage(MessageType.Warning, Message.C_Msg_ID5, i.Code);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 指数价格
        private const string C_SQL_GetIndexPrice = @"SELECT * FROM CIHDQUOTE WHERE 1=1 ";
        public override void LoadIndexPrice(SeriesIndexPrice pxs)
        {
            try
            {
                //读数据
                string sql = C_SQL_GetIndexPrice;
                sql += " AND " + base.BuildSQLClauseIn(pxs.Code, "SYMBOL");
                sql += " AND Tdate >= '" + pxs.TimeSeriesStartExtended.ToString("yyyyMMdd") + "' AND Tdate <= '" + pxs.TimeSeriesEnd.ToString("yyyyMMdd") + "'";
                sql += " ORDER BY Symbol, Tdate Desc";
                DataSet ds = base.DBInstance.ExecuteSQL(sql);

                //更新数据
                this.updateIndexPrice(ds, pxs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override void LoadIndexPrice(ASecurityGroup g)
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
                string sql = C_SQL_GetIndexPrice;
                sql += " AND " + base.BuildSQLClauseIn(g.SecurityCodes, "SYMBOL");
                sql += " AND Tdate >= '" + g.TimeSeriesStartExtended.ToString("yyyyMMdd") + "' AND Tdate <= '" + g.TimeSeriesEnd.ToString("yyyyMMdd") + "'";
                sql += " ORDER BY Symbol, Tdate Desc";
                DataSet ds = base.DBInstance.ExecuteSQL(sql);

                //更新数据
                foreach (Index i in g.SecurityHoldings)
                {
                    this.updateIndexPrice(ds, (SeriesIndexPrice)i.TradingPrice);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void updateIndexPrice(DataSet ds, SeriesIndexPrice pxs)
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
                    px.AdjustCoefficient = 1;

                    //判断停牌
                    if (px.Close == 0)
                    {
                        px.IsTrading = false;
                        px.Close = px.PreClose;
                    }

                    pxs.OriginalTimeSeries.Add(px);
                }

                //复权并计算涨跌幅
                pxs.Adjust();
            }
            else
            {
                MessageManager.GetInstance().AddMessage(MessageType.Warning, Message.C_Msg_ID4, pxs.Code);
            }
        }
        #endregion        
    }
}
