using Readrix.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Readrix.Schedule
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Schedulepage : ContentPage
    {
       readonly Monday Mon = new Monday();
       readonly Tuesday Tue = new Tuesday();
       readonly Wednesday Wed = new Wednesday();
       readonly Thursday Thu = new Thursday();
       readonly Friday Fri = new Friday();
       readonly Saturday Sat = new Saturday();
       readonly Sunday Sun = new Sunday();
       readonly Daily Daily = new Daily();
        public Schedulepage()
        {
            InitializeComponent();
        }
        void Handle_ValueChanged(object sender, SegmentedControl.FormsPlugin.Abstractions.ValueChangedEventArgs e)
        {
            switch (e.NewValue)
            {
                case 0:
                    SegContent.Children.Clear();
                    SegContent.Children.Add(Daily);
                    break;
                case 1:
                    SegContent.Children.Clear();
                    SegContent.Children.Add(Sun);
                    break;
                case 2:
                    SegContent.Children.Clear();
                    SegContent.Children.Add(Mon);
                    break;
                case 3:
                    SegContent.Children.Clear();
                    SegContent.Children.Add(Tue);
                    break;
                case 4:
                    SegContent.Children.Clear();
                    SegContent.Children.Add(Wed);
                    break;
                case 5:
                    SegContent.Children.Clear();
                    SegContent.Children.Add(Thu);
                    break;
               case 6:
                    SegContent.Children.Clear();
                    SegContent.Children.Add(Fri);
                    break;
              case 7:
                    SegContent.Children.Clear();
                    SegContent.Children.Add(Sat);
                    break;
            }
        }
    }
}