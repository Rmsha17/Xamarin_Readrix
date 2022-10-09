using Acr.UserDialogs;
using Google.Protobuf.WellKnownTypes;
using Newtonsoft.Json;
using Readrix.Genre;
using Readrix.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Readrix.Utils;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Readrix.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Comicsgenres : ContentView
    {
        public static int cat1;
        ApiCRUD api = new ApiCRUD();
        public Comicsgenres()
        {
            InitializeComponent();
            LoadData();
         
        }

        private async void LoadData()
        {
            UserDialogs.Instance.ShowLoading("Loading Please Wait...");

            var list = await api.CallApiGetAsync<List<Category>>("api/Category/getcategory");

            cat1 = list.Where(x => x.CATEGORY_NAME == "Comics").Select(x => x.CATEGORY_ID).FirstOrDefault();
            var comicslist = await api.CallApiGetAsync<List<SubCategory>>("api/Subcategory/getidsubcategories/?id=" + cat1);
            ListData.ItemsSource = comicslist;
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