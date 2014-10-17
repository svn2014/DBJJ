using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Security;

namespace RiskMonitor
{
    /// <summary>
    /// 货币基金风险观察
    /// </summary>
    public class RiskMonitorMF : ARiskMonitor
    {
        public override void Check(List<CheckParameter> paralist)
        {
            base.Check(paralist);

            //特殊条款
            this.CheckSecurityType();
        }

        #region 检查项目
        /// <summary>
        /// 检查证券类型
        ///     货币基金不得投资股票等资产
        /// </summary>
        protected void CheckSecurityType()
        {
            string title = C_Title_SecurityType;
            string description;
            int order = C_Order_SecurityType;

            MonitorItem mi;
            int id = 0;
            foreach (ASecurityGroup g in this.SecurityPortfolio.SecurityGroupList)
            {
                if (g.SecurityList == null || g.SecurityList.Count == 0)
                    continue;

                foreach (ASecurity s in g.SecurityList)
                {
                    mi = new MonitorItem();
                    mi.code = s.Code;
                    mi.name = s.Name;
                    mi.title = title;
                    mi.order = order;
                    mi.securityid = id++;
                    mi.status = Status.Pass;
                    mi.value = s.Type.ToString();
                    mi.threshhold = "满足条件的债券";

                    description = C_Desc_PositionPct.Replace("<1>", s.Name + "(" + s.Code + ")");
                    switch (s.Type)
                    {
                        case SecurityType.Bond:
                            switch (((Bond)s).SubType)
                            {
                                case BondType.Convertable:
                                case BondType.Exchangable:
                                case BondType.Other:
                                    mi.status = Status.Fail;
                                    mi.description = description.Replace("<2>", "可转换或可交换债券");
                                    break;
                                default:
                                    break;
                            }
                            switch (((Bond)s).IntAccruType)
                            {
                                case InterestType.Float:
                                case InterestType.Other:
                                    mi.status = Status.Warning;
                                    mi.description = description.Replace("<2>", "以定期存款利率为基准利率的浮动利率债券");
                                    mi.description += "（基准利率：" + ((Bond)s).IntFloatRateBase + "）";
                                    if (((Bond)s).IntFloatRateBaseType == BaseRateType.TimeDeposit)
                                        mi.status = Status.Fail;
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case SecurityType.Equity:
                        case SecurityType.Fund:
                        case SecurityType.Warrant:
                            mi.status = Status.Fail;
                            mi.description = description.Replace("<2>", "股票/基金/权证等高风险证券");
                            break;
                        default:
                            break;
                    }

                    this.Report.Add(mi);
                }
            }
        }
        #endregion
    }
}
