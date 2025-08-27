using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TBBEgitim.Models
{
    public class EgitimViewModel
    {
        public int Id { get; set; }
        public string EgitimAdi { get; set; }
        public string Aciklama { get; set; }
        public int KategoriId { get; set; }
        public string KategoriAdi { get; set; }
        public DateTime? BaslangicTarihi { get; set; }
        public DateTime? BitisTarihi { get; set; }
        public int IlId { get; set; }
        public string EgitimYeri { get; set; }
        public int Kontenjan { get; set; }
        public decimal? Ucret { get; set; }
        public decimal? AvUcret { get; set; }
        public int KalanKontenjan { get; set; }
        public string EgitimDurumu { get; set; }
    }
}
