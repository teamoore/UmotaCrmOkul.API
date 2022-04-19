using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UmotaCrmOkul.API.ModelDto.Request;
using UmotaCrmOkul.API.ModelDto.Response;
using UmotaCrmOkul.API.Services.Infrastructure;

namespace UmotaCrmOkul.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KullaniciController : ControllerBase
    {
        private ILogger<KullaniciController> _logger;
        private ISisKullaniciService SisKullaniciService { get; set; }

        public KullaniciController(ILogger<KullaniciController> logger, ISisKullaniciService sisKullaniciService)
        {
            _logger = logger;
            SisKullaniciService = sisKullaniciService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ServiceResponse<SisKullaniciLoginResponseDto>> Login(SisKullaniciLoginRequestDto request)
        {
            var result = new ServiceResponse<SisKullaniciLoginResponseDto>();
            try
            {
                if (string.IsNullOrEmpty(request.Kod))
                    throw new Exception("Kullanıcı kodu boş olamaz");
                if (string.IsNullOrEmpty(request.Sifre))
                    throw new Exception("Kullanıcı şifre boş olamaz");

                result.Value = await SisKullaniciService.Login(request);

                return result;
            }
            catch (Exception ex)
            {
                result.SetException(ex);
                _logger.Log(LogLevel.Error, ex.Message);
            }

            return result;
        }

    }
}
