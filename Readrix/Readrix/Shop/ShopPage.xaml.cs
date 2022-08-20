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

namespace Readrix.Shop
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShopPage : ContentPage
    {
        public ShopPage()
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
            var uri = App.Base_url + "api/ShopArtifacts/getshopartifacts";
            var result = await client.GetStringAsync(uri);
            List<Shopartifact> list = JsonConvert.DeserializeObject<List<Shopartifact>>(result);
            List<Artifact> RefinedList = new List<Artifact>();
            foreach (var item in list)
            {
                var uri2 = App.Base_url + "api/Artifacts/getartifact/?id=" + item.ARTIFACT_FID;
                var result2 = await client.GetStringAsync(uri2);
                Artifact list2 = JsonConvert.DeserializeObject<Artifact>(result2);
               
                list2.SALE_PRICE = item.SALE_PRICE;
                list2.shopartidactid = item.ID;
                list2.PURCHASE_PRICE = item.PURCHASE_PRICE;
                RefinedList.Add(list2);
            }
            ListData.ItemsSource = RefinedList;
            UserDialogs.Instance.HideLoading();
        }

        private async void collectionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await UpdateSelectionDataAsync(e.CurrentSelection);
        }
        async Task UpdateSelectionDataAsync(IEnumerable<object> currentSelected)
        {
            var selected = currentSelected.FirstOrDefault() as Artifact;
            await Navigation.PushAsync(new Shopartifactindex(selected));
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CartControls());
        }
    }
}