using Acr.UserDialogs;
using Newtonsoft.Json;
using Readrix.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Readrix.ReaderLoginSystem
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotPassword : ContentPage
    {
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

                UserDialogs.Instance.ShowLoading("Loading Please Wait...");
                //var client = new HttpClient();
                var httpClientHandler = new HttpClientHandler();
                httpClientHandler.ServerCertificateCustomValidationCallback =
                    (message, certificate, chain, sslPolicyErrors) => true;
                var client = new HttpClient(httpClientHandler);

                var uri = App.Base_url + "api/readers/forgotpassword?email=" + txtEmailReset.Text;

                var result = await client.GetStringAsync(uri);

                Reader check = JsonConvert.DeserializeObject<Reader>(result);

                if (check == null)
                {
                    await DisplayAlert("Message", "The email you have entered is not registered.", "OK");
                    return;
                }



                // EMAIL SENDING ================================================================

                MailMessage mail = new MailMessage();
                mail.To.Add(check.READER_EMAIL);
                mail.From = new MailAddress("readrix28@gmail.com", "Password Forgotton",System.Text.Encoding.UTF8);
                mail.Subject = "Password Forgot Request";
                mail.SubjectEncoding = System.Text.Encoding.UTF8;

                mail.Body = "Dear Reader, Your Current Login Details are as Follows : <br><br><br>Username = " + check.READER_EMAIL + " <br>Password = " + check.READER_PASSWORD + " <br><br>Readrix Team";
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                SmtpClient client2 = new SmtpClient("smtp.gmail.com", 587);
                client2.Credentials = new System.Net.NetworkCredential("readrix28@gmail.com", "ejinwqithimbfxxs");
                client2.EnableSsl = true;

                client2.Send(mail);

                await DisplayAlert("Message", "Your Login Details are sent to your email address please find that in your inbox", "OK");
                await Navigation.PopAsync();

                UserDialogs.Instance.HideLoading();

            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Message", "Somthing went wrong this may be a problem with internet or application please ensure that you have a working internet connectiony . \nError Details : " + ex.Message, "OK");
            }
        }

    }
}