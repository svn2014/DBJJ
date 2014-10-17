using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarketDataAdapter
{
    public enum DataMessageType
    {
        Infomation,
        Warning,
        Error
    }

    public class DataMessage
    {
        public long Order = 0;
        public string TradeDate = "";
        public string TableName = "";
        public string Code = "";
        public string Field = "";
        public object MainValue = null;
        public object SubValue = null;
        public object CheckValue = null;
        public string Message = "";
        public DataMessageType Type = DataMessageType.Infomation;
    }

    public class DataMessageManager
    {
        private static DataMessageManager _Instance = null;
        public static DataMessageManager GetInstance() 
        {
            if (_Instance == null)
            {
                _Instance = new DataMessageManager();
            }

            return _Instance;
        }

        public bool PrintAtOnce = true;
        private List<DataMessage> _MessageList = new List<DataMessage>();

        public void AddMessage(DataMessageType type, string message, string tableName, string tradeDate)
        {
            AddMessage(type, message, "", "", null, null, null, tableName, tradeDate);
        }

        public void AddMessage(DataMessageType type, string message, string code, string field, string tableName, string tradeDate)
        {
            AddMessage(type, message, code, field, null, null, null, tableName, tradeDate);
        }

        public void AddMessage(DataMessageType type, string message, string code, string field, object mainValue, object subValue, object checkValue, string tableName, string tradeDate)
        { 
            DataMessage msg = new DataMessage();
            msg.Code = (code == null ? "" : code);
            msg.Field = (field == null ? "" : field);

            msg.MainValue = mainValue;
            msg.SubValue = subValue;
            msg.CheckValue = checkValue;
            msg.Message = message;
            msg.Type = type;

            msg.TradeDate = tradeDate;
            msg.TableName = tableName;
            msg.Order = this._MessageList.Count + 1;

            this._MessageList.Add(msg);

            if (this.PrintAtOnce)
                this.printMessage(msg);
        }

        public void Print()
        {
            if (this.PrintAtOnce)
                return;

            foreach (DataMessage m in _MessageList)
            {
                this.printMessage(m);
            }
        }

        private void printMessage(DataMessage m)
        {
            Console.Write(m.Type.ToString().Substring(0, 1) + "\t");
            
            Console.Write(m.TradeDate + "\t");
            Console.Write(m.TableName + "\t");
            Console.Write(m.Code + "\t");
            Console.Write(m.Field + "\t");

            if (m.MainValue != null || m.SubValue != null)
                Console.Write("A：" + m.MainValue + "\tB：" + m.SubValue + "\tC: " + m.CheckValue);

            Console.WriteLine(m.Message);
        }

        public void Clear()
        {
            this._MessageList.Clear();
        }

        public List<DataMessage> GetMessages()
        {
            return this._MessageList;   
        }

        public List<DataMessage> GetMessages(DataMessageType type)
        {
            return this._MessageList.FindAll(delegate(DataMessage m) { return m.Type == type; });
        }
    }
}
