using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarketDataAdapter
{
    public enum VandorType
    {
        DataCenter,
        Wind,       //万得
        JuYuan,     //聚源
        CaiHui,     //财汇
    }    

    public class MarketDataAdapterFactory
    {
        public static AMarketDataAdapter GetAdapter(VandorType type)
        {
            switch (type)
            {
                case VandorType.DataCenter:
                case VandorType.Wind:
                    return new MarketDataAdapterWind();

                case VandorType.CaiHui:
                    return new MarketDataAdapterCaiHui();

                case VandorType.JuYuan:
                    return new MarketDataAdapterJuYuan();

                default:
                    return null;
            }
        }
    }
}
