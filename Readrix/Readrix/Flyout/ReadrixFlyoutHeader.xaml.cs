using Readrix.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Readrix.ReaderLoginSystem
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReadrixFlyoutHeader : ContentView
    {
        public ReadrixFlyoutHeader()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            if (App.LoggedInReader != null)
            {
                Reader reader = new Reader();
                reader = App.LoggedInReader;
                readeremail.Text = reader.READER_EMAIL;
                readerimage.Source = reader.READER_IMAGE;
                readername.Text = reader.READER_NAME;
            }

        }
    }
}