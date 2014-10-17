using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ReportCenter.GZBService;
using ReportCenter.MDService;

namespace ReportCenter
{
    public class DataService
    {
        private static DataService _Instance;
        public static DataService GetInstance()
        {
            if (_Instance == null)
                _Instance = new DataService();

            return _Instance;
        }

        public DataTable GetGZB(DateTime startDate, DateTime endDate, string fundCode, string itemCode)
        {
            DBServiceClient client = new DBServiceClient();
            ProcedureParameter[] paraList = new ProcedureParameter[5];
            
            ProcedureParameter para0 =  new ProcedureParameter();
            para0.Name = "o_cursor";
            para0.Direction = ParameterDirection.Output;
            para0.Type = ProcedureParameter.DBType.Cursor;
            paraList[0] = para0;

            ProcedureParameter para1 = new ProcedureParameter();
            para1.Name = "i_fundcode"; para1.Value = fundCode;
            para1.Direction = ParameterDirection.Input;
            para1.Type = ProcedureParameter.DBType.NVarChar;
            paraList[1] = para1;

            ProcedureParameter para2 = new ProcedureParameter();
            para2.Name = "i_startdate"; para2.Value = startDate.ToString("yyyyMMdd");
            para2.Direction = ParameterDirection.Input;
            para2.Type = ProcedureParameter.DBType.NVarChar;
            paraList[2] = para2;

            ProcedureParameter para3 = new ProcedureParameter();
            para3.Name = "i_enddate"; para3.Value = endDate.ToString("yyyyMMdd");
            para3.Direction = ParameterDirection.Input;
            para3.Type = ProcedureParameter.DBType.NVarChar;
            paraList[3] = para3;

            ProcedureParameter para4 = new ProcedureParameter();
            para4.Name = "i_itemcode"; para4.Value = itemCode;
            para4.Direction = ParameterDirection.Input;
            para4.Type = ProcedureParameter.DBType.NVarChar;
            paraList[4] = para4;

            return client.ExecuteStoredProcedure("db5_proc_selectgzb", paraList).Tables[0];
        }

        public DataTable GetHS300Weight(DateTime endDate)
        {
            MarketDataServiceClient client = new MarketDataServiceClient();
            return client.GetHS300Weight(endDate).Tables[0];
        }

        public DataTable GetIndexPrices(DateTime startDate, DateTime endDate, string code)
        {
            MarketDataServiceClient client = new MarketDataServiceClient();
            return client.GetIndexPrices(startDate, endDate, code).Tables[0];
        }

        public DataTable GetStockPrices(DateTime startDate, DateTime endDate)
        {
            MarketDataServiceClient client = new MarketDataServiceClient();
            DataSet dsPrices;
            DataTable dtOutput = null;

            int i = 0, pageSize = 20;
            while (startDate.AddDays(pageSize * i) <= endDate)
            {
                DateTime start, end;
                start = (startDate.AddDays(i * pageSize));
                end = (startDate.AddDays(pageSize * (i + 1) - 1) > endDate) ? endDate : startDate.AddDays(pageSize * (i + 1) - 1);
                dsPrices = client.GetStockPrices(start, end, null);

                if (dtOutput == null)
                    dtOutput = dsPrices.Tables[0].Copy();
                else
                {
                    foreach (DataRow oRow in dsPrices.Tables[0].Rows)
                    {
                        dtOutput.ImportRow(oRow);
                    }
                }

                i++;
            }

            return dtOutput;
        }

        //public DataTable GetStockPrices(DateTime startDate, DateTime endDate, List<string> codeList)
        //{
        //    try
        //    {
        //        MarketDataServiceClient client = new MarketDataServiceClient();


        //        //TODO: 每次取20天的数据
        //        DataSet dsPrices;
        //        DataTable dtOutput = null;

        //        if (codeList != null && codeList.Count > 0)
        //        {
        //            //check duplication
        //            codeList.Sort();

        //            int i = 1;
        //            while (i < codeList.Count)
        //            {
        //                if (codeList[i] == codeList[i - 1])
        //                    codeList.RemoveAt(i);
        //                else
        //                    i++;
        //            }

        //            //get data
        //            foreach (string code in codeList)
        //            {
        //                dsPrices = client.GetStockPrices(startDate, endDate, code);

        //                if (dtOutput == null)
        //                {
        //                    dtOutput = dsPrices.Tables[0].Copy();
        //                }
        //                else
        //                {
        //                    foreach (DataRow oRow in dsPrices.Tables[0].Rows)
        //                    {
        //                        dtOutput.ImportRow(oRow);
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            dsPrices = client.GetStockPrices(startDate, endDate, null);
        //            if (dsPrices.Tables.Count > 0)
        //                dtOutput = dsPrices.Tables[0].Copy();
        //        }

        //        return dtOutput;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public DataTable GetStockPrices(DateTime startDate, DateTime endDate, List<string> codeList)
        //{
        //    MarketDataServiceClient client = new MarketDataServiceClient();
        //    DataSet ds;

        //    if (codeList == null || codeList.Count == 0)
        //        ds = client.GetStockPricesByCodeList(startDate, endDate, null);
        //    else
        //    {
        //        //check duplication
        //        codeList.Sort();
                
        //        int i = 1;
        //        while(i<codeList.Count)
        //        {
        //            if (codeList[i] == codeList[i - 1])
        //                codeList.RemoveAt(i);
        //            else
        //                i++;
        //        }


        //        //get data
        //        ds = client.GetStockPricesByCodeList(startDate, endDate, codeList.ToArray());
        //    }

        //    if (ds.Tables.Count > 0)
        //        return ds.Tables[0];
        //    else
        //        return null;
        //}

        public DataTable GetAShareStockList(string code)
        {
            MarketDataServiceClient client = new MarketDataServiceClient();
            return client.GetAShareStockList(code, null, null, "1").Tables[0];
        }

        public DataTable GetFreeFloatCapital(DateTime endDate, string code)
        {
            MarketDataServiceClient client = new MarketDataServiceClient();
            return client.GetFreeFloatCapital(endDate, code).Tables[0];
        }

        public DataTable GetSWIndustryList()
        {
            MarketDataServiceClient client = new MarketDataServiceClient();
            return client.GetSWIndustryList().Tables[0];
        }

        public DataTable GetTradingDaysList(DateTime start, DateTime end)
        {
            MarketDataServiceClient client = new MarketDataServiceClient();
            return client.GetTradingDays(start, end).Tables[0];
        }
    }
}
