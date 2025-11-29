# İkinci El Eşya Satış Platformu

Web Programlama Dersi Proje Ödevi

## Proje Hakkında
Bu proje, kullanıcıların ilan verip ikinci el eşya satabildiği, adminlerin kategorileri yönetebildiği ASP.NET Core MVC tabanlı bir uygulamadır.
Mimari olarak **Repository Pattern** ve **Identity** kullanılmıştır.

## Kurulum
1. Projeyi klonlayın.
2. `appsettings.json` dosyasındaki ConnectionString'i kendi SQL Server'ınıza göre düzenleyin.
3. Package Manager Console'u açın ve `update-database` komutunu çalıştırın.
4. Projeyi başlatın.

## Giriş Bilgileri (Admin)
Proje ilk çalıştığında otomatik olarak Admin kullanıcısı oluşturur.
* **Email:** admin@admin.com
* **Şifre:** Admin123!

## Kullanılan Teknolojiler
* ASP.NET Core 7.0 MVC
* Entity Framework Core (Code First)
* Identity (Authentication & Authorization)
* Bootstrap 5