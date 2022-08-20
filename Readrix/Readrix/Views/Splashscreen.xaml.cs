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
    public partial class Splashscreen : ContentPage
    {
        public Splashscreen()
        {
            InitializeComponent();

            Animation();
        }
        public async void Animation()
        {
            await Task.Delay(5000);
            Application.Current.MainPage = new NavigationPage(new SplashPage());
        }
    }
}