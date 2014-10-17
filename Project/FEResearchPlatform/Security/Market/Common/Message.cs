﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Security
{
    public enum MessageType
    { 
        Error,
        Warning,
        Information
    }

    public class Message
    {
        #region 消息文本
        #region 数据库
        public const string C_Msg_DB1 = "关闭数据库时出错";
        public const string C_Msg_DB2 = "打开数据库时出错";
        public const string C_Msg_DB3 = "读取数据库时出错";
        public const string C_Msg_DB4 = "更新数据库时出错";
        public const string C_Msg_DB5 = "没有可用的数据库";
        #endregion

        #region 通用
        public const string C_Msg_GE1 = "未知的数据类型";
        public const string C_Msg_GE2 = "校验交易日期时出错";
        #endregion

        #region 股票
        public const string C_Msg_EQ2 = "未知的股票交易所";
        public const string C_Msg_EQ3 = "未知的股票代码";
        public const string C_Msg_EQ4 = "未找到股票价格数据";
        public const string C_Msg_EQ5 = "未找到股票基础数据";
        #endregion

        #region 基金
        public const string C_Msg_MF0 = "未知的基金类型";
        public const string C_Msg_MF3 = "未找到基金价格数据";
        public const string C_Msg_MF4 = "未找到基金净值数据";
        public const string C_Msg_MF5 = "未找到基金基础数据";
        public const string C_Msg_MF6 = "未知的基金分类（银河）";
        public const string C_Msg_MF7 = "未知的基金分类体系";
        public const string C_Msg_MF8 = "找不到对应的子基金";
        public const string C_Msg_MF11 = "构建整体基金净值时出错";
        #endregion

        #region 指数
        public const string C_Msg_ID4 = "未找到指数价格数据";
        public const string C_Msg_ID5 = "未找到指数基础数据";
        #endregion
        #endregion

        public MessageType Type = MessageType.Information;
        public string MessageText = "";
        public string MessageKeyword = "";

        public Message(MessageType type, string text, string keyword)
        {
            this.Type = type;
            this.MessageText = text;
            this.MessageKeyword = keyword;
        }
    }

    public class MessageManager
    {
        private static MessageManager _Instance;
        public static MessageManager GetInstance()
        {
            if (_Instance == null)
                _Instance = new MessageManager();

            return _Instance;
        }

        public List<Message> MessageList = new List<Message>();
        public void AddMessage(MessageType type, string text, string keyword)
        {
            Message m = new Message(type, text, keyword);
            MessageList.Add(m);
        }
        public void Print()
        {
            if (MessageList == null || MessageList.Count == 0)
                return;

            foreach (Message m in this.MessageList)
            {
                switch (m.Type)
                {
                    case MessageType.Error:
                        Console.Write(">>>>>错误: ");
                        break;
                    case MessageType.Warning:
                        Console.Write("警告: ");
                        break;
                    case MessageType.Information:
                        Console.Write("消息: ");
                        break;
                    default:
                        break;
                }
                Console.WriteLine(m.MessageText + "(" + m.MessageKeyword + ")");
            }
        }
    }
}
