using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;

namespace DBWcfService
{    
    [ServiceContract]
    public interface ICHService
    {
        [OperationContract]
        DataSet ExecuteSQL(string sql);

        [OperationContract]
        DataSet GetTradingDays(DateTime startDate, DateTime endDate);

        [OperationContract]
        DataSet GetStockPrices(DateTime startDate, DateTime endDate, string code);

        [OperationContract]
        DataSet GetIndexPrices(DateTime startDate, DateTime endDate, string code);

        [OperationContract]
        DataSet GetAShareStockList(string code, string exchange, string charIsST, string charIsActive);

        [OperationContract]
        DataSet GetIndexWeights(string code, DateTime monthEndDate);

        [OperationContract]
        DataSet GetSWIndustryList();

        [OperationContract]
        DataSet GetFreeFloatCapital(DateTime endDate, string code);

        [OperationContract]
        DataSet GetChinaBondList(string code, string exchange);

        [OperationContract]
        DataSet GetChinaBondsList(string codeList);

        [OperationContract]
        DataSet GetIPOSEO(DateTime startDate, DateTime endDate, string code);   //首发增发

        [OperationContract]
        DataSet GetBlockTrade(DateTime startDate, DateTime endDate, string code);   //大宗交易

        [OperationContract]
        DataSet GetMarginTrade(DateTime startDate, DateTime endDate, string code);  //融资融券

        [OperationContract]
        DataSet GetStrangeTrade(DateTime startDate, DateTime endDate, string code);  //异常交易

        [OperationContract]
        DataSet GetRestrictStock(DateTime startDate, DateTime endDate, string code);  //限售解禁

        [OperationContract]
        DataSet GetMarketNews(DateTime startDate, DateTime endDate, string code);   //新闻资讯

        [OperationContract]
        DataSet GetMarketNewsContent(string newsId);   //新闻资讯内容
    }
}
