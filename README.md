# BabsKitapEvi

.NET 9 Clean Architecture yaklaşımlı bir kitap evi backend projesi. Katmanlar:

- Entities: Entity ve DTO tanımları
- DataAccess: EF Core DbContext, Migrations ve Seed
- Business: Servisler, arayüzler ve AutoMapper profilleri
- WebAPI: ASP.NET Core Web API katmanı, middleware ve controllerlar

## Gereksinimler

- .NET SDK 9.x
- SQL Server (LocalDB veya başka bir instance)
- Git (opsiyonel)

## Hızlı Başlangıç

1. Bağımlılıkları geri yükle

```
dotnet restore
```

2. Veritabanı migrasyonlarını uygula

```
dotnet ef database update --project DataAccess --startup-project WebAPI
```

3. Uygulamayı çalıştır

```
dotnet run --project WebAPI
```

4. Swagger arayüzü

- Çalıştıktan sonra tipik olarak: http://localhost:5000/swagger veya http://localhost:5175/swagger
- Launch profile ve portlar ortamınıza göre değişebilir.

## Çevre Değişkenleri ve Ayarlar

- WebAPI/appsettings.json altında JWT, ConnectionStrings vb. ayarlar bulunur.
- appsettings.Development.json dosyası .gitignore ile takip dışıdır.

## Geliştirme Notları

- Clean Architecture prensipleri: bağımlılıklar dış katmanlara değil iç katmanlara doğru akmalı.
- Mapping için AutoMapper kullanılır (Business/Mappings/AutoMapperProfile.cs).
- Kimlik doğrulama ve yetkilendirme için JWT tabanlı akış mevcuttur.

## Komutlar

- Derleme: `dotnet build`
- Test (varsa): `dotnet test`
- Yeni migration:

```
dotnet ef migrations add <MigrationName> --project DataAccess --startup-project WebAPI
```

## Lisans

Bu proje için henüz bir lisans belirtilmemiştir.
