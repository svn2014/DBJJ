using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace PortfolioEvaluation
{
    public class FundDataAdapterYSS : IFundDataAdapter
    {
        private List<GZBItem> _GZBList = new List<GZBItem>();
        //private List<string> _EquityCodeList = new List<string>();
        //private List<string> _BondCodeList = new List<string>();

        public List<GZBItem> BuildGZBList(DataTable dtGZB)
        {
            //==================================================================================================
            //字段名
            //FDate, FKmbm, FKmmc, FHqjg, FZqsl, FZqcb, FZqsz, Gz_zz, FSz_Jz_bl, FCb_Jz_bl
            //==================================================================================================

            try
            {
                _GZBList.Clear();

                if (dtGZB != null && dtGZB.Rows.Count > 0)
                {
                    foreach (DataRow oRow in dtGZB.Rows)
                    {
                        GZBItem gzb = new GZBItem();
                        string pct = "";

                        foreach (DataColumn oCol in dtGZB.Columns)
                        {
                            if (oRow[oCol.ColumnName] == DBNull.Value)
                                continue;

                            string colNameAdjusted = oCol.ColumnName.ToUpper();
                            switch (colNameAdjusted)
                            {
                                case DBConsts.C_GZB_ColName_FDate:
                                    gzb.ItemDate = Convert.ToDateTime(oRow[oCol.ColumnName]);
                                    break;

                                case DBConsts.C_GZB_ColName_FKmbm:
                                    gzb.ItemCode = oRow[oCol.ColumnName].ToString();
                                    //this.parseCode(gzb.ItemCode);
                                    break;

                                case DBConsts.C_GZB_ColName_FKmmc:
                                    gzb.ItemName = oRow[oCol.ColumnName].ToString();
                                    break;

                                case DBConsts.C_GZB_ColName_FZqsl:
                                    gzb.HoldingQuantity = Convert.ToDouble(oRow[oCol.ColumnName]);
                                    break;

                                case DBConsts.C_GZB_ColName_FZqcb:
                                    gzb.HoldingCost = Convert.ToDouble(oRow[oCol.ColumnName]);
                                    break;

                                case DBConsts.C_GZB_ColName_FZqsz:
                                    gzb.HoldingMarketValue = Convert.ToDouble(oRow[oCol.ColumnName]);
                                    break;

                                case DBConsts.C_GZB_ColName_FGz_zz:
                                    gzb.HoldingValueAdded = Convert.ToDouble(oRow[oCol.ColumnName]);
                                    break;

                                case DBConsts.C_GZB_ColName_FHqjg:
                                    gzb.CurrentPrice = Convert.ToDouble(oRow[oCol.ColumnName]);
                                    break;

                                case DBConsts.C_GZB_ColName_FCb_Jz_bl:
                                    //比例：原值="15.23%",新值="0.1523"                                    
                                    if (oRow[oCol.ColumnName] != DBNull.Value)
                                    {
                                        pct = oRow[oCol.ColumnName].ToString();
                                        pct = pct.Substring(0, pct.Length - 1);
                                        gzb.HoldingCostPctofNAV = Convert.ToDouble(pct) / 100;
                                    }
                                    break;

                                case DBConsts.C_GZB_ColName_FSz_Jz_bl:
                                    //比例：原值="15.23%",新值="0.1523"   
                                    if (oRow[oCol.ColumnName] != DBNull.Value)
                                    {
                                        pct = oRow[oCol.ColumnName].ToString();
                                        pct = pct.Substring(0, pct.Length - 1);
                                        gzb.HoldingMVPctofNAV = Convert.ToDouble(pct) / 100;
                                    }
                                    break;

                                default:
                                    break;
                            }
                        }

                        _GZBList.Add(gzb);
                    }
                }

                return _GZBList;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        //private void parseCode(string gzbKMBM)
        //{
        //    if (gzbKMBM.Length != 14)
        //        return;

        //    string secTypeCode = gzbKMBM.Substring(0, 4);
        //    string exchange = gzbKMBM.Substring(0, 8);
        //    string seccode = gzbKMBM.Substring(8, 6);
        //    string windAppendix = "";

        //    switch (exchange)
        //    {
        //        case DBConsts.C_GZB_KMBM_Bond_SH_CB:
        //        case DBConsts.C_GZB_KMBM_Bond_SH_Corp:
        //        case DBConsts.C_GZB_KMBM_Bond_SH_Govn:
        //        case DBConsts.C_GZB_KMBM_Stock_SH:
        //            windAppendix = ".SH";
        //            break;

        //        case DBConsts.C_GZB_KMBM_Bond_SZ_CB:
        //        case DBConsts.C_GZB_KMBM_Bond_SZ_Corp:
        //        case DBConsts.C_GZB_KMBM_Bond_SZ_Govn:
        //        case DBConsts.C_GZB_KMBM_Stock_SZ:
        //        case DBConsts.C_GZB_KMBM_Stock_CY:
        //            windAppendix = ".SZ";
        //            break;

        //        default:
        //            return;
        //    }

        //    switch (secTypeCode)
        //    {
        //        case DBConsts.C_GZB_KMBM_Bond:
        //            this._BondCodeList.Add(seccode + windAppendix);
        //            break;
        //        case DBConsts.C_GZB_KMBM_Stock:
        //            this._EquityCodeList.Add(seccode + windAppendix);
        //            break;
        //        default:
        //            break;
        //    }
        //}

        //public List<string> GetEquityHoldingList()
        //{
        //    return this._EquityCodeList;
        //}

        //public List<string> GetBondHoldingList()
        //{
        //    return this._BondCodeList;
        //}
    }
}
