using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TBBEgitim.Models;


namespace TBBEgitim.Controllers
{
    public class GirişController : BaseController
    {
        private readonly TBBEgitimDbEntities db = new TBBEgitimDbEntities();
        public ActionResult Anasayfa()
        {
            return View();
        }

        public ActionResult Eğitimler()
        {
            //eğitimler tablosu için düzenleme
            var egitimler = (from e in db.Egitimler
                             join k in db.Kategoriler on e.kategoriId equals k.id
                             join b in db.Basvurular on e.id equals b.egitimId into basvuruGrubu
                             select new EgitimViewModel
                             {
                                 Id = e.id,
                                 EgitimAdi = e.egitimAdi,
                                 Aciklama = e.aciklama,
                                 KategoriId = k.id,
                                 KategoriAdi = k.kategoriAdi,
                                 BaslangicTarihi = e.baslangicTarihi,
                                 BitisTarihi = e.bitisTarihi,
                                 IlId = e.ilId ?? 0,
                                 EgitimYeri = e.egitimYeri,
                                 Kontenjan = e.kontenjan ?? 0,
                                 Ucret = e.ucret,
                                 AvUcret = e.avUcret,
                                 KalanKontenjan = e.kontenjan - (basvuruGrubu.Any() ? basvuruGrubu.Count() : 0) ?? 0,
                                 EgitimDurumu = (e.kontenjan - (basvuruGrubu.Any() ? basvuruGrubu.Count() : 0)) > 0
                                                 && e.bitisTarihi >= DateTime.Today
                                                 ? "Aktif" : "Aktif Değil"
                             }).ToList();

            return View(egitimler);
        }

        public ActionResult Başvurular()
        {
            //başvurular tablosu için düzenleme
            var basvurular = (from b in db.Basvurular
                              join e in db.Egitimler on b.egitimId equals e.id
                              join k in db.Kategoriler on e.kategoriId equals k.id
                              select new BasvuruViewModel
                              {
                                  EgitimId = b.egitimId ?? 0,
                                  EgitimAdi = e.egitimAdi,
                                  KategoriId = k.id,
                                  KategoriAdi = k.kategoriAdi,
                                  CinsiyetId = b.cinsiyetId,
                                  Ad = b.ad,
                                  Soyad = b.soyad,
                                  TcNo = b.tcNo,
                                  Eposta = b.eposta
                              }).ToList();

            return View(basvurular);
        }

        public ActionResult Kategoriler(string search = "") {
            var db = new TBBEgitim.Models.TBBEgitimDbEntities();
            var kategoriler = db.Kategoriler.ToList();

            ViewBag.Kategoriler = kategoriler;

            return View();
        }
    }
}