using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UmotaCrmOkul.API.ModelDto.Request
{
    public class FiyatRequestDto
    {
        public string TcKimlik { get; set; }
        public string Sezon { get; set; }
        public string Seviye { get; set; }
        public int Sayisal { get; set; }
        public int BursOrani { get; set; }
        public int OdemeSekli { get; set; }
    }
}
