using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarketDataAdapter;

namespace DataWorkCenterLib
{
    public class DataLoaderJuYuan : ADataLoader
    {
        public DataLoaderJuYuan(ADBInstance dbInstance)
        {
            base._DBInstance = dbInstance;
            base.CurrentVandor = VandorType.JuYuan;
            base._MarketDataAdapter = MarketDataAdapterFactory.GetAdapter(VandorType.JuYuan);
        }

        public override List<AShareDescription> GetAShareDescription()
        {
            try
            {
                string sql = @"SELECT * 
                                FROM SecuMain
                                WHERE Secucategory = 1
                                AND secumarket in (83,90)
                                AND listedsector in (1,2,6)
                                AND listedstate in (1,3,5) ";

                if (this._WindCode != null && this._WindCode.Trim().Length > 0)
                    sql += "And secucode = '" + this._WindCode.Substring(0, 6).ToUpper() + "'";

                return base.ReadDbData<AShareDescription>(sql);
            }
            catch (Exception ex)
            {                
                throw ex;
            }            
        }

        public override List<AShareCalendar> GetAShareCalendar()
        {
            try
            {
                string sql = @"Select tradingdate, secumarket
                                From QT_TradingDayNew
                                WHERE SecuMarket = 83
                                AND IfTradingDay = 1 ";

                if (this._StartDate > LibConsts.C_Null_Date)
                    sql += "And tradingdate >= " + base._DBInstance.ConvertSqlDate(this._StartDate);
                if (this._EndDate > LibConsts.C_Null_Date)
                    sql += "And tradingdate <= " + base._DBInstance.ConvertSqlDate(this._EndDate);

                return base.ReadDbData<AShareCalendar>(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override List<AShareEODPrices> GetAShareEODPrices()
        {
            try
            {
                string sql = @"Select A.Secucode, A.Secuabbr, A.Secumarket, B.*
                                From SecuMain A
                                INNER JOIN Qt_DailyQuote B
                                        ON A.Innercode = B.Innercode
                                Where A.Secucategory = 1
                                AND A.Secumarket in (83,90)
                                AND A.listedsector in (1,2,6)
                                AND A.listedstate in (1,3,5)
                                And B.TRADINGDAY = " + base._DBInstance.ConvertSqlDate(this._LoopingDate);

                if (this._WindCode != null && this._WindCode.Trim().Length > 0)
                    sql += "And A.Secucode = '" + this._WindCode.Substring(0, 6).ToUpper() + "'";

                return base.ReadDbData<AShareEODPrices>(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public override List<AShareBalanceSheet> GetAShareBalanceSheet()
        {
            try
            {
                DateTime annDate = (this._LoopingDate == LibConsts.C_Null_Date) ? this._EndDate : this._LoopingDate;
                string sql = @"Select A.Secucode, A.Secuabbr,A.Secumarket, B.* 
                                From SecuMain A
                                INNER Join LC_BalanceSheetAll B 
                                     On A.CompanyCode = B.CompanyCode
                                Where A.Secucategory = 1
                                AND A.Secumarket in (83,90)
                                AND A.listedsector in (1,2,6)
                                AND A.listedstate in (1,3,5)
                                And B.IfMerged=1
                                And B.InfoPublDate = " + base._DBInstance.ConvertSqlDate(annDate);

                if (this._WindCode != null && this._WindCode.Length > 0)
                    sql += "And Secucode = '" + this._WindCode + "'";

                return base.ReadDbData<AShareBalanceSheet>(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override List<AShareIncome> GetAShareIncome()
        {
            try
            {
                DateTime annDate = (this._LoopingDate == LibConsts.C_Null_Date) ? this._EndDate : this._LoopingDate;
                string sql = @"SELECT A.Secucode,A.Secuabbr,A.Secumarket,B.*
                                From SecuMain A
                                INNER Join LC_IncomeStatementAll B 
                                      On A.CompanyCode = B.CompanyCode
                                INNER Join LC_MainIndexNew C 
                                      On A.CompanyCode = C.CompanyCode And B.EndDate = C.EndDate
                                Where A.SecuCategory=1
                                And B.IfMerged=1
                                And B.InfoPublDate = " + base._DBInstance.ConvertSqlDate(annDate);

                if (this._WindCode != null && this._WindCode.Length > 0)
                    sql += "And Secucode = '" + this._WindCode + "'";

                return base.ReadDbData<AShareIncome>(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
