using Acr.UserDialogs;
using Newtonsoft.Json;
using Readrix.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Readrix.Shop
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Payment : ContentPage
    {
        int dollerrate = (int)Conversionhelper.GetCurrentDollorprice();
        public static bool PaymentStatus = false;
        public Payment()
        {
            InitializeComponent();

            string uri = "https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_xclick&amount=" + (App.Total / dollerrate) + "&business=readrix28@gmail.com&item_name=ReadRix&return=https://readrixsuccesspagesite.on.drv.tw/Payment/success.html";
            WebPageViewer.Source = uri;
        }

        private  void WebPageViewer_Navigating(object sender, WebNavigatingEventArgs e)
        {
            if (e.Url.Contains("https://readrixsuccesspagesite.on.drv.tw/Payment/success.html?"))
            {
                PaymentStatus = true;
                btnproceeed.IsVisible = true;
            }
            else
            {
                PaymentStatus = false;
            }

        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (PaymentStatus == true)
            {

                try
                {
                    await DisplayAlert("Success", "Payment has been Successfully Completed. Please Click to Proceed for Final Confirmation.Thank You!! ", "OK");
                    UserDialogs.Instance.ShowLoading("Loading Please Wait...");
                    Models.Order order = new Models.Order()
                    {
                        ORDER_DATE = DateTime.Now.Date,
                        ORDER_STATUS = "Completed",
                        ORDER_TYPE = "Sale",
                        PAYMENT_TYPE = "Paypal",
                        READER_FID = App.LoggedInReader.READER_ID
                    };


                    var uri = App.Base_url + "/api/order/postorder";

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

                        var uri1 = App.Base_url + "api/orderdetail/postorderdetail";
                        string JsonData1 = JsonConvert.SerializeObject(item);
                        StringContent StringData1 = new StringContent(JsonData1, Encoding.UTF8, "application/json");
                        var responseMessage1 = await client.PostAsync(uri1, StringData1);
                        if (responseMessage1.IsSuccessStatusCode)
                        {

                        }

                    }
                    UserDialogs.Instance.HideLoading();
                    await Navigation.PushAsync(new Success());

                }
                catch (Exception ex)
                {
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert("Error", "Somthing went wrong please try again " + ex, "OK");
                    return;
                }

            }
            else
            {
                await DisplayAlert("Message", "Payment is Not Confirmed. Please Try Again..", "OK");
            }

        }
        private void WebPageViewer_Navigated(object sender, WebNavigatedEventArgs e)
        {

        }
    }
}