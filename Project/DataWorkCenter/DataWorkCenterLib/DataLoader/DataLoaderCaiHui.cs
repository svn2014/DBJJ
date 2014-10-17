using System;
using System.Collections.Generic;
using MarketDataAdapter;
using System.Data;

namespace DataWorkCenterLib
{
    public class DataLoaderCaiHui: ADataLoader
    {
        public DataLoaderCaiHui(ADBInstance dbInstance)
        {
            base._DBInstance = dbInstance;
            base.CurrentVandor = VandorType.CaiHui;
            base._MarketDataAdapter = MarketDataAdapterFactory.GetAdapter(VandorType.CaiHui);
        }

        public override List<AShareDescription> GetAShareDescription()
        {
            try
            {
                string sql = @"Select * 
                                From SecurityCode 
                                Where exchange in ('CNSESH','CNSESZ') 
                                And stype = 'EQA' ";

                if (this._WindCode != null && this._WindCode.Trim().Length > 0)
                    sql += "And symbol = '" + this._WindCode.Substring(0, 6).ToUpper() + "'";

                return base.ReadDbData<AShareDescription>(sql);
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
                string sql = @"Select A.*, B.*
                                From SecurityCode S
                                INNER JOIN CHDQUOTE A
                                      ON S.Symbol = A.SYMBOL
                                Left join DERC_EQACHQUOTE_2 B
                                     On A.tdate = to_char(B.tdate,'yyyyMMdd') and A.Symbol=B.symbol and A.Exchange = B.Exchange
                                Where exchange in ('CNSESH','CNSESZ') 
                                And Stype = 'EQA'
                                And A.tdate = '" + this._LoopingDate.ToString("yyyyMMdd") + "'";

                if (this._WindCode != null && this._WindCode.Trim().Length > 0)
                    sql += "And A.symbol = '" + this._WindCode.Substring(0, 6).ToUpper() + "'";

                return base.ReadDbData<AShareEODPrices>(sql);
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
                string sql = @"Select * 
                                From TRADEDATE 
                                Where exchange = 'CNSESH' ";

                if (this._StartDate > LibConsts.C_Null_Date)
                    sql += "And TDate >= '" + this._StartDate.ToString("yyyyMMdd") + "'";
                if (this._EndDate > LibConsts.C_Null_Date)
                    sql += "And TDate <= '" + this._EndDate.ToString("yyyyMMdd") + "'";

                return base.ReadDbData<AShareCalendar>(sql);
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
                string sql = @"Select A.Symbol, A.Exchange, A.Sname, P.ITPROFILE4, B.*
                                From SecurityCode A
                                LEFT JOIN CBSHEET_NEW B
                                     ON A.Companycode = B.Companycode
                                LEFT JOIN ITProfile P
                                     ON A.COMPANYCODE = P.COMPANYCODE
                                WHERE B.ReportStyle in ('11','12')
                                AND A.Stype = 'EQA'
                                AND Publishdate = " + base._DBInstance.ConvertSqlDate(annDate);

                if (this._WindCode != null && this._WindCode.Length > 0)
                    sql += "AND Symbol = '" + this._WindCode.Substring(0, 6) + "'";

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
                string sql = @"Select A.Symbol, A.Exchange, A.Sname, P.ITPROFILE4,C.MFRatio_R3, B.*
                                From SecurityCode A
                                LEFT JOIN CINST_new B
                                     ON A.Companycode = B.Companycode
                                LEFT JOIN MFRATIO_R C
                                     ON A.Companycode = C.Companycode AND B.Reportdate = C.Reportdate
                                LEFT JOIN ITProfile P
                                     ON A.COMPANYCODE = P.COMPANYCODE
                                WHERE B.ReportStyle in ('11','12')
                                AND A.Stype = 'EQA'
                                AND Publishdate = " + base._DBInstance.ConvertSqlDate(annDate);

                if (this._WindCode != null && this._WindCode.Length > 0)
                    sql += "AND Symbol = '" + this._WindCode.Substring(0, 6) + "'";

                return base.ReadDbData<AShareIncome>(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
