using System;
using System.Linq;
using System.Web.ModelBinding;
using WebAPI.Controllers;
using WebAPI.Models;

namespace WebAPI
{
    public partial class ProductDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public Product GetProduct([QueryString("ProductID")] Guid? productId)
        {
            if (productId != null)
            {
                return CartController.GetAllProducts().Where(p => p.Id == productId).First();
            }
            return null;
        }
    }
}