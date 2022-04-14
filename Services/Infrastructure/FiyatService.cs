using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using UmotaCrmOkul.API.Consts;
using UmotaCrmOkul.API.ModelDto;
using UmotaCrmOkul.API.ModelDto.Request;

namespace UmotaCrmOkul.API.Services.Infrastructure
{
    public class FiyatService : IFiyatService
    {
        public IConfiguration configuration { get; set; }
        public FiyatService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<IEnumerable<FiyatDto>> GetFiyatByDonem(FiyatRequestDto request)
        {
            IEnumerable<FiyatDto> result;

            using (var db = new SqlConnection(configuration.GetConnectionString(CrmConsts.CompanyConnectionString)))
            {
                var sql = "select top 10 * from fiyat (nolock)";

                result = await db.QueryAsync<FiyatDto>(sql, commandType: System.Data.CommandType.Text);

                return result;
            }
        }
    }
}
