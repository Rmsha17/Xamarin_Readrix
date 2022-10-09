using Acr.UserDialogs;
using Google.Protobuf.WellKnownTypes;
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
using Readrix.Utils;

namespace Readrix.Genre
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ArtifactIndex : ContentPage
    {
        ApiCRUD api = new ApiCRUD();
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

            var epilist = await api.CallApiGetAsync<List<Episodes>>("api/Episode/getepisodesbyid/?id=" + cat1);
            ListData.ItemsSource = epilist;

            var artcomments = await api.CallApiGetAsync<List<Feedback>>("api/Feedback/getartfeedbacks/?id=" + idart);
            listfeedback.ItemsSource = artcomments;

            UserDialogs.Instance.HideLoading();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading("Loading Please Wait...");
            Bookmark bookmark = new Bookmark();
            bookmark.ARTIFACT_FID = idart;
            bookmark.READER_FID = App.LoggedInReader.READER_ID;

            var bookmarkart = await api.CallApiPostAsync("api/Bookmark/addbookmark" , bookmark);
            if(bookmarkart == true)
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Message", "Successfully Bookmarked!!", "ok");

            }
            else
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Message", "Already Bookmarked!!", "ok");
            }

        }

        private async void ListData_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selected = e.Item as Episodes;

            UserDialogs.Instance.ShowLoading("Loading Please Wait...");

          

            var list = await api.CallApiGetAsync<ReaderPremiums>("api/ReaderPremium/getloggedreader?id=" + App.LoggedInReader.READER_ID);

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
                    
                    var postview = await api.CallApiPostAsync("api/View/postview", view);
                    if (postview == true)
                    {
                        UserDialogs.Instance.HideLoading();
                        await Navigation.PushAsync(new EpisodePDF(selected, lblname.Text));
                       
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        await DisplayAlert("Message", "Somthing Went Wrong!!", "ok");
                    }

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

                    var commentpost = await api.CallApiPostAsync("api/Feedback/postfeedback", feedback);

                    if(commentpost == true) { 
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert("Message", "Comment Posted!!", "ok");
                    txtmessage.Text = null;
                    LoadData(); 
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        await DisplayAlert("Message", "Somthing Went Wrong!!", "ok");
                    }

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
