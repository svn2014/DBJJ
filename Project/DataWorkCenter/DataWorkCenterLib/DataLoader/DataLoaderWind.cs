using System;
using System.Collections.Generic;
using MarketDataAdapter;
using System.Data;

namespace DataWorkCenterLib
{
    public class DataLoaderWind: ADataLoader
    {
        
        public DataLoaderWind(ADBInstance dbInstance)
        {
            base._DBInstance = dbInstance;
            base.CurrentVandor = VandorType.Wind;
            base._MarketDataAdapter = MarketDataAdapterFactory.GetAdapter(VandorType.Wind);
        }

        public override List<AShareDescription> GetAShareDescription()
        {
            try
            {
                string sql = @"Select * 
                                From asharedescription 
                                Where 1=1 ";

                if (this._WindCode != null && this._WindCode.Trim().Length > 0)
                    sql += "And s_info_windcode = '" + this._WindCode.ToUpper().Trim() + "'";

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
                string sql = @"Select * 
                                From ashareeodprices 
                                Where trade_dt ='" + this._LoopingDate.ToString("yyyyMMdd") + "'";

                if (this._WindCode != null && this._WindCode.Trim().Length > 0)
                    sql += "And s_info_windcode = '" + this._WindCode.ToUpper().Trim() + "'";

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
                                From asharecalendar 
                                Where s_info_exchmarket = 'SSE' ";

                if (this._StartDate > LibConsts.C_Null_Date)
                    sql += "And trade_days >= '" + this._StartDate.ToString("yyyyMMdd") + "'";
                if (this._EndDate > LibConsts.C_Null_Date)
                    sql += "And trade_days <= '" + this._EndDate.ToString("yyyyMMdd") + "'";

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
                string sql = @"Select *
                                From AShareBalanceSheet
                                Where Statement_Type in ('408001000','408005000')
                                And Ann_Dt = '" + annDate.ToString("yyyyMMdd") + "' ";

                if (this._WindCode != null && this._WindCode.Length > 0)
                    sql += "And s_info_windcode = '" + this._WindCode + "'";

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
                string sql = @"Select *
                                From AShareIncome
                                Where Statement_Type ('408001000','408005000')
                                And Ann_Dt = '" + annDate.ToString("yyyyMMdd") + "' ";

                if (this._WindCode != null && this._WindCode.Length > 0)
                    sql += "And s_info_windcode = '" + this._WindCode + "'";

                return base.ReadDbData<AShareIncome>(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
