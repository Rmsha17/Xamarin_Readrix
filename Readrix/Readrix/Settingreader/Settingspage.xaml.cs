using Acr.UserDialogs;
using Google.Protobuf.WellKnownTypes;
using Readrix.ReaderLoginSystem;
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
    public partial class Settingspage : ContentPage
    {
        ApiCRUD api = new ApiCRUD();
        public Settingspage()
        {
            InitializeComponent();
        }

        private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var select = e.Item as string;
            if (select == "Booked Orders")
            {
                await Navigation.PushAsync(new Orderhistory());
            }
            if (select == "Delete Account")
            {
                var q = await DisplayAlert("Successfully", "Are you sure to Delete your account permanently!", "Yes", "No");
                if (q)
                {
                    UserDialogs.Instance.ShowLoading("Loading Please Wait...");


                    var result = await api.CallApiDeleteAsync("api/Reader/removereader/?id=" +App.LoggedInReader.READER_ID);

                    if (result == true)
                    {
                        UserDialogs.Instance.HideLoading();
                        await DisplayAlert("Success", " Permanently Delete your account. You wont be able to login with this account .Thank You.", "OK");
                        App.LoggedInReader = null;
                        App.Current.MainPage = new NavigationPage(new ReaderLogin());
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        await DisplayAlert("Error", "Somthing went wrong!!", "OK");
                    }
                }
  
            }
            if (select == "Cancelled Orders")
            {
                await Navigation.PushAsync(new Cancelorder());
            }
            if (select == "Login to another Account")
            {
                await Navigation.PushAsync(new ReaderLogin());
            }
            if (select == "Logout")
            {
                bool q = await DisplayAlert("Message", "Are you sure to log out!", "Yes", "No");
                if (q)
                {
                    App.LoggedInReader = null;
                    App.Current.MainPage = new NavigationPage(new ReaderLogin());
                }
            }
            if (select == "Manage Profile")
            {
                await Navigation.PushAsync(new Editprofile());
            }
        }
    }
}