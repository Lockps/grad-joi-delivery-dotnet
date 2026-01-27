using System;
using System.Collections.Generic;
using System.Linq;

namespace grad_joi_delivery_dotnet.Models
{
    public class Cart
    {
        public string CartId { get; set; }
        public string CustomerId { get; set; }
        public string StoreId { get; set; }
        public List<OrderItem> Items { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CartName { get; set; }

        public Cart()
        {
            Items = new List<OrderItem>();
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public Cart(string customerId, string storeId, string cartName = "My Cart")
        {
            CartId = Guid.NewGuid().ToString();
            CustomerId = customerId;
            StoreId = storeId;
            CartName = cartName;
            Items = new List<OrderItem>();
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public void AddItem(OrderItem orderItem)
        {
            if (orderItem == null)
            {
                throw new ArgumentException("Order item cannot be null", nameof(orderItem));
            }

            var existingItem = Items.FirstOrDefault(i => 
                i.Item.Id == orderItem.Item.Id && 
                AreModifiersEqual(i.Modifiers, orderItem.Modifiers));

            if (existingItem != null)
            {
                existingItem.UpdateQuantity(existingItem.Quantity + orderItem.Quantity);
            }
            else
            {
                Items.Add(orderItem);
            }

            UpdatedAt = DateTime.Now;
        }

        public void RemoveItem(string itemId)
        {
            var item = Items.FirstOrDefault(i => i.Item.Id == itemId);
            if (item != null)
            {
                Items.Remove(item);
                UpdatedAt = DateTime.Now;
            }
        }

        public void UpdateItemQuantity(string itemId, int quantity)
        {
            var item = Items.FirstOrDefault(i => i.Item.Id == itemId);
            if (item != null)
            {
                item.UpdateQuantity(quantity);
                UpdatedAt = DateTime.Now;
            }
        }

        public double CalculateTotal()
        {
            return Items?.Sum(item => item.LineTotal) ?? 0.0;
        }

        public int GetItemCount()
        {
            return Items?.Sum(item => item.Quantity) ?? 0;
        }

        public void Clear()
        {
            Items.Clear();
            UpdatedAt = DateTime.Now;
        }

        private bool AreModifiersEqual(List<ItemModifier> modifiers1, List<ItemModifier> modifiers2)
        {
            if (modifiers1 == null && modifiers2 == null) return true;
            if (modifiers1 == null || modifiers2 == null) return false;
            if (modifiers1.Count != modifiers2.Count) return false;

            return modifiers1.All(m1 => modifiers2.Any(m2 => 
                m1.Name == m2.Name && m1.Value == m2.Value));
        }
    }
}
