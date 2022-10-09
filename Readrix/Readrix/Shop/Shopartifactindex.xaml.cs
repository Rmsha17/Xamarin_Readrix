using Readrix.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Readrix.Shop
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Shopartifactindex : ContentPage
    {
        Artifact Art = new Artifact();
        public Shopartifactindex(Artifact artifact)
        {
            InitializeComponent();
            Art = artifact;
            lblname.Text = artifact.ARTIFACT_NAME;
            lbldescription.Text = artifact.ARTIFACT_DESCRIPTION;
            lblprice.Text = artifact.SALE_PRICE.ToString();
            imageartifact.Source = artifact.ARTIFACT_PICTURE;
            lblqty.Text = artifact.Available_Quantity.ToString();


        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var item = Art;
            if(Art.Available_Quantity != 0) { 
            int Quantity = 0;
            var QtyRaw = await DisplayActionSheet("Select Quantity", "Close", "", "1", "2","3","4","5","10");
            if (QtyRaw != "Other" && QtyRaw != "Close" && QtyRaw != null )
            {
                Quantity = int.Parse(QtyRaw);

            }
            else
            {
                await DisplayAlert("Message", "Please select quantity at least 1", "OK");
                return;
            }

            int index = -1;
            for (int i = 0; i < App.Cart.Count; i++)
            {
                if (item.shopartidactid == App.Cart[i].SHOPARTIFACT_FID)
                {
                    index = i;
                    var ques = await DisplayAlert("Message", item.ARTIFACT_NAME + " is already entered in Cart do you want to increase the quantity of already entered item?", "Yes", "No");
                    if (ques)
                    {
                       
                            App.Cart[index].QUANTITY += Quantity;
                            if (Art.Available_Quantity >= App.Cart[index].QUANTITY)
                            {
                                await DisplayAlert("Message", item.ARTIFACT_NAME + " quantity increased... ", "OK");
                            }
                            else
                            {
                                App.Cart[index].QUANTITY -= Quantity;
                                await DisplayAlert("Message", item.ARTIFACT_NAME + "Not Enough Quantity available. ", "OK");
                            }

                    }
                }
            }

            if (index == -1)
            {
                App.Cart.Add(new Order_details { SHOPARTIFACT_FID = item.shopartidactid, QUANTITY = Quantity, ARTIFACT_PURCHASEPRICE = item.PURCHASE_PRICE, ARTIFACT_SALEPRICE = item.SALE_PRICE, ARTIFACT_NAME = item.ARTIFACT_NAME, ARTIFACT_IMAGE = item.ARTIFACT_PICTURE });
                await DisplayAlert("Message", item.ARTIFACT_NAME + " is added to cart... ", "OK");

            }
            }
            else
            {
                await DisplayAlert("Message", "Out of Stock", "OK");
                return;
            }
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CartControls());

        }

       
    }
}