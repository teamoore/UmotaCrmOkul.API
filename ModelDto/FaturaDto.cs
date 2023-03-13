using System;

namespace UmotaCrmOkul.API.ModelDto
{
    public class FaturaDto
    {
        public int LOGICALREF { get; set; }
        public string FICHENO { get; set; }
        public DateTime DATE_ { get; set; }
        public string GUID { get; set; }
        public string GENEXP1 { get; set; }
        public double NETTOTAL { get; set; }
        public int  EINVOICE { get; set; }
    }
}
