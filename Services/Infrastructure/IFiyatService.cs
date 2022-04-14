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
        public Task<IEnumerable<FiyatDto>> GetFiyatByDonem(FiyatRequestDto request);
    }
}
