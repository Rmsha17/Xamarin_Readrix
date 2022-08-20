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

namespace Readrix.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Genres : ContentPage
    {
        
        Comicsgenres view1 = new Comicsgenres();
        Novelsgenres view2 = new Novelsgenres();
        public Genres()
        {
            InitializeComponent();
            
        }
      
        void Handle_ValueChanged(object sender, SegmentedControl.FormsPlugin.Abstractions.ValueChangedEventArgs e)
        {
            switch (e.NewValue)
            {
                case 0:
                    SegContent.Children.Clear();
                    SegContent.Children.Add(view1);
                    break;
                case 1:
                    SegContent.Children.Clear();
                    SegContent.Children.Add(view2);
                    break;
            }
        }
    }
}