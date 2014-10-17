using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortfolioEvaluation
{
    public class GZBItem
    {
        public DateTime ItemDate = DateTime.Today;
        public string ItemCode = "";
        public string ItemName = "";
        public double CurrentPrice = 0;
        public double HoldingQuantity = 0;
        public double HoldingCost = 0;
        public double HoldingMarketValue = 0;
        public double HoldingValueAdded = 0;
        public double HoldingCostPctofNAV = 0;
        public double HoldingMVPctofNAV = 0;
    }

    public class EquityPrice : IComparable
    {
        public string WindCode = "";
        public string Name = "";
        public DateTime TradeDate = DateTime.Today;
        public double Close = 0;                    //未复权收盘价
        public double Average = 0;
        public double PreClose = 0;                 //前收盘
        public double AdjustedFactor = 0;   
        public double AdjustedClose = 0;            //复权收盘价
        public double AdjustedAverage = 0;          //复权均价
        public double AdjustedPreClose = 0;         
        public double Volume = 0;
        public double Amount = 0;

        public int CompareTo(object obj)
        {
            try
            {
                //Order by TradeDate Desc
                EquityPrice p = (EquityPrice)obj;
                if (this.TradeDate > p.TradeDate)
                    return -1;
                else
                    return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class AssetPosition : IComparable
    {
        public AssetCategory Category = AssetCategory.Other;
        public DateTime ReportDate = DateTime.Today;
        public string Code;
        public string WindCode;
        public string Name;
        public EquityPrice Price = new EquityPrice();
        public double HoldingQuantity = 0;          //当前持仓量
        public double HoldingCost = 0;              //持仓成本(元)
        public double HoldingMarketValue = 0;       //市值(元)
        public double TradingVolume = 0;            //当日成交量(股)
        public double FreefloatCapital = 100;       //自由流通股本(股)

        public double Pctof_Nav = 0;
        public double Pctof_Portfolio = 0;          //持仓个股市值/持仓股票总市值

        public double LiquidityCycle = 0;           //HoldingQuantity / TradingVolume

        //T期和T-1期对比
        public double AccruedInterestAndDividend = 0;

        public double RealizedReturn;
        public double UnrealizedReturn;
        public double TotalReturn;
        public double TotalNetReturn;   //净收益：用于债券
        public double TotalReturnPct;
        public double TotalNetReturnPct;//净收益率：用于债券
        public DateTime BondMaturityDate;

        public string SWIndustryName;
        public string SWIndustryIndex;

        public int CompareTo(object obj)
        {
            try
            {
                //Order by pctof_equity Desc
                AssetPosition p = (AssetPosition)obj;
                if (this.Pctof_Portfolio > p.Pctof_Portfolio)
                    return -1;
                else
                    return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class EquityIndustry : IComparable
    {
        public string SWIndustryName;
        public string SWIndustryIndex;
        public double PortfolioWeight = 0;  //行业持仓的组合权重

        //public double Return = 0;
        public double ReturnAmt = 0;
        public double ReturnPct = 0;
        public double CostAmt = 0;

        public double HoldingMarketValue = 0;

        public int CompareTo(object obj)
        {
            try
            {
                //Order by pctof_equity Desc
                EquityIndustry ind = (EquityIndustry)obj;
                if (this.PortfolioWeight > ind.PortfolioWeight)
                    return -1;  
                else
                    return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class TradingDays : IComparable
    {
        public DateTime Trade_Days;

        public int CompareTo(object obj)
        {
            try
            {
                //Order by Trade_Days Desc
                TradingDays d = (TradingDays)obj;
                if (this.Trade_Days > d.Trade_Days)
                    return -1;
                else
                    return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class Transaction : IComparable
    {
        public enum TradingType
        { 
            Null,
            BuyNew,
            BuyMore,
            SellSome,
            SellAll
        }

        public TradingType TransactionType = TradingType.Null;
        public DateTime TransactionDate;
        public string Code = "";
        public string Name = "";
        public string IndustryName = "";
        public double TradingPrice = 0;
        public double TradingVolume = 0;
        public double MarketPrice = 0;
        public double Pctof_Nav = 0;
        public double Pctof_Portfolio = 0;

        public int CompareTo(object obj)
        {
            try
            {
                //Order by TransactionDate Desc, TradingDirection Desc, Code Asc
                Transaction d = (Transaction)obj;
                if (this.TransactionDate > d.TransactionDate)
                    return -1;
                else if (this.TransactionDate < d.TransactionDate)
                    return 1;
                else
                {
                    if (this.TransactionType > d.TransactionType)
                        return -1;
                    else if (this.TransactionType < d.TransactionType)
                        return 1;
                    else
                    {
                        if (this.Code.CompareTo(d.Code) > 0)
                            return 1;
                        else
                            return -1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class AssetEvaluation : IComparable
    {
        //Technical Info
        public int OrderId = 0;
        public string Code;
        public string Name;
        public string SWIndustryIndex;
        public AssetCategory Category = AssetCategory.Other;

        //Basic info
        public string EconomicSector = "";  //Industry
        public double PortfolioWeight = 0;
        public double SectorBenchmarkWeight = 0;
        public double PortfolioReturnPct = 0;
        public double PortfolioNetReturnPct = 0;   //债券：净价收益率
        public double PortfolioReturn = 0;
        public double PortfolioNetReturn = 0;   //债券：净价收益率
        public double SectorBenchmarkReturnPct = 0;
        
        //Performance Attribution
        public double PureSectorAllocation = 0;
        public double AllocationSelectionInteraction = 0;
        public double WithinSectorSelection = 0;
        public double TotalValueAdded = 0;

        //Extra info
        public double HoldingVolume = 0;
        public double HoldingMarketValue = 0;
        public double AnnualYield = 0;      //年化收益率
        public double pctofNAV = 0;
        public double pctofListedShare = 0; //=持有量/流通量
        public double TurnoverRate = 0;     //=交易量/流通量
        public double LiquidityCycle = 0;   //=持有量/交易量

        public DateTime BondMaturityDate;

        public int CompareTo(object obj)
        {
            try
            {
                //Order by OrderId, PortfolioWeight Desc
                AssetEvaluation ep = (AssetEvaluation)obj;

                if (this.OrderId < ep.OrderId)
                    return -1;
                else if (this.OrderId > ep.OrderId)
                    return 1;
                else
                {
                    if (this.pctofNAV > ep.pctofNAV)
                        return -1;
                    else if (this.pctofNAV < ep.pctofNAV)
                        return 1;
                    else
                    {
                        if (this.PortfolioWeight > ep.PortfolioWeight)
                            return -1;
                        else if (this.PortfolioWeight < ep.PortfolioWeight)
                            return 1;
                        else
                        {
                            if (this.SectorBenchmarkWeight > ep.SectorBenchmarkWeight)
                                return -1;
                            else
                                return 1;
                        }
                    }                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class SubscribeRedemp : IComparable
    {
        public DateTime ReportDate;
        public double SubscribeAmount = 0;
        public double RedeemAmount = 0;
        public double CurrentBalance = 0;
        public double UnitCumNAV = 0;
        public double EquityWeight = 0;
        public double BondWeight = 0;
        public double YieldOnEquity = 0;
        public double NetYieldOnBond = 0;

        public int CompareTo(object obj)
        {
            try
            {
                //Order by ReportDate Desc
                SubscribeRedemp d = (SubscribeRedemp)obj;
                if (this.ReportDate > d.ReportDate)
                    return -1;
                else
                    return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
