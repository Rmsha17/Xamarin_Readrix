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

namespace Readrix.Writers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Writerlist : ContentPage
    {
        public Writerlist()
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
            var uri = App.Base_url + "api/Writer/getwriters";
            var result = await client.GetStringAsync(uri);
            List<Writer> list = JsonConvert.DeserializeObject<List<Writer>>(result);
           
            //List<Writer> RefinedList = new List<Writer>();
            //foreach (var item in list)
            //{
            //    item.WRITER_IMAGE = item.WRITER_IMAGE.Replace("~/", "");
            //    item.WRITER_IMAGE = App.Base_urlPic + item.WRITER_IMAGE;
            //    RefinedList.Add(item);
            //}
            
            Crousalview.ItemsSource = list;
            UserDialogs.Instance.HideLoading();
        }
    }
}