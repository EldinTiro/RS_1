using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.ModelViews;

namespace RS1_Ispit_asp.net_core.Controllers
{
    public class AjaxController : Controller
    {
        private MojContext db;

        public AjaxController (MojContext db)
        {
            this.db = db;
        }

        public IActionResult Index(int IspitId)
        {
            var model = new AjaxIndexVM
            {
                ispitID = IspitId,
                Rows = db.IspitStavka.Where(w => w.IspitId == IspitId).Select(s => new AjaxIndexVM.Row
                {
                    ImeUcenika = s.OdjeljenjeStavka.Ucenik.ImePrezime,
                    ispitStavkaID = s.Id,
                    pristupioIspitu = s.Pristuan,
                    Rezultati = s.Bodovi,
                    prosjekOcjena = db.DodjeljenPredmet.Where(x => x.OdjeljenjeStavkaId == s.OdjeljenjeStavkaId).Average(a => (double?)a.ZakljucnoKrajGodine) ?? 0
                }).ToList()
            };

            return PartialView(model);
        }
        public IActionResult Odsutan (int ispitStavkaId)
        {
            var ispitStavka = db.IspitStavka.Find(ispitStavkaId);

            ispitStavka.Pristuan = false;
            db.SaveChanges();

            return Redirect("/Ajax/Index?IspitId=" + ispitStavka.IspitId);

        }

        public IActionResult Prisutan(int ispitStavkaId)
        {
            var ispitStavka = db.IspitStavka.Find(ispitStavkaId);

            ispitStavka.Pristuan = true;
            db.SaveChanges();

            return Redirect("/Ajax/Index?IspitId=" + ispitStavka.IspitId);

        }

        public IActionResult Uredi(int ispitStavkaId)
        {
            var model = db.IspitStavka.Where(w => w.Id == ispitStavkaId).Select(s => new AjaxUrediVM
            {
                bodovi = s.Bodovi,
                imePrezime = s.OdjeljenjeStavka.Ucenik.ImePrezime,
                ispitStavkaID = s.Id
            }).FirstOrDefault();

            return View(model);
        }

        public IActionResult Snimi (AjaxUrediVM model)
        {
                var ispitStavka = db.IspitStavka.Find(model.ispitStavkaID);
                ispitStavka.Bodovi = model.bodovi;
                
                db.SaveChanges();

            return Redirect("/OdrzanaNastava/Uredi?ispitId=" + ispitStavka.IspitId);
        }
    }
}