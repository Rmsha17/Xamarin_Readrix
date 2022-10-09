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

namespace Readrix.Schedule
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Thursday : ContentView
    {
        ApiCRUD api = new ApiCRUD();
        public static int cat1;
        public Thursday()
        {
            InitializeComponent();
            LoadData();
           
        }
        private async void LoadData()
        {
            UserDialogs.Instance.ShowLoading("Loading Please Wait...");

            var list = await api.CallApiGetAsync<List<Schedules>>("api/Schedule/getschedule");
            cat1 = list.Where(x => x.SCHDULE_DAYS == "Thursday").Select(x => x.SCHEDULE_ID).FirstOrDefault();
            var RefinedList = await api.CallApiGetAsync<List<Artifact>>("api/schedule/getartifactbyid/?id=" + cat1);
            ListData.ItemsSource = RefinedList;


            UserDialogs.Instance.HideLoading();
        }
        private void ListData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateSelectionData(e.CurrentSelection);
        }
        void UpdateSelectionData(IEnumerable<object> currentSelected)
        {
            var selected = currentSelected.FirstOrDefault() as Artifact;
            Navigation.PushAsync(new ArtifactIndex(selected));
        }

    }
}