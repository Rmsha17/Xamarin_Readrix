using Readrix.Flyout;
using Readrix.Models;
using Readrix.ReaderLoginSystem;
using Readrix.Services;
using Readrix.Views;
using Readrix.Schedule;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Readrix.Writers;
using Readrix.Shop;
using Readrix.Settingreader;
using Readrix.Premiumship;
using System.Collections.Generic;
using Plugin.FirebasePushNotification;

namespace Readrix
{
    public partial class App : Application
    {
        public static string Base_url = "https://readrixapi.blackpanthermart.com/";
        public static string Base_urlPic = "https://readrix.blackpanthermart.com/";
        public static string EpisodeViewer = "https://drive.google.com/viewerng/viewer?embedded=true&url=https://readrix.blackpanthermart.com";
        public static List<Order_details> Cart = new List<Order_details>();
        public static Reader LoggedInReader = null;
     
        public static decimal? Total = 0;
        public App()
        {
            InitializeComponent();
            MainPage = new Splashscreen();

            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine($"TOKEN : {p.Token}");
            };
            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {

                System.Diagnostics.Debug.WriteLine("Received");
                foreach (var data in p.Data)
                {
                    System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                }

            }; 
            //CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            //{
            //    System.Diagnostics.Debug.WriteLine("Opened");
            //    foreach (var data in p.Data)
            //    {
            //        System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
            //    }

            //};
        }
      
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
