using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Security
{
    public partial class DataLoaderCH
    {
        #region 股票信息
        public override void LoadEquityInfo(Equity e)
        {
            try
            {
                if (_EquityInfo == null)
                {
                    string sql = @"SELECT SYMBOL, Exchange, SNAME, LISTDATE, ENDDATE
                            FROM SECURITYCODE A
                            WHERE STYPE = 'EQA' ";

                    //获得所有股票的信息做缓存
                    _EquityInfo = base.DBInstance.ExecuteSQL(sql);
                }

                //更新数据
                DataRow[] rows = _EquityInfo.Tables[0].Select("SYMBOL='" + e.Code + "'");
                if (rows.Length >= 1)
                {
                    DataRow row = rows[0];
                    e.Name = row[C_ColName_Name].ToString();
                    e.ListedDate = DataManager.ConvertToDate(row[C_ColName_ListedDate]);
                    e.DelistedDate = DataManager.ConvertToDate(row[C_ColName_EndDate]);
                    e.Exchange = this.GetExchange(row[C_ColName_Exchange].ToString());
                    e.DataSource = this.DataSource;
                }
                else
                {
                    MessageManager.GetInstance().AddMessage(MessageType.Warning, Message.C_Msg_EQ5, e.Code);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 股票价格
        private const string C_SQL_GetEquityPrice = @"Select A.Tdate, A.Symbol, A.Exchange, A.LCLOSE, A.TOPEN
                              ,A.HIGH, A.Low, A.TCLOSE, A.VOTURNOVER,A.VATURNOVER,A.AVGPRICE,A.Chg,A.Pchg,B.Price2                            
                              From SecurityCode S
                              INNER JOIN CHDQUOTE A
                                    ON S.Symbol = A.SYMBOL
                              Left join DERC_EQACHQUOTE_2 B
                                   On A.tdate = to_char(B.tdate,'yyyyMMdd') and A.Symbol=B.symbol and A.Exchange = B.Exchange
                              Where exchange in ('CNSESH','CNSESZ')
                              And Stype = 'EQA' ";

        public override void LoadEquityPrice(SeriesEquityPrice pxs)
        {
            try
            {
                //读数据
                string sql = C_SQL_GetEquityPrice;
                sql += " AND " + base.BuildSQLClauseIn(pxs.Code, "A.SYMBOL");
                sql += " AND A.Tdate >= '" + pxs.TimeSeriesStartExtended.ToString("yyyyMMdd") + "' AND A.Tdate <= '" + pxs.TimeSeriesEnd.ToString("yyyyMMdd") + "'";
                sql += " ORDER BY A.Symbol, Tdate Desc";
                DataSet ds = base.DBInstance.ExecuteSQL(sql);

                //更新数据
                this.updateEquityPrice(ds, pxs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override void LoadEquityPrice(ASecurityGroup g)
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
                string sql = C_SQL_GetEquityPrice;
                sql += " AND " + base.BuildSQLClauseIn(g.SecurityCodes, "A.SYMBOL");
                sql += " AND A.Tdate >= '" + g.TimeSeriesStartExtended.ToString("yyyyMMdd") + "' AND A.Tdate <= '" + g.TimeSeriesEnd.ToString("yyyyMMdd") + "'";
                sql += " ORDER BY A.Symbol, Tdate Desc";
                DataSet ds = base.DBInstance.ExecuteSQL(sql);

                //更新数据
                foreach (Equity e in g.SecurityHoldings)
                {
                    this.updateEquityPrice(ds, (SeriesEquityPrice)e.TradingPrice);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void updateEquityPrice(DataSet ds, SeriesEquityPrice pxs)
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
                MessageManager.GetInstance().AddMessage(MessageType.Warning, Message.C_Msg_EQ4, pxs.Code);
            }
        }
        #endregion
    }
}
