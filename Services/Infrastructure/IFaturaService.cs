using System.Threading.Tasks;
using System.Collections.Generic;
using UmotaCrmOkul.API.ModelDto;
using UmotaCrmOkul.API.ModelDto.Request;
using UmotaCrmOkul.API.ModelDto.Response;

namespace UmotaCrmOkul.API.Services.Infrastructure
{
    public interface IFaturaService
    {
        public Task<List<FaturaDto>> GetFaturaList(FaturaRequestDto request);
        public Task<FaturaDataResponseDto> GetFaturaData(FaturaDataRequestDto request);
    }
}
