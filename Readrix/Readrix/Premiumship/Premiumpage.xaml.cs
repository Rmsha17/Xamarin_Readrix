using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Readrix.Premiumship
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Premiumpage : ContentPage
    {
        Premium3months p3m = new Premium3months();
        Premium6months p6m = new Premium6months();
        public Premiumpage()
        {
            InitializeComponent();
        }
        void Handle_ValueChanged(object sender, SegmentedControl.FormsPlugin.Abstractions.ValueChangedEventArgs e)
        {
            switch (e.NewValue)
            {
                case 0:
                    SegContent.Children.Clear();
                    SegContent.Children.Add(p3m);
                    break;
                case 1:
                    SegContent.Children.Clear();
                    SegContent.Children.Add(p6m);
                    break;
            }
        }
    }
}