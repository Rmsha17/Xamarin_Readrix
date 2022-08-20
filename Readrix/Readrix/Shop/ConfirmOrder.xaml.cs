using Newtonsoft.Json;
using Readrix.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Ubiety.Dns.Core;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Readrix.Shop
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfirmOrder : ContentPage
    {
        public ConfirmOrder()
        {
            InitializeComponent();
        }

        private async void btnCurrentLocation_Clicked(object sender, EventArgs e)
        {
            try
            {

                Models.Order order = new Models.Order()
                {
                    ORDER_DATE = DateTime.Now.Date,
                    ORDER_STATUS = "Booked",
                    ORDER_TYPE = "Sale",
                    PAYMENT_TYPE = "CashOnDelivery",
                    READER_FID= App.LoggedInReader.READER_ID
                };


                var uri = App.Base_url + "api/order/postorder";

                var httpClientHandler = new HttpClientHandler();
                httpClientHandler.ServerCertificateCustomValidationCallback =
                    (message, certificate, chain, sslPolicyErrors) => true;
                var client = new HttpClient(httpClientHandler);

                string JsonData = JsonConvert.SerializeObject(order);
                StringContent StringData = new StringContent(JsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = await client.PostAsync(uri, StringData);
                string responseData = await responseMessage.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<Order>(responseData);


                foreach (var item in App.Cart)
                {
                    item.ORDER_FID = response.ORDER_ID;
                    item.QUANTITY = item.QUANTITY * -1;

                    var uri1 = App.Base_url +  "api/orderdetail/postorderdetail";
                    string JsonData1 = JsonConvert.SerializeObject(item);
                    StringContent StringData1 = new StringContent(JsonData1, Encoding.UTF8, "application/json");
                    var responseMessage1 = await client.PostAsync(uri1, StringData1);
                    if (responseMessage1.IsSuccessStatusCode)
                    {
 
                    }

                }

                await Navigation.PushAsync(new Success());
               

            }
            catch (Exception ex)
            {
                await DisplayAlert("Message", "Somthing went wrong this may be a problem with internet or application please ensure that you have a working internet connection and GPS enabled. \nError Details : " + ex.Message, "OK");
            }
        }
    }
}