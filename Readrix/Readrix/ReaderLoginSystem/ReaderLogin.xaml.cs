using Acr.UserDialogs;
using MongoDB.Driver.Core.WireProtocol.Messages;
using Newtonsoft.Json;
using Readrix.Flyout;
using Readrix.Models;
using Readrix.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Ubiety.Dns.Core;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Readrix.ReaderLoginSystem
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReaderLogin : ContentPage
    {
       

        public ReaderLogin()
        {
            InitializeComponent();

        }

     
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ReaderRegistration());
        }

        private async void Signin_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                await DisplayAlert("Error", "Please Fill all the required fields and try again!!", "ok");
                return;
            }
            else
            {
                try
                {
                    UserDialogs.Instance.ShowLoading("Loading Please Wait...");
                    //var client = new HttpClient();
                    var httpClientHandler = new HttpClientHandler();
                    httpClientHandler.ServerCertificateCustomValidationCallback =
                        (message, certificate, chain, sslPolicyErrors) => true;
                    var client = new HttpClient(httpClientHandler);
                   
                    var uri = App.Base_url + "api/readers/loginchk?email=" + txtEmail.Text + "&password=" + txtPassword.Text;

                    var result = await client.GetStringAsync(uri);

                    Reader Reader = JsonConvert.DeserializeObject<Reader>(result);
                    
                    if (Reader != null)
                    {
                        if(Reader.IS_ACCOUNT_ACTIVATE == true)
                        {
                            UserDialogs.Instance.HideLoading();
                            App.LoggedInReader = Reader;
                            App.Current.MainPage = new ReadrixFlyout();
                        }
                        else
                        {
                            UserDialogs.Instance.HideLoading();
                            await DisplayAlert("Oops", "Your Prevoius Account has been deleted. please Re-Create New!!", "ok");
                        }
                        
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        await DisplayAlert("message", "Somthing went wrong", "ok");
                    }

                }
                catch (Exception ex)
                {
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert("message", "Error Occured please try again:" + ex.Message, "ok");
                }
            }
        }

        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ForgotPassword());
        }
    }
}