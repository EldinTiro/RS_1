using RS1_Ispit_asp.net_core.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ModelViews
{
    public class OdrzanaNastavaIndexVM
    {
        public List<Row> rows { get; set; }

        public class Row
        {
            public int nastavnikID { get; set; }
            public List<string> skola { get; set; }
            public string ImePrezime { get; set; }
        }
    }
}
