using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.ModelBinding;
using System.Web.UI.WebControls;
using WebAPI.Controllers;
using WebAPI.Models;

namespace WebAPI
{
    public partial class ProductList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public List<Category> GetCategories()
        {
            return CartController.GetAllCategories();
        }
        public List<Product> GetProducts([QueryString("CategoryID")] int? categoryId)
        {
            if (categoryId.HasValue && categoryId > 0)
            {
                return CartController.GetAllProducts().Where(p => p.CategoryID == categoryId).ToList();
            }

            return CartController.GetAllProducts();
        }
    }
}