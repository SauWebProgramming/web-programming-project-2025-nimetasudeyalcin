using System.Xml.Linq;

namespace IkinciElEsya.Services
{
    public class CurrencyService
    {
        // Bu metot TCMB'den güncel dolar kurunu çeker
        public decimal GetUsdRate()
        {
            try
            {
                string url = "https://www.tcmb.gov.tr/kurlar/today.xml";
                XDocument xmlDoc = XDocument.Load(url);

                var usdRate = xmlDoc.Descendants("Currency")
                    .FirstOrDefault(x => x.Attribute("Kod")?.Value == "USD")
                    ?.Element("ForexSelling")?.Value;

                // Türkiye sunucularında virgül/nokta farkını yönetmek için
                if (decimal.TryParse(usdRate?.Replace(".", ","), out decimal rate))
                {
                    return rate;
                }
                return 35.0m; // Hata olursa varsayılan (Site patlamasın diye)
            }
            catch
            {
                return 35.0m; // İnternet yoksa varsayılan
            }
        }
    }
}