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
using Readrix.Utils;
using Google.Protobuf.WellKnownTypes;

namespace Readrix.Addbookmark
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Bookmarklist : ContentPage
    {
        ApiCRUD api = new ApiCRUD();
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
            var Refinedlist = await api.CallApiGetAsync<List<Artifact>>("api/Bookmark/getllist?id=" + App.LoggedInReader.READER_ID);
            collectionList.ItemsSource = Refinedlist;
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

                    var removebookmark = await api.CallApiDeleteAsync("api/Bookmark/remove?id="+ selected.Bookmarkid);
                    LoadData();
                    if (removebookmark == true)
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