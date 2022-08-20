using Acr.UserDialogs;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Readrix.Flyout;
using Readrix.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Readrix.ReaderLoginSystem
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Editprofile : ContentPage
    {
        public static string PicPath = App.LoggedInReader.READER_IMAGE;
        public static bool isnewpictureselected = false;
        private MediaFile _mediaFile;
        private string image = App.LoggedInReader.READER_IMAGE;
        public Editprofile()
        {
            InitializeComponent();
           
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadData();
        }
        private void LoadData()
        {
            txtName.Text = App.LoggedInReader.READER_NAME;
            txtAddress.Text = App.LoggedInReader.READER_ADDRESS;
            txtContact.Text = App.LoggedInReader.READER_CONTACT;
            txtPassword.Text = App.LoggedInReader.READER_PASSWORD;
            txtEmail.Text = App.LoggedInReader.READER_EMAIL;
            imgfile.Source = App.LoggedInReader.READER_IMAGE;

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {

            try
            {
                UserDialogs.Instance.ShowLoading("Loading Please Wait...");
                var httpClientHandler = new HttpClientHandler();
                httpClientHandler.ServerCertificateCustomValidationCallback =
                    (message, certificate, chain, sslPolicyErrors) => true;
                var client = new HttpClient(httpClientHandler);
                if (isnewpictureselected == true)
                {
                    var content = new MultipartFormDataContent();
                    content.Add(new StreamContent(_mediaFile.GetStream()), "\"file\"", $"\"{_mediaFile.Path}\"");
                    var uri1 = App.Base_urlPic + "api/readers/postimage";


                    var httpResponse = await client.PostAsync(uri1, content);
                    string ResponseMessage = await httpResponse.Content.ReadAsStringAsync();
                    image = ResponseMessage.Substring(1, ResponseMessage.Length - 2);

                }

                var reader = new Reader
                {
                    READER_ID = App.LoggedInReader.READER_ID,
                    READER_EMAIL = txtEmail.Text,
                    READER_NAME = txtName.Text,
                    READER_CONTACT = txtContact.Text,
                    READER_PASSWORD = txtPassword.Text,
                    READER_ADDRESS = txtAddress.Text,
                    READER_IMAGE = image,
                    IS_ACCOUNT_ACTIVATE = true

                };
               

                var uri = App.Base_url + "api/Readers/" + reader.READER_ID;
                var json = JsonConvert.SerializeObject(reader);
                HttpContent httpContent = new StringContent(json);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage result = await client.PutAsync(uri, httpContent);

              
                if (result.IsSuccessStatusCode)
                {
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert("Success", "Successfully Update Profile", "OK");
                   //reader.READER_IMAGE =reader.READER_IMAGE.Replace("~/", App.Base_urlPic);
                    App.LoggedInReader = reader;
                    LoadData();
                    // await Navigation.PopAsync();
                    App.Current.MainPage = new ReadrixFlyout();
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert("Error", "Somthing went wrong!!", "OK");
                }

            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("message", "Error Occured please try again:" + ex.Message, "ok");
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
                   
                    imgfile.Source = SelectedImg.Path;
                    isnewpictureselected = true;
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
                   
                    imgfile.Source = SelectedImg.Path;
                    isnewpictureselected = true;

                }
               

            }
            catch (Exception ex)
            {
                await DisplayAlert("Message", "Something Went wrong \n" + ex.Message, "OK");

            }

        }
    }
}