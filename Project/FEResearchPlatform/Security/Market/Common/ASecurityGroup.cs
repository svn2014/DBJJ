using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Security
{
    public abstract class ASecurityGroup: ASecurity
    {
        #region 基础方法
        public ASecurityGroup() : base("") { }
        public ASecurityGroup(Type type) : base("") { this.SecurityClass = type; }
        public ASecurityGroup(string code) : base(code) { }
        public ASecurityGroup(string code, DateTime start, DateTime end) : base(code, start, end) { }
        protected override void BuildSecurity()
        {
            base.Exchange = ExchangeType.OTC;
            base.Type = SecurityType.Group;            
        }
        protected override void BuildTimeSeries(string code, DateTime start, DateTime end)
        {
            //调整持仓个股的时间区间
            if (this.SecurityHoldings != null && this.SecurityHoldings.Count > 0)
            {
                foreach (ASecurity s in this.SecurityHoldings)
                {
                    s.SetDatePeriod(start, end);
                }
            }
        }
        public override void DebugPrint(DataInfoType type, bool showheader)
        {
            //if (this.SecurityHoldings != null && this.SecurityHoldings.Count > 0)
            //{
            //    if (showheader)
            //    {
            //        Debug.Write("日期\t");
            //        for (int j = 0; j < this.SecurityHoldings.Count; j++)
            //        {
            //            Debug.Write(this.SecurityHoldings[j].Code + "\t");
            //        }
            //        Debug.WriteLine("");
            //    }

            //    int totalcount = 0;
            //    int maxlengthindex = 0;
            //    for (int k = 0; k < this.SecurityHoldings.Count; k++)
            //    {
            //        if (((MutualFund)this.SecurityHoldings[k]).TradingNAV.TradingDates.Count > totalcount)
            //        {
            //            totalcount = ((MutualFund)this.SecurityHoldings[k]).TradingNAV.TradingDates.Count;
            //            maxlengthindex = k;
            //        }
            //    }

            //    for (int i = 0; i < totalcount; i++)
            //    {
            //        Debug.Write(((MutualFund)this.SecurityHoldings[maxlengthindex]).TradingNAV.TradingDates[i].ToString("yyyy-MM-dd") + "\t");
            //        foreach (ASecurity s in this.SecurityHoldings)
            //        {
            //            switch (type)
            //            {
            //                case DataInfoType.TradingPrice:
            //                    if (i < s.TradingPrice.AdjustedTimeSeries.Count)
            //                        Debug.Write(s.TradingPrice.AdjustedTimeSeries[i].UpAndDown.KLineDay5 + "\t");
            //                    else
            //                        Debug.Write("-");
            //                    break;
            //                case DataInfoType.FundNetAssetValue:
            //                    if (i < ((MutualFund)s).TradingNAV.AdjustedTimeSeries.Count)
            //                        Debug.Write(((MutualFund)s).TradingNAV.AdjustedTimeSeries[i].UpAndDown.KLineDay5 + "\t");
            //                    else
            //                        Debug.Write("-");
            //                    break;
            //                default:
            //                    break;
            //            }
            //        }
            //        Debug.WriteLine("");
            //    }
            //}
        }
        #endregion

        #region 扩展属性
        public List<ASecurity> SecurityHoldings;
        public List<string> SecurityCodes;
        protected System.Type SecurityClass;
        #endregion

        #region 扩展方法
        #region Add Security
        public void Add(string code)
        {
            List<string> codelist = new List<string>();
            codelist.Add(code);
            Add(codelist);
        }
        public void Add(List<string> codelist)
        {
            if (codelist == null || codelist.Count == 0)
                return;

            foreach (string code in codelist)
            {
                if (!IsHolding(code))
                {
                    string[] para= new string[]{code};
                    ASecurity s = (ASecurity)Activator.CreateInstance(SecurityClass, para);
                    this.Add(s);
                }
            }
        }
        public void Add(ASecurity s)
        {
            if (!IsHolding(s.Code))
            {
                //调整各股的时间区间同组合一致
                s.SetDatePeriod(this.TimeSeriesStart, this.TimeSeriesEnd);

                this.SecurityCodes.Add(s.Code);
                this.SecurityHoldings.Add(s);
            }
        }
        public void Add(ASecurityGroup g)
        {
            foreach (ASecurity s in g.SecurityHoldings)
                this.Add(s);
        }
        #endregion

        protected bool IsHolding(string code)
        {
            if (code.Length == 0)
                return true;

            if (this.SecurityCodes == null)
            {
                this.SecurityHoldings = new List<ASecurity>();
                this.SecurityCodes = new List<string>();
                return false;
            }

            int idx = this.SecurityCodes.FindIndex(delegate(string s) { return s == code; });
            return idx >= 0;
        }
        protected void LoadSecurityInfo()
        {
            if (this.SecurityHoldings == null || this.SecurityHoldings.Count == 0)
                return;
            else
            {
                foreach (ASecurity s in this.SecurityHoldings)
                    s.LoadData(DataInfoType.SecurityInfo);
            }
        }
        #endregion
    }
}
