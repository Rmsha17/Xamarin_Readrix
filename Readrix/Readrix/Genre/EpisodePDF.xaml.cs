using Acr.UserDialogs;
using Newtonsoft.Json;
using Readrix.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Readrix.Genre
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EpisodePDF : ContentPage
    {
  
        public EpisodePDF(Episodes episode, string name)
        {
          
            InitializeComponent();
           string file = episode.EPISODE_FILE.Replace("~/", "/");
            webView.Source = App.EpisodeViewer + file;
            lblepisode.Text = name + "/ Episode No: " + episode.EPISODE_NO;
            Title = "Episode " + episode.EPISODE_NO;
           
        }

    }
}