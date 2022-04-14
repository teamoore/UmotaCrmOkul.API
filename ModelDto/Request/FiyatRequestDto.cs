using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UmotaCrmOkul.API.ModelDto.Request
{
    public class FiyatRequestDto
    {
        public string TcKimlik { get; set; } // 11 karakterden oluşan metinsel bir ifade olarak gönderilecektir
        public string Sezon { get; set; }  // “2022-2023” formatında gönderilecektir
        public string Seviye { get; set; } // PREP, L09, L10, L11 şeklinde gönderilecektir
        public int Yatili { get; set; }  // Sayısal olarak gönderilecektir. 10 (Yatılı Değil), 20 (5 Gün Yatılı), 30 (Yedi Gün Yatılı)
        public int BursOrani { get; set; } // Tam sayı şeklinde gönderilecektir
        public int OdemeSekli { get; set; } // Tam sayı şeklinde gönderilecektir. Gönderilen tam sayı değeri taksit sayısını ifade etmektedir. Eğer 0 (sıfır) değeri gönderilirse peşin ödeme olarak yorumlanacaktır
    }
}
