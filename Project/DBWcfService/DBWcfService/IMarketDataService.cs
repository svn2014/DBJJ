using System;
using System.Data;
using System.ServiceModel;
using System.Collections.Generic;

namespace DBWcfService
{
    [ServiceContract]
    public interface IMarketDataService
    {
        [OperationContract]
        DataSet GetHS300Weight(DateTime endDate);

        [OperationContract]
        DataSet GetIndexPrices(DateTime startDate, DateTime endDate, string code);

        [OperationContract]
        DataSet GetStockPrices(DateTime startDate, DateTime endDate, string code);

        [OperationContract]
        DataSet GetAShareStockList(string code, string exchange, string charIsST, string charIsActive);

        [OperationContract]
        DataSet GetChinaBondList(string code, string exchange);

        [OperationContract]
        DataSet GetSWIndustryList();

        [OperationContract]
        DataSet GetFreeFloatCapital(DateTime endDate, string code);

        [OperationContract]
        DataSet GetTradingDays(DateTime startDate, DateTime endDate);
    }
}
