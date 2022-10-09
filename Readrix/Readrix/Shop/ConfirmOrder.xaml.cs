using Google.Protobuf.WellKnownTypes;
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
using Readrix.Utils;
using Acr.UserDialogs;

namespace Readrix.Shop
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfirmOrder : ContentPage
    {
        ApiCRUD api = new ApiCRUD();
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

                var orderadd = await api.CallApiPostedAsync<Order>("api/order/postorder",order);


                foreach (var item in App.Cart)
                {
                    item.ORDER_FID = orderadd.ORDER_ID;
                    item.QUANTITY = item.QUANTITY * -1;
             
                    var orderdetailsadd = await api.CallApiPostAsync("api/orderdetail/postorderdetail", item);

                    if (orderdetailsadd == true)
                    {
                    }

                }
                MailProvider.SenttoMail(App.LoggedInReader.READER_EMAIL, "Order Confirmation", "Dear " + App.LoggedInReader.READER_NAME + "!!Your order has been successfull booked and will be delivered as per terms.<br/> Regards Readrix Team");
                UserDialogs.Instance.HideLoading();
                await Navigation.PushAsync(new Success());
               

            }
            catch (Exception ex)
            {
                await DisplayAlert("Message", "Somthing went wrong this may be a problem with internet or application please ensure that you have a working internet connection and GPS enabled. \nError Details : " + ex.Message, "OK");
            }
        }
    }
}