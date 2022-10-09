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

namespace Readrix.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Contact : ContentPage
    {
        ApiCRUD api = new ApiCRUD();
        public Contact()
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
            var details= await api.CallApiGetAsync<WEBSITE_DETAILS>("api/Website/Getdetails");
            lblemail.Text = details.WEBSITE_EMAIL;
            lblphn.Text = details.WEBSITE_CONTACT;
            lbladdress.Text = details.WEBSITE_ADDRESS;
            UserDialogs.Instance.HideLoading();
        }

    }
}