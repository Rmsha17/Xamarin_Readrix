using Acr.UserDialogs;
using Readrix.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Readrix.Premiumship
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PremiumPayment : ContentPage
    {
        int dollerrate = (int)Conversionhelper.GetCurrentDollorprice();
        public static bool PaymentStatus = false;
        public static Premium readerpremium;
        public PremiumPayment(Premium premium)
        {
            InitializeComponent();

            string uri = "https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_xclick&amount=" + (premium.PREMIUM_PRICE / dollerrate) + "&business=readrix28@gmail.com&item_name=ReadRix&return=https://readrixsuccesspagesite.on.drv.tw/Payment/success.html";
            readerpremium = premium;
            WebPageViewer.Source = uri;
        }
        private void WebPageViewer_Navigating(object sender, WebNavigatingEventArgs e)
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

        private void WebPageViewer_Navigated(object sender, WebNavigatedEventArgs e)
        {

        }


        private async void btnproceeed_Clicked(object sender, EventArgs e)
        {
            if (PaymentStatus == true)
            {

                try
                {
                    await DisplayAlert("Success", "Payment has been Successfully Completed. Please Click to Proceed for Final Confirmation.Thank You!! ", "OK");
                    UserDialogs.Instance.ShowLoading("Loading Please Wait...");
                    ReaderPremiums Premium = new ReaderPremiums();


                    Premium.BUY_DATE = System.DateTime.Now;
                    Premium.PREMIUM_FID = readerpremium.PREMIUM_ID;
                    Premium.PREMIUM_END_DATE = Premium.BUY_DATE.AddMonths(readerpremium.DURATION_IN_MONTHS);
                    Premium.READER_FID = App.LoggedInReader.READER_ID;
                   


                    var uri = App.Base_url + "api/ReaderPremiums";

                    var httpClientHandler = new HttpClientHandler();
                    httpClientHandler.ServerCertificateCustomValidationCallback =
                        (message, certificate, chain, sslPolicyErrors) => true;
                    var client = new HttpClient(httpClientHandler);

                    string JsonData = JsonConvert.SerializeObject(Premium);
                    StringContent StringData = new StringContent(JsonData, Encoding.UTF8, "application/json");
                    HttpResponseMessage responseMessage = await client.PostAsync(uri, StringData);
                    //string responseData = await responseMessage.Content.ReadAsStringAsync();
                    //var response = JsonConvert.DeserializeObject<Order>(responseData);
                    if (responseMessage.IsSuccessStatusCode)
                    {

                    }

                    UserDialogs.Instance.HideLoading();
                    await Navigation.PushAsync(new SuccessPremium());

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
    }
}