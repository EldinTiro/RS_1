using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.EntityModels;
using RS1_Ispit_asp.net_core.ModelViews;

namespace RS1_Ispit_asp.net_core.Controllers
{
    public class OdrzanaNastavaController : Controller
    {
        private MojContext db;

        public OdrzanaNastavaController (MojContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            OdrzanaNastavaIndexVM model = new OdrzanaNastavaIndexVM();

            model.rows = db.Nastavnik.Select(s => new OdrzanaNastavaIndexVM.Row
            {
                nastavnikID = s.Id,
                ImePrezime = s.Ime + " " + s.Prezime,

                skola = db.PredajePredmet
                .Where(o => o.NastavnikID == s.Id)
                .GroupBy(g => g.Odjeljenje.Skola.Naziv)
                .Select(q => q.Key).ToList()
            }).ToList();

            return View(model);
        }
        public IActionResult Odaberi(int nastavnikId)
        {
            var model = new OdrzanaNastavaOdaberiVM {

            NastavnikID = nastavnikId,
            row = db.Ispit.Where(w => w.NastavnikId == nastavnikId).Select(s => new OdrzanaNastavaOdaberiVM.Row
            {
                Datum = s.Datum.ToString("dd.MM.yyyy"),
                ispitID = s.Id,
                predmet = s.Predmet.Naziv,
                skola = s.Skola.Naziv,
                Odsutni = db.IspitStavka.Where(x=>x.IspitId == s.Id && !x.Pristuan).Select(c=>c.OdjeljenjeStavka.Ucenik.ImePrezime).ToList()              

            }).ToList()
            };

            return View(model);
        }
        public IActionResult Dodaj(int nastavnikID)
        {
            var model = new OdrzanaNastavaDodajVM();

            model.skola = db.Skola.Select(s => new SelectListItem
            {
                Text = s.Naziv,
                Value = s.Id.ToString()

            }).ToList();

            model.predmet = db.Predmet.Select(s => new SelectListItem
            {
                Text = s.Naziv,
                Value = s.Id.ToString()

            }).ToList();

            model.ImePrezime = db.Nastavnik.Where(w => w.Id == nastavnikID).Select(s => s.Ime + "" + s.Prezime).FirstOrDefault();
            model.nastavnikID = nastavnikID;
            model.skolskaGodina = db.SkolskaGodina.Where(w => w.Aktuelna).Select(s => s.Naziv).FirstOrDefault();

            return View(model);
        }
        public IActionResult Snimi (OdrzanaNastavaDodajVM Pmodel)
        {
            Ispit novi = new Ispit
            {
                Datum = Pmodel.datumIspit,
                NastavnikId = Pmodel.nastavnikID,
                PredmetId = Pmodel.PredmetId,
                SkolaId = Pmodel.SkolaId
            };

            db.Ispit.Add(novi);
            db.SaveChanges();

            db.IspitStavka.AddRange(db.DodjeljenPredmet
                                    .GroupBy(g => g.OdjeljenjeStavkaId)
                                    .Where(w => w.Select(q => q.OdjeljenjeStavka.Odjeljenje.Razred).FirstOrDefault() == 4
                                    && w.Select(q => q.OdjeljenjeStavka.Odjeljenje.SkolaID).FirstOrDefault() == Pmodel.SkolaId
                                    && w.Count(a => a.ZakljucnoKrajGodine == 1) == 0
                                    ).Select(s => new IspitStavka
                                    {
                                        Bodovi = 0,
                                        IspitId = novi.Id,
                                        OdjeljenjeStavkaId = s.Key,
                                        Pristuan = true
                                    }).ToList());

            db.SaveChanges();

            return Redirect("Odaberi?nastavnikId="+Pmodel.nastavnikID);
        }

        public IActionResult Uredi(int ispitId)
        {
            var Model = db.Ispit.Where(w => w.Id == ispitId).Select(s => new OdrzanaNastavaUrediVM
            {
                Datum = s.Datum.ToString("dd.mm.yyyy"),
                Predmet = s.Predmet.Naziv,
                Napomena = s.Napomena,
                IspitId = ispitId
            }).SingleOrDefault();

                       
            return View(Model);
        }

        public IActionResult SnimiDodaj(OdrzanaNastavaUrediVM model)
        {
            Ispit novi = db.Ispit.Find(model.IspitId);
            novi.Napomena = model.Napomena;

            db.Ispit.Update(novi);
            db.SaveChanges();

            return Redirect("Odaberi?nastavnikId=" + novi.NastavnikId);
        }
    }
}