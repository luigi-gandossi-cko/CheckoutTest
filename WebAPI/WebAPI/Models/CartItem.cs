using System;
using System.Collections.Generic;

namespace WebAPI.Models
{
    public class CartItem
    {
        public Guid Id { get;  set; }
        public string CartId { get;  set; }
        public Guid ProductId { get;  set; }
        public int Quantity { get; set; }
        public virtual Product Product { get;  set; }

        public CartItem(Guid id, String cartId, Guid productId, int quantity, Product product)
        {
            Id = id;
            CartId = cartId;
            ProductId = productId;
            Quantity = quantity;
            Product = product;
        }

        /// <summary>
        /// Contain all Item in every Cart
        /// </summary>
        public static List<CartItem> AllItem = new List<CartItem> { };
    }
}