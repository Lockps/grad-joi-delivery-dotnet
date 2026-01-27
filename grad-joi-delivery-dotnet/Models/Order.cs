using System;
using System.Collections.Generic;
using System.Linq;

namespace grad_joi_delivery_dotnet.Models
{
    public class Order
    {
        public string OrderId { get; set; }
        public string CustomerId { get; set; }
        public string StoreId { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public OrderStatus Status { get; set; }
        public double SubTotal { get; set; }
        public double DiscountAmount { get; set; }
        public double TotalPrice { get; set; }
        public string SpecialInstructions { get; set; }

        public Order() 
        {
            OrderItems = new List<OrderItem>();
        }

        public Order(string orderId, string customerId, string storeId, List<OrderItem> orderItems, OrderStatus status = OrderStatus.CREATED)
        {
            OrderId = orderId;
            CustomerId = customerId;
            StoreId = storeId;
            OrderItems = orderItems ?? new List<OrderItem>();
            Status = status;
            SpecialInstructions = string.Empty;
            CalculateTotals();
        }

        public void AddOrderItem(OrderItem orderItem)
        {
            if (orderItem == null)
            {
                throw new ArgumentException("Order item cannot be null", nameof(orderItem));
            }

            var existingItem = OrderItems.FirstOrDefault(i => 
                i.Item.Id == orderItem.Item.Id && 
                AreModifiersEqual(i.Modifiers, orderItem.Modifiers));

            if (existingItem != null)
            {
                existingItem.UpdateQuantity(existingItem.Quantity + orderItem.Quantity);
            }
            else
            {
                OrderItems.Add(orderItem);
            }

            CalculateTotals();
        }

        public void RemoveOrderItem(string itemId)
        {
            var item = OrderItems.FirstOrDefault(i => i.Item.Id == itemId);
            if (item != null)
            {
                OrderItems.Remove(item);
                CalculateTotals();
            }
        }

        public void ApplyDiscount(double discountPercent)
        {
            if (discountPercent < 0 || discountPercent > 100)
            {
                throw new ArgumentException("Discount percentage must be between 0 and 100", nameof(discountPercent));
            }

            DiscountAmount = SubTotal * (discountPercent / 100.0);
            CalculateTotals();
        }

        public void UpdateStatus(OrderStatus newStatus)
        {
            Status = newStatus;
        }

        private void CalculateTotals()
        {
            SubTotal = OrderItems?.Sum(item => item.LineTotal) ?? 0.0;
            TotalPrice = SubTotal - DiscountAmount;
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
