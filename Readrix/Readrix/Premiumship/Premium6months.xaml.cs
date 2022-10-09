using Acr.UserDialogs;
using Google.Protobuf.WellKnownTypes;
using Newtonsoft.Json;
using Readrix.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Readrix.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Readrix.Premiumship
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Premium6months : ContentView
    {
        ApiCRUD api = new ApiCRUD();
        public static int pre;
        public static Premium premium;
        public Premium6months()
        {
            InitializeComponent();
            LoadData();
        }
        private async void LoadData()
        {
            UserDialogs.Instance.ShowLoading("Loading Please Wait...");

            var httpClientHandler = new HttpClientHandler();
            var prelist = await api.CallApiGetAsync<List<Premium>>("api/Preiums/getpremium");
            pre = prelist.Where(x => x.PREMIUM_NAME == "Advanced").Select(x => x.PREMIUM_ID).FirstOrDefault();

            var premium6 = await api.CallApiGetAsync<Premium>("api/Premiums/" + pre);
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