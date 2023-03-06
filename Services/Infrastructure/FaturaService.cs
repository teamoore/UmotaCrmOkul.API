using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Data;
using UmotaCrmOkul.API.Consts;
using UmotaCrmOkul.API.ModelDto;
using UmotaCrmOkul.API.ModelDto.Request;
using System;
using System.Linq;
using static Dapper.SqlBuilder;
using System.IO.Compression;
using System.IO;
using LogoService;
using UmotaCrmOkul.API.ModelDto.Response;

namespace UmotaCrmOkul.API.Services.Infrastructure
{
    public class FaturaService : IFaturaService
    {
        public IConfiguration configuration { get; set; }
        public FaturaService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<List<FaturaDto>> GetFaturaList(FaturaRequestDto request)
        {
            var faturalist = new List<FaturaDto>();

            using (var db = new SqlConnection(configuration.GetConnectionString(CrmConsts.LogoConnectionString)))
            {
                string LogoDbName = configuration["LogoDbName"];
                string LogoFirmaNo = configuration["LogoFirmaNo"];
                string LogoDonemNo = configuration["LogoDonemNo"];
                string INVOICE = LogoDbName + ".dbo.LG_" + LogoFirmaNo + "_" + LogoDonemNo + "_INVOICE";
                string CLCARD = LogoDbName + ".dbo.LG_" + LogoFirmaNo + "_CLCARD";
                string CLCARDXT = LogoDbName + ".dbo.LG_XT1015_" + LogoFirmaNo;

                string sqlstring = "SELECT A.LOGICALREF, A.FICHENO, A.DATE_, A.GUID, A.NETTOTAL, A.GENEXP1 " +

                    "FROM " + INVOICE + " A WITH(NOLOCK) " +
                    "INNER JOIN " + CLCARD + " B WITH(NOLOCK) ON A.CLIENTREF = B.LOGICALREF " +
                    "INNER JOIN " + CLCARDXT + " C WITH(NOLOCK) ON B.LOGICALREF = C.PARLOGREF " +
                    "WHERE C.OGRENCI_TC LIKE '" + request.TcKimlikNo + "'";

                var result = await db.QueryAsync<FaturaDto>(sqlstring);

                db.Close();

                return result.ToList();
            }
        }

        public async Task<FaturaDataResponseDto> GetFaturaData(FaturaDataRequestDto request)
        {
            string uuid = request.GUID;
            string EFaturaUserName = "6130492852";//ConfigurationManager.AppSettings["EFaturaUserName"].ToString();
            string EFaturaUserPass = "[ZHzL3BP";//ConfigurationManager.AppSettings["EFaturaUserPass"].ToString();
            var datadto = new FaturaDataResponseDto();

            using (LogoService.PostBoxServiceClient svc = new LogoService.PostBoxServiceClient(LogoService.PostBoxServiceClient.EndpointConfiguration.PostBoxServiceEndpoint))
            {
                LogoService.LoginRequest loginreq = new LogoService.LoginRequest();
                LogoService.LoginType login = new LogoService.LoginType();
                login.userName = EFaturaUserName;
                login.passWord = EFaturaUserPass;
                loginreq.login = login;
                var loginres = await svc.LoginAsync(loginreq);
                if (loginres.LoginResult)
                {
                    try
                    {
                        LogoService.DocumentType dt = await svc.getDocumentDataAsync(loginres.sessionID, uuid, LogoService.GetDocumentType.EINVOICE, LogoService.DocumentDataType.PDF);
                        datadto.PdfData = dt.binaryData.Value;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Hata oluştu. Hata mesajı : " + ex.Message);
                    }
                    finally
                    {
                        await svc.LogoutAsync(loginres.sessionID);
                    }
                }
            }

            return datadto;
        }
    }
}
