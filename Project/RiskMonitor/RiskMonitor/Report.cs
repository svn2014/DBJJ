using System.Collections.Generic;
using System.Data;

namespace RiskMonitor
{
    public enum Status
    {
        //顺序不可以乱，后续要比较大小
        Pass,       //0
        Warning,    //1
        Fail        //2
    }

    public class MonitorItem
    {
        public long securityid = -1;
        public long order = 0;
        public Status status = Status.Pass;
        public string value;
        public string threshhold;
        public string title;
        public string description;
        public string code, name;
    }

    public class MonitorSummary
    {
        public string code, name;
        public long securityid = -1;
        public Status status = Status.Pass;
        public int WarningCounts = 0;
        public int FailCounts = 0;
        public List<MonitorItem> ItemList = new List<MonitorItem>();

        public void Add(MonitorSummary s)
        {
            foreach (MonitorItem i in s.ItemList)
            {
                this.Add(i);
            }
        }

        public void Add(MonitorItem i)
        {
            if (i == null || i.securityid < 0)
                return;

            if (this.securityid < 0)
            {
                this.securityid = i.securityid;
                this.code = i.code;
                this.name = i.name;
                this.ItemList.Add(i);
            }
            else
            {
                if (i.securityid == this.securityid)
                    this.ItemList.Add(i);
                else
                    return;
            }

            //更新状态
            if (i.status > this.status)
                this.status = i.status;

            switch (i.status)
            {
                case Status.Warning:
                    this.WarningCounts += 1;
                    break;
                case Status.Fail:
                    this.FailCounts += 1;
                    break;
                default:
                    break;
            }
        }
    }

    public class MonitorReport
    {
        public Status status = Status.Pass;
        public int WarningCounts = 0;
        public int FailCounts = 0;
        public List<MonitorSummary> SummaryList = new List<MonitorSummary>();

        public void Add(MonitorSummary s)
        {
            if (s == null || s.securityid < 0)
                return;

            //查找Summary是否存在
            MonitorSummary sfind = this.SummaryList.Find(delegate(MonitorSummary sd) { return sd.securityid == s.securityid; });
            if (sfind == null)
                this.SummaryList.Add(s);
            else
                sfind.Add(s);
        }

        public void Add(MonitorItem i)
        {
            if (i == null || i.securityid < 0)
                return;

            MonitorSummary s = new MonitorSummary();
            s.Add(i);
            this.Add(s);
        }

        public void RefreshStatus()
        {
            if (SummaryList.Count == 0)
                return;

            foreach (MonitorSummary s in this.SummaryList)
            {
                if (s.status > this.status)
                    this.status = s.status;

                this.WarningCounts += s.WarningCounts;
                this.FailCounts += s.FailCounts;
            }
        }


        #region 输出表
        public const string C_ColName_Order = "ORDER";
        public const string C_ColName_Code = "CODE";
        public const string C_ColName_Name = "NAME";
        public const string C_ColName_Title = "TITLE";
        public const string C_ColName_Status = "STATUS";
        public const string C_ColName_Description = "DESC";

        private DataTable _ReportTable;
        public DataTable GetReportTable()
        {
            //初始化
            this.initTable();

            //赋值
            this.RefreshStatus();
            foreach (MonitorSummary s in this.SummaryList)
            {
                foreach (MonitorItem item in s.ItemList)
                {
                    if (item.status == Status.Pass)
                        continue;

                    DataRow row = _ReportTable.NewRow();
                    row[C_ColName_Code] = item.code;
                    row[C_ColName_Name] = item.name;
                    row[C_ColName_Status] = item.status;
                    row[C_ColName_Description] = item.description;
                    row[C_ColName_Order] = item.order;
                    row[C_ColName_Title] = item.title;
                    _ReportTable.Rows.Add(row);
                }
            }

            //输出
            return _ReportTable;
        }
        private void initTable()
        {
            if (_ReportTable == null)
            {
                _ReportTable = new DataTable();
                _ReportTable.Columns.Add(new DataColumn(C_ColName_Code,typeof(string)));
                _ReportTable.Columns.Add(new DataColumn(C_ColName_Name, typeof(string)));
                _ReportTable.Columns.Add(new DataColumn(C_ColName_Title, typeof(string)));
                _ReportTable.Columns.Add(new DataColumn(C_ColName_Status, typeof(string)));
                _ReportTable.Columns.Add(new DataColumn(C_ColName_Description, typeof(string)));
                _ReportTable.Columns.Add(new DataColumn(C_ColName_Order, typeof(int)));
            }
        }
        #endregion
    }

}
