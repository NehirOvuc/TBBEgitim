using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TBBEgitim.Models
{
    public class BasvuruViewModel
    {
        public int EgitimId { get; set; }
        public string EgitimAdi { get; set; }
        public int KategoriId { get; set; }
        public string KategoriAdi { get; set; }
        public int? CinsiyetId { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string TcNo { get; set; }
        public string Eposta { get; set; }
    }
}
