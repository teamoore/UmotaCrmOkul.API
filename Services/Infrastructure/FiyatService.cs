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

namespace UmotaCrmOkul.API.Services.Infrastructure
{
    public class FiyatService : IFiyatService
    {
        public IConfiguration configuration { get; set; }
        public FiyatService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<FiyatDto> GetFiyatByDonem(FiyatRequestDto request)
        {
            if (request.OdemeSekli == null || request.OdemeSekli == 0)
                throw new Exception("Ödeme şekli gönderilmelidir");

            if (request.OdemeSekli != 0 && request.OdemeSekli != 4 && request.OdemeSekli != 7 && request.OdemeSekli != 10)
                throw new Exception("Ödeme şekli 0,4,7,10 gönderilmelidir");

            FiyatDto fiyat;
            var odemeplani = new List<OdemePlaniDto>();

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


                fiyat = await db.QuerySingleOrDefaultAsync<FiyatDto>(sql, commandType: System.Data.CommandType.Text);
                if (fiyat == null)
                    throw new Exception("Ödeme Planı bulunamadı");

                OdemePlaniDto odeme;
                switch (request.OdemeSekli)
                {
                    case 0:
                        odeme = new OdemePlaniDto();
                        odeme.TaksitNo = 0;
                        odeme.Tarih = fiyat.Tarih0;
                        odeme.Tutar = fiyat.Taksit0;
                        odemeplani.Add(odeme);
                        break;
                    case 4:
                        odeme = new OdemePlaniDto();
                        odeme.TaksitNo = 1;
                        odeme.Tarih = fiyat.Tarih1;
                        odeme.Tutar = fiyat.Taksit1;
                        odemeplani.Add(odeme);

                        odeme = new OdemePlaniDto();
                        odeme.TaksitNo = 2;
                        odeme.Tarih = fiyat.Tarih2;
                        odeme.Tutar = fiyat.Taksit2;
                        odemeplani.Add(odeme);

                        odeme = new OdemePlaniDto();
                        odeme.TaksitNo = 3;
                        odeme.Tarih = fiyat.Tarih3;
                        odeme.Tutar = fiyat.Taksit3;
                        odemeplani.Add(odeme);

                        odeme = new OdemePlaniDto();
                        odeme.TaksitNo = 4;
                        odeme.Tarih = fiyat.Tarih4;
                        odeme.Tutar = fiyat.Taksit4;
                        odemeplani.Add(odeme);
                        break;
                    case 7:
                        odeme = new OdemePlaniDto();
                        odeme.TaksitNo = 1;
                        odeme.Tarih = fiyat.Tarih1;
                        odeme.Tutar = fiyat.Taksit1;
                        odemeplani.Add(odeme);

                        odeme = new OdemePlaniDto();
                        odeme.TaksitNo = 2;
                        odeme.Tarih = fiyat.Tarih2;
                        odeme.Tutar = fiyat.Taksit2;
                        odemeplani.Add(odeme);

                        odeme = new OdemePlaniDto();
                        odeme.TaksitNo = 3;
                        odeme.Tarih = fiyat.Tarih3;
                        odeme.Tutar = fiyat.Taksit3;
                        odemeplani.Add(odeme);

                        odeme = new OdemePlaniDto();
                        odeme.TaksitNo = 4;
                        odeme.Tarih = fiyat.Tarih4;
                        odeme.Tutar = fiyat.Taksit4;
                        odemeplani.Add(odeme);

                        odeme = new OdemePlaniDto();
                        odeme.TaksitNo = 5;
                        odeme.Tarih = fiyat.Tarih5;
                        odeme.Tutar = fiyat.Taksit5;
                        odemeplani.Add(odeme);

                        odeme = new OdemePlaniDto();
                        odeme.TaksitNo = 6;
                        odeme.Tarih = fiyat.Tarih6;
                        odeme.Tutar = fiyat.Taksit6;
                        odemeplani.Add(odeme);

                        odeme = new OdemePlaniDto();
                        odeme.TaksitNo = 7;
                        odeme.Tarih = fiyat.Tarih7;
                        odeme.Tutar = fiyat.Taksit7;
                        odemeplani.Add(odeme);
                        break;
                    case 10:
                        odeme = new OdemePlaniDto();
                        odeme.TaksitNo = 1;
                        odeme.Tarih = fiyat.Tarih1;
                        odeme.Tutar = fiyat.Taksit1;
                        odemeplani.Add(odeme);

                        odeme = new OdemePlaniDto();
                        odeme.TaksitNo = 2;
                        odeme.Tarih = fiyat.Tarih2;
                        odeme.Tutar = fiyat.Taksit2;
                        odemeplani.Add(odeme);

                        odeme = new OdemePlaniDto();
                        odeme.TaksitNo = 3;
                        odeme.Tarih = fiyat.Tarih3;
                        odeme.Tutar = fiyat.Taksit3;
                        odemeplani.Add(odeme);

                        odeme = new OdemePlaniDto();
                        odeme.TaksitNo = 4;
                        odeme.Tarih = fiyat.Tarih4;
                        odeme.Tutar = fiyat.Taksit4;
                        odemeplani.Add(odeme);

                        odeme = new OdemePlaniDto();
                        odeme.TaksitNo = 5;
                        odeme.Tarih = fiyat.Tarih5;
                        odeme.Tutar = fiyat.Taksit5;
                        odemeplani.Add(odeme);

                        odeme = new OdemePlaniDto();
                        odeme.TaksitNo = 6;
                        odeme.Tarih = fiyat.Tarih6;
                        odeme.Tutar = fiyat.Taksit6;
                        odemeplani.Add(odeme);

                        odeme = new OdemePlaniDto();
                        odeme.TaksitNo = 7;
                        odeme.Tarih = fiyat.Tarih7;
                        odeme.Tutar = fiyat.Taksit7;
                        odemeplani.Add(odeme);

                        odeme = new OdemePlaniDto();
                        odeme.TaksitNo = 8;
                        odeme.Tarih = fiyat.Tarih8;
                        odeme.Tutar = fiyat.Taksit8;
                        odemeplani.Add(odeme);

                        odeme = new OdemePlaniDto();
                        odeme.TaksitNo = 9;
                        odeme.Tarih = fiyat.Tarih9;
                        odeme.Tutar = fiyat.Taksit9;
                        odemeplani.Add(odeme);

                        odeme = new OdemePlaniDto();
                        odeme.TaksitNo = 10;
                        odeme.Tarih = fiyat.Tarih10;
                        odeme.Tutar = fiyat.Taksit10;
                        odemeplani.Add(odeme);
                        break;
                    default:
                        odeme = new OdemePlaniDto();
                        odeme.TaksitNo = 1;
                        odeme.Tarih = fiyat.Tarih1;
                        odeme.Tutar = fiyat.Taksit1;
                        odemeplani.Add(odeme);

                        odeme = new OdemePlaniDto();
                        odeme.TaksitNo = 2;
                        odeme.Tarih = fiyat.Tarih2;
                        odeme.Tutar = fiyat.Taksit2;
                        odemeplani.Add(odeme);

                        odeme = new OdemePlaniDto();
                        odeme.TaksitNo = 3;
                        odeme.Tarih = fiyat.Tarih3;
                        odeme.Tutar = fiyat.Taksit3;
                        odemeplani.Add(odeme);

                        odeme = new OdemePlaniDto();
                        odeme.TaksitNo = 4;
                        odeme.Tarih = fiyat.Tarih4;
                        odeme.Tutar = fiyat.Taksit4;
                        odemeplani.Add(odeme);

                        odeme = new OdemePlaniDto();
                        odeme.TaksitNo = 5;
                        odeme.Tarih = fiyat.Tarih5;
                        odeme.Tutar = fiyat.Taksit5;
                        odemeplani.Add(odeme);

                        odeme = new OdemePlaniDto();
                        odeme.TaksitNo = 6;
                        odeme.Tarih = fiyat.Tarih6;
                        odeme.Tutar = fiyat.Taksit6;
                        odemeplani.Add(odeme);

                        odeme = new OdemePlaniDto();
                        odeme.TaksitNo = 7;
                        odeme.Tarih = fiyat.Tarih7;
                        odeme.Tutar = fiyat.Taksit7;
                        odemeplani.Add(odeme);

                        odeme = new OdemePlaniDto();
                        odeme.TaksitNo = 8;
                        odeme.Tarih = fiyat.Tarih8;
                        odeme.Tutar = fiyat.Taksit8;
                        odemeplani.Add(odeme);

                        odeme = new OdemePlaniDto();
                        odeme.TaksitNo = 9;
                        odeme.Tarih = fiyat.Tarih9;
                        odeme.Tutar = fiyat.Taksit9;
                        odemeplani.Add(odeme);

                        odeme = new OdemePlaniDto();
                        odeme.TaksitNo = 10;
                        odeme.Tarih = fiyat.Tarih10;
                        odeme.Tutar = fiyat.Taksit10;
                        odemeplani.Add(odeme);
                        break;
                }

                fiyat.OdemePlani = odemeplani;
                return fiyat;
            }
        }

        public async Task<FiyatDto> OdemePlaniOlustur(FiyatRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.TcKimlik))
                throw new Exception("Öğrenci TCKN gönderilmelidir");

            var fiyat = await GetFiyatByDonem(request); 

            if (fiyat == null)
                throw new Exception("Ödeme Planı bulunamadı");

            var cardref = await CardRefAl(request.TcKimlik);

            if (cardref == 0)
                throw new Exception("Cari Kart bulunamadı");

            var ogrenciref = await OgrenciRefAl(cardref);

            if (ogrenciref == 0)
                throw new Exception("Öğrenci Kartı bulunamadı");

            var ogrencidonemref = await OgrenciDonemRefAl(ogrenciref,request.Sezon,request.Seviye);

            if (ogrencidonemref != 0)
                throw new Exception("Öğrenci dönem kaydı mevcut, yeni kayıt yapılamaz");

            await KullaniciControl(request.KullaniciKodu);

            var ogrencidonem = await OgrenciDonemOlustur(ogrenciref,request);
            var ogrencidonemodeme = await OgrenciDonemOdemeOlustur(ogrencidonem, fiyat, request);
            await SaveDb(ogrencidonem, ogrencidonemodeme);

            return fiyat;
        }
        public async Task<int> CardRefAl(string tckno)
        {
            using (var db = new SqlConnection(configuration.GetConnectionString(CrmConsts.LogoConnectionString)))
            {
                string LogoDbName = configuration["LogoDbName"];
                string LogoFirmaNo = configuration["LogoFirmaNo"];
                string CLCARD = LogoDbName + ".dbo.LG_" + LogoFirmaNo + "_CLCARD";
                string CLCARDXT = LogoDbName + ".dbo.LG_XT1015_" + LogoFirmaNo;

                string sqlstring = "SELECT top 1 A.LOGICALREF " +
                    "FROM " + CLCARD + " A WITH(NOLOCK) " +
                    "INNER JOIN " + CLCARDXT + " B WITH(NOLOCK) ON A.LOGICALREF = B.PARLOGREF " +
                    "WHERE B.OGRENCI_TC LIKE '" + tckno + "'";

                return await db.QuerySingleOrDefaultAsync<int>(sqlstring);
            }
        }

        public async Task<int> OgrenciRefAl(int cardref)
        {
            using (var db = new SqlConnection(configuration.GetConnectionString(CrmConsts.CompanyConnectionString)))
            {
                string sqlstring = "SELECT top 1 A.logref " +
                    "FROM ogrenci_kart A WITH(NOLOCK) " +
                    "WHERE A.cardref = " + cardref;

                return await db.QuerySingleOrDefaultAsync<int>(sqlstring);
            }
        }

        public async Task<int> OgrenciDonemRefAl(int ogrenciref, string donem, string sinif)
        {
            using (var db = new SqlConnection(configuration.GetConnectionString(CrmConsts.CompanyConnectionString)))
            {
                string sqlstring = "select top 1 a.logref " +
                    "FROM ogrenci_donem a WITH(NOLOCK) " +
                    "inner join UmotaCRM_OKUL.dbo.sis_sabitler_detay b with(nolock) on a.donemref = b.sabit_detay_id " +
                    "inner join UmotaCRM_OKUL.dbo.sis_sabitler_detay c with(nolock) on a.sinifref = c.sabit_detay_id " +
                    " WHERE a.ogrenciref = " + ogrenciref +
                    " and a.status < 2 " +
                    " and b.adi like '" + donem + "'" +
                    " and c.kodu like '" + sinif + "'";

                return await db.QuerySingleOrDefaultAsync<int>(sqlstring);
            }
        }

        public async Task<int> OgrenciDonemNoAl(int ogrenciref)
        {
            using (var db = new SqlConnection(configuration.GetConnectionString(CrmConsts.CompanyConnectionString)))
            {
                string sqlstring = "select max(isnull(a.donemno,0)) " +
                    " FROM ogrenci_donem a WITH(NOLOCK) " +
                    " WHERE a.ogrenciref = " + ogrenciref +
                    " and a.status < 2 ";

                var say = await db.QuerySingleOrDefaultAsync<int>(sqlstring);
                return say + 1;
            }
        }

        public async Task<OgrenciDonem> OgrenciDonemOlustur(int ogrenciref, FiyatRequestDto request)
        {
            var ogrencidonem = new OgrenciDonem();

            ogrencidonem.logref = await RefNoAl("ogrenci_donem");
            ogrencidonem.ogrenciref = ogrenciref;
            ogrencidonem.donemno = await OgrenciDonemNoAl(ogrenciref);
            ogrencidonem.donemref = await GetSabitDetayIDByName(100, request.Sezon);
            ogrencidonem.sinifref = await GetSabitDetayIDByCode(102, request.Seviye);
            ogrencidonem.yatiliref = await GetSabitDetayIDByCode(101, request.Yatili.ToString());
            ogrencidonem.gunduzluref = await GetSabitDetayIDByCode(101, "10"); // Gündüzlü Referansı
            ogrencidonem.bursref = await GetSabitDetayIDByCode(104, request.BursOrani.ToString());
            ogrencidonem.odemeref = await GetSabitDetayIDByCode(105, request.OdemeSekli.ToString());
            ogrencidonem.bankaref = 0;
            ogrencidonem.bankahesapno = "";
            ogrencidonem.bankahesapadi = "";
            ogrencidonem.insuser = request.KullaniciKodu;
            ogrencidonem.kdvsiz = request.BursOrani == 100 ? true : false;

            return ogrencidonem;
        }
        public async Task<List<OgrenciDonemOdeme>> OgrenciDonemOdemeOlustur(OgrenciDonem donem, FiyatDto fiyat, FiyatRequestDto request)
        {
            var donemodemelist = new List<OgrenciDonemOdeme>();

            // Veli Ödeme Bölümü (%100 burs yoksa)
            if (request.BursOrani != 100)
            {
                foreach (OdemePlaniDto odemesatiri in fiyat.OdemePlani)
                {
                    var Egitim_Tutar = odemesatiri.Tutar;
                    double Yatili_Tutar = 0;
                    if (request.Yatili == 20 || request.Yatili == 30)
                    {
                        Egitim_Tutar = await TabloTaksitTutarAl(donem, odemesatiri.TaksitNo, request.BursOrani, donem.gunduzluref);
                        Yatili_Tutar = odemesatiri.Tutar.Value - Egitim_Tutar.Value;
                    }

                    var donemodeme = new OgrenciDonemOdeme();
                    donemodeme.logref = await RefNoAl("ogrenci_donem_odeme");
                    donemodeme.ogrenciref = donem.ogrenciref;
                    donemodeme.ogrencidonemref = donem.logref;
                    donemodeme.odemetipi = 1;
                    donemodeme.taksitno = odemesatiri.TaksitNo;
                    donemodeme.taksittar = odemesatiri.Tarih;
                    donemodeme.tutar_veli = odemesatiri.Tutar;
                    donemodeme.tutar_egitim = Egitim_Tutar;
                    donemodeme.tutar_yatili = Yatili_Tutar;
                    donemodeme.kdvsiz = false;
                    donemodeme.insuser = request.KullaniciKodu;
                    donemodemelist.Add(donemodeme);
                }
            }

            // Veli Fatura Bölümü (%100 burs yoksa)
            if (request.BursOrani != 100)
            {
                double Fatura_Tutar = fiyat.Toplam.Value / 10;
                double Egitim_Tutar = Fatura_Tutar;
                double Yatili_Tutar = 0;
                if (request.Yatili == 20 || request.Yatili == 30)
                {
                    Egitim_Tutar = await TabloToplamTutarAl(donem, request.BursOrani, donem.gunduzluref) / 10;
                    Yatili_Tutar = Fatura_Tutar - Egitim_Tutar;
                }

                for (int i = 1; i < 11; i++)
                {
                    var tarih = await FaturaTarihiAl(donem.donemref,i);
                    var donemodeme = new OgrenciDonemOdeme();
                    donemodeme.logref = await RefNoAl("ogrenci_donem_odeme");
                    donemodeme.ogrenciref = donem.ogrenciref;
                    donemodeme.ogrencidonemref = donem.logref;
                    donemodeme.odemetipi = 3;
                    donemodeme.taksitno = i;
                    donemodeme.taksittar = tarih;
                    donemodeme.tutar_fatura = Fatura_Tutar;
                    donemodeme.tutar_fatura_egitim = Egitim_Tutar;
                    donemodeme.tutar_fatura_yatili = Yatili_Tutar;
                    donemodeme.kdvsiz = false;
                    donemodeme.insuser = request.KullaniciKodu;
                    donemodemelist.Add(donemodeme);
                }
            }

            // Burs Bölümü
            if (request.BursOrani > 0)
            {
                if (request.BursOrani == 100) // %100 burslar KDVsiz tek fatura
                {
                    double Fatura_Tutar = fiyat.Toplam.Value;
                    double Egitim_Tutar = Fatura_Tutar;
                    double Yatili_Tutar = 0;
                    if (request.Yatili == 20 || request.Yatili == 30)
                    {
                        Egitim_Tutar = await TabloToplamTutarAl(donem, 0, donem.gunduzluref);
                        Yatili_Tutar = Fatura_Tutar - Egitim_Tutar;
                    }

                    var tarih = await BursFaturaTarihiAl(donem.donemref, 1);
                    var donemodeme = new OgrenciDonemOdeme();
                    donemodeme.logref = await RefNoAl("ogrenci_donem_odeme");
                    donemodeme.ogrenciref = donem.ogrenciref;
                    donemodeme.ogrencidonemref = donem.logref;
                    donemodeme.odemetipi = 2;
                    donemodeme.taksitno = 1;
                    donemodeme.taksittar = tarih;
                    donemodeme.tutar_burs = Math.Round(Fatura_Tutar / 1.08,2);
                    donemodeme.tutar_burs_egitim = Math.Round(Egitim_Tutar / 1.08, 2);
                    donemodeme.tutar_burs_yatili = Math.Round(Yatili_Tutar / 1.08, 2);
                    donemodeme.kdvsiz = true;
                    donemodeme.insuser = request.KullaniciKodu;
                    donemodemelist.Add(donemodeme);
                } else
                {
                    double Burssuz_Tutar = await TabloToplamTutarAl(donem, 0, donem.yatiliref);
                    double Fatura_Tutar = Burssuz_Tutar - fiyat.Toplam.Value;
                    double Egitim_Tutar = Fatura_Tutar;
                    double Yatili_Tutar = 0;
                    if (request.Yatili == 20 || request.Yatili == 30)
                    {
                        Egitim_Tutar = await TabloToplamTutarAl(donem, 0, donem.gunduzluref);
                        Yatili_Tutar = Fatura_Tutar - Egitim_Tutar;
                    }

                    for (int i = 1; i < 11; i++)
                    {
                        var tarih = await BursFaturaTarihiAl(donem.donemref, i);
                        var donemodeme = new OgrenciDonemOdeme();
                        donemodeme.logref = await RefNoAl("ogrenci_donem_odeme");
                        donemodeme.ogrenciref = donem.ogrenciref;
                        donemodeme.ogrencidonemref = donem.logref;
                        donemodeme.odemetipi = 2;
                        donemodeme.taksitno = i;
                        donemodeme.taksittar = tarih;
                        donemodeme.tutar_burs = Fatura_Tutar / 10;
                        donemodeme.tutar_burs_egitim = Egitim_Tutar / 10;
                        donemodeme.tutar_burs_yatili = Yatili_Tutar / 10;
                        donemodeme.kdvsiz = false;
                        donemodeme.insuser = request.KullaniciKodu;
                        donemodemelist.Add(donemodeme);
                    }
                }
            }
            return donemodemelist;
        }

        public async Task SaveDb(OgrenciDonem donem,List<OgrenciDonemOdeme> odemeler)
        {
            using (var db = new SqlConnection(configuration.GetConnectionString(CrmConsts.CompanyConnectionString)))
            {
                string sqlstring = "select top 1 hizmetref, hizmetref2, hizmetref3 from hizmet_kodlari with(nolock) where donemref = " + donem.donemref;

                var hizmetkodlari = await db.QuerySingleOrDefaultAsync<HizmetKodlari>(sqlstring, commandType: System.Data.CommandType.Text);

                db.Open();
                var transaction = await db.BeginTransactionAsync();
                try
                {

                    sqlstring = "INSERT INTO [dbo].[ogrenci_donem] " +
                        "(logref,ogrenciref,donemno,donemref,sinifref,yatiliref,bursref,odemeref,bankaref,bankahesapno,bankahesapadi " +
                        ",kdvsiz,status,insuser,insdate) " +
                        "VALUES " +
                        "(@logref,@ogrenciref,@donemno,@donemref,@sinifref,@yatiliref,@bursref,@odemeref,@bankaref,@bankahesapno,@bankahesapadi " +
                        ",@kdvsiz,0,@insuser,GetDate()) ";

                    var p = new DynamicParameters();
                    p.Add("@logref", donem.logref);
                    p.Add("@ogrenciref", donem.ogrenciref);
                    p.Add("@donemno", donem.donemno);
                    p.Add("@donemref", donem.donemref);
                    p.Add("@sinifref", donem.sinifref);
                    p.Add("@yatiliref", donem.yatiliref);
                    p.Add("@bursref", donem.bursref);
                    p.Add("@odemeref", donem.odemeref);
                    p.Add("@bankaref", donem.bankaref);
                    p.Add("@bankahesapno", donem.bankahesapno);
                    p.Add("@bankahesapadi", donem.bankahesapadi);
                    p.Add("@insuser", donem.insuser);
                    p.Add("@kdvsiz", donem.kdvsiz);

                    await db.ExecuteAsync(sqlstring, p, commandType: CommandType.Text, transaction:transaction);

                    sqlstring = "INSERT INTO [dbo].[ogrenci_donem_odeme] " +
                        "(logref,ogrenciref,ogrencidonemref,odemetipi,taksitno,taksittar,tutar_veli,tutar_egitim,tutar_yatili,tutar_burs " +
                        ",tutar_burs_egitim,tutar_burs_yatili,tutar_fatura,tutar_fatura_egitim,tutar_fatura_yatili,kdvsiz,status,insuser,insdate" +
                        ",hizmetref, hizmetref2, hizmetref3 ) " +
                        "VALUES " +
                        "(@logref,@ogrenciref,@ogrencidonemref,@odemetipi,@taksitno,@taksittar,@tutar_veli,@tutar_egitim,@tutar_yatili,@tutar_burs " +
                        ",@tutar_burs_egitim,@tutar_burs_yatili,@tutar_fatura,@tutar_fatura_egitim,@tutar_fatura_yatili " +
                        ",@kdvsiz,0,@insuser,GetDate(),@hizmetref, @hizmetref2, @hizmetref3) ";

                    foreach (OgrenciDonemOdeme odeme in odemeler)
                    {
                        p = new DynamicParameters();
                        p.Add("@logref", odeme.logref);
                        p.Add("@ogrenciref", odeme.ogrenciref);
                        p.Add("@ogrencidonemref", odeme.ogrencidonemref);
                        p.Add("@odemetipi", odeme.odemetipi);
                        p.Add("@taksitno", odeme.taksitno);
                        p.Add("@taksittar", odeme.taksittar);
                        p.Add("@tutar_veli", odeme.tutar_veli);
                        p.Add("@tutar_egitim", odeme.tutar_egitim);
                        p.Add("@tutar_yatili", odeme.tutar_yatili);
                        p.Add("@tutar_burs", odeme.tutar_burs);
                        p.Add("@tutar_burs_egitim", odeme.tutar_burs_egitim);
                        p.Add("@tutar_burs_yatili", odeme.tutar_burs_yatili);
                        p.Add("@tutar_fatura", odeme.tutar_fatura);
                        p.Add("@tutar_fatura_egitim", odeme.tutar_fatura_egitim);
                        p.Add("@tutar_fatura_yatili", odeme.tutar_fatura_yatili);
                        p.Add("@insuser", odeme.insuser);
                        p.Add("@kdvsiz", donem.kdvsiz);

                        switch (odeme.odemetipi)
                        {
                            case 2:
                                p.Add("@hizmetref", 0);
                                p.Add("@hizmetref2", hizmetkodlari.hizmetref2);
                                p.Add("@hizmetref3", hizmetkodlari.hizmetref3);
                                break;
                            case 3:
                                p.Add("@hizmetref", hizmetkodlari.hizmetref);
                                p.Add("@hizmetref2", 0);
                                p.Add("@hizmetref3", hizmetkodlari.hizmetref3);
                                break;
                            default:
                                p.Add("@hizmetref", 0);
                                p.Add("@hizmetref2", 0);
                                p.Add("@hizmetref3", 0);
                                break;
                        }

                        await db.ExecuteAsync(sqlstring, p, commandType: CommandType.Text, transaction: transaction);
                    }

                    await transaction.CommitAsync();
                }
                catch (Exception exception)
                {
                    await transaction.RollbackAsync();
                    throw new Exception($"Error encountered whilst executing  SQL: {sqlstring}, Message: {exception.Message}");
                }
            }
        }
        public async Task<double> TabloTaksitTutarAl(OgrenciDonem donem, int taksitno, int bursoran, int yatiliref)
        {
            using (var db = new SqlConnection(configuration.GetConnectionString(CrmConsts.CompanyConnectionString)))
            {
                string sqlstring = "select tutar_taksit"+ taksitno +
                    " FROM fiyat_tablo a WITH(NOLOCK) " +
                    " WHERE a.donemref = " + donem.donemref +
                    " and a.sinifref = " + donem.sinifref +
                    " and a.odemeref = " + donem.odemeref +
                    " and a.yatiliref = " + yatiliref +
                    " and a.bursyuz = " + bursoran +
                    " and a.status < 2 ";

                var tutar = await db.QuerySingleOrDefaultAsync<double>(sqlstring);
                return tutar;
            }
        }
        public async Task<double> TabloToplamTutarAl(OgrenciDonem donem, int bursoran, int yatiliref)
        {
            using (var db = new SqlConnection(configuration.GetConnectionString(CrmConsts.CompanyConnectionString)))
            {
                string sqlstring = "select tutar_toplam" +
                    " FROM fiyat_tablo a WITH(NOLOCK) " +
                    " WHERE a.donemref = " + donem.donemref +
                    " and a.sinifref = " + donem.sinifref +
                    " and a.odemeref = " + donem.odemeref +
                    " and a.yatiliref = " + yatiliref +
                    " and a.bursyuz = " + bursoran +
                    " and a.status < 2 ";

                var tutar = await db.QuerySingleOrDefaultAsync<double>(sqlstring);
                return tutar;
            }
        }
        public async Task<DateTime> FaturaTarihiAl(int donemref, int taksitno)
        {
            using (var db = new SqlConnection(configuration.GetConnectionString(CrmConsts.CompanyConnectionString)))
            {
                string sqlstring = "select taksittar_fatura" +
                    " FROM burs_odeme_tarihleri a WITH(NOLOCK) " +
                    " WHERE a.donemref = " + donemref +
                    " and a.taksitno = " + taksitno;

                var tarih = await db.QuerySingleOrDefaultAsync<DateTime>(sqlstring);
                return tarih;
            }
        }
        public async Task<DateTime> BursFaturaTarihiAl(int donemref, int taksitno)
        {
            using (var db = new SqlConnection(configuration.GetConnectionString(CrmConsts.CompanyConnectionString)))
            {
                string sqlstring = "select taksittar_burs" +
                    " FROM burs_odeme_tarihleri a WITH(NOLOCK) " +
                    " WHERE a.donemref = " + donemref +
                    " and a.taksitno = " + taksitno;

                var tarih = await db.QuerySingleOrDefaultAsync<DateTime>(sqlstring);
                return tarih;
            }
        }
        public async Task<int> RefNoAl(string tablename)
        {
            using (var db = new SqlConnection(configuration.GetConnectionString(CrmConsts.MasterConnectionString)))
            {
                var p = new DynamicParameters();
                p.Add("@tablename", tablename);
                p.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                await db.ExecuteAsync("RefNoAl", p, commandType: CommandType.StoredProcedure);

                int c = p.Get<int>("@ReturnValue");

                return c;
            }
        }
        public async Task<int> GetSabitDetayIDByName(int tip ,string adi)
        {
            using (var db = new SqlConnection(configuration.GetConnectionString(CrmConsts.MasterConnectionString)))
            {
                var p = new DynamicParameters();
                p.Add("@tip", tip);
                p.Add("@adi", adi);
                p.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                await db.ExecuteAsync("GetSabitDetayIDByName", p, commandType: CommandType.StoredProcedure);

                int c = p.Get<int>("@ReturnValue");

                return c;
            }
        }
        public async Task<int> GetSabitDetayIDByCode(int tip, string kodu)
        {
            using (var db = new SqlConnection(configuration.GetConnectionString(CrmConsts.MasterConnectionString)))
            {
                var p = new DynamicParameters();
                p.Add("@tip", tip);
                p.Add("@kodu", kodu);
                p.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                await db.ExecuteAsync("GetSabitDetayIDByCode", p, commandType: CommandType.StoredProcedure);

                int c = p.Get<int>("@ReturnValue");

                return c;
            }
        }
        public async Task<FiyatRequestDto> OdemePlaniGeriAl(FiyatRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.TcKimlik))
                throw new Exception("Öğrenci TCKN gönderilmelidir");

            var cardref = await CardRefAl(request.TcKimlik);

            if (cardref == 0)
                throw new Exception("Cari Kart bulunamadı");

            var ogrenciref = await OgrenciRefAl(cardref);

            if (ogrenciref == 0)
                throw new Exception("Öğrenci Kartı bulunamadı");

            var ogrencidonemref = await OgrenciDonemRefAl(ogrenciref, request.Sezon, request.Seviye);

            if (ogrencidonemref == 0)
                throw new Exception("Öğrenci dönem kaydı bulunamadı");

            await KullaniciControl(request.KullaniciKodu);
            await DeleteControls(ogrencidonemref, request.KullaniciKodu);
            await DeleteDb(ogrencidonemref, request.KullaniciKodu);

            return request;
        }
        public async Task DeleteControls(int ogrencidonemref, string kullanicikodu)
        {
            using (var db = new SqlConnection(configuration.GetConnectionString(CrmConsts.CompanyConnectionString)))
            {
                var sqlstring = "Select logref, insuser, insdate, upduser, upddate from [dbo].[ogrenci_donem] with(nolock) where logref = " + ogrencidonemref;

                var ogrencidonem = await db.QuerySingleOrDefaultAsync<OgrenciDonem>(sqlstring, commandType: System.Data.CommandType.Text);
                if (ogrencidonem == null)
                    throw new Exception("Öğrenci dönem kaydı bulunamadı");

                if (!ogrencidonem.insuser.Equals(kullanicikodu))
                    throw new Exception("Öğrenci dönem kaydını siz oluşturmadınız, bu kaydı geri alamazsınız");

                if (!string.IsNullOrWhiteSpace(ogrencidonem.upduser))
                    throw new Exception("Öğrenci dönem kaydı değiştirilmiş, bu kaydı geri alamazsınız");

                if (ogrencidonem.insdate.Value.AddDays(3) < DateTime.Now)
                    throw new Exception("Öğrenci dönem kaydı üzerinden 3 gün geçmiş, bu kaydı geri alamazsınız");
            }
        }
        public async Task DeleteDb(int ogrencidonemref, string kullanicikodu)
        {
            using (var db = new SqlConnection(configuration.GetConnectionString(CrmConsts.CompanyConnectionString)))
            {
                string sqlstring = "";
                db.Open();
                var transaction = await db.BeginTransactionAsync();
                try
                {
                    sqlstring = "Update [dbo].[ogrenci_donem] Set status = 2, upddate=GetDate(), upduser= @upduser where logref = @ogrencidonemref ";

                    var p = new DynamicParameters();
                    p.Add("@ogrencidonemref", ogrencidonemref);
                    p.Add("@upduser", kullanicikodu);

                    await db.ExecuteAsync(sqlstring, p, commandType: CommandType.Text, transaction: transaction);

                    sqlstring = "Update [dbo].[ogrenci_donem_odeme] Set status = 2, upddate=GetDate(), upduser= @upduser where ogrencidonemref = @ogrencidonemref ";

                    await db.ExecuteAsync(sqlstring, p, commandType: CommandType.Text, transaction: transaction);

                    await transaction.CommitAsync();
                }
                catch (Exception exception)
                {
                    await transaction.RollbackAsync();
                    throw new Exception($"Error encountered whilst executing  SQL: {sqlstring}, Message: {exception.Message}");
                }
            }
        }
        public async Task KullaniciControl(string kullanicikodu)
        {
            using (var db = new SqlConnection(configuration.GetConnectionString(CrmConsts.MasterConnectionString)))
            {
                var sqlstring = "Select count(*) from [dbo].[sis_kullanici] with(nolock) where kullanici_kodu like '" + kullanicikodu + "' and isnull(kullanici_iptal,0) = 0";

                var say = await db.QuerySingleOrDefaultAsync<int>(sqlstring);
                if (say == 0)
                    throw new Exception("Kullanıcı kodu bulunamadı");
            }
        }
    }
}
