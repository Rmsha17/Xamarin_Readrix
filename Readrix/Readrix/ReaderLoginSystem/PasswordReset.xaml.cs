using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Readrix.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using Google.Protobuf.WellKnownTypes;
using Readrix.Utils;
namespace Readrix.ReaderLoginSystem
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PasswordReset : ContentPage
    {
        ApiCRUD api = new ApiCRUD();
        public PasswordReset()
        {
            InitializeComponent();
        }

        private async void btnReset_Clicked(object sender, EventArgs e)
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Loading Please Wait...");
                // Required Field Validator =======================================================================
                if (string.IsNullOrEmpty(txtpassword.Text))
                {
                    await DisplayAlert("Message", "Please Enter new Password", "OK");
                    UserDialogs.Instance.HideLoading();
                    return;
                }

                Reader reader = App.passwardreset;
                reader.READER_PASSWORD = txtpassword.Text;

                var modifiedlist = await api.CallApiPutAsync("api/Readers/", reader.READER_ID, reader);
                

                if (modifiedlist == true)
                {
                    MailProvider.SenttoMail(reader.READER_EMAIL, "Password Resetted" , "Dear " + reader.READER_NAME + "!!Your password has been successfull changed.<br/> Regards Readrix Team");
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert("Successfully", " Your Password has been successfully resetted " + reader.READER_EMAIL + " again", "OK");
                    App.code = 0;
                    App.passwardreset = null;
                    App.Current.MainPage = new NavigationPage(new ReaderLogin());
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert("Error", "Somthing went wrong!!", "OK");
                    App.Current.MainPage = new NavigationPage(new ReaderLogin());
                }

            }
            catch
            {

            }

        }
    }
}