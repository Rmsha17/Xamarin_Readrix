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

namespace Readrix.Schedule
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Thursday : ContentView
    {
        public static int cat1;
        public Thursday()
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
            var uri = App.Base_url + "api/Schedule/getschedule";
            var result = await client.GetStringAsync(uri);
            List<Schedules> list = JsonConvert.DeserializeObject<List<Schedules>>(result);
            cat1 = list.Where(x => x.SCHDULE_DAYS == "Thursday").Select(x => x.SCHEDULE_ID).FirstOrDefault();


            var uri2 = App.Base_url + "api/schedule/getartifactbyid/?id=" + cat1;
            var result2 = await client.GetStringAsync(uri2);
            List<Artifact> list2 = JsonConvert.DeserializeObject<List<Artifact>>(result2);

            List<Artifact> RefinedList = new List<Artifact>();
            foreach (var item in list2)
            {
               
                var uri3 = App.Base_url + "api/SubCategories/" + item.SUB_CATEGORY_FID;
                var result3 = await client.GetStringAsync(uri3);
                SubCategory sub = JsonConvert.DeserializeObject<SubCategory>(result3);
                item.SubCategory_Name = sub.SUB_CATEGORY_NAME;
                RefinedList.Add(item);
            }
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