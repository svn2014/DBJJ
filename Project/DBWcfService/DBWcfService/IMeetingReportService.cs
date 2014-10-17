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
    public interface IMeetingReportService
    {
        [OperationContract]
        DataSet GetCategoryList();

        [OperationContract]
        DataSet GetContentByCategory(DateTime reportDate, int categoryId);

        [OperationContract]
        DataSet GetReportByDate(DateTime reportDate);

        [OperationContract]
        DataSet GetSearchResult(DateTime startDate, DateTime endDate, string keyword);

        [OperationContract]
        long SubmitReport(DateTime submitDate, int categoryId, string keywords, string content);

        [OperationContract]
        string GetReportTitle();
    }
}
