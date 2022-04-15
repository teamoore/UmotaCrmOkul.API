using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UmotaCrmOkul.API.ModelDto
{
    public class OgrenciDonemOdeme
    {
        public int logref  { get; set; }
        public int ogrenciref  { get; set; }
        public int ogrencidonemref  { get; set; }
        public byte odemetipi  { get; set; }
        public int taksitno  { get; set; }
        public DateTime? taksittar { get; set; }
        public double? tutar_veli  { get; set; }
        public double? tutar_egitim  { get; set; }
        public double? tutar_yatili { get; set; }
        public double? tutar_burs  { get; set; }
        public double? tutar_burs_egitim  { get; set; }
        public double? tutar_burs_yatili  { get; set; }
        public double? tutar_fatura { get; set; }
        public double? tutar_fatura_egitim { get; set; }
        public double? tutar_fatura_yatili { get; set; }
        public bool kdvsiz { get; set; }
        public string insuser  { get; set; }

    }
}
