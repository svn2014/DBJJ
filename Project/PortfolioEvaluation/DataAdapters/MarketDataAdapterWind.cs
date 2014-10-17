using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace PortfolioEvaluation
{
    public class MarketDataAdapterWind:IMarketDataAdapter
    {
        public List<EquityPrice> GetPriceList(DataTable dtPriceInfo)
        {
            //==================================================================================================
            //Wind字段名
            //s_info_windcode, trade_dt, s_dq_close, s_dq_adjclose, s_dq_adjfactor, s_dq_volume(手), s_dq_amount(千元)
            //股票专有
            //s_dq_avgprice
            //==================================================================================================
            try
            {
                List<EquityPrice> priceList = new List<EquityPrice>();

                if (dtPriceInfo != null && dtPriceInfo.Rows.Count > 0)
                {
                    foreach (DataRow oRow in dtPriceInfo.Rows)
                    {
                        EquityPrice px = new EquityPrice();

                        foreach (DataColumn oCol in dtPriceInfo.Columns)
                        {
                            switch (oCol.ColumnName.ToLower())
                            {
                                case DBConsts.C_Mkt_ColumnName_WindCode:
                                    px.WindCode = oRow[oCol].ToString();
                                    break;
                                case DBConsts.C_Mkt_ColumnName_Trade_Date:
                                    string strDate = oRow[oCol].ToString();
                                    px.TradeDate = new DateTime(
                                        Convert.ToInt16(strDate.Substring(0, 4))
                                        , Convert.ToInt16(strDate.Substring(4, 2))
                                        , Convert.ToInt16(strDate.Substring(6, 2))
                                        );
                                    break;
                                case DBConsts.C_Mkt_ColumnName_PreClose:
                                    px.PreClose = Convert.ToDouble(oRow[oCol]);
                                    break;
                                case DBConsts.C_Mkt_ColumnName_Close:
                                    px.Close = Convert.ToDouble(oRow[oCol]);
                                    break;
                                case DBConsts.C_Mkt_ColumnName_Volume:
                                    px.Volume = Convert.ToDouble(oRow[oCol]) * 100; //手->股
                                    break;
                                case DBConsts.C_Mkt_ColumnName_Amount:
                                    px.Amount = Convert.ToDouble(oRow[oCol]) *1000;  //千元->元
                                    break;
                                case DBConsts.C_Mkt_ColumnName_AdjClose:
                                    if (oRow[oCol] != DBNull.Value)
                                        px.AdjustedClose = Convert.ToDouble(oRow[oCol]);
                                    break;
                                case DBConsts.C_Mkt_ColumnName_AdjFactor:
                                    if (oRow[oCol] != DBNull.Value)
                                        px.AdjustedFactor = Convert.ToDouble(oRow[oCol]);
                                    break;
                                case DBConsts.C_Mkt_ColumnName_AvgPrice:
                                    if (oRow[oCol] != DBNull.Value)
                                        px.Average = Convert.ToDouble(oRow[oCol]);
                                    break;
                                default:
                                    break;
                            }
                        }

                        px.AdjustedAverage = px.Average * px.AdjustedFactor;
                        px.AdjustedPreClose = px.Close * px.AdjustedFactor;
                        priceList.Add(px);
                    }
                }

                return priceList;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public List<AssetPosition> GetPositionList(DataTable dtPositionInfo)
        {
            //==================================================================================================
            //Wind字段名
            //trade_dt, s_con_windcode, i_weight,industriescode, industriesname
            //s_info_name,sw_ind_code1,sw_ind_name1, s_info_indexcode, s_share_freeshares(万股)
            //b_info_maturitydated
            //==================================================================================================

            try
            {
                string strDate = "";
                List<AssetPosition> positionList = new List<AssetPosition>();

                if (dtPositionInfo != null && dtPositionInfo.Rows.Count > 0)
                {
                    foreach (DataRow oRow in dtPositionInfo.Rows)
                    {
                        AssetPosition p = new AssetPosition();

                        foreach (DataColumn oCol in dtPositionInfo.Columns)
                        {
                            switch (oCol.ColumnName.ToLower())
                            {
                                case DBConsts.C_Mkt_ColumnName_Trade_Date:
                                    strDate = oRow[oCol].ToString();
                                    p.ReportDate = new DateTime(
                                          Convert.ToInt16(strDate.Substring(0, 4))
                                        , Convert.ToInt16(strDate.Substring(4, 2))
                                        , Convert.ToInt16(strDate.Substring(6, 2))
                                        );
                                    break;
                                    
                                case DBConsts.C_Mkt_ColumnName_ConWindCode:
                                case DBConsts.C_Mkt_ColumnName_WindCode:
                                    p.Code = oRow[oCol].ToString();
                                    p.WindCode = p.Code;
                                    break;

                                case DBConsts.C_Mkt_ColumnName_StockName:
                                    p.Name = oRow[oCol].ToString();
                                    break;

                                case DBConsts.C_Mkt_ColumnName_SWIndustryName:                                
                                case DBConsts.C_Mkt_ColumnName_IndustryName:
                                    p.SWIndustryName = oRow[oCol].ToString();
                                    break;

                                case DBConsts.C_Mkt_ColumnName_SWIndustryIndexCode:
                                    p.SWIndustryIndex = oRow[oCol].ToString();
                                    break;

                                case DBConsts.C_Mkt_ColumnName_ConWeight:
                                    p.Pctof_Portfolio = Convert.ToDouble(oRow[oCol]) / 100;  //%
                                    break;
                                case DBConsts.C_Mkt_ColumnName_FreeFloatCapital:
                                    p.FreefloatCapital = Convert.ToDouble(oRow[oCol]) * 10000;
                                    break;
                                case DBConsts.C_Mkt_ColumnName_MaturityDate:
                                    strDate = oRow[oCol].ToString();
                                    p.BondMaturityDate = new DateTime(
                                          Convert.ToInt16(strDate.Substring(0, 4))
                                        , Convert.ToInt16(strDate.Substring(4, 2))
                                        , Convert.ToInt16(strDate.Substring(6, 2))
                                        );
                                    break;
                                default:
                                    break;
                            }
                        }

                        positionList.Add(p);
                    }
                }

                return positionList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TradingDays> GetTradeingDays(DataTable dtTradingDays)
        {
            //==================================================================================================
            //Wind字段名
            //trade_days
            //==================================================================================================

            try
            {
                List<TradingDays> tradingDayList = new List<TradingDays>();

                if (dtTradingDays != null && dtTradingDays.Rows.Count > 0)
                {
                    foreach (DataRow oRow in dtTradingDays.Rows)
                    {
                        TradingDays d = new TradingDays();
                        d.Trade_Days = Convert.ToDateTime(oRow[DBConsts.C_Mkt_ColumnName_Trade_Days]);
                        tradingDayList.Add(d);
                    }
                }
                
                //按时间倒序排列
                tradingDayList.Sort();

                return tradingDayList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<EquityIndustry> GetIndustryList(DataTable dtIndustryInfo)
        {
            //==================================================================================================
            //Wind字段名
            //s_info_indexcode, industriescode, industriesname
            //==================================================================================================

            try
            {
                List<EquityIndustry> industryList = new List<EquityIndustry>();

                if (dtIndustryInfo != null && dtIndustryInfo.Rows.Count > 0)
                {
                    foreach (DataRow oRow in dtIndustryInfo.Rows)
                    {
                        EquityIndustry industry = new EquityIndustry();

                        foreach (DataColumn oCol in dtIndustryInfo.Columns)
                        {
                            switch (oCol.ColumnName.ToLower())
                            {
                                case DBConsts.C_Mkt_ColumnName_SWIndustryIndexCode:
                                    industry.SWIndustryIndex = oRow[oCol].ToString();
                                    break;
                                case DBConsts.C_Mkt_ColumnName_IndustryName:
                                    industry.SWIndustryName = oRow[oCol].ToString();
                                    break;
                                default:
                                    break;
                            }
                        }

                        industryList.Add(industry);
                    }
                }

                return industryList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
