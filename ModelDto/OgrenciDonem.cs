using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UmotaCrmOkul.API.ModelDto
{
    public class OgrenciDonem
    {
        public int logref   { get; set; }
        public int ogrenciref   { get; set; }
        public int donemno   { get; set; }
        public int donemref   { get; set; }
        public int sinifref   { get; set; }
        public int yatiliref   { get; set; }
        public int bursref  { get; set; }
        public int odemeref   { get; set; }
        public int bankaref   { get; set; }
        public string bankahesapno   { get; set; }
        public string bankahesapadi   { get; set; }
        public bool kdvsiz { get; set; }
        public int gunduzluref { get; set; }
        public string insuser { get; set; }
        public DateTime? insdate { get; set; }
        public string upduser { get; set; }
        public DateTime? upddate { get; set; }
        public bool isupdate { get; set; }
    }
}
