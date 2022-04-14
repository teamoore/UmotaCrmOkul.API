using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UmotaCrmOkul.API.ModelDto.Response
{
    public class ServiceResponse<T> : BaseResponse
    {
        public T Value { get; set; }
    }
}
