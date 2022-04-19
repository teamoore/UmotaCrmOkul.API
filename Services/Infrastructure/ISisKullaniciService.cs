using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UmotaCrmOkul.API.ModelDto.Request;
using UmotaCrmOkul.API.ModelDto.Response;

namespace UmotaCrmOkul.API.Services.Infrastructure
{
    public interface ISisKullaniciService
    {
        Task<SisKullaniciLoginResponseDto> Login(SisKullaniciLoginRequestDto request);
    }
}
