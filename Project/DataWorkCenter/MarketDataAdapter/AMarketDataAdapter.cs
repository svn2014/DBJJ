using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MarketDataAdapter
{
    public abstract partial class AMarketDataAdapter
    {
        protected DateTime NullDate = new DateTime(1900, 1, 1);
        
        public abstract List<AShareDescription> GetAShareDescription(DataTable dtDataSrc);
        public abstract List<AShareEODPrices> GetAShareEODPrices(DataTable dtDataSrc);
        public abstract List<AShareCalendar> GetAShareCalendar(DataTable dtDataSrc);
        public abstract List<AShareBalanceSheet> GetAShareBalanceSheet(DataTable dtDataSrc);
        public abstract List<AShareIncome> GetAShareIncome(DataTable dtDataSrc);

        protected virtual string getWindCode(string code, ExchangeType exchange)
        {
            switch (exchange)
            {
                case ExchangeType.NULL:
                    return code;

                case ExchangeType.SSE:
                    return code + ".SH";

                case ExchangeType.SZSE:
                    return code + ".SZ";

                default:
                    return code;
            }
        }
        protected abstract ExchangeType getExchangeType(string exchange);
        protected abstract ListedBoardType getListedBoardType(string listedBorad);
        protected abstract FinancialStatementType getStatementType(string statementType);
        protected abstract CompanyType getCompanyType(string companyType);


        //辅助功能
        protected double? getDoubleValue(object data)
        {
            if (data == DBNull.Value || data == null)
                return null;
            else
                return Convert.ToDouble(data);
        }
        protected DateTime? getDateTimeValue(object data)
        {
            try
            {
                DateTime outputDate = Convert.ToDateTime(data);

                if (outputDate == NullDate)
                    return null;
                else
                    return outputDate;
            }
            catch (Exception)
            {
                return null;
            }            
        }
        protected DateTime? getDateTimeValueBy8Digits(object data)
        {
            string strDate = data.ToString();

            if (strDate.Length == 8)
            {
                DateTime outputDate = new DateTime(
                    Convert.ToInt16(strDate.Substring(0, 4)),
                    Convert.ToInt16(strDate.Substring(4, 2)),
                    Convert.ToInt16(strDate.Substring(6, 2))
                    );

                if (outputDate == NullDate)
                    return null;
                else
                    return outputDate;
            }
            else if (strDate.Length == 0)
                return null;
            else
                return getDateTimeValue(strDate);
        }

        
    }
}
