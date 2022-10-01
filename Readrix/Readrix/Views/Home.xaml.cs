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
    public partial class Home : ContentPage
    {
        public Home()
        {
            InitializeComponent();
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

            var uri = App.Base_url + "api/Artifact/topratedcomics";
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
            }
            ColltopratedComic.ItemsSource = RefinedList;

            var uri3 =App.Base_url + "api/Artifact/topratednovels";
            var result3 = await client.GetStringAsync(uri3);
            List<Artifact> list3 = JsonConvert.DeserializeObject<List<Artifact>>(result3);

            List<Artifact> RefinedList3 = new List<Artifact>();
            foreach (var item in list3)
            {
               
                var uri2 = App.Base_url + "api/SubCategories/" + item.SUB_CATEGORY_FID;
                var result2 = await client.GetStringAsync(uri2);
                SubCategory sub = JsonConvert.DeserializeObject<SubCategory>(result2);
                item.SubCategory_Name = sub.SUB_CATEGORY_NAME;
                RefinedList3.Add(item);
            }
            ColltopratedNovels.ItemsSource = RefinedList3;


            var uri4 = App.Base_url + "api/Artifact/topfeatured";
            var result4 = await client.GetStringAsync(uri4);
            List<Artifact> topfeatured = JsonConvert.DeserializeObject<List<Artifact>>(result4);

            List<Artifact> RefinedListfeatured = new List<Artifact>();
            foreach (var item in topfeatured)
            {
              
                var uri2 = App.Base_url + "api/SubCategories/" + item.SUB_CATEGORY_FID;
                var result2 = await client.GetStringAsync(uri2);
                SubCategory sub = JsonConvert.DeserializeObject<SubCategory>(result2);
                item.SubCategory_Name = sub.SUB_CATEGORY_NAME;
                RefinedListfeatured.Add(item);
            }
            Colltopfeatured.ItemsSource = RefinedListfeatured;

            var uri5 = App.Base_url + "api/Artifact/topnewest";
            var result5 = await client.GetStringAsync(uri5);
            List<Artifact> newest = JsonConvert.DeserializeObject<List<Artifact>>(result5);

            List<Artifact> RefinedListnewest = new List<Artifact>();
            foreach (var item in newest)
            {
              
                var uri2 = App.Base_url + "api/SubCategories/" + item.SUB_CATEGORY_FID;
                var result2 = await client.GetStringAsync(uri2);
                SubCategory sub = JsonConvert.DeserializeObject<SubCategory>(result2);
                item.SubCategory_Name = sub.SUB_CATEGORY_NAME;
                RefinedListnewest.Add(item);
            }
            Collectionnewest.ItemsSource = RefinedListnewest;

            var uri6 = App.Base_url + "api/Artifact/topratedromance";
            var result6 = await client.GetStringAsync(uri6);
            List<Artifact> toprome = JsonConvert.DeserializeObject<List<Artifact>>(result6);

            List<Artifact> RefinedListrome = new List<Artifact>();
            foreach (var item in toprome)
            {
                var uri2 = App.Base_url + "api/SubCategories/" + item.SUB_CATEGORY_FID;
                var result2 = await client.GetStringAsync(uri2);
                SubCategory sub = JsonConvert.DeserializeObject<SubCategory>(result2);
                item.SubCategory_Name = sub.SUB_CATEGORY_NAME;
                RefinedListrome.Add(item);
            }
            Colltopromance.ItemsSource = RefinedListrome;

            var uri7 = App.Base_url + "api/Artifact/topratedhorror";
            var result7 = await client.GetStringAsync(uri7);
            List<Artifact> tophorror = JsonConvert.DeserializeObject<List<Artifact>>(result7);

            List<Artifact> RefinedListhorror = new List<Artifact>();
            foreach (var item in tophorror)
            {
                //item.ARTIFACT_PICTURE = item.ARTIFACT_PICTURE.Replace("~/", "");
                //item.ARTIFACT_PICTURE = App.Base_urlPic + item.ARTIFACT_PICTURE;
                var uri2 = App.Base_url + "api/SubCategories/" + item.SUB_CATEGORY_FID;
                var result2 = await client.GetStringAsync(uri2);
                SubCategory sub = JsonConvert.DeserializeObject<SubCategory>(result2);
                item.SubCategory_Name = sub.SUB_CATEGORY_NAME;
                RefinedListhorror.Add(item);
            }
            Colltophorror.ItemsSource = RefinedListhorror;

            UserDialogs.Instance.HideLoading();
        }

        private async void ColltopratedComic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await UpdateSelectionDataAsync(e.CurrentSelection);
        }

        private async void ColltopratedNovels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await UpdateSelectionDataAsync(e.CurrentSelection);
        }

        private async void Colltopfeatured_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await UpdateSelectionDataAsync(e.CurrentSelection);
        }

        private async void Collectionnewest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await UpdateSelectionDataAsync(e.CurrentSelection);
        }

        private async void Colltopromance_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await UpdateSelectionDataAsync(e.CurrentSelection);
        }

        private async void Colltopcomedy_SelectionChanged(object sender, SelectionChangedEventArgs e)
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