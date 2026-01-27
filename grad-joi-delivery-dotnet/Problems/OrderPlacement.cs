using System;
using System.Collections.Generic;
using System.Linq;
using grad_joi_delivery_dotnet.Models;
using grad_joi_delivery_dotnet.Services;
using grad_joi_delivery_dotnet.StaticData;

namespace grad_joi_delivery_dotnet.Problems
{
    public static class OrderPlacement
    {
        public static void TestOrderPlacement()
        {
            Console.WriteLine("=== JOI Delivery - Customization System Demo ===\n");

            // Demo 1: Basic order with quantities
            Console.WriteLine("1. Creating order with quantities:");
            var orderItems1 = new List<OrderItem>
            {
                new OrderItem(StaticData.StaticData.Items.First(i => i.Id == "1"), quantity: 2), // 2 Notebooks
                new OrderItem(StaticData.StaticData.Items.First(i => i.Id == "2"), quantity: 1)  // 1 Keyboard
            };

            Order order1 = CreateOrder("CUST001", "1", orderItems1);
            PrintOrderDetails(order1);
            Console.WriteLine();

            // Demo 2: Order with modifiers and special instructions
            Console.WriteLine("2. Creating order with customization:");
            var notebook = StaticData.StaticData.Items.First(i => i.Id == "1");
            var orderItem2 = new OrderItem(notebook, quantity: 1);
            orderItem2.AddModifier(new ItemModifier("Size", "Large", 5.0));
            orderItem2.AddModifier(new ItemModifier("Color", "Blue", 2.0));
            orderItem2.SetSpecialInstructions("Please wrap as gift");

            var order2 = CreateOrder("CUST002", "1", new List<OrderItem> { orderItem2 });
            PrintOrderDetails(order2);
            Console.WriteLine();

            // Demo 3: Order with discount (loyalty tier)
            Console.WriteLine("3. Creating order with loyalty discount:");
            var customer = new Customer("CUST003", "John", "Doe", "5000", LoyaltyTier.GOLD);
            var orderItems3 = new List<OrderItem>
            {
                new OrderItem(StaticData.StaticData.Items.First(i => i.Id == "4"), quantity: 1) // Monitor
            };

            Order order3 = CreateOrderWithDiscount(customer, "1", orderItems3);
            PrintOrderDetails(order3);
            Console.WriteLine();

            // Demo 4: Saved cart functionality
            Console.WriteLine("4. Saved Cart System:");
            DemoCartSystem();
        }

        public static Order CreateOrder(string customerId, string storeId, List<OrderItem> orderItems)
        {
            ValidateOrderInputs(customerId, storeId, orderItems);

            string orderId = GenerateOrderId();
            Order order = new Order(orderId, customerId, storeId, orderItems, OrderStatus.CREATED);

            return order;
        }

        public static Order CreateOrderWithDiscount(Customer customer, string storeId, List<OrderItem> orderItems)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            ValidateOrderInputs(customer.CustomerId, storeId, orderItems);

            string orderId = GenerateOrderId();
            Order order = new Order(orderId, customer.CustomerId, storeId, orderItems, OrderStatus.CREATED);

            // Apply loyalty discount
            int discountPercent = customer.Tier.GetDiscount();
            order.ApplyDiscount(discountPercent);

            return order;
        }

        public static Order CreateOrderFromCart(string cartId)
        {
            if (string.IsNullOrWhiteSpace(cartId))
            {
                throw new ArgumentException("Cart ID cannot be null or empty", nameof(cartId));
            }

            Cart cart = CartService.GetCart(cartId);

            if (cart.Items == null || cart.Items.Count == 0)
            {
                throw new InvalidOperationException("Cannot create order from empty cart");
            }

            string orderId = GenerateOrderId();
            Order order = new Order(orderId, cart.CustomerId, cart.StoreId, cart.Items, OrderStatus.CREATED);

            return order;
        }

        private static void ValidateOrderInputs(string customerId, string storeId, List<OrderItem> orderItems)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new ArgumentException("Customer ID cannot be null or empty", nameof(customerId));
            }

            if (string.IsNullOrWhiteSpace(storeId))
            {
                throw new ArgumentException("Store ID cannot be null or empty", nameof(storeId));
            }

            var store = StaticData.StaticData.Stores.FirstOrDefault(s => s.StoreId == storeId);
            if (store == null)
            {
                throw new ArgumentException($"Store with ID '{storeId}' does not exist", nameof(storeId));
            }

            if (orderItems == null || orderItems.Count == 0)
            {
                throw new ArgumentException("Order must contain at least one item", nameof(orderItems));
            }

            foreach (var orderItem in orderItems)
            {
                if (orderItem == null || orderItem.Item == null)
                {
                    throw new ArgumentException("Order contains null items", nameof(orderItems));
                }

                var validItem = StaticData.StaticData.Items.FirstOrDefault(i => i.Id == orderItem.Item.Id);
                if (validItem == null)
                {
                    throw new ArgumentException($"Item with ID '{orderItem.Item.Id}' does not exist", nameof(orderItems));
                }

                if (orderItem.Quantity <= 0)
                {
                    throw new ArgumentException($"Item '{orderItem.Item.Name}' must have quantity greater than 0", nameof(orderItems));
                }
            }
        }

        private static string GenerateOrderId()
        {
            return $"ORD-{DateTime.Now:yyyyMMddHHmmss}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }

        private static void PrintOrderDetails(Order order)
        {
            Console.WriteLine($"  Order ID: {order.OrderId}");
            Console.WriteLine($"  Customer: {order.CustomerId}, Store: {order.StoreId}");
            Console.WriteLine($"  Status: {order.Status}");
            Console.WriteLine($"  Items:");
            foreach (var orderItem in order.OrderItems)
            {
                Console.WriteLine($"    - {orderItem.Item.Name} x{orderItem.Quantity} = ₹{orderItem.LineTotal}");
                if (orderItem.Modifiers != null && orderItem.Modifiers.Count > 0)
                {
                    foreach (var modifier in orderItem.Modifiers)
                    {
                        Console.WriteLine($"      Modifier: {modifier.Name} = {modifier.Value} (+₹{modifier.AdditionalPrice})");
                    }
                }
                if (!string.IsNullOrEmpty(orderItem.SpecialInstructions))
                {
                    Console.WriteLine($"      Instructions: {orderItem.SpecialInstructions}");
                }
            }
            Console.WriteLine($"  Subtotal: ₹{order.SubTotal:F2}");
            if (order.DiscountAmount > 0)
            {
                Console.WriteLine($"  Discount: -₹{order.DiscountAmount:F2}");
            }
            Console.WriteLine($"  Total Price: ₹{order.TotalPrice:F2}");
        }

        private static void DemoCartSystem()
        {
            // Create a cart
            Cart cart = CartService.CreateCart("CUST004", "1", "Weekly Groceries");
            Console.WriteLine($"  Created cart: {cart.CartName} (ID: {cart.CartId})");

            // Add items with quantities
            var item1 = new OrderItem(StaticData.StaticData.Items.First(i => i.Id == "1"), quantity: 3);
            var item2 = new OrderItem(StaticData.StaticData.Items.First(i => i.Id == "2"), quantity: 1);
            item2.AddModifier(new ItemModifier("Warranty", "Extended 2 years", 10.0));

            cart.AddItem(item1);
            cart.AddItem(item2);

            Console.WriteLine($"  Cart contains {cart.GetItemCount()} items");
            Console.WriteLine($"  Cart total: ₹{cart.CalculateTotal():F2}");

            // Update quantity
            cart.UpdateItemQuantity("1", 5);
            Console.WriteLine($"  Updated quantity - Cart total: ₹{cart.CalculateTotal():F2}");

            // Save cart
            CartService.SaveCart(cart);
            Console.WriteLine($"  Cart saved successfully");

            // Retrieve cart
            Cart retrievedCart = CartService.GetCart(cart.CartId);
            Console.WriteLine($"  Retrieved cart: {retrievedCart.CartName}");

            // Create order from cart
            Order orderFromCart = CreateOrderFromCart(cart.CartId);
            Console.WriteLine($"  Created order from cart: {orderFromCart.OrderId}");
            PrintOrderDetails(orderFromCart);
        }
    }
}
