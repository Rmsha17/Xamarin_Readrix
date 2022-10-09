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
using Readrix.Utils;
using Google.Protobuf.WellKnownTypes;

namespace Readrix.ReaderLoginSystem
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReaderLogin : ContentPage
    {

        ApiCRUD api = new ApiCRUD();
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
       
                    var Reader = await api.CallApiGetAsync<Reader>("api/readers/loginchk?email=" + txtEmail.Text + "&password=" + txtPassword.Text);
                    if (Reader.READER_EMAIL != null)
                    {
                        if(Reader.IS_ACCOUNT_ACTIVATE == true)
                        {
                            UserDialogs.Instance.HideLoading();
                            App.LoggedInReader = Reader;
                            App.Current.MainPage = new ReadrixFlyout();
                        } 
                        else if(Reader.IS_ACCOUNT_ACTIVATE != true)
                        {
                            UserDialogs.Instance.HideLoading();
                            await DisplayAlert("Oops", "Your Prevoius Account has been deleted. please Re-Create New!!", "ok");
                        }
                        else
                        {
                            UserDialogs.Instance.HideLoading();
                            await DisplayAlert("Oops", "Something went wrong.Please Try again !!", "ok");
                        }
                        
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        await DisplayAlert("Oops", "Incorrect Email OR Passwoed. please Re-Enter !!", "ok");
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