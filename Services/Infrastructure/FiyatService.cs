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
                var sql = "select top 1 " +
                    "a.tutar_taksit0 Taksit0 " +
                    ",a.tutar_taksit1 Taksit1 " +
                    ",a.tutar_taksit2 Taksit2 " +
                    ",a.tutar_taksit3 Taksit3 " +
                    ",a.tutar_taksit4 Taksit4 " +
                    ",a.tutar_taksit5 Taksit5 " +
                    ",a.tutar_taksit6 Taksit6 " +
                    ",a.tutar_taksit7 Taksit7 " +
                    ",a.tutar_taksit8 Taksit8 " +
                    ",a.tutar_taksit9 Taksit9 " +
                    ",a.tutar_taksit10 Taksit10 " +
                    ",a.tutar_toplam Toplam " +
                    ",(select top 1 aa.taksittar from burs_odeme_tarihleri aa with(nolock) where aa.donemref = a.donemref and taksitno = 0) Tarih0 " +
                    ",(select top 1 aa.taksittar from burs_odeme_tarihleri aa with(nolock) where aa.donemref = a.donemref and taksitno = 1) Tarih1 " +
                    ",(select top 1 aa.taksittar from burs_odeme_tarihleri aa with(nolock) where aa.donemref = a.donemref and taksitno = 2) Tarih2 " +
                    ",(select top 1 aa.taksittar from burs_odeme_tarihleri aa with(nolock) where aa.donemref = a.donemref and taksitno = 3) Tarih3 " +
                    ",(select top 1 aa.taksittar from burs_odeme_tarihleri aa with(nolock) where aa.donemref = a.donemref and taksitno = 4) Tarih4 " +
                    ",(select top 1 aa.taksittar from burs_odeme_tarihleri aa with(nolock) where aa.donemref = a.donemref and taksitno = 5) Tarih5 " +
                    ",(select top 1 aa.taksittar from burs_odeme_tarihleri aa with(nolock) where aa.donemref = a.donemref and taksitno = 6) Tarih6 " +
                    ",(select top 1 aa.taksittar from burs_odeme_tarihleri aa with(nolock) where aa.donemref = a.donemref and taksitno = 7) Tarih7 " +
                    ",(select top 1 aa.taksittar from burs_odeme_tarihleri aa with(nolock) where aa.donemref = a.donemref and taksitno = 8) Tarih8 " +
                    ",(select top 1 aa.taksittar from burs_odeme_tarihleri aa with(nolock) where aa.donemref = a.donemref and taksitno = 9) Tarih9 " +
                    ",(select top 1 aa.taksittar from burs_odeme_tarihleri aa with(nolock) where aa.donemref = a.donemref and taksitno = 10) Tarih10 " +
                    " from fiyat_tablo a with(nolock) " +
                    " inner join UmotaCRM_OKUL.dbo.sis_sabitler_detay b with(nolock) on a.donemref = b.sabit_detay_id" +
                    " inner join UmotaCRM_OKUL.dbo.sis_sabitler_detay c with(nolock) on a.sinifref = c.sabit_detay_id" +
                    " inner join UmotaCRM_OKUL.dbo.sis_sabitler_detay d with(nolock) on a.yatiliref = d.sabit_detay_id" +
                    " inner join UmotaCRM_OKUL.dbo.sis_sabitler_detay e with(nolock) on a.odemeref = e.sabit_detay_id" +
                    " where b.adi like '" + request.Sezon +"'" +
                    " and c.kodu like '" + request.Seviye + "'" +
                    " and d.kodu like '" + request.Yatili + "'" +
                    " and e.kodu like '" + request.OdemeSekli + "'" +
                    " and a.bursyuz = " + request.BursOrani;

                //throw new Exception("Teklif bulunamadı");

                result = await db.QueryAsync<FiyatDto>(sql, commandType: System.Data.CommandType.Text);

                return result;
            }
        }
    }
}
