using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarketDataAdapter;

namespace DataWorkCenterLib
{
    public enum DataLoaderType
    { 
        NULL,
        MainSource,
        SubSource
    }

    public class DataLoaderFactory
    {
        public static ADataLoader GetDataLoader(ADBInstance dbInstance, VandorType type)
        {
            switch (type)
            {
                case VandorType.DataCenter:
                case VandorType.Wind:
                    return new DataLoaderWind(dbInstance);

                case VandorType.CaiHui:
                    return new DataLoaderCaiHui(dbInstance);

                case VandorType.JuYuan:
                    return new DataLoaderJuYuan(dbInstance);

                default:
                    return null;
            }
        }
    }
}
