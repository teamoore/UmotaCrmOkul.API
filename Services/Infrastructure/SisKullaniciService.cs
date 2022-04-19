﻿using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UmotaCrmOkul.API.Consts;
using UmotaCrmOkul.API.ModelDto;
using UmotaCrmOkul.API.ModelDto.Request;
using UmotaCrmOkul.API.ModelDto.Response;

namespace UmotaCrmOkul.API.Services.Infrastructure
{
    public class SisKullaniciService : ISisKullaniciService
    {
        public IConfiguration Configuration { get; set; }
        public SisKullaniciService(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public async Task<SisKullaniciLoginResponseDto> Login(SisKullaniciLoginRequestDto request)
        {
            using (var db = new SqlConnection(Configuration.GetConnectionString(CrmConsts.MasterConnectionString)))
            {
                var sql = @"select top 1  [kullanici_kodu] as KullaniciKodu
      ,[kullanici_adi] as KullaniciAdi
      ,[kullanici_sifre] as KullaniciSifre
      ,[kullanici_aktif] as KullaniciAktif
      ,[kullanici_iptal] as KullaniciIptal 
        from  [dbo].[sis_kullanici] (nolock)
                            where kullanici_kodu = '" + request.Kod +"' and kullanici_sifre = '"+ request.Sifre +"'";

                var kullanici = await db.QueryFirstOrDefaultAsync<SisKullaniciDto>(sql, commandType: System.Data.CommandType.Text);

                if (kullanici == null)
                    throw new Exception("Kullanıcı kodu ve/veya şifre hatalı girildi.");

                if (kullanici.KullaniciIptal)
                    throw new Exception("Kullanıcının sisteme girişi engellenmiş. Sistem yöneticinize başvurunuz.");

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSecurityKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expires = DateTime.Now.AddDays(int.Parse(Configuration["JwtExpiresInDay"].ToString()));

                var claims = new[]
                {
                new Claim(ClaimTypes.Name, kullanici.KullaniciKodu)
            };

                var token = new JwtSecurityToken(Configuration["JwtIssuer"], Configuration["JwtAudience"], claims, null, expires, creds);

                string tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

                var response = new SisKullaniciLoginResponseDto();
                response.ApiToken = tokenStr;

                return response;
            }
        }
    }
}
