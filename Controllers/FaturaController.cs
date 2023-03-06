using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UmotaCrmOkul.API.ModelDto;
using UmotaCrmOkul.API.ModelDto.Request;
using UmotaCrmOkul.API.ModelDto.Response;
using UmotaCrmOkul.API.Services.Infrastructure;

namespace UmotaCrmOkul.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class FaturaController : ControllerBase
    {

        private readonly ILogger<FaturaController> _logger;
        private IFaturaService faturaService;

        public FaturaController(ILogger<FaturaController> logger, IFaturaService faturaService)
        {
            _logger = logger;
            this.faturaService = faturaService;
        }

        [HttpPost("FaturaListesiGetir")]
        public async Task<ServiceResponse<List<FaturaDto>>> GetFaturalar(FaturaRequestDto request)
        {
            try
            {
                return new ServiceResponse<List<FaturaDto>>()
                {
                    Value = await faturaService.GetFaturaList(request)
                };
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);

                var e = new ServiceResponse<List<FaturaDto>>();
                e.SetException(ex);
                return e;
            }
        }

        [HttpPost("FaturaDataGetir")]
        public async Task<ServiceResponse<FaturaDataResponseDto>> GetFaturaDatasi(FaturaDataRequestDto request)
        {
            try
            {
                return new ServiceResponse<FaturaDataResponseDto>()
                {
                    Value = await faturaService.GetFaturaData(request)
                };
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);

                var e = new ServiceResponse<FaturaDataResponseDto>();
                e.SetException(ex);
                return e;
            }
        }
    }
}
