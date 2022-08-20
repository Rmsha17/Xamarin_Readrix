using Acr.UserDialogs;
using Newtonsoft.Json;
using Readrix.Models;
using Readrix.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Ubiety.Dns.Core;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Readrix.Genre
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ArtifactIndex : ContentPage
    {
        public static int cat1;
        public static int idart;
        public ArtifactIndex(Artifact artifact)
        {
            InitializeComponent();
            cat1 = artifact.ARTIFACT_ID;
            lblname.Text = artifact.ARTIFACT_NAME;
            idart = artifact.ARTIFACT_ID;

            lbldescription.Text = artifact.ARTIFACT_DESCRIPTION;
            lbldate.Text = artifact.ARTIFACT_DATE.ToString();
            lblstatus.Text = artifact.ARTIFACT_STATUS;
            artifactimage.Source = artifact.ARTIFACT_PICTURE;
            Title = artifact.ARTIFACT_NAME;
            
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadData();
        }

        private async void LoadData()
        {
            UserDialogs.Instance.ShowLoading("Loading Please Wait...");

            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback =
                (message, certificate, chain, sslPolicyErrors) => true;
            var client = new HttpClient(httpClientHandler);
            var uri = App.Base_url + "api/Episode/getepisodesbyid/?id=" + cat1;
            var result = await client.GetStringAsync(uri);
            List<Episodes> list = JsonConvert.DeserializeObject<List<Episodes>>(result);
            ListData.ItemsSource = list;

            var uri1 = App.Base_url + "api/Feedback/getartfeedbacks/?id=" + idart;
            var result1 = await client.GetStringAsync(uri1);
            List<Feedback> list1 = JsonConvert.DeserializeObject<List<Feedback>>(result1);
            
            List<Feedback> modifiedlist = new List<Feedback>();
            foreach(var item in list1){
                var uri2 = App.Base_url + "api/Readers/" + item.READER_FID;
                var result2 = await client.GetStringAsync(uri2);
                Reader reader = JsonConvert.DeserializeObject<Reader>(result2);
                item.READER_NAME = reader.READER_NAME;
                item.READER_IMAGE = reader.READER_IMAGE;
                modifiedlist.Add(item);
            }
            listfeedback.ItemsSource = modifiedlist;
            UserDialogs.Instance.HideLoading();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading("Loading Please Wait...");
            Bookmark bookmark = new Bookmark();
            bookmark.ARTIFACT_FID = idart;
            bookmark.READER_FID = App.LoggedInReader.READER_ID;
            var uri = App.Base_url + "api/Bookmark/addbookmark";

            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback =
                (message, certificate, chain, sslPolicyErrors) => true;
            var client = new HttpClient(httpClientHandler);

            string JsonData = JsonConvert.SerializeObject(bookmark);
            StringContent StringData = new StringContent(JsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage responseMessage = await client.PostAsync(uri, StringData);
            string responseData = await responseMessage.Content.ReadAsStringAsync();
            Response response = JsonConvert.DeserializeObject<Response>(responseData);
            UserDialogs.Instance.HideLoading();
            await DisplayAlert("Message", "Successfully Bookmarked!!", "ok");

        }

        private async void ListData_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selected = e.Item as Episodes;

            UserDialogs.Instance.ShowLoading("Loading Please Wait...");

            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback =
                (message, certificate, chain, sslPolicyErrors) => true;
            var client = new HttpClient(httpClientHandler);
            var uri = App.Base_url + "api/ReaderPremium/getloggedreader?id=" + App.LoggedInReader.READER_ID;
            var result = await client.GetStringAsync(uri);
            ReaderPremiums list = JsonConvert.DeserializeObject<ReaderPremiums>(result);

            if (list != null)
            {
                if (list.PREMIUM_END_DATE > DateTime.Now)
                {
                    var view = new EpiView
                    {
                        READER_FID = App.LoggedInReader.READER_ID,
                        ARTIFACT_FID = idart,
                        VIEW_DATE = System.DateTime.Now,
                        EPISODE_FID = selected.EPISODE_ID
                    };
                    var uri2 = App.Base_url + "api/View/postview";
                   
                    string JsonData = JsonConvert.SerializeObject(view);
                    StringContent StringData = new StringContent(JsonData, Encoding.UTF8, "application/json");
                    HttpResponseMessage responseMessage = await client.PostAsync(uri2, StringData);
                    string responseData = await responseMessage.Content.ReadAsStringAsync();
                    Response response = JsonConvert.DeserializeObject<Response>(responseData);
                    await Navigation.PushAsync(new EpisodePDF(selected,lblname.Text));
                    UserDialogs.Instance.HideLoading();
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert("Message", "Your Premium has been ended Please Renew it to enjoy our latest Artifacts!!", "OK");
                }

            }
            else
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Message", "Your Premium has been ended Please Renew it to enjoy our latest Artifacts!!", "OK");
            }

            UserDialogs.Instance.HideLoading();
        }

        private async void listfeedback_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selected = e.Item as Feedback;
            await DisplayAlert(selected.READER_NAME , "Comment:" + selected.FEEDBACK_DESCRIPTION, "Ok");
        }

        private async void btnfeeback_Clicked(object sender, EventArgs e)
        {
           
            if (string.IsNullOrEmpty(txtmessage.Text))
            {
                await DisplayAlert("Error", "Please Enter your comment", "ok");
                return;
            }
            else
            {

                try
                {
                    UserDialogs.Instance.ShowLoading("Comment posting...");
                    var feedback = new Feedback
                    {
                        FEEDBACK_DESCRIPTION = txtmessage.Text,
                        FEEDBACK_DATE = System.DateTime.Now,
                        READER_FID = App.LoggedInReader.READER_ID,
                        ARTIFACT_FID = idart
                    };

                    var uri = App.Base_url + "api/Feedback/postfeedback";

                    var httpClientHandler = new HttpClientHandler();
                    httpClientHandler.ServerCertificateCustomValidationCallback =
                        (message, certificate, chain, sslPolicyErrors) => true;
                    var client = new HttpClient(httpClientHandler);

                    string JsonData = JsonConvert.SerializeObject(feedback);
                    StringContent StringData = new StringContent(JsonData, Encoding.UTF8, "application/json");

                    HttpResponseMessage responseMessage = await client.PostAsync(uri, StringData);
                    string responseData = await responseMessage.Content.ReadAsStringAsync();
                    Response response = JsonConvert.DeserializeObject<Response>(responseData);


                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert("Message", "Comment Posted!!", "ok");
                    txtmessage.Text = null;
                    LoadData(); 
                }
                catch (Exception ex)
                {
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert("message", "Error Occured please try again:" + ex.Message, "ok");
                }


            }
        }

    }
    }
