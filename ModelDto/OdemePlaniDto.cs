using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UmotaCrmOkul.API.ModelDto
{
    public class OdemePlaniDto
    {
        public int TaksitNo { get; set; }
        public double? Tutar { get; set; }
        public DateTime? Tarih { get; set; }
    }
}
