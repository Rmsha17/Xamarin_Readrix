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
using Xamarin.Forms;
using Readrix.Utils;
using Xamarin.Forms.Xaml;

namespace Readrix.Genre
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Artifactlist : ContentPage
    {
        public static int cat1;
        ApiCRUD api = new ApiCRUD();
        public Artifactlist(SubCategory sub)
        {
            InitializeComponent();
            cat1 = sub.SUB_CATEGORY_ID;
            LoadData();
        }
        private async void LoadData()
        {
            UserDialogs.Instance.ShowLoading("Loading Please Wait...");

        
            var artlist = await api.CallApiGetAsync<List<Artifact>>("api/Artifact/getartifactbysubid/?id=" + cat1);
            Title = artlist.FirstOrDefault().SubCategory_Name;

            if (artlist.Count != 0){
                ListData.ItemsSource = artlist;
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