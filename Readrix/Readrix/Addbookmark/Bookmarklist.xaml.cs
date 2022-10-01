using Acr.UserDialogs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Readrix.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Readrix.Genre;

namespace Readrix.Addbookmark
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Bookmarklist : ContentPage
    {
        public Bookmarklist()
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
            var uri = App.Base_url + "api/Bookmark/getllist?id=" + App.LoggedInReader.READER_ID;
            var result = await client.GetStringAsync(uri);
            List<Bookmark> list = JsonConvert.DeserializeObject<List<Bookmark>>(result);
            List<Artifact> RefinedList = new List<Artifact>();
            foreach(var item in list)
            {
                var uri2 = App.Base_url + "api/Artifacts/getartifact/?id=" + item.ARTIFACT_FID;
                var result2 = await client.GetStringAsync(uri2);
                Artifact list2 = JsonConvert.DeserializeObject<Artifact>(result2);
               
                list2.Bookmarkid = item.BOOKMARK_ID;
                var uri3 = App.Base_url + "api/SubCategories/" + list2.SUB_CATEGORY_FID;
                var result3 = await client.GetStringAsync(uri3);
                SubCategory sub = JsonConvert.DeserializeObject<SubCategory>(result3);
                list2.SubCategory_Name = sub.SUB_CATEGORY_NAME;
                RefinedList.Add(list2);
            }
            collectionList.ItemsSource = RefinedList;
            UserDialogs.Instance.HideLoading();
        }

        private async void collectionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           await UpdateSelectionDataAsync(e.CurrentSelection);
        }
        async Task UpdateSelectionDataAsync(IEnumerable<object> currentSelected)
        {
            var selected = currentSelected.FirstOrDefault() as Artifact;
            var actionSheet = await DisplayActionSheet("Options", "Cancel", null, "Open Artifact", "Remove Bookmark");
            if(actionSheet== "Remove Bookmark")
            {
                var q = await DisplayAlert("Successfully", "Are you sure to remove " + selected.ARTIFACT_NAME + " from bookmark", "Yes", "No");
                if (q)
                {
                    UserDialogs.Instance.ShowLoading("Loading Please Wait...");
                    var httpClientHandler = new HttpClientHandler();
                    httpClientHandler.ServerCertificateCustomValidationCallback =
                        (message, certificate, chain, sslPolicyErrors) => true;
                    var client = new HttpClient(httpClientHandler);
                    var uri = App.Base_url + "api/Bookmark/remove?id=" + selected.Bookmarkid;
                    var result = await client.DeleteAsync(uri);
                    LoadData();
                    if (result.IsSuccessStatusCode)
                    {
                        UserDialogs.Instance.HideLoading();
                        await DisplayAlert("Successfully", selected.ARTIFACT_NAME + " removed from bookmark", "OK");
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        await DisplayAlert("Error", "Somthing went wrong!!", "OK");
                    }
                }
            }
            if(actionSheet == "Open Artifact")
            {
                await Navigation.PushAsync(new ArtifactIndex(selected));
            }
        }
    }
}