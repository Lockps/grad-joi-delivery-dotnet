using System;
using System.Collections.Generic;
using System.Linq;
using grad_joi_delivery_dotnet.Models;

namespace grad_joi_delivery_dotnet.Services
{
    public static class CartService
    {
        private static Dictionary<string, Cart> _carts = new Dictionary<string, Cart>();

        public static Cart CreateCart(string customerId, string storeId, string cartName = "My Cart")
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new ArgumentException("Customer ID cannot be null or empty", nameof(customerId));
            }

            if (string.IsNullOrWhiteSpace(storeId))
            {
                throw new ArgumentException("Store ID cannot be null or empty", nameof(storeId));
            }

            var cart = new Cart(customerId, storeId, cartName);
            _carts[cart.CartId] = cart;
            return cart;
        }

        public static Cart GetCart(string cartId)
        {
            if (string.IsNullOrWhiteSpace(cartId))
            {
                throw new ArgumentException("Cart ID cannot be null or empty", nameof(cartId));
            }

            if (!_carts.ContainsKey(cartId))
            {
                throw new KeyNotFoundException($"Cart with ID '{cartId}' not found");
            }

            return _carts[cartId];
        }

        public static List<Cart> GetCustomerCarts(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new ArgumentException("Customer ID cannot be null or empty", nameof(customerId));
            }

            return _carts.Values.Where(c => c.CustomerId == customerId).ToList();
        }

        public static Cart? GetActiveCart(string customerId, string storeId)
        {
            var carts = GetCustomerCarts(customerId);
            return carts.FirstOrDefault(c => c.StoreId == storeId && c.Items.Count > 0);
        }

        public static void SaveCart(Cart cart)
        {
            if (cart == null)
            {
                throw new ArgumentNullException(nameof(cart));
            }

            if (string.IsNullOrWhiteSpace(cart.CartId))
            {
                cart.CartId = Guid.NewGuid().ToString();
            }

            _carts[cart.CartId] = cart;
        }

        public static void DeleteCart(string cartId)
        {
            if (string.IsNullOrWhiteSpace(cartId))
            {
                throw new ArgumentException("Cart ID cannot be null or empty", nameof(cartId));
            }

            if (_carts.ContainsKey(cartId))
            {
                _carts.Remove(cartId);
            }
        }

        public static void ClearAllCarts()
        {
            _carts.Clear();
        }
    }
}
