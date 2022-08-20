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

namespace Readrix.Genre
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Artifactlist : ContentPage
    {
        public static int cat1;
        public Artifactlist(SubCategory sub)
        {
            InitializeComponent();
            cat1 = sub.SUB_CATEGORY_ID;
            LoadData();
        }
        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
           
        //}
        private async void LoadData()
        {
            UserDialogs.Instance.ShowLoading("Loading Please Wait...");

            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback =
                (message, certificate, chain, sslPolicyErrors) => true;
            var client = new HttpClient(httpClientHandler);
            var uri = App.Base_url + "api/Artifact/getartifactbysubid/?id=" + cat1;
            var result = await client.GetStringAsync(uri);
            List<Artifact> list = JsonConvert.DeserializeObject<List<Artifact>>(result);

            List<Artifact> RefinedList = new List<Artifact>();
            foreach (var item in list)
            {
                
                var uri2 = App.Base_url + "api/SubCategories/" + item.SUB_CATEGORY_FID;
                var result2 = await client.GetStringAsync(uri2);
                SubCategory sub = JsonConvert.DeserializeObject<SubCategory>(result2);
                item.SubCategory_Name = sub.SUB_CATEGORY_NAME;
                RefinedList.Add(item);
                Title = sub.SUB_CATEGORY_NAME;
            }
            if(list.Count != 0){
                ListData.ItemsSource = list;
            }
            else
            {
                lblartifact.Text = "There are no Artifacts of this Genre";
                lblartifact.IsVisible = true;
            }
            
            UserDialogs.Instance.HideLoading();
        }

        private async void ListData_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selected = e.Item as Artifact;
            await Navigation.PushAsync(new ArtifactIndex(selected));
        }

        private async void ListData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await UpdateSelectionDataAsync(e.CurrentSelection);
        }
        async Task UpdateSelectionDataAsync(IEnumerable<object> currentSelected)
        {
            var selected = currentSelected.FirstOrDefault() as Artifact;
            await Navigation.PushAsync(new ArtifactIndex(selected));

        }
     }
}