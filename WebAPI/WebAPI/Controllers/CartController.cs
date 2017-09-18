using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class CartController : ApiController
    {
        private String SessionCartId { get; set; }
        public const string CartSession = "CartId";

        //Create some Data --> Product
        private static List<Product> Products = new List<Product>
        {
            new Product{Id= Guid.NewGuid(), Name =  "Laptop A", Price = 1099.99M, ImagePath="Laptop.png",Description = "A good Laptop", CategoryID = 1 },
            new Product{Id= Guid.NewGuid(), Name =  "Laptop B", Price = 2099, ImagePath="Laptop.png",Description = "A very good Laptop", CategoryID = 1 },
            new Product{Id= Guid.NewGuid(), Name =  "Desktop A", Price = 1120, ImagePath="Desktop.png",Description = "A good Desktop", CategoryID = 2 },
            new Product{Id= Guid.NewGuid(), Name =  "Desktop B", Price = 650.99M, ImagePath="Desktop.png",Description = "A very good Desktop", CategoryID = 2 },
            new Product{Id= Guid.NewGuid(), Name =  "Mouse", Price = 70, ImagePath="Mouse.png",Description = "A good Mouse", CategoryID = 3 },
        };
        //Create some Data --> Category
        private static List<Category> Categories = new List<Category>
        {
            new Category{Id = 1,Name = "Laptop"},
            new Category{Id = 2,Name = "Desktop"},
            new Category{Id = 3,Name = "Mouse"},
        };


        /// <summary>
        /// Add a product to the cart
        /// </summary>
        /// <param name="ProductId">ID of the Product</param>
        public void AddItemToCart(Guid ProductId)
        {
            SessionCartId = GetCurrentCartId();

            var cartItem = GetCartItem(ProductId, SessionCartId);

            if (cartItem == null)
            {
                cartItem = new CartItem(Guid.NewGuid(), SessionCartId, ProductId, 1, GetProductById(ProductId));
                CartItem.AllItem.Add(cartItem);
            }
        }

        /// <summary>
        /// Delete all product of the cart
        /// </summary>
        public void DeleteAllItems()
        {           
            foreach (CartItem item in GetAllCartItems())
            {
                CartItem.AllItem.Remove(item);
            }
        }

        /// <summary>
        /// Delete a product for the current session cart
        /// </summary>
        /// <param name="cartId">Id of the current Session</param>
        /// <param name="productId">Id of the Product</param>
        public void DeleteItem(String cartId, Guid productId)
        {
            foreach (CartItem item in CartItem.AllItem)
            {
                if (item.CartId == cartId && item.ProductId == productId)
                {
                    CartItem.AllItem.Remove(item);
                    break;
                }
            }
        }

        /// <summary>
        /// Update the quantity of a product
        /// </summary>
        /// <param name="cartId">Session ID</param>
        /// <param name="productId">Product ID</param>
        /// <param name="quantity">Quantity of the product</param>
        public void UpdateItem(String cartId, Guid productId, int quantity)
        {
            foreach (CartItem item in CartItem.AllItem)
            {
                if (item.CartId == cartId && item.ProductId == productId)
                {
                    item.Quantity = quantity;
                    break;
                }
            }
        }

        /// <summary>
        /// Get the current session cart ID
        /// </summary>
        /// <returns>Session cart ID</returns>
        public string GetCurrentCartId()
        {
            if (HttpContext.Current.Session[CartSession] == null)
            {
                if (!string.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name))
                {
                    HttpContext.Current.Session[CartSession] = HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    Guid tempCartId = Guid.NewGuid();
                    HttpContext.Current.Session[CartSession] = tempCartId.ToString();
                }
            }
            return HttpContext.Current.Session[CartSession].ToString();
        }

        /// <summary>
        /// Get an item of the Session Cart
        /// </summary>
        /// <param name="ProductID">The ID of the product</param>
        /// <param name="SessionCartID">The ID of the current session</param>
        /// <returns>A cartItem</returns>
        public CartItem GetCartItem(Guid ProductID, String SessionCartID)
        {
            foreach (CartItem item in CartItem.AllItem)
            {
                if (item.ProductId == ProductID && item.CartId == SessionCartID)
                {
                    return item;
                }
            }
            return null;
        }

         /// <summary>
         /// Get all the item of the session cart
         /// </summary>
         /// <returns>A list of all the product in the current session cart</returns>
        public List<CartItem> GetAllCartItems()
        {
            SessionCartId = GetCurrentCartId();
            return CartItem.AllItem.Where(c => c.CartId == SessionCartId).ToList();
        }

        /// <summary>
        /// Get a product by his ID
        /// </summary>
        /// <param name="ProductID">ID of the Product wanted</param>
        /// <returns>A product object</returns>
        public Product GetProductById(Guid ProductID)
        {
            foreach (Product product in GetAllProducts())
            {
                if (product.Id == ProductID)
                {
                    return product;
                }
            }
            return null;
        }

        /// <summary>
        /// Get the total price of the current session cart
        /// </summary>
        /// <returns>The total price of the current session cart</returns>
        public decimal GetTotalPrice()
        {
            SessionCartId = GetCurrentCartId();

            decimal? total = (from cartItems in GetAllCartItems() select (int?)cartItems.Quantity * cartItems.Product.Price).Sum();

            return total ?? decimal.Zero;
        }

        /// <summary>
        /// Get the list of all the products categories
        /// </summary>
        /// <returns>List of categories</returns>
        public static List<Category> GetAllCategories()
        {
            return Categories;
        }

        /// <summary>
        /// Get a list of all the products
        /// </summary>
        /// <returns>List of products</returns>
        public static List<Product> GetAllProducts()
        {
            return Products;
        }

        /// <summary>
        /// Struct that store which type of update is needed for a product
        /// </summary>
        public struct ShoppingCartUpdates
        {
            public Guid ProductId;
            public int Quantity;
            public bool RemoveItem;
        }

        /// <summary>
        /// Update the products of a cart
        /// </summary>
        /// <param name="cartId">Current session cart ID</param>
        /// <param name="CartItemUpdates">List of products and the way they need to be update</param>
        public void UpdateShoppingCart(String cartId, ShoppingCartUpdates[] CartItemUpdates)
        {
            int CartItemCount = CartItemUpdates.Count();
            List<CartItem> myCart = GetAllCartItems();
            foreach (var cartItem in myCart)
            {
                for (int i = 0; i < CartItemCount; i++)
                {
                    if (cartItem.Product.Id == CartItemUpdates[i].ProductId)
                    {
                        if (CartItemUpdates[i].Quantity < 1 || CartItemUpdates[i].RemoveItem == true)
                        {
                            DeleteItem(cartId, cartItem.ProductId);
                        }
                        else
                        {
                            UpdateItem(cartId, cartItem.ProductId, CartItemUpdates[i].Quantity);
                        }
                        break;
                    }
                }
            }
        }
    }
}
