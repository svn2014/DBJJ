using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataWorkCenterLib;
using MarketDataAdapter;

namespace DataWorkCenterApp
{
    class Program
    {
        public const string C_Para_WindCode = "WINDCODE";
        public const string C_Para_StartDate = "STARTDATE";
        public const string C_Para_EndDate = "ENDDATE";
        public const string C_Para_OnTestMode = "TESTMODE";

        static string _WindCode = "";
        static DateTime _StartDate = DateTime.Today.AddDays(-1);
        static DateTime _EndDate = DateTime.Today;
        static bool _OnTestMode = true;

        static void Main(string[] args)
        {
            Console.WriteLine("===Start: "+DateTime.Now +"===");
            
            InitialLastTradingDate();
            GetPara(args);

            Console.WriteLine(C_Para_WindCode + "\t" + _WindCode + "\t");
            Console.WriteLine(C_Para_StartDate + "\t" + _StartDate.ToString("yyyy-MM-dd") + "\t");
            Console.WriteLine(C_Para_EndDate + "\t\t" + _EndDate.ToString("yyyy-MM-dd") + "\t");
            Console.WriteLine(C_Para_OnTestMode + "\t" + _OnTestMode.ToString());

            Run(_WindCode, _StartDate, _EndDate, _OnTestMode);

            Console.WriteLine("===End: " + DateTime.Now + "===");
        }

        private static void GetPara(string[] args)
        {
            foreach (string arg in args)
            {
                try
                {
                    string[] items = arg.Split("=".ToCharArray());

                    if (items.Length != 2)
                        continue;

                    string key = items[0].Trim().ToUpper();
                    string value = items[1].Trim();

                    switch (key)
                    {
                        case C_Para_WindCode:    //WINDCODE=600000.SH
                            _WindCode = value.ToUpper();
                            break;

                        case C_Para_StartDate:   //STARTDATE=20120810
                            _StartDate = new DateTime(
                                            Convert.ToInt32(value.Substring(0,4)),
                                            Convert.ToInt32(value.Substring(4,2)),
                                            Convert.ToInt32(value.Substring(6,2))
                                            );
                            break;

                        case C_Para_EndDate:     //ENDDATE=20120810
                            _EndDate = new DateTime(
                                            Convert.ToInt32(value.Substring(0, 4)),
                                            Convert.ToInt32(value.Substring(4, 2)),
                                            Convert.ToInt32(value.Substring(6, 2))
                                            );
                            break;

                        case C_Para_OnTestMode:    //TESTMODE=1
                            if (value == "1")
                                _OnTestMode = true;
                            else
                                _OnTestMode = false;
                            break;

                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {                    
                    throw ex;
                }                
            }
        }

        private static void InitialLastTradingDate()
        {
            if (DateTime.Now.Hour > 18)
            {
                _StartDate = DateTime.Today;
            }
            else
            {
                _StartDate = DateTime.Today.AddDays(-1);
            }
            _EndDate = _StartDate;
        }

        private static void Run(string WindCode, DateTime StartDate, DateTime EndDate, bool OnTestMode)
        {
            string dataCenterName = "DataCenter";

            string datacenterDBType = System.Configuration.ConfigurationManager.ConnectionStrings[dataCenterName].ProviderName;
            string datacenterConnString = System.Configuration.ConfigurationManager.ConnectionStrings[dataCenterName].ConnectionString;

            DataLoaderProcessor proc = new DataLoaderProcessor();
            proc.OnTestMode = OnTestMode;
            proc.AddDataCenter(datacenterDBType, datacenterConnString);
            proc.BuildWorkerByDBConfig();
            proc.Run(WindCode, StartDate, EndDate);

            DataMessageManager.GetInstance().PrintAtOnce = true;
            DataMessageManager.GetInstance().Print();

            proc.Close();        
        }
    }
}
