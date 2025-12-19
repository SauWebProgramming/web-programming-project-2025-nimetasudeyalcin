# DÖNGÜ: İkinci El Eşya Satış Platformu
Web Programlama Dersi Proje Ödevi

## Proje Hakkında
Bu proje, kullanıcıların ilan verip ikinci el eşya satabildiği, adminlerin kategorileri yönetebildiği ASP.NET Core MVC tabanlı bir uygulamadır.

## Kurulum
1. Projeyi klonlayın.
2. `appsettings.json` dosyasındaki ConnectionString'i kendi SQL Server'ınıza göre düzenleyin.
3. Package Manager Console'u açın ve `update-database` komutunu çalıştırın.
4. Projeyi başlatın.

## Giriş Bilgileri (Admin)
Proje ilk çalıştığında otomatik olarak Admin kullanıcısı oluşturur.
* **Email:** admin@admin.com
* **Şifre:** Admin123!

## Eklenen Bonus Özellikler
* AJAX ile Dinamik Favori Sistemi (Sayfa Yenilenmeden): Kullanıcılar ürün kartlarındaki kalp ikonuna tıkladığında, sayfa yenilenmeden (arka planda jQuery/AJAX ile) ürün favorilere eklenir veya çıkarılır.
* 3.Parti API Entegrasyonu (Canlı Döviz Kuru): TCMB (Merkez Bankası) servislerine bağlanılarak anlık Dolar ($) kuru çekilmektedir. Her ürünün TL fiyatının altında, API'den gelen güncel kur ile hesaplanmış Dolar karşılığı otomatik olarak gösterilmektedir.

## Kullanılan Teknolojiler
* Backend: ASP.NET Core 7.0 MVC, C#, Entity Framework Core
* Frontend: HTML5, CSS3, Bootstrap 5, jQuery (AJAX)
* Veritabanı: MS SQL Server (LocalDB)
* Mimari: MVC (Model-View-Controller), Repository Pattern, N-Tier Architecture
* Güvenlik: ASP.NET Core Identity (Authentication & Authorization)

