using BelediyeEnvanter.Models;
using Microsoft.EntityFrameworkCore;

namespace BelediyeEnvanter.Data;

public static class SeedData
{
    public static void Initialize(AppDbContext db)
    {
        db.Database.EnsureCreated();
        db.Database.ExecuteSqlRaw("CREATE TABLE IF NOT EXISTS Categories (Id INTEGER NOT NULL CONSTRAINT PK_Categories PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL)");
        db.Database.ExecuteSqlRaw("CREATE TABLE IF NOT EXISTS AuditLogs (Id INTEGER NOT NULL CONSTRAINT PK_AuditLogs PRIMARY KEY AUTOINCREMENT, Kullanici TEXT NOT NULL, Islem TEXT NOT NULL, Aciklama TEXT NOT NULL, Tarih TEXT NOT NULL)");
        try { db.Database.ExecuteSqlRaw("ALTER TABLE Assets ADD COLUMN KullananPersonel TEXT NOT NULL DEFAULT ''"); } catch (Microsoft.Data.Sqlite.SqliteException) { }
        var kategoriler = new[] { "Bilişim", "Elektronik", "Mobilya", "İklimlendirme", "Ofis Ekipmanı", "Makine ve Teçhizat", "Ulaşım", "Güvenlik", "Diğer" };
        foreach (var ad in kategoriler) if (!db.Categories.Any(x => x.Name == ad)) db.Categories.Add(new Category { Name = ad });
        db.SaveChanges();
        var birimler = new Dictionary<string, string[]>
        {
            ["Afet İşleri ve Risk Yönetimi Daire Başkanlığı"] = ["Afet Planlama ve Eğitim Müdürlüğü", "Afet Risk Azaltma ve Deprem Müdürlüğü"],
            ["Akıllı Şehircilik ve Bilgi İşlem Daire Başkanlığı"] = ["Yazılım Müdürlüğü", "Bilgi Teknolojileri Müdürlüğü", "Coğrafi Bilgi Sistemleri Müdürlüğü", "Elektronik Sistemler ve Haberleşme Müdürlüğü", "Bilim ve Toplum Müdürlüğü"],
            ["Basın Yayın ve Halkla İlişkiler Daire Başkanlığı"] = ["Halkla İlişkiler Müdürlüğü", "Basın Yayın Müdürlüğü"],
            ["Çevre Koruma ve Kontrol Daire Başkanlığı"] = ["Mezarlıklar Müdürlüğü", "Çevre Koruma ve Kontrol Müdürlüğü"],
            ["Destek Hizmetleri Daire Başkanlığı"] = ["Doğrudan Temin Müdürlüğü", "Hizmet ve Mal Alım İhale Müdürlüğü"],
            ["Etüt ve Projeler Daire Başkanlığı"] = ["Yapım İşleri İhale Müdürlüğü", "Kesin Hesap Müdürlüğü", "Proje ve Planlama Müdürlüğü", "Yapı Kontrol Müdürlüğü"],
            ["Fen İşleri Daire Başkanlığı"] = ["Yol Yapım ve Planlama Müdürlüğü", "Asfalt Üretim Müdürlüğü", "AYKOME Müdürlüğü", "Bakım Onarım Müdürlüğü", "Yapı İşleri Müdürlüğü"],
            ["İklim Değişikliği ve Sıfır Atık Daire Başkanlığı"] = ["Atık Yönetimi ve Sıfır Atık Müdürlüğü", "İklim Değişikliği ve Enerji Verimliliği Müdürlüğü", "Hafriyat Yönetimi ve GSM Ruhsat Müdürlüğü"],
            ["İmar ve Şehircilik Daire Başkanlığı"] = ["Harita ve Kadastro Müdürlüğü", "İmar ve Şehircilik Müdürlüğü", "Tapu ve İstimlak Müdürlüğü", "İmar Uygulama Müdürlüğü"],
            ["İnsan Kaynakları ve Eğitim Daire Başkanlığı"] = ["İnsan Kaynakları Müdürlüğü", "Eğitim Müdürlüğü"],
            ["İşletmeler ve İştirakler Daire Başkanlığı"] = ["Teknik Bakım ve Onarım Müdürlüğü", "İdari İşler Müdürlüğü", "İşletme ve İştirakler Koordinasyon Müdürlüğü"],
            ["İtfaiye Daire Başkanlığı"] = ["İtfaiye Önlem ve Eğitim Müdürlüğü", "İtfaiye Müdahale ve Arama Kurtarma Müdürlüğü"],
            ["Kentsel Dönüşüm Daire Başkanlığı"] = ["Kentsel Dönüşüm Müdürlüğü", "Proje Uygulama ve Uzlaşma Müdürlüğü", "Proje Geliştirme Müdürlüğü"],
            ["Kent Tarihi, Tanıtım ve Turizm Daire Başkanlığı"] = ["KUDEB Müdürlüğü", "Turizm Müdürlüğü", "Kültür Varlıkları Müdürlüğü", "Müzeler ve Tarihi Yapılar Müdürlüğü"],
            ["Kırsal Hizmetler Daire Başkanlığı"] = ["Kırsal Hizmetler Müdürlüğü", "İlçeler Yol Müdürlüğü", "Araştırma ve Geliştirme Müdürlüğü", "Tarımsal Hizmetler Müdürlüğü"],
            ["Kültür, Sanat ve Sosyal İşler Daire Başkanlığı"] = ["Konservatuvar Müdürlüğü", "Kültür ve Sosyal İşler Müdürlüğü", "Kütüphaneler Müdürlüğü"],
            ["Makine İkmal Bakım ve Onarım Daire Başkanlığı"] = ["Makine İşletme Müdürlüğü", "Akaryakıt Dağıtım Müdürlüğü", "Sevk ve Koordinasyon Müdürlüğü"],
            ["Mali Hizmetler Daire Başkanlığı"] = ["Bütçe Müdürlüğü", "Gelir Müdürlüğü", "Strateji Geliştirme Müdürlüğü", "İcra ve Tahakkuk Müdürlüğü", "Hesap İşleri Müdürlüğü"],
            ["Muhtarlık İşleri Daire Başkanlığı"] = ["Muhtarlık İşleri Müdürlüğü", "İş Sağlığı ve Güvenliği Müdürlüğü"],
            ["Park ve Bahçeler Daire Başkanlığı"] = ["Yeşil Alanlar Proje ve Uygulama Müdürlüğü", "Millet Bahçesi ve Parklar Müdürlüğü", "Park ve Bahçeler İdari İşler Müdürlüğü", "Hobi Bahçeleri Müdürlüğü"],
            ["Sivil Toplum Kuruluşları ile İlişkiler Daire Başkanlığı"] = ["Sivil Toplum Kuruluşları ile İlişkiler Müdürlüğü"],
            ["Sosyal Hizmetler Daire Başkanlığı"] = ["Engelliler Koordinasyon Müdürlüğü", "Huzurevi Müdürlüğü", "Sosyal Hizmetler Müdürlüğü"],
            ["Ulaşım Daire Başkanlığı"] = ["Akıllı Ulaşım Sistemleri Müdürlüğü", "Toplu Taşıma Müdürlüğü", "Raylı Sistem Yapım Müdürlüğü", "Trafik Hizmetleri Müdürlüğü", "UKOME Müdürlüğü"],
            ["Veteriner İşleri Daire Başkanlığı"] = ["Veteriner İşleri Müdürlüğü", "Doğal Hayatı Koruma ve Hayvanat Bahçesi Müdürlüğü"],
            ["Yazı İşleri ve Kararlar Daire Başkanlığı"] = ["Yazı ve Kararlar Müdürlüğü"],
            ["Zabıta Daire Başkanlığı"] = ["Zabıta Müdürlüğü"]
        };
        foreach (var item in birimler)
        {
            var d = db.Departments.FirstOrDefault(x => x.Name == item.Key) ?? new Department { Name = item.Key };
            if (d.Id == 0) db.Departments.Add(d);
            db.SaveChanges();
            foreach (var mudurluk in item.Value)
                if (!db.Directorates.Any(x => x.Name == mudurluk && x.DepartmentId == d.Id)) db.Directorates.Add(new Directorate { Name = mudurluk, DepartmentId = d.Id });
        }
        db.SaveChanges();
        var yazilim = db.Directorates.First(x => x.Name == "Yazılım Müdürlüğü");
        var odalar = new[] { ("101", "1"), ("102", "1"), ("201", "2"), ("202", "2"), ("301", "3"), ("302", "3"), ("303", "3"), ("304", "3"), ("305", "3"), ("401", "4") };
        foreach (var (no, kat) in odalar)
            if (!db.Rooms.Any(x => x.RoomNumber == no && x.Building == "Başkanlık Binası")) db.Rooms.Add(new Room { RoomNumber = no, Building = "Başkanlık Binası", Floor = kat, DirectorateId = yazilim.Id });
        db.SaveChanges();
        var odaListesi = db.Rooms.OrderBy(x => x.Id).Take(10).ToList();
        var demirbasAdlari = new[] { "Masaüstü Bilgisayar", "Dizüstü Bilgisayar", "Monitör", "Yazıcı", "Klima", "Telefon", "Çalışma Masası", "Ofis Koltuğu", "Dosya Dolabı", "Projeksiyon Cihazı" };
        var personeller = new[] { "Ahmet Yılmaz", "Ayşe Demir", "Mehmet Kaya", "Zeynep Şahin", "Mustafa Çelik" };
        for (var i = db.Assets.Count(); i < 30; i++)
        {
            var ad = demirbasAdlari[i % demirbasAdlari.Length];
            db.Assets.Add(new Asset { Name = ad, Category = ad is "Çalışma Masası" or "Ofis Koltuğu" or "Dosya Dolabı" ? "Mobilya" : "Elektronik", BrandModel = "Örnek Marka Model", SerialNumber = $"SN-{i + 1:0000}", InventoryNumber = $"ZM-{i + 1:0000}", ResponsiblePerson = personeller[i % personeller.Length], KullananPersonel = personeller[(i + 1) % personeller.Length], RoomId = odaListesi[i % odaListesi.Count].Id, Status = "Aktif" });
        }
        db.SaveChanges();
    }
}
