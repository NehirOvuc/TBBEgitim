using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TBBEgitim.Models
{
    public class EgitimKategoriRaporuModel
    {
        public int KategoriId { get; set; }
        public string KategoriAdi { get; set; }
        public int EgitimSayisi { get; set; }
    }

    public class EgitimKontenjanRaporuModel
    {
        public int KategoriId { get; set; }
        public string KategoriAdi { get; set; }
        public int ToplamKontenjan { get; set; }
        public int KalanKontenjan { get; set; }
    }

    public class EgitimRaporlariViewModel
    {
        public List<EgitimKategoriRaporuModel> KategoriRaporlari { get; set; }
        public List<EgitimKontenjanRaporuModel> KontenjanRaporlari { get; set; }
    }
}
