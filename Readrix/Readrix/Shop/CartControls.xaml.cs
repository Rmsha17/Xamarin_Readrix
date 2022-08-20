using Readrix.Models;
using Readrix.ReaderLoginSystem;
using Readrix.ViewModels;
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
    public partial class CartControls : ContentPage
    {
        public CartControls()
        {
            InitializeComponent();
            try
            {
                UpdateCartAsync();
            }
            catch (Exception ex)
            {
                DisplayAlert("Message", "Somthing went wrong this may be a problem with internet or application please ensure that you have a working internet connection and GPS enabled. \nError Details : " + ex.Message, "OK");
            }
        }

        private void UpdateCartAsync()
        {

            List<Cartcell_VM> CartItems = new List<Cartcell_VM>();
            decimal? Amount = 0;
            lblcount.Text = App.Cart.Count().ToString();
            foreach (var item in App.Cart)
            {
                
                decimal? total = item.ARTIFACT_SALEPRICE * (item.QUANTITY);
                Amount += total;

                CartItems.Add(new Cartcell_VM
                {
                    ID = item.SHOPARTIFACT_FID,
                    image = item.ARTIFACT_IMAGE,
                    Name = item.ARTIFACT_NAME ,
                    Detail = "Rs. " + item.ARTIFACT_SALEPRICE + " X  " + item.QUANTITY + "      Total = " + total.ToString() + "Rs"
                });
            }


            App.Total = Amount;

            lblTotal.Text = Amount.ToString();
            DataList.ItemsSource = CartItems;

        }

        private async void btnRemove_Clicked(object sender, EventArgs e)
        {
            try
            {
                ImageButton btn = sender as ImageButton;
                var item = btn.CommandParameter as Cartcell_VM;

                for (int i = 0; i < App.Cart.Count; i++)
                {
                    if (App.Cart[i].SHOPARTIFACT_FID == item.ID)
                    {
                        var res = await DisplayAlert("Question", "Are you sure you want to remove " + item.Name + "?", "Yes", "No");
                        if (res)
                        {
                            App.Cart.RemoveAt(i);
                        }
                    }
                }

                UpdateCartAsync();

            }
            catch (Exception ex)
            {
                await DisplayAlert("Message", "Somthing went wrong this may be a problem with internet or application please ensure that you have a working internet connection and GPS enabled. \nError Details : " + ex.Message, "OK");
            }

        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            if (App.Cart.Count < 1)
            {
                await DisplayAlert("Message", "Cart Page is Empty Please add at least one item in cart", "OK");
                return;
            }

            if (App.LoggedInReader == null)
            {
                var q = await DisplayAlert("Message", "You have to login for to place order.\n\nLog in Now?", "Yes", "No");
                if (q)
                {
                    await Navigation.PushAsync(new ReaderLogin());
                }
            }
            else
            {
                await Navigation.PushAsync(new ConfirmOrder());
            }




        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ShopPage());
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            
            if (App.Cart.Count < 1)
            {
                await DisplayAlert("Message", "Cart Page is Empty Please add at least one item in cart", "OK");
                return;
            }

            if (App.LoggedInReader == null)
            {
                var q = await DisplayAlert("Message", "You have to login for to place order.\n\nLog in Now?", "Yes", "No");
                if (q)
                {
                    await Navigation.PushAsync(new ReaderLogin());
                }
            }
            else
            {
                await Navigation.PushAsync(new Payment());
            }
        }
    }
}