# Hastane Sistemi

Bu proje ASP.NET Core MVC ile gelistirilmis basit bir hastane otomasyon uygulamasidir.

## Gereksinimler

- **.NET 5 SDK**
- Microsoft SQL Server (varsayilan baglanti dizesi `appsettings.json` dosyasinda bulunur)

## Projeyi Derlemek ve Calistirmak

1. Depoyu klonlayin ve dizine girin:
   ```bash
   git clone <repo-url>
   cd HastaneSistemi
   ```
2. Bagimliliklari geri yuklemek ve projeyi derlemek icin:
   ```bash
   dotnet build
   ```
3. Uygulamayi calistirmak icin:
   ```bash
   dotnet run --project HastaneSistemi
   ```
   Komut basarili oldugunda uygulamaya `http://localhost:5000` veya `https://localhost:5001` adreslerinden erisebilirsiniz.

## Veritabani Olusturma

Proje, SQL Server kullanarak `HastaneDB` adinda bir veritabani bekler. Gerekirse `appsettings.json` dosyasindaki `HastaneDB` baglanti dizesini guncelleyebilir ve asagidaki komutla var olan girisleri veritabanina uygulayabilirsiniz:

```bash
 dotnet ef database update --project HastaneSistemi
```

## Visual Studio ile Calisma

`HastaneSistemi.sln` dosyasini Visual Studio 2019 veya daha yeni bir surumle acarak da calisabilirsiniz. Visual Studio, gerekli NuGet paketlerini otomatik olarak geri yukleyecek ve `IIS Express` profili ile uygulamayi calistirabileceksiniz.
