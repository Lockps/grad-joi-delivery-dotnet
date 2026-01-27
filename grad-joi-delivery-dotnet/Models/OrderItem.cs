using System.Collections.Generic;
using System.Linq;

namespace grad_joi_delivery_dotnet.Models
{
    public class OrderItem
    {
        public Item Item { get; set; }
        public int Quantity { get; set; }
        public List<ItemModifier> Modifiers { get; set; }
        public string SpecialInstructions { get; set; }
        public double LineTotal { get; set; }

        public OrderItem() 
        {
            Modifiers = new List<ItemModifier>();
        }

        public OrderItem(Item item, int quantity = 1)
        {
            Item = item;
            Quantity = quantity;
            Modifiers = new List<ItemModifier>();
            SpecialInstructions = string.Empty;
            CalculateLineTotal();
        }

        public void AddModifier(ItemModifier modifier)
        {
            if (modifier != null)
            {
                Modifiers.Add(modifier);
                CalculateLineTotal();
            }
        }

        public void RemoveModifier(ItemModifier modifier)
        {
            if (modifier != null && Modifiers.Contains(modifier))
            {
                Modifiers.Remove(modifier);
                CalculateLineTotal();
            }
        }

        public void UpdateQuantity(int quantity)
        {
            if (quantity > 0)
            {
                Quantity = quantity;
                CalculateLineTotal();
            }
        }

        public void SetSpecialInstructions(string instructions)
        {
            SpecialInstructions = instructions ?? string.Empty;
        }

        private void CalculateLineTotal()
        {
            double basePrice = Item?.Price ?? 0.0;
            double modifierPrice = Modifiers?.Sum(m => m.AdditionalPrice) ?? 0.0;
            LineTotal = (basePrice + modifierPrice) * Quantity;
        }
    }
}
