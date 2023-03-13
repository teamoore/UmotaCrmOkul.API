namespace UmotaCrmOkul.API.ModelDto.Request
{
    public class FaturaDataRequestDto
    {
        public string TcKimlikNo { get; set; } // 11 karakterden oluşan metinsel bir ifade olarak gönderilecektir
        public string GUID { get; set; }
        public int EINVOICE { get; set; }
    }
}
