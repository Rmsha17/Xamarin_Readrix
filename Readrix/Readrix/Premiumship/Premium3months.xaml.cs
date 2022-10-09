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
    public partial class Premium3months : ContentView
    {
        ApiCRUD api = new ApiCRUD();
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

            var prelist = await api.CallApiGetAsync<List<Premium>>("api/Preiums/getpremium");
            pre = prelist.Where(x => x.PREMIUM_NAME == "Pro").Select(x => x.PREMIUM_ID).FirstOrDefault();
           
            var premium3 = await api.CallApiGetAsync<Premium>("api/Premiums/" + pre);
            premium = premium3;
            lblname.Text = premium3.PREMIUM_NAME;
            lblduration.Text = premium3.DURATION_IN_MONTHS.ToString();
            lblprice.Text = premium3.PREMIUM_PRICE.ToString();
            UserDialogs.Instance.HideLoading();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PremiumPayment(premium));
        }
    }
}