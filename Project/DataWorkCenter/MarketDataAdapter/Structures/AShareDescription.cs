using System;

namespace MarketDataAdapter
{
    public class AShareDescription : AShareBase, IComparable
    {
        public readonly string PrimaryKey = "WindCode";
        public string WindCode = "";
        public string Code = "";
        public string Name = "";
        public ExchangeType Exchange = ExchangeType.NULL;
        public ListedBoardType ListedBoard = ListedBoardType.NULL;
        public DateTime? ListDate = null;
        public DateTime? DelistDate = null;

        public int CompareTo(object obj)
        {
            try
            {
                //按代码降序
                AShareDescription desc = (AShareDescription)obj;
                if (this.WindCode.CompareTo(desc.WindCode) > 0)
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
