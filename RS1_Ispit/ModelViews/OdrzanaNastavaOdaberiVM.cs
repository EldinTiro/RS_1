using RS1_Ispit_asp.net_core.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ModelViews
{
    public class OdrzanaNastavaOdaberiVM
    {
        public List<Row> row { get; set; }

        public int NastavnikID { get; set; }

        public class Row
        {
            public string Datum { get; set; }
            
            public string skola { get; set; }
            public int skolaID { get; set; }
            public string predmet { get; set; }
            public int ispitID { get; set; }
            public List<string> Odsutni { get; set; }
            
        }
    }
}
