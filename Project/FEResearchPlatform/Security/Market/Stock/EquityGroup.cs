
namespace Security
{
    public class EquityGroup : ASecurityGroup
    {
        #region 基础方法
        public EquityGroup() : base(typeof(Equity)) { }
        public override void LoadData(DataInfoType type)
        {
            switch (type)
            {
                case DataInfoType.SecurityInfo:
                    base.LoadSecurityInfo();
                    break;
                case DataInfoType.TradingPrice:
                    DataManager.GetDataLoader().LoadEquityPrice(this);
                    break;
                default:
                    MessageManager.GetInstance().AddMessage(MessageType.Warning, Message.C_Msg_GE1, type.ToString());
                    return;
            }
        }
        #endregion
    }
}
