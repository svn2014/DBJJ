using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Security
{
    public class NetAssetValue : ATimeItem
    {
        #region 扩展属性
        /// <summary>
        /// 单位净值：当前申赎的价格
        /// 复权净值: 将分红加回单位净值，并作为再投资进行复利计算。 
        /// 累计净值: 将分红加回单位净值，但不作为再投资计算复利。
        /// </summary>
        public double UnitNAV = 0;           //单位净值
        public double AccumUnitNAV = 0;      //累计净值
        #endregion

        //#region 基础方法
        ////public override void DebugPrint()
        ////{
        ////    Debug.Write(base.TradeDate.ToString("yyyy-MM-dd") + "\t");
        ////    Debug.Write(this.UnitNAV.ToString("N4") + "\t");
        ////    Debug.Write(this.AccumUnitNAV.ToString("N4") + "\t|\t");

        ////    base.DebugPrint();

        ////    Debug.WriteLine("");
        ////}
        //#endregion
    }
}
