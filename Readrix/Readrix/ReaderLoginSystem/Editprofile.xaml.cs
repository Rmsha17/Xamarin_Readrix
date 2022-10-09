using Acr.UserDialogs;
using Google.Protobuf.WellKnownTypes;
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
using Readrix.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Readrix.ReaderLoginSystem
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Editprofile : ContentPage
    {
        ApiCRUD api = new ApiCRUD();
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
              
                if (isnewpictureselected == true)
                {
                    var content = new MultipartFormDataContent();
                    content.Add(new StreamContent(_mediaFile.GetStream()), "\"file\"", $"\"{_mediaFile.Path}\"");

                    string ResponseMessage = await api.CallApiPostimageAsync("api/readers/postimage", content);
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

                var modifiedlist = await api.CallApiPutAsync("api/Readers/", reader.READER_ID, reader);

                LoadData();
                if (modifiedlist == true)
                {
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert("Success", "Successfully Update Profile", "OK");

                    App.LoggedInReader = reader;
                    LoadData();

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