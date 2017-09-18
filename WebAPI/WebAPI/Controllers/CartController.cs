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

        private static List<Product> Products = new List<Product>
        {
            new Product{Id= Guid.NewGuid(), Name =  "Laptop A", Price = 1099.99M, ImagePath="Laptop.png",Description = "A good Laptop", CategoryID = 1 },
            new Product{Id= Guid.NewGuid(), Name =  "Laptop B", Price = 2099, ImagePath="Laptop.png",Description = "A very good Laptop", CategoryID = 1 },
            new Product{Id= Guid.NewGuid(), Name =  "Desktop A", Price = 1120, ImagePath="Desktop.png",Description = "A good Desktop", CategoryID = 2 },
            new Product{Id= Guid.NewGuid(), Name =  "Desktop B", Price = 650.99M, ImagePath="Desktop.png",Description = "A very good Desktop", CategoryID = 2 },
            new Product{Id= Guid.NewGuid(), Name =  "Mouse", Price = 70, ImagePath="Mouse.png",Description = "A good Mouse", CategoryID = 3 },
        };

        private static List<Category> Categories = new List<Category>
        {
            new Category{Id = 1,Name = "Laptop"},
            new Category{Id = 2,Name = "Desktop"},
            new Category{Id = 3,Name = "Mouse"},
        };

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

        public void DeleteAllItems()
        {           
            foreach (CartItem item in GetAllCartItems())
            {
                CartItem.AllItem.Remove(item);
            }
        }

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
        /// Get a CartItem
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

        public List<CartItem> GetAllCartItems()
        {
            SessionCartId = GetCurrentCartId();
            return CartItem.AllItem.Where(c => c.CartId == SessionCartId).ToList();
        }

        /// <summary>
        /// Get a product
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

        public decimal GetTotalPrice()
        {
            SessionCartId = GetCurrentCartId();

            decimal? total = (from cartItems in GetAllCartItems() select (int?)cartItems.Quantity * cartItems.Product.Price).Sum();

            return total ?? decimal.Zero;
        }

        public static List<Category> GetAllCategories()
        {
            return Categories;
        }

        public static List<Product> GetAllProducts()
        {
            return Products;
        }

        public struct ShoppingCartUpdates
        {
            public Guid ProductId;
            public int Quantity;
            public bool RemoveItem;
        }

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
