using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UrunStokTakip.Models;

namespace UrunStokTakip.Controllers
{
    [Authorize(Roles = "A")]
    public class KategoriController : Controller
    {
        // GET: Kategori
        Takip_SistemiEntities5 db = new Takip_SistemiEntities5();
        
        public ActionResult Index()
        {

            return View(db.Kategoris.Where(x=>x.Durum==true).ToList());
        }
        
        public ActionResult Ekle()
        {

            return View();
        }
        [HttpPost]
        
        public ActionResult Ekle(Kategori data)
        {
            db.Kategoris.Add(data);
            data.Durum = true;
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        
        public ActionResult Sil(int id)
        {
            var ktgr = db.Kategoris.Where(x => x.Id == id).FirstOrDefault();
            db.Kategoris.Remove(ktgr);
            ktgr.Durum = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Guncelle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Guncelle(Kategori data)
        {
            var guncelle = db.Kategoris.Where(x => x.Id == data.Id).FirstOrDefault();
            guncelle.Aciklama = data.Aciklama;
            guncelle.Ad = data.Ad;

            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}