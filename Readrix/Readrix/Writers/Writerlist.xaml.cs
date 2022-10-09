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
using Readrix.Utils;
using Google.Protobuf.WellKnownTypes;

namespace Readrix.Writers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Writerlist : ContentPage
    {
        ApiCRUD api = new ApiCRUD();
        public Writerlist()
        {
            InitializeComponent();
            LoadData();
        }
        private async void LoadData()
        {
            UserDialogs.Instance.ShowLoading("Loading Please Wait...");
            var list = await api.CallApiGetAsync<List<Writer>>("api/Writer/getwriters");
            Crousalview.ItemsSource = list;
            UserDialogs.Instance.HideLoading();
        }
    }
}