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

namespace Readrix.Settingreader
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Invoice : ContentPage
    {
        Order orders = new Order();
        decimal total = 200;
        public Invoice(Order order)
        {
            
            InitializeComponent();
            lbldate.Text = order.ORDER_DATE.ToLongDateString();
            lblid.Text = order.ORDER_ID.ToString();
            orders = order;
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
            var uri = App.Base_url + "api/Artifact/invoice/?id=" + orders.ORDER_ID ;
            var result = await client.GetStringAsync(uri);
            List<Order_details> list = JsonConvert.DeserializeObject<List<Order_details>>(result);
            List<Order_details> modifiedlist = new List<Order_details>();
            
            foreach (var item in list)
            {
                
                var uri2 = App.Base_url + "api/ShopArtifacts/getidshopartifacts/?id=" + item.SHOPARTIFACT_FID;
                var result2 = await client.GetStringAsync(uri2);
                Shopartifact shopart = JsonConvert.DeserializeObject<Shopartifact>(result2);
                var uri3 = App.Base_url + "api/Artifacts/getartifact/?id=" + shopart.ARTIFACT_FID;
                var result3 = await client.GetStringAsync(uri3);
                Artifact artifact = JsonConvert.DeserializeObject<Artifact>(result3);

                item.ARTIFACT_NAME = artifact.ARTIFACT_NAME;
                item.QUANTITY = Math.Abs(item.QUANTITY);
                item.ARTIFACT_IMAGE = artifact.ARTIFACT_PICTURE.Replace("~/", "");
                item.ARTIFACT_IMAGE = App.Base_urlPic + item.ARTIFACT_IMAGE;
                item.Total = Math.Abs(item.QUANTITY) * item.ARTIFACT_SALEPRICE;
                total += (decimal)item.Total;
                
                modifiedlist.Add(item);
            }
            lbltotal.Text = total.ToString();
            
            if (list == null)
            {
               await DisplayAlert("Maessage", "Null", "ok");
            }
            else
            {
                ListData.ItemsSource = modifiedlist;
            }

            UserDialogs.Instance.HideLoading();
        }
    }
}