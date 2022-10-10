using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UrunStokTakip.Models;

namespace UrunStokTakip.Controllers
{
    public class SepetController : Controller
    {
        // GET: Sepet
        Takip_SistemiEntities5 db = new Takip_SistemiEntities5();
        public ActionResult Index(decimal?Tutar)
        {
            if (User.Identity.IsAuthenticated)
            {
                var kullaniciadi = User.Identity.Name;
                var kullanici = db.Kullanicis.FirstOrDefault(x => x.Email == kullaniciadi);
                var model = db.Sepets.Where(x => x.KullaniciId == kullanici.Id).ToList();
                var kid = db.Sepets.FirstOrDefault(x => x.KullaniciId == kullanici.Id);
                if (model!=null)
                {
                    if (kid==null)
                    {
                        ViewBag.Tutar = "Sepetinizde ürün bulunmamaktadır";

                    }
                    else if (kid!=null)
                    {
                        Tutar = db.Sepets.Where(x => x.KullaniciId == kid.KullaniciId).Sum(x => x.Fiyat * x.Adet);
                        ViewBag.Tutar = "Toplam Tutar =" + Tutar + " TL";
                    }
                    return View(model);
                }
                
            }
            return HttpNotFound();
        }
        public ActionResult SepeteEkle(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var kullaniciadi = User.Identity.Name;
                var model = db.Kullanicis.FirstOrDefault(x => x.Email == kullaniciadi);
                var u = db.Uruns.Find(id);
                var sepet = db.Sepets.FirstOrDefault(x => x.KullaniciId == model.Id && x.UrunId == id);
                if (model!=null)
                {
                    if (sepet!=null)
                    {
                        sepet.Adet++;
                        sepet.Fiyat = u.Fiyat * sepet.Adet;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    var s = new Sepet
                    {
                        KullaniciId = model.Id,
                        UrunId = u.Id,
                        Adet = 1,
                        Fiyat = u.Fiyat,
                        Tarih = DateTime.Now
                    };
                    db.Sepets.Add(s);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View();
            }
            return HttpNotFound();
        }
        public ActionResult SepetCount(int?count)
        {
            if (User.Identity.IsAuthenticated)
            {
                var model = db.Kullanicis.FirstOrDefault(x => x.Email == User.Identity.Name);
                count = db.Sepets.Where(x => x.KullaniciId == model.Id).Count();
                ViewBag.count = count;
                if (count==0)
                {
                    ViewBag.count = "";
                }
                return PartialView();
            }
            return HttpNotFound();
        }
        public ActionResult arttir(int id,Urun b)
        {
            var model = db.Sepets.Find(id);
            model.Adet++;
            model.Fiyat = b.Fiyat * model.Adet;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult azalt(int id, Urun b)
        {
            var model = db.Sepets.Find(id);
            if (model.Adet==1)
            {
                db.Sepets.Remove(model);
                db.SaveChanges();
            }
            model.Adet--;
            model.Fiyat = b.Fiyat * model.Adet;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public void AdetYaz(int id,int miktari,Urun b)
        {
            var model = db.Sepets.Find(id);
            model.Adet = miktari;
            model.Fiyat = b.Fiyat * model.Adet;
            db.SaveChanges();
        }
        public ActionResult Sil(int id)
        {
            var sil = db.Sepets.Find(id);
            db.Sepets.Remove(sil);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult HepsiniSil()
        {
            if (User.Identity.IsAuthenticated)
            {
                var kullaniciadi = User.Identity.Name;
                var model = db.Kullanicis.FirstOrDefault(x => x.Email == kullaniciadi);
                var sil = db.Sepets.Where(x => x.KullaniciId == model.Id);
                db.Sepets.RemoveRange(sil);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound();
        }
    }
}