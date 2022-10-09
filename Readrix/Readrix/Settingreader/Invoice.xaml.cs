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

namespace Readrix.Settingreader
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Invoice : ContentPage
    {
        ApiCRUD api = new ApiCRUD();
        Order orders = new Order();
        decimal total = 200;
        public Invoice(Order order)
        {
            
            InitializeComponent();
            lbldate.Text = order.ORDER_DATE.ToLongDateString();
            lblid.Text = order.ORDER_ID.ToString();
            orders = order;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadData();
        }
        private async void LoadData()
        {
            UserDialogs.Instance.ShowLoading("Loading Please Wait...");
            var modifiedlist = await api.CallApiGetAsync<List<Order_details>>("api/Artifact/invoice/?id=" + orders.ORDER_ID);
            foreach (var item in modifiedlist)
            {
                total += (decimal)item.Total;
            }
            lbltotal.Text = total.ToString();
            if (modifiedlist == null)
            {
               await DisplayAlert("Maessage", "Null", "ok");
            }
            else
            {
                ListData.ItemsSource = modifiedlist;
            }
            UserDialogs.Instance.HideLoading();
        }
    }
}