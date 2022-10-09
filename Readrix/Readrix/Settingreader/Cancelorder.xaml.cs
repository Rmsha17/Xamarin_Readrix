using Acr.UserDialogs;
using Google.Protobuf.WellKnownTypes;
using Newtonsoft.Json;
using Readrix.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Readrix.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Readrix.Settingreader
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Cancelorder : ContentPage
    {
        ApiCRUD api = new ApiCRUD();
        public Cancelorder()
        {
            InitializeComponent();
            LoadData();
        }
        private async void LoadData()
        {
            UserDialogs.Instance.ShowLoading("Loading Please Wait...");

            var modifiedlist = await api.CallApiGetAsync<List<Order>>("api/readers/getordercancel/?id=" + App.LoggedInReader.READER_ID);
            if (modifiedlist == null)
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

                    var modifiedlist = await api.CallApiPutAsync("api/Orders/", selected.ORDER_ID, Order);

                    LoadData();
                    if (modifiedlist == true)
                    {
                        MailProvider.SenttoMail(App.LoggedInReader.READER_EMAIL, "Oder Re-Booked", "Dear " + App.LoggedInReader.READER_NAME + "!!Your order has been successfull re-booked.<br/> Regards Readrix Team");
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