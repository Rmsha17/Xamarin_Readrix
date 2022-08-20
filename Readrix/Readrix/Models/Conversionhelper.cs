using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Readrix.Models
{
   public class Conversionhelper
    {
        public static double GetCurrentDollorprice()
        {
            try
            {
                String URLString = "https://v6.exchangerate-api.com/v6/9f618dda741ef0ec5b66aac7/latest/USD";
                using (var webClient = new System.Net.WebClient())
                {
                    var json = webClient.DownloadString(URLString);
                    var ConversionPrice = JsonConvert.DeserializeObject<ConversionData>(json);
                    return ConversionPrice.conversion_rates.PKR;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }

}
