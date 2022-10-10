using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UrunStokTakip.Models;
using PagedList;
using PagedList.Mvc;

namespace UrunStokTakip.Controllers
{
    public class UrunController : Controller
    {
        // GET: Urun

        Takip_SistemiEntities5 db = new Takip_SistemiEntities5();
        [Authorize]
        public ActionResult Index(int sayfa=1)
        {
            var degerler = db.Uruns.ToList().ToPagedList(sayfa, 7);
            List<SelectListItem> deger1 = (from x in db.Kategoris.ToList()

                                           select new SelectListItem
                                           {
                                               Text = x.Ad,
                                               Value = x.Id.ToString()
                                           }).ToList();
            ViewBag.ktgr = deger1;
            return View(degerler);
        }
        [HttpGet]
        [Authorize(Roles ="A")]
        public ActionResult Ekle()
        {
            List<SelectListItem> deger1 = (from x in db.Kategoris.ToList()

                                           select new SelectListItem
                                           {
                                               Text = x.Ad,
                                               Value = x.Id.ToString()
                                           }).ToList();
            ViewBag.ktgr = deger1;
            return View();
        }
        [Authorize(Roles = "A")]
        [HttpPost]
        public ActionResult Ekle(Urun Data, HttpPostedFileBase File)
        {
            if (Request.Files.Count > 0)
            {
                string dosyaadi = Path.GetFileName(Request.Files[0].FileName);
                string uzanti = Path.GetExtension(Request.Files[0].FileName);
                string yol = "~/Image/" + dosyaadi + uzanti;
                Request.Files[0].SaveAs(Server.MapPath(yol));
                Data.Resim = "/Image/" + dosyaadi + uzanti;
            }
            db.Uruns.Add(Data);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "A")]
        public ActionResult Sil(int id)
        {
            var urun = db.Uruns.Where(x => x.Id == id).FirstOrDefault();
            db.Uruns.Remove(urun);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "A")]
        public ActionResult Guncelle(int id)
        {
            var guncelle = db.Uruns.Where(x => x.Id == id).FirstOrDefault();
            List<SelectListItem> deger1 = (from x in db.Kategoris.ToList()

                                           select new SelectListItem
                                           {
                                               Text = x.Ad,
                                               Value = x.Id.ToString()
                                           }).ToList();
            ViewBag.ktgr = deger1;
            return View(guncelle);
        }
        [HttpPost]
        [Authorize(Roles = "A")]
        public ActionResult Guncelle(Urun model,HttpPostedFile File)
        {
            var urun = db.Uruns.Find(model.Id);
            
            if (File==null)
            {
            urun.Ad = model.Ad;
            urun.Aciklama = model.Aciklama;
            urun.Fiyat = model.Fiyat;
            urun.Stok = model.Stok;
            urun.Populer = model.Populer;
            urun.KategoriId = model.KategoriId;
                db.SaveChanges();
            return RedirectToAction("Index");
            }
                urun.Resim = model.Resim;
                urun.Ad = model.Ad;
                urun.Aciklama = model.Aciklama;
                urun.Fiyat = model.Fiyat;
                urun.Stok = model.Stok;
                urun.Populer = model.Populer;
                urun.KategoriId = model.KategoriId;
                db.SaveChanges();
                return RedirectToAction("Index");
        }
        [Authorize(Roles =("A"))]
        public ActionResult KritikStok()
        {
            var kritik = db.Uruns.Where(x => x.Stok <= 50).ToList();

            return View(kritik);
        }
        public PartialViewResult StokCount()
        {
            if (User.Identity.IsAuthenticated)
            {
                var count = db.Uruns.Where(x => x.Stok <= 50).Count();
                ViewBag.count = count;
                var azalan = db.Uruns.Where(x => x.Stok == 50).Count();
                ViewBag.azalan = azalan;
            }
            return PartialView();
        }

    }
}