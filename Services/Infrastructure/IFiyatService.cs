using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UmotaCrmOkul.API.ModelDto;
using UmotaCrmOkul.API.ModelDto.Request;

namespace UmotaCrmOkul.API.Services.Infrastructure
{
    public interface IFiyatService
    {
        public Task<FiyatDto> GetFiyatByDonem(FiyatRequestDto request);
        public Task<FiyatDto> OdemePlaniOlustur(FiyatRequestDto request);
    }
}
