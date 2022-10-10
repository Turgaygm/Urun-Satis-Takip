using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using UrunStokTakip.Models;

namespace UrunStokTakip.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        Takip_SistemiEntities5 db = new Takip_SistemiEntities5();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult Login(Kullanici p)
        {
            var bilgiler = db.Kullanicis.FirstOrDefault(x => x.Email == p.Email && x.Sifre == p.Sifre);
            if (bilgiler!=null)
            {
                FormsAuthentication.SetAuthCookie(bilgiler.Email, false);
                Session["EMail"] = bilgiler.Email.ToString();
                Session["Ad"] = bilgiler.Ad.ToString();
                Session["Soyad"] = bilgiler.Soyad.ToString();
                Session["Foto"] = bilgiler.Foto.ToString();
                return RedirectToAction("Index", "Home");
            }

            else
            {
                ViewBag.hata = "Kullanıcı Adı Veya Şifre Hatalı";
            }
                return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Kullanici data)
        {
            if (Request.Files.Count > 0)
            {
                string dosyaadi = Path.GetFileName(Request.Files[0].FileName);
                string uzanti = Path.GetExtension(Request.Files[0].FileName);
                string yol = "~/Image/" + dosyaadi + uzanti;
                Request.Files[0].SaveAs(Server.MapPath(yol));
                data.Foto = "/Image/" + dosyaadi + uzanti;
            }
            db.Kullanicis.Add(data);
            data.Rol = "U";
            db.SaveChanges();
            return RedirectToAction("Login", "Account");
        }
        public ActionResult Guncelle()
        {
            var kullanicilar = (string)Session["EMail"];
            var degerler = db.Kullanicis.FirstOrDefault(x => x.Email == kullanicilar);
            return View(degerler);
        }
        [HttpPost]
        public ActionResult Guncelle(Kullanici data)
        {
            var kullanicilar = (string)Session["EMail"];
            var user = db.Kullanicis.Where(x => x.Email == kullanicilar).FirstOrDefault();
            user.Ad = data.Ad;
            user.Soyad = data.Soyad;
            user.Email = data.Email;
            user.Ad = data.Ad;
            user.Sifre = data.Sifre;
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}