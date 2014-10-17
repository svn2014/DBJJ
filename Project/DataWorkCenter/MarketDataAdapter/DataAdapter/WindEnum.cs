using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MarketDataAdapter
{
    public enum ExchangeType
    {
        NULL,
        SSE,    //上交所
        SZSE    //深交所
    }

    public enum ListedBoardType
    {
        NULL,
        ZB,     //主板
        ZXB,    //中小板
        CYB     //创业板
    }
    
    public enum TradingStatus
    {
        Trading,
        Suspend,
        ExRight,
        ExDividend,
        ExRightAndDividend
    }

    public enum FinancialStatementType
    {
        Merged,                 //合并报表
        MergedAdjusted,
        MergedUpdated,
        Parent,                 //母公司
        ParentAdjusted,
        ParentUpdated
    }

    public enum CompanyType
    { 
        NonFinancial=1,
        Bank=2,
        Insurance=3,
        Security=4
    }

    public class WindEnum
    {
        private static WindEnum _Instance = null;
        public static WindEnum GetInstance()
        {
            if (_Instance == null)
                _Instance = new WindEnum();

            return _Instance;
        }

        

        public WindEnum()
        {
            this.buildBoardTypeList();
            this.buildTradingStatusList();
            this.buildStatementTypeList();
        }

        private Hashtable _htEnumList = new Hashtable();

        private List<string> _BoardTypeList = new List<string>();
        private void buildBoardTypeList()
        {
            foreach (ListedBoardType t in Enum.GetValues(typeof(ListedBoardType)))
            {
                switch (t)
                {
                    case ListedBoardType.ZB:
                        _BoardTypeList.Add("434004000");
                        break;
                    case ListedBoardType.ZXB:
                        _BoardTypeList.Add("434003000");
                        break;
                    case ListedBoardType.CYB:
                        _BoardTypeList.Add("434001000");
                        break;

                    default:
                        _BoardTypeList.Add(null);
                        break;
                }
            }

            _htEnumList.Add(typeof(ListedBoardType).Name, _BoardTypeList);
        }

        private List<string> _TradingStatusList = new List<string>();
        private void buildTradingStatusList()
        {
            foreach (TradingStatus t in Enum.GetValues(typeof(TradingStatus)))
            {
                switch (t)
                {
                    case TradingStatus.Trading:
                        _TradingStatusList.Add(null);
                        break;
                    case TradingStatus.Suspend:
                        _TradingStatusList.Add("S");
                        break;
                    case TradingStatus.ExRight:
                        _TradingStatusList.Add("XR");
                        break;
                    case TradingStatus.ExDividend:
                        _TradingStatusList.Add("XD");
                        break;
                    case TradingStatus.ExRightAndDividend:
                        _TradingStatusList.Add("DR");
                        break;
                    default:
                        _TradingStatusList.Add(null);
                        break;
                }
            }

            _htEnumList.Add(typeof(TradingStatus).Name, _TradingStatusList);
        }

        private List<string> _StatementTypeList = new List<string>();
        private void buildStatementTypeList()
        {
            foreach (FinancialStatementType t in Enum.GetValues(typeof(FinancialStatementType)))
            {
                switch (t)
                {
                    case FinancialStatementType.Merged:     //合并报表
                        _StatementTypeList.Add("408001000");
                        break;

                    case FinancialStatementType.Parent:     //母公司报表
                        _StatementTypeList.Add("408006000");
                        break;

                    default:
                        _StatementTypeList.Add(null);
                        break;
                }
            }

            _htEnumList.Add(typeof(FinancialStatementType).Name, _StatementTypeList);
        }


        public string GetEnumTypeValue<T>(int enumValue)
        {
            try
            {
                if (!_htEnumList.Contains(typeof(T).Name))
                    return null;

                List<string> lst = (List<string>)_htEnumList[typeof(T).Name];

                return lst[enumValue];
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
