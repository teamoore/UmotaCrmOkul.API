using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public class FiyatController : ControllerBase
    {

        private readonly ILogger<FiyatController> _logger;
        private IFiyatService fiyatService;

        public FiyatController(ILogger<FiyatController> logger, IFiyatService fiyatService)
        {
            _logger = logger;
            this.fiyatService = fiyatService;
        }

        [HttpPost("FiyatlarGetir")]
        public async Task<ServiceResponse<IEnumerable<FiyatDto>>> GetFiyatlar(FiyatRequestDto request)
        {
            try
            {
                return new ServiceResponse<IEnumerable<FiyatDto>>()
                {
                    Value = await fiyatService.GetFiyatByDonem(request)
                };
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);

                var e = new ServiceResponse<IEnumerable<FiyatDto>>();
                e.SetException(ex);
                return e;
            }
        }
    }
}
