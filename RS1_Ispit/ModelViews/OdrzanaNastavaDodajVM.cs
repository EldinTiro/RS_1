using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ModelViews
{
    public class OdrzanaNastavaDodajVM
    {
        public int nastavnikID { get; set; }
        public List<SelectListItem> skola { get; set; }
        public string ImePrezime { get; set; }
        public string skolskaGodina { get; set; }
        public DateTime datumIspit { get; set; }
        public List<SelectListItem> predmet { get; set; }
        public int SkolaId { get; set; }
        public int PredmetId { get; set; }
    }
}
