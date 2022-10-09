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

namespace Readrix.Shop
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Payment : ContentPage
    {
        ApiCRUD api = new ApiCRUD();
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


                    var orderadd = await api.CallApiPostedAsync<Order>("api/order/postorder", order);


                    foreach (var item in App.Cart)
                    {
                        item.ORDER_FID = orderadd.ORDER_ID;
                        item.QUANTITY = item.QUANTITY * -1;

                        var orderdetailsadd = await api.CallApiPostAsync("api/orderdetail/postorderdetail", item);

                        if (orderdetailsadd == true)
                        {
                        }

                    }
                    MailProvider.SenttoMail(App.LoggedInReader.READER_EMAIL, "Order Confirmation", "Dear " + App.LoggedInReader.READER_NAME + "!!Your order has been successfull purchased and will be delivered as per terms.<br/> Regards Readrix Team");
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