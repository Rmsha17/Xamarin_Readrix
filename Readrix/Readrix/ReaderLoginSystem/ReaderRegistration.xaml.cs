using System;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Readrix.Models;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Acr.UserDialogs;
using System.Text.RegularExpressions;

namespace Readrix.ReaderLoginSystem
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReaderRegistration : ContentPage
    {
        public static string PicPath = "Usericon.png";
       
        private MediaFile _mediaFile;
        public ReaderRegistration()
        {
            InitializeComponent();
        }
       
        private void Button_Clicked(object sender, EventArgs e)
        {
            new  NavigationPage(new ReaderRegistration());
        }
        private async void NavigateButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ReaderLogin());
        }

        private async void RegisterAccount(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtPassword.Text) || string.IsNullOrEmpty(txtAddress.Text) || string.IsNullOrEmpty(txtContact.Text) || _mediaFile == null)
            {
                await DisplayAlert("Error", "Please Fill all the required fields and try again!!", "ok");
                return;
            }
            else
            {

                try
                {
                    UserDialogs.Instance.ShowLoading("Loading Please Wait...");

                    var content = new MultipartFormDataContent();
                    content.Add(new StreamContent(_mediaFile.GetStream()), "\"file\"", $"\"{_mediaFile.Path}\"");
                    var uri1 = App.Base_urlPic + "api/readers/postimage";

                    var httpClientHandler = new HttpClientHandler();
                    httpClientHandler.ServerCertificateCustomValidationCallback =
                        (message, certificate, chain, sslPolicyErrors) => true;
                    var client = new HttpClient(httpClientHandler);
                    var httpResponse = await client.PostAsync(uri1, content);
                    string ResponseMessage = await httpResponse.Content.ReadAsStringAsync();
                    string image = ResponseMessage.Substring(1, ResponseMessage.Length - 2);
                    
                  //await DisplayAlert("Message", ResponseMessage, "ok");



                    var reader = new Reader
                    {
                        READER_EMAIL = txtEmail.Text,
                        READER_NAME = txtName.Text,
                        READER_CONTACT = txtContact.Text,
                        READER_PASSWORD = txtPassword.Text,
                        READER_ADDRESS = txtAddress.Text,
                        READER_IMAGE = image,
                        IS_ACCOUNT_ACTIVATE = true

                    };
                    
                    var uri = App.Base_url + "api/readers/postreader";
                   

                    string JsonData = JsonConvert.SerializeObject(reader);
                    StringContent StringData = new StringContent(JsonData, Encoding.UTF8, "application/json");

                    HttpResponseMessage responseMessage = await client.PostAsync(uri, StringData);
                    string responseData = await responseMessage.Content.ReadAsStringAsync();
                   //var response = JsonConvert.DeserializeObject<Response>(responseData);
                    
                    if (responseData != "")
                    {
                        UserDialogs.Instance.HideLoading();
                        await DisplayAlert("Info", "Email Already Existed!!", "ok");
                        await Navigation.PushAsync(new ReaderRegistration());
                    }
                    if (responseData == "")
                    {
                        UserDialogs.Instance.HideLoading();
                        await DisplayAlert("Success", "Successfully Created!!", "ok");
                        await Navigation.PushAsync(new ReaderLogin());
                    }
                    
                }
                catch (Exception ex)
                {
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert("message", "Error Occured please try again:" + ex.Message, "ok");
                }

                
            }
        }


        private async void Button_Clicked_1(object sender, EventArgs e)
        {
          
            try
            {
                var response = await DisplayActionSheet("Select Image Source", "Close", "", "From Gallery", "From Camera");
                if (response == "From Camera")
                {
                    await CrossMedia.Current.Initialize();
                    if (!CrossMedia.Current.IsTakePhotoSupported)
                    {
                        await DisplayAlert("Error", "Phone is not Take Photo Supported", "OK");
                        return;
                    }

                    var mediaOptions = new StoreCameraMediaOptions()
                    {
                        PhotoSize = PhotoSize.Medium
                    };

                    var SelectedImg = await CrossMedia.Current.TakePhotoAsync(mediaOptions);

                    if (SelectedImg == null)
                    {
                        await DisplayAlert("Error", "Error Taking Image...", "OK");
                        return;
                    }

                    _mediaFile = SelectedImg;

                    PicPath = SelectedImg.Path;
                    imgfile.Source = SelectedImg.Path;

                }
                if (response == "From Gallery")
                {
                    await CrossMedia.Current.Initialize();
                    if (!CrossMedia.Current.IsPickPhotoSupported)
                    {
                        await DisplayAlert("Error", "Phone is not Pick Photo Supported", "OK");
                        return;
                    }

                    var mediaOptions = new PickMediaOptions()
                    {
                        PhotoSize = PhotoSize.Medium
                    };

                    var SelectedImg = await CrossMedia.Current.PickPhotoAsync(mediaOptions);

                    if (SelectedImg == null)
                    {
                        await DisplayAlert("Error", "Error Picking Image...", "OK");
                        return;
                    }
                    _mediaFile = SelectedImg;
                    PicPath = SelectedImg.Path;
                    imgfile.Source = SelectedImg.Path;


                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Message", "Something Went wrong \n" + ex.Message, "OK");

            }

        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
             await Navigation.PushAsync(new ReaderLogin());

        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            var EmailPattern = @"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$";
            if (Regex.IsMatch(e.NewTextValue, EmailPattern))
            {
                
                lblEmailVali.Text = "Valid Email";
                lblEmailVali.TextColor = Color.Green;
            }
            else
            {
                lblEmailVali.Text = "InValid Email! Email must contain @ and .com";
                lblEmailVali.TextColor = Color.Red;
            }
        }

        private void txtPassword_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (e.NewTextValue.Length < 8)
            {
                lblPassVali.IsVisible = true;
                lblPassVali.Text = "Password should be of at least 8 charaters";
                lblPassVali.TextColor = Color.Red;
            }

            var PasswordPattern = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])";
            if (Regex.IsMatch(e.NewTextValue, PasswordPattern))
            {
                lblPassVali.Text = "Strong Password!";
                lblPassVali.TextColor = Color.Green;
            }
           
            else
            {
                lblPassVali.Text = "Weak Password! Password should contain Uppercase Letter , Lowercase Letter, Number(s) and special characters [$@$!%*#?&]";
                lblPassVali.TextColor = Color.Red;
            }
        }

        private void txtContact_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (e.NewTextValue.Length < 11 || e.NewTextValue.Length > 13)
            {
                lblPhoneVali.IsVisible = true;
                lblPhoneVali.Text = "InValid Phone! Missing digit(s)";
                lblPhoneVali.TextColor = Color.Red;
            }
           
            else
            {
                lblPhoneVali.Text = "Valid Phone";
                lblPhoneVali.TextColor = Color.Green;
            }
        }

        private void txtAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length < 15 )
            {
                lbladdVali.IsVisible = true;
                lbladdVali.Text = "Provide Complete Address for Product delivery purpuse!";
                lbladdVali.TextColor = Color.Red;
            }
            else
            {
                lbladdVali.Text = "Valid Address";
                lbladdVali.TextColor = Color.Green;
            }
        }
    }
}