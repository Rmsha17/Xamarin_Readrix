using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace Readrix.ReaderLoginSystem
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerifyCode : ContentPage
    {
        public VerifyCode()
        {
            InitializeComponent();
        }

        private async void btnReset_Clicked(object sender, EventArgs e)
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Loading Please Wait...");
                // Required Field Validator =======================================================================
                if (string.IsNullOrEmpty(txtcode.Text))
                {
                    await DisplayAlert("Message", "Please Enter Code", "OK");
                    UserDialogs.Instance.HideLoading();
                    return;
                }

                if (App.code.ToString() == txtcode.Text)
                {
                    await DisplayAlert("Message", "Code Verified!!Please Set your new Password", "OK");
                    UserDialogs.Instance.HideLoading();
                    await Navigation.PushAsync(new PasswordReset());
                }
            }
            catch
            {

            }

          }
    }
}