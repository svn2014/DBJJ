using System;
using System.Diagnostics;

namespace Security
{
    public class PriceUpAndDown : ICloneable
    {
        //涨跌幅
        public Nullable<double> KLineDay1 = null;
        public Nullable<double> KLineDay2 = null;
        public Nullable<double> KLineDay3 = null;
        public Nullable<double> KLineDay4 = null;
        public Nullable<double> KLineDay5 = null;
        public Nullable<double> KLineDay10 = null;
        public Nullable<double> KLineDay20 = null;

        //public virtual void DebugPrint()
        //{
        //    Debug.Write(this.KLineDay1 == null ? "-.---" : this.KLineDay1.Value+"\t");
        //    Debug.Write((this.KLineDay2 == null ? "-.---" : this.KLineDay2.Value.ToString("P2"))+"\t");
        //    Debug.Write((this.KLineDay3 == null ? "-.---" : this.KLineDay3.Value.ToString("P2"))+"\t");
        //    Debug.Write((this.KLineDay4 == null ? "-.---" : this.KLineDay4.Value.ToString("P2"))+"\t");
        //    Debug.Write((this.KLineDay5 == null ? "-.---" : this.KLineDay5.Value.ToString("P2"))+"\t");
        //    Debug.Write((this.KLineDay10 == null ? "-.---" : this.KLineDay10.Value.ToString("P2"))+"\t");
        //    Debug.Write((this.KLineDay20 == null ? "-.---" : this.KLineDay20.Value.ToString("P2")) + "\t");
        //}

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public abstract class ATimeItem : ICloneable
    {
        public DateTime TradeDate;
        public DateTime PublishDate;
        public DateTime ReportDate;
        public bool IsTrading = true;
        public bool IsOutsideSamplePeriod = false;  //样本外数据
        public PriceUpAndDown UpAndDown = new PriceUpAndDown();

        public virtual void Adjust() { }
        public object Clone()
        {
            object obj = this.MemberwiseClone();
            ATimeItem item = (ATimeItem)obj;
            item.UpAndDown = (PriceUpAndDown)this.UpAndDown.Clone();
            return item;
        }

        //public virtual void DebugPrint() 
        //{
        //    if (this.UpAndDown != null)
        //        this.UpAndDown.DebugPrint();

        //    if (this.IsTrading)
        //        Debug.Write("\t");
        //    else
        //        Debug.Write("停牌 \t");
        //}
    }
}
