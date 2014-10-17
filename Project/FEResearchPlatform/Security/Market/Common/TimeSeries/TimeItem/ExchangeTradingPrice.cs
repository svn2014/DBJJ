
using System.Diagnostics;
namespace Security
{
    public class ExchangeTradingPrice : ATimeItem
    {
        #region 扩展属性
        public double PreClose;
        public double Close;
        public double High;
        public double Low;
        public double Open;
        public double Average;

        public double AdjustCoefficient = 1;

        public double Volume;   //股
        public double Amount;   //元
        #endregion

        #region 基础方法
        public override void Adjust()
        {
            this.PreClose *= this.AdjustCoefficient;
            this.Close *= this.AdjustCoefficient;
            this.High *= this.AdjustCoefficient;
            this.Low *= this.AdjustCoefficient;
            this.Open *= this.AdjustCoefficient;
        }
        //public override void DebugPrint()
        //{
        //    Debug.Write(base.TradeDate.ToString("yyyy-MM-dd") + "\t");
        //    Debug.Write(this.Open.ToString("N2") + "\t");
        //    Debug.Write(this.High.ToString("N2") + "\t");
        //    Debug.Write(this.Low.ToString("N2") + "\t");
        //    Debug.Write(this.Close.ToString("N2") + "\t|\t");

        //    base.DebugPrint();

        //    Debug.WriteLine("");
        //}
        #endregion
    }
}
