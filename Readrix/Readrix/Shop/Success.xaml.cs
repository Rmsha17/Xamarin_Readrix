using Readrix.Flyout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Readrix.Shop
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Success : ContentPage
    {
        public Success()
        {
            InitializeComponent();
            App.Cart.Clear();
            App.LoggedInReader.READER_IMAGE = App.LoggedInReader.READER_IMAGE.Replace(App.Base_urlPic, "~/");

        }

        private void btnCurrentLocation_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new ReadrixFlyout();
        }
    }
}