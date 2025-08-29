using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TBBEgitim.Models;

namespace TBBEgitim.Controllers
{
    public class RaporlarController : BaseController
    {
        private readonly TBBEgitimDbEntities db = new TBBEgitimDbEntities();
        //raporları listeleyecek actionlar
        public ActionResult EgitimRaporları()
        {
            var kategoriRaporu = (from e in db.Egitimler
                                  join k in db.Kategoriler on e.kategoriId equals k.id
                                  group e by new { e.kategoriId, k.kategoriAdi } into g
                                  select new EgitimKategoriRaporuModel
                                  {
                                      KategoriId = g.Key.kategoriId,
                                      EgitimSayisi = g.Count(),
                                      KategoriAdi = g.Key.kategoriAdi
                                  }).ToList();

            var kontenjanRaporu = (from e in db.Egitimler
                                   join k in db.Kategoriler on e.kategoriId equals k.id
                                   join b in db.Basvurular on e.id equals b.egitimId into basvuruGrubu
                                   group new { e, basvuruGrubu } by new { k.id, k.kategoriAdi } into g
                                   select new EgitimKontenjanRaporuModel
                                   {
                                       KategoriId = g.Key.id,
                                       KategoriAdi = g.Key.kategoriAdi,
                                       ToplamKontenjan = g.Sum(x => x.e.kontenjan) ?? 0,
                                       KalanKontenjan = g.Sum(x => x.e.kontenjan) - g.Sum(x => x.basvuruGrubu.Count()) ?? 0
                                   }).ToList();

            var model = new EgitimRaporlariViewModel
            {
                KategoriRaporlari = kategoriRaporu,
                KontenjanRaporlari = kontenjanRaporu
            };

            return View(model);
        }
        public ActionResult BasvuruRaporları()
        {
            var cinsiyetRaporu = (from b in db.Basvurular
                                 group b by new { b.cinsiyetId } into cinsiyetGrubu
                                 select new BasvuruCinsiyetRaporuModel
                                 {
                                     Cinsiyet = cinsiyetGrubu.Key.cinsiyetId == 1 ? "Erkek" : "Kadın",
                                     BasvuruSayisi = cinsiyetGrubu.Count()
                                 }).ToList();
            var basvurulanEgitimlerRaporu = (from b in db.Basvurular
                                             join e in db.Egitimler on b.egitimId equals e.id
                                             group e by new { e.id, e.egitimAdi } into egitimGrubu
                                             select new BasvurulanEgitimRaporuModel
                                             {
                                                 EgitimId = egitimGrubu.Key.id,
                                                 EgitimAdi = egitimGrubu.Key.egitimAdi,
                                                 BasvuruSayisi = egitimGrubu.Count()
                                             }).ToList();
            var basvurulanKategoriRaporu = (from b in db.Basvurular
                                            join k in db.Kategoriler on b.kategoriId equals k.id
                                            group k by new { k.id, k.kategoriAdi } into kategoriGrubu
                                            select new BasvurulanKategoriRaporuModel
                                            {
                                                KategoriId = kategoriGrubu.Key.id,
                                                KategoriAdi = kategoriGrubu.Key.kategoriAdi,
                                                BasvuruSayisi = kategoriGrubu.Count()
                                            }).ToList();
            var model = new BasvuruRaporlariViewModel
            {
                CinsiyetRaporlari = cinsiyetRaporu,
                BasvurulanEgitimRaporlari = basvurulanEgitimlerRaporu,
                BasvurulanKategoriRaporlari = basvurulanKategoriRaporu
            };

            return View(model);
        }
    }
}