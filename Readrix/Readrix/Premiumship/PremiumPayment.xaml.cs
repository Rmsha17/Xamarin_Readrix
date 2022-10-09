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
using Google.Protobuf.WellKnownTypes;
using Org.BouncyCastle.Asn1.X509;
using Readrix.Utils;
namespace Readrix.Premiumship
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PremiumPayment : ContentPage
    {
        ApiCRUD api = new ApiCRUD();
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
                  

                    var premadd = await api.CallApiPostAsync("api/ReaderPremiums", Premium);

                    if (premadd==true)
                    {
                        
                        MailProvider.SenttoMail(App.LoggedInReader.READER_EMAIL, "Prmiumship Package", "Dear " + App.LoggedInReader.READER_NAME + "!!Your package has been successfull purchased.<br/> Regards Readrix Team");
                        UserDialogs.Instance.HideLoading();
                        await Navigation.PushAsync(new SuccessPremium());

                    }

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