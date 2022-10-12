using Acr.UserDialogs;
using Google.Protobuf.WellKnownTypes;
using Newtonsoft.Json;
using Readrix.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Readrix.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Readrix.ReaderLoginSystem
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotPassword : ContentPage
    {
        ApiCRUD api = new ApiCRUD();
        public ForgotPassword()
        {
            InitializeComponent();
        }
        private async void btnReset_Clicked(object sender, EventArgs e)
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Loading Please Wait...");
                // Required Field Validator =======================================================================
                if (string.IsNullOrEmpty(txtEmailReset.Text))
                {
                    await DisplayAlert("Message", "Please Enter Email", "OK");
                    return;
                }

              
               
                var check = await api.CallApiGetAsync<Reader>("api/readers/forgotpassword?email=" + txtEmailReset.Text);
                if (check == null)
                {
                    await DisplayAlert("Message", "The email you have entered is not registered.", "OK");
                    return;
                }

                MailProvider.SenttoMail(check.READER_EMAIL, "Password Forgot Request", "Dear " + check.READER_NAME + "!!Your Account Credentials are :<br/>" + check.READER_EMAIL + "<br/>"+ check.READER_PASSWORD + " <br/> Regards Readrix Team");
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Message", "Your Login Details are sent to your email address please find that in your inbox", "OK");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Message", "Somthing went wrong this may be a problem with internet or application please ensure that you have a working internet connectiony . \nError Details : " + ex.Message, "OK");
            }
        }

        private async void btnPassReset_Clicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading("Loading Please Wait...");
            // Required Field Validator =======================================================================
            if (string.IsNullOrEmpty(txtEmailReset.Text))
            {
                await DisplayAlert("Message", "Please Enter Email", "OK"); 
                UserDialogs.Instance.HideLoading();
                return;
            }
            var check = await api.CallApiGetAsync<Reader>("api/readers/forgotpassword?email=" + txtEmailReset.Text);
            if (check == null)
            {
                await DisplayAlert("Message", "The email you have entered is not registered.", "OK");
                UserDialogs.Instance.HideLoading();
                return;
            }

            Random random = new Random();
            int code = random.Next(1001, 9999);
            App.code = code;
            App.passwardreset = check;
            MailProvider.SenttoMail(check.READER_EMAIL, "Passward Reset Code", "Dear " + check.READER_NAME + "!!Please verify this code " + code +".<br/> Regards Readrix Team");
            UserDialogs.Instance.HideLoading();
            await Navigation.PushAsync(new VerifyCode());
        }
    }
}