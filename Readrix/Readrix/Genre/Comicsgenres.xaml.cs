using Acr.UserDialogs;
using Newtonsoft.Json;
using Readrix.Genre;
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
    public partial class Comicsgenres : ContentView
    {
        public static int cat1;
        public Comicsgenres()
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
            var uri = App.Base_url + "api/Category/getcategory";
            var result = await client.GetStringAsync(uri);
            List<Category> list = JsonConvert.DeserializeObject<List<Category>>(result);
            cat1 = list.Where(x => x.CATEGORY_NAME == "Comics").Select(x => x.CATEGORY_ID).FirstOrDefault();
            var uri2 = App.Base_url + "api/Subcategory/getidsubcategories/?id=" + cat1;

            var result2 = await client.GetStringAsync(uri2);
            List<SubCategory> list2 = JsonConvert.DeserializeObject<List<SubCategory>>(result2);
            ListData.ItemsSource = list2;
            UserDialogs.Instance.HideLoading();
        }

        private async void ListData_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selected = e.Item as SubCategory;
            await Navigation.PushAsync(new Artifactlist(selected));
        }

        private async void ListData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await UpdateSelectionDataAsync(e.CurrentSelection);
        }
        async Task UpdateSelectionDataAsync(IEnumerable<object> currentSelected)
        {
            var selected = currentSelected.FirstOrDefault() as SubCategory;
            await Navigation.PushAsync(new Artifactlist(selected));

        }
    }
}