using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ModelViews
{
    public class AjaxIndexVM
    {
        public int ispitID { get; set; }
        public List<Row> Rows { get; set; }
        public class Row
        {
            public int ispitStavkaID { get; set; }
            public string ImeUcenika { get; set; }
            public double prosjekOcjena { get; set; }
            public bool pristupioIspitu { get; set; }
            public int Rezultati { get; set; }
        }
    }
}
