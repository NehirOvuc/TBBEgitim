using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TBBEgitim.Models
{
    public class BasvuruCinsiyetRaporuModel
    {
        public string Cinsiyet { get; set; }
        public int BasvuruSayisi { get; set; }
    }
    public class BasvurulanEgitimRaporuModel
    {
        public int EgitimId { get; set; }
        public string EgitimAdi { get; set; }
        public int BasvuruSayisi { get; set; }
    }
    public class BasvurulanKategoriRaporuModel
    {
        public int KategoriId { get; set; }
        public string KategoriAdi { get; set; }
        public int BasvuruSayisi { get; set; }
    }
    public class BasvuruRaporlariViewModel
    {
        public List<BasvuruCinsiyetRaporuModel> CinsiyetRaporlari { get; set; }
        public List<BasvurulanEgitimRaporuModel> BasvurulanEgitimRaporlari { get; set; }
        public List<BasvurulanKategoriRaporuModel> BasvurulanKategoriRaporlari { get; set; }
    }
}