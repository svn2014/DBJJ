using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SASObjectManager;
using SAS;
using System.Data;
using System.Data.OleDb;

namespace SASDataHelper
{
    public class SASWorker
    {
        private string _SASWorkerId = "ID" + DateTime.Now.ToString("yyyyMMddhhmmss");
        private ObjectFactory _ObjectFactory;
        private Workspace _SASWorkSpace;
        private string _OleDbConnString = "provider=sas.iomprovider.1; SAS Workspace ID=<ID>";

        public SASWorker()
        {
            try
            {
                _ObjectFactory = new ObjectFactory();
                _SASWorkSpace = _ObjectFactory.CreateObjectByServer(_SASWorkerId, true, null, "", "");//Local
                _OleDbConnString = _OleDbConnString.Replace("<ID>", _SASWorkSpace.UniqueIdentifier);
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        public void Execute(string sascode)
        {
            try
            {
                _SASWorkSpace.LanguageService.Submit(sascode);
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        public void Execute(string sascode, System.Data.DataSet dataset)
        {
            try
            {
                //Save all datatables to fileSystem
                foreach (DataTable dt in dataset.Tables)
                {
                    string code = BuildSASDataset(dt);
                    _SASWorkSpace.LanguageService.Submit(code);//code for creating dataset
                }

                //All tables are used in sascode
                _SASWorkSpace.LanguageService.Submit(sascode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Execute(string sascode, DataTable datatable)
        {
            try
            {
                System.Data.DataSet ds = new System.Data.DataSet();
                ds.Tables.Add(datatable);
                Execute(sascode, ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string BuildSASDataset(DataTable dt)
        {
            string tableName = dt.TableName;
            if (tableName.Trim().Length == 0)
                tableName = "_NULL_";

            //Columns
            string input = "Input ";
            foreach (DataColumn oCol in dt.Columns)
            {
                input += oCol.ColumnName + " ";
            }
            input += ";";

            //Data Rows
            string datalines = "datalines;\r\n";
            foreach (DataRow oRow in dt.Rows)
            {
                foreach (DataColumn oCol in dt.Columns)
                {
                    datalines += oRow[oCol.ColumnName].ToString() + " ";
                }
                datalines += "\r\n";
            }
            datalines += ";";

            //code for dataset
            string sascode = "Data " + tableName + ";" + "\r\n";
            sascode += input + "\r\n";
            sascode += datalines + "\r\n";
            sascode += "Run;";

            return sascode;
        }


        public DataTable GetResult(string tableName)
        {
            if (tableName.Trim().Length == 0)
                return null;

            ObjectKeeper objKeeper = new ObjectKeeper();
            objKeeper.AddObject(0, _SASWorkSpace.Name, _SASWorkSpace);

            string sql = "select * from " + tableName;

            DataTable oDT = new DataTable();
            OleDbDataAdapter obAdapter = new OleDbDataAdapter(sql, _OleDbConnString);
            obAdapter.Fill(oDT);

            objKeeper.RemoveObject(_SASWorkSpace);

            return oDT;
        }

        public void Dispose()
        {
            try
            {
                if (_SASWorkSpace != null)
                    _SASWorkSpace.Close();
            }
            catch (Exception ex)
            {                
                throw ex;
            }            
        }
    }
}
