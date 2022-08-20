using Acr.UserDialogs;
using Newtonsoft.Json;
using Readrix.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Readrix.Settingreader
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Cancelorder : ContentPage
    {
        public Cancelorder()
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

            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback =
                (message, certificate, chain, sslPolicyErrors) => true;
            var client = new HttpClient(httpClientHandler);
            var uri = App.Base_url + "api/readers/getordercancel/?id=" + App.LoggedInReader.READER_ID;
            var result = await client.GetStringAsync(uri);
            List<Order> list = JsonConvert.DeserializeObject<List<Order>>(result);
            List<Order> modifiedlist = new List<Order>();
            foreach (var item in list)
            {
                var uri2 = App.Base_url + "api/Readers/" + item.READER_FID;
                var result2 = await client.GetStringAsync(uri2);
                Reader reader = JsonConvert.DeserializeObject<Reader>(result2);
                item.Reader_Name = reader.READER_NAME;
                modifiedlist.Add(item);
            }
            if (list == null)
            {
                lblname.IsVisible = true;
                lblname.Text = "You have no booked orders yet!";
            }
            else
            {
                ListData.ItemsSource = modifiedlist;
            }



            UserDialogs.Instance.HideLoading();
        }

        private async void ListData_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selected = e.SelectedItem as Order;
            var actionSheet = await DisplayActionSheet("Options", "Cancel", null, "Active Order", "View Invoice");
            if (actionSheet == "Active Order")
            {
                var q = await DisplayAlert("Successfully", "Are you sure to book/active your order No:" + selected.ORDER_ID + " again", "Yes", "No");
                if (q)
                {
                    UserDialogs.Instance.ShowLoading("Loading Please Wait...");
                    var Order = new Order
                    {
                        ORDER_ID = selected.ORDER_ID,
                        ORDER_STATUS = "Booked",
                        ORDER_TYPE = selected.ORDER_TYPE,
                        PAYMENT_TYPE = selected.PAYMENT_TYPE,
                        READER_FID = selected.READER_FID,
                        ORDER_DATE = DateTime.Now
                    };

                    var httpClientHandler = new HttpClientHandler();
                    httpClientHandler.ServerCertificateCustomValidationCallback =
                        (message, certificate, chain, sslPolicyErrors) => true;
                    var httpClient = new HttpClient(httpClientHandler);
                    var uri = App.Base_url + "api/Orders/" + selected.ORDER_ID;
                    var json = JsonConvert.SerializeObject(Order);
                    HttpContent httpContent = new StringContent(json);
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    HttpResponseMessage result = await httpClient.PutAsync(uri, httpContent);

                    LoadData();
                    if (result.IsSuccessStatusCode)
                    {
                        UserDialogs.Instance.HideLoading();
                        await DisplayAlert("Successfully", " Booked your order No:" + selected.ORDER_ID + " again", "OK");
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        await DisplayAlert("Error", "Somthing went wrong!!", "OK");
                    }
                }
               
            }
            if (actionSheet == "View Invoice")
            {
                await Navigation.PushAsync(new Invoice(selected));
            }
        }
    }
}