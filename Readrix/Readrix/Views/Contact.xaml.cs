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

namespace Readrix.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Contact : ContentPage
    {
        public Contact()
        {
            InitializeComponent();         
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadData();
        }
        private async void LoadData()
        {
            UserDialogs.Instance.ShowLoading("Loading Please Wait...");

            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback =
                (message, certificate, chain, sslPolicyErrors) => true;
            var client = new HttpClient(httpClientHandler);
            var uri = App.Base_url + "api/Website/Getdetails";
            var result = await client.GetStringAsync(uri);
            WEBSITE_DETAILS details = JsonConvert.DeserializeObject<WEBSITE_DETAILS>(result);
            lblemail.Text = details.WEBSITE_EMAIL;
            lblphn.Text = details.WEBSITE_CONTACT;
            lbladdress.Text = details.WEBSITE_ADDRESS;

            UserDialogs.Instance.HideLoading();
        }

    }
}