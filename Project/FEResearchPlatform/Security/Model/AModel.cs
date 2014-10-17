using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;

namespace Security.Model
{
    public abstract class AModel
    {
        public string Name = "";
        public string Description = "";
        public string Version = "1.0";

        public abstract void Run();
        public abstract void SetParameters(Hashtable ht);
        public abstract DataSet GetResult();
    }
}
