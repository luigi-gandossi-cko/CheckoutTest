using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.ModelBinding;
using System.Web.UI.WebControls;
using WebAPI.Controllers;
using WebAPI.Models;

namespace WebAPI
{
    public partial class ShoppingCart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (CartController cartController = new CartController())
            {
                decimal cartTotalPrice = cartController.GetTotalPrice();

                if (cartTotalPrice > 0)
                {
                    lblTotal.Text = String.Format("{0:c}", cartTotalPrice);
                }
                else
                {
                    CartList.Visible = false;
                    LabelTotalText.Text = "";
                    lblTotal.Text = "";
                    ShoppingCartTitle.InnerText = "Shopping Cart is Empty";
                    UpdateBtn.Visible = false;
                    DeleteAllBtn.Visible = false;
                }
            }
            }
        public List<CartItem> GetShoppingCartItems([QueryString("ProductID")] Guid? productId)
        {
            CartController cartController = new CartController();
            return cartController.GetAllCartItems();
            
        }

        protected void UpdateBtn_Click(object sender, EventArgs e)
        {
            UpdateCartItems();
            GetShoppingCartItems(null);
        }
        protected void DeleteAllBtn_Click(object sender, EventArgs e)
        {
            using (CartController cartController = new CartController())
            {
                cartController.DeleteAllItems();
                Response.Redirect("ProductList.aspx");
            }
        }
        

        public List<CartItem> UpdateCartItems()
        {
            using (CartController cartController = new CartController())
            {
                String cartId = cartController.GetCurrentCartId();

                CartController.ShoppingCartUpdates[] cartUpdates = new CartController.ShoppingCartUpdates[CartList.Rows.Count];
                for (int i = 0; i < CartList.Rows.Count; i++)
                {
                    IOrderedDictionary rowValues = new OrderedDictionary();
                    rowValues = GetValues(CartList.Rows[i]);
                    cartUpdates[i].ProductId = Guid.Parse(Convert.ToString(rowValues["Product.Id"]));

                    CheckBox cbRemove = new CheckBox();
                    cbRemove = (CheckBox)CartList.Rows[i].FindControl("Remove");
                    cartUpdates[i].RemoveItem = cbRemove.Checked;

                    TextBox quantityTextBox = new TextBox();
                    quantityTextBox = (TextBox)CartList.Rows[i].FindControl("PurchaseQuantity");
                    cartUpdates[i].Quantity = Convert.ToInt16(quantityTextBox.Text.ToString());
                }
                cartController.UpdateShoppingCart(cartId, cartUpdates);
                CartList.DataBind();
                lblTotal.Text = String.Format("{0:c}", cartController.GetTotalPrice());
                    return cartController.GetAllCartItems();
            }
        }

        public static IOrderedDictionary GetValues(GridViewRow row)
        {
            IOrderedDictionary values = new OrderedDictionary();
            foreach (DataControlFieldCell cell in row.Cells)
            {
                    cell.ContainingField.ExtractValuesFromCell(values, cell, row.RowState, true);
            }
            return values;
        }
    }
}