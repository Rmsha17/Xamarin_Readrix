using Readrix.Models;
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
    public partial class Cart : ContentPage
    {
        public Cart()
        {
            InitializeComponent();
            try
            {
                List<Order_details> CartDatalist = new List<Order_details>();

                foreach (var item in App.Cart)
                {
                  
                    CartDatalist.Add(new Order_details
                    {
                        ARTIFACT_NAME=item.ARTIFACT_NAME,
                        ARTIFACT_IMAGE = item.ARTIFACT_IMAGE,
                        QUANTITY = item.QUANTITY,
                        ARTIFACT_SALEPRICE = item.ARTIFACT_SALEPRICE,
                        Total = (item.ARTIFACT_SALEPRICE * item.QUANTITY),
                    });



                }
                CartList.ItemsSource = CartDatalist;

            }
            catch (Exception ex)
            {

                DisplayAlert("Message", " Some thing wrong...\n\n errors Details.." + ex.Message, "Ok");

            }
        }

    }
}
