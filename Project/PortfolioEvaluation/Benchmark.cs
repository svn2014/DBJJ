using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Diagnostics;

namespace PortfolioEvaluation
{
    public class BenchmarkDef
    {
        public string Code;
        public string Name;
        public double ConstRate = 0;
        public double Weight;

        public List<EquityPrice> BenchmarkPriceList = new List<EquityPrice>();
        public BenchmarkDef(string code, string name, double weight, double constrate, List<EquityPrice> benchmarkPriceList)
        {
            Code = code;
            Name = name;
            Weight = weight;
            ConstRate = constrate;

            BenchmarkPriceList = benchmarkPriceList;
        }
    }

    public class BenchmarkPortfolio
    {
        private List<BenchmarkDef> _BenchmarkDefList = new List<BenchmarkDef>();
        private List<EquityPrice> _BenchmarkIndexList = new List<EquityPrice>();
        private List<AssetPosition> _BenchmarkPositionList = new List<AssetPosition>();
        private List<EquityIndustry> _BenchmarkIndustryList = new List<EquityIndustry>();

        //Benchmark Date for display, catch a longer time series
        public DateTime BenchmarkStartDate = DateTime.Today.AddDays(-1);
        public DateTime BenchmarkEndDate = DateTime.Today.AddDays(-1);

        //Report Date for return calculation
        public DateTime ReportStartDate = DateTime.Today.AddDays(-1);
        public DateTime ReportEndDate = DateTime.Today.AddDays(-1);
        
        public BenchmarkPortfolio(DateTime bmStartDate, DateTime bmEndDate, DateTime reportStartDate, DateTime reportEndDate)
        {
            if (bmStartDate > bmEndDate)
            {
                this.BenchmarkStartDate = bmEndDate;
                this.BenchmarkEndDate = bmStartDate;
            }
            else
            {
                this.BenchmarkStartDate = bmStartDate;
                this.BenchmarkEndDate = bmEndDate;
            }
            
            if(reportStartDate > reportEndDate)
            {
                this.ReportStartDate = reportEndDate;
                this.ReportEndDate = reportStartDate;
            }
            else
            {
                this.ReportStartDate = reportStartDate;
                this.ReportEndDate = reportEndDate;
            }
        }

        public void BuildBenchmarkComponents(List<AssetPosition> indexComponentWeightList, List<EquityPrice> equityPriceList)
        {
            _BenchmarkPositionList = indexComponentWeightList;
            
            //计算各成分股回报
            foreach (AssetPosition p in _BenchmarkPositionList)
            {
                try
                {
                    //计算个股回报
                    double pxStart=0, pxEnd =0;

                    //选择日期范围并降序排列（为了消除非交易日无数据的影响）
                    List<EquityPrice> selectedPriceList = equityPriceList.FindAll(delegate(EquityPrice e) { return (e.WindCode == p.Code && e.TradeDate >= this.ReportStartDate && e.TradeDate <= this.ReportEndDate); });
                    selectedPriceList.Sort();

                    if (selectedPriceList.Count == 0)
                    {
                        pxStart = 0;
                    }
                    else
                    {
                        pxStart = selectedPriceList[selectedPriceList.Count - 1].AdjustedClose;                             //期初复权价 //不用担心停牌，停牌会有前期价格
                        pxEnd = selectedPriceList[0].AdjustedClose;                                                         //期末复权价
                    }

                    if (pxStart == 0)
                        p.TotalReturnPct = 0;
                    else
                        p.TotalReturnPct = (pxEnd / pxStart - 1);
                
                    //计算行业权重及行业平均回报
                    int industryIndex = _BenchmarkIndustryList.FindIndex(delegate(EquityIndustry ind) { return (ind.SWIndustryIndex == p.SWIndustryIndex); });
                    if (industryIndex < 0)
                    {
                        EquityIndustry newInd = new EquityIndustry();
                        newInd.SWIndustryName = p.SWIndustryName;
                        newInd.SWIndustryIndex = p.SWIndustryIndex;
                        newInd.PortfolioWeight = p.Pctof_Portfolio;
                        _BenchmarkIndustryList.Add(newInd);
                    }
                    else
                    {
                        _BenchmarkIndustryList[industryIndex].PortfolioWeight += p.Pctof_Portfolio;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            //计算行业回报
            foreach (AssetPosition p in _BenchmarkPositionList)
            {
                int industryIndex = _BenchmarkIndustryList.FindIndex(delegate(EquityIndustry ind) { return (ind.SWIndustryIndex == p.SWIndustryIndex); });
                if (industryIndex >= 0)
                {
                    _BenchmarkIndustryList[industryIndex].ReturnPct += p.Pctof_Portfolio / _BenchmarkIndustryList[industryIndex].PortfolioWeight * p.TotalReturnPct;
                }
            }
        }

        public void AddBenchmarkIndex(BenchmarkDef bmi)
        {
            if (bmi != null)
            {
                _BenchmarkDefList.Add(bmi);
            }
        }

        public List<EquityPrice> GetBenchmarkIndex()
        {
            if (_BenchmarkIndexList.Count == 0)
                this.buildBenchmarkIndex();

            return _BenchmarkIndexList;
        }

        private void buildBenchmarkIndex()
        {
            if (_BenchmarkDefList.Count == 0)
                throw new Exception(DBConsts.C_ErrMsg_NoBenchmark);

            if (_BenchmarkDefList.Count == 1)
                _BenchmarkIndexList = _BenchmarkDefList[0].BenchmarkPriceList;

            int dayDiff = 0;
            bool flg = false;
            while (BenchmarkStartDate.AddDays(dayDiff) <= BenchmarkEndDate)
            {
                flg = false;
                DateTime currDate = BenchmarkStartDate.AddDays(dayDiff);

                EquityPrice indexPrice = new EquityPrice();
                indexPrice.TradeDate = currDate;

                foreach (BenchmarkDef bmi in _BenchmarkDefList)
                {
                    if (bmi.ConstRate > 0)
                    { //常数: 基期=1
                        indexPrice.Close += (1 + bmi.ConstRate / 365 * dayDiff) * bmi.Weight;
                    }
                    else
                    {//指数
                        int idx = bmi.BenchmarkPriceList.FindIndex(delegate(EquityPrice e) { return (e.TradeDate == currDate); });
                        if (idx >= 0)
                        {
                            indexPrice.Close += bmi.BenchmarkPriceList[idx].Close * bmi.Weight;
                            flg = true;
                        }
                    }
                }

                if (flg)    //去除非交易作日
                    _BenchmarkIndexList.Add(indexPrice);

                dayDiff++;
            }
        }

        public List<AssetPosition> GetBenchmarkPosition()
        {
            return _BenchmarkPositionList;
        }

        public List<EquityIndustry> GetBenchmarkIndustry()
        {
            return _BenchmarkIndustryList;
        }

        public List<BenchmarkDef> GetBenchmarkDef()
        {
            return _BenchmarkDefList;
        }

        public double GetBenchmarkReturn()
        {
            if (!HasIndexComponent())
                return 0;

            //计算完成后日期从小到大排列
            if (_BenchmarkIndexList.Count == 0)
                this.buildBenchmarkIndex();

            //筛选排序法：消除非交易日价格缺失的影响
            List<EquityPrice> selectedIndexPriceList = _BenchmarkIndexList.FindAll(
                delegate(EquityPrice p)
                { return (p.TradeDate >= this.ReportStartDate && p.TradeDate <= this.ReportEndDate); }
                );
            selectedIndexPriceList.Sort();  //按交易日降序排列

            double startIndex, endIndex;
            if (selectedIndexPriceList.Count >= 2)
            {
                startIndex = selectedIndexPriceList[selectedIndexPriceList.Count -1].Close;
                endIndex = selectedIndexPriceList[0].Close;

                if (startIndex == 0)
                    throw new Exception(DBConsts.C_ErrMsg_ErrorInBenchmark);
                else
                    return endIndex / startIndex - 1;
            }
            else
                throw new Exception(DBConsts.C_ErrMsg_ErrorInBenchmark);
        }

        public bool HasIndexComponent()
        {
            //查询股票比较基准,没有则返回False，例如：债券基金
            if (_BenchmarkDefList.Count == 0)
                return false;

            foreach (BenchmarkDef bmi in _BenchmarkDefList)
            {
                if (bmi.Code == null || bmi.Code.Trim().Length == 0)
                    continue;

                return true;
            }

            return false;
        }
    }
}
