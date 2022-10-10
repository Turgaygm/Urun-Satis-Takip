using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UrunStokTakip.Models;
using PagedList;
using PagedList.Mvc;

namespace UrunStokTakip.Controllers
{
    public class SatisController : Controller
    {
        // GET: Satis
        Takip_SistemiEntities5 db = new Takip_SistemiEntities5();
        public ActionResult Index()
        {
            var list = db.Satislars.ToList();
            return View(list);

        }
        public ActionResult SatinAl(int id)
        {
            var model = db.Sepets.FirstOrDefault(x => x.Id == id);
            return View(model);
        }
        [HttpPost]
        public ActionResult SatinAl2(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = db.Sepets.FirstOrDefault(x => x.Id == id);
                    var satis = new Satislar
                    {
                        KullaniciId = model.KullaniciId,
                        UrunId = model.UrunId,
                        Adet = model.Adet,
                        Resim = model.Resim,
                        Fiyat = model.Fiyat,
                        Tarih = model.Tarih,
                    };
                    db.Sepets.Remove(model);
                    db.Satislars.Add(satis);
                    db.SaveChanges();
                    ViewBag.islem = "Satin alma işlemi Başarılı bir şekilde tamamlanmıştır";
                }
            }
            catch (Exception)
            {

                ViewBag.islem = "Satın alma işlemi başarısız";
            }
            return View("islem");
        }
        public ActionResult HepsiniSatinAl(decimal? Tutar)
        {
            if (User.Identity.IsAuthenticated)
            {
                var kullaniciadi = User.Identity.Name;
                var kullanici = db.Kullanicis.FirstOrDefault(x => x.Email == kullaniciadi);
                var model = db.Sepets.Where(x => x.KullaniciId == kullanici.Id).ToList();
                var kid = db.Sepets.FirstOrDefault(x => x.KullaniciId == kullanici.Id);
                if (model != null)
                {
                    if (kid == null)
                    {
                        ViewBag.Tutar = "Sepetinizde ürün bulunmamaktadır";
                    }
                    else if (kid != null)
                    {
                        Tutar = db.Sepets.Where(x => x.KullaniciId == kid.KullaniciId).Sum(x => x.Urun.Fiyat * x.Adet);
                        ViewBag.Tutar = "Toplam Tutar = " + Tutar + "₺";
                    }

                    return View(model);
                }

                return View();
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult HepsiniSatinAl2()
        {
            var username = User.Identity.Name;
            var kullanici = db.Kullanicis.FirstOrDefault(x => x.Email == username);
            var model = db.Sepets.Where(x => x.KullaniciId == kullanici.Id).ToList();
            int satir = 0;
            foreach (var item in model)
            {
                var satis = new Satislar
                {
                    KullaniciId = model[satir].KullaniciId,
                    UrunId = model[satir].UrunId,
                    Adet = model[satir].Adet,
                    Fiyat = model[satir].Fiyat,
                    Resim = model[satir].Urun.Resim,
                    Tarih = DateTime.Now
                };

                db.Satislars.Add(satis);
                db.SaveChanges();
                ViewBag.islem = "Satın alma işlemi başarıyla gerçekleşmiştir.";


                satir++;
            }
            db.Sepets.RemoveRange(model);
            db.SaveChanges();

            return View("islem");
        }
    }
}