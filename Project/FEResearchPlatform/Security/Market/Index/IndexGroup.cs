using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Security
{
    public class IndexGroup: ASecurityGroup
    {
        #region 基础方法
        public IndexGroup() : base(typeof(Index)) { }
        public override void LoadData(DataInfoType type)
        {
            switch (type)
            {
                case DataInfoType.SecurityInfo:
                    base.LoadSecurityInfo();
                    break;
                case DataInfoType.TradingPrice:
                    DataManager.GetDataLoader().LoadIndexPrice(this);
                    break;
                default:
                    MessageManager.GetInstance().AddMessage(MessageType.Warning, Message.C_Msg_GE1, type.ToString());
                    return;
            }
        }
        #endregion
    }
}
