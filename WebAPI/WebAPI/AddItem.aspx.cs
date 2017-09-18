using System;
using System.Diagnostics;
using WebAPI.Controllers;

namespace WebAPI
{
    public partial class AddItem : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Guid productID = Guid.Parse(Request.QueryString["ProductID"]);
            if (productID != null)
            {
                using (CartController cartController = new CartController())
                {
                    cartController.AddItemToCart(productID);
                }
            }
            else
            {
                Debug.Fail("ERROR : No product has been selected");
                throw new Exception("ERROR : No product has been selected");
            }
            Response.Redirect("ShoppingCart.aspx");
        }
    }
}