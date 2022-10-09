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
using Readrix.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Readrix.Shop
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShopPage : ContentPage
    {
        ApiCRUD api = new ApiCRUD();
        public ShopPage()
        {
            InitializeComponent();
            LoadData();
        }
        private async void LoadData()
        {

            UserDialogs.Instance.ShowLoading("Loading Please Wait...");
            var modifiedlist = await api.CallApiGetAsync<List<Artifact>>("api/ShopArtifacts/getshopartifacts");
            ListData.ItemsSource = modifiedlist;
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