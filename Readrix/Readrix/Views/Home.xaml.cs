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
using Readrix.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Readrix.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : ContentPage
    {
        ApiCRUD api = new ApiCRUD();
        public Home()
        {
            InitializeComponent();
            LoadData();
        }
        private async void LoadData()
        {
            UserDialogs.Instance.ShowLoading("Loading Please Wait...");

            var topratedcomics = await api.CallApiGetAsync<List<Artifact>>("api/Artifact/topratedcomics");
            ColltopratedComic.ItemsSource = topratedcomics;

        
            var topratednovels = await api.CallApiGetAsync<List<Artifact>>("api/Artifact/topratednovels");
            ColltopratedNovels.ItemsSource = topratednovels;


            var topfeatured = await api.CallApiGetAsync<List<Artifact>>("api/Artifact/topfeatured");
            Colltopfeatured.ItemsSource = topfeatured;

           
            var topnewest = await api.CallApiGetAsync<List<Artifact>>("api/Artifact/topnewest");
            Collectionnewest.ItemsSource = topnewest;


            var topratedromance = await api.CallApiGetAsync<List<Artifact>>("api/Artifact/topratedromance");
            Colltopromance.ItemsSource = topratedromance;

            var topratedhorror = await api.CallApiGetAsync<List<Artifact>>("api/Artifact/topratedhorror");
            Colltophorror.ItemsSource = topratedhorror;

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