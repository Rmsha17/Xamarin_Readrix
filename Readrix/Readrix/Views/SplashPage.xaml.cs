using Readrix.Flyout;
using Readrix.ReaderLoginSystem;
using Readrix.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Readrix.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashPage : ContentPage
    {
        public SplashPage()
        {
            
            InitializeComponent();
           
        }

        private  void Getstarted_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new ReaderLogin());
        }
    }
}