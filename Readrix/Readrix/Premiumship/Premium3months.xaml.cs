using Acr.UserDialogs;
using Newtonsoft.Json;
using Readrix.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Readrix.Premiumship
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Premium3months : ContentView
    {
        public static int pre;
        public static Premium premium;
        public Premium3months()
        {
            InitializeComponent();
            LoadData();
        }
        private async void LoadData()
        {
            UserDialogs.Instance.ShowLoading("Loading Please Wait...");

            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback =
                (message, certificate, chain, sslPolicyErrors) => true;
            var client = new HttpClient(httpClientHandler);
            var uri = App.Base_url + "api/Preiums/getpremium";
            var result = await client.GetStringAsync(uri);
            List<Premium> list = JsonConvert.DeserializeObject<List<Premium>>(result);
            pre = list.Where(x => x.PREMIUM_NAME == "Pro").Select(x => x.PREMIUM_ID).FirstOrDefault();
            var uri2 = App.Base_url + "api/Premiums/" + pre;

            var result2 = await client.GetStringAsync(uri2);
            Premium premium6 = JsonConvert.DeserializeObject<Premium>(result2);
            premium = premium6;

            lblname.Text = premium6.PREMIUM_NAME;
            lblduration.Text = premium6.DURATION_IN_MONTHS.ToString();
            lblprice.Text = premium6.PREMIUM_PRICE.ToString();
            UserDialogs.Instance.HideLoading();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PremiumPayment(premium));
        }
    }
}