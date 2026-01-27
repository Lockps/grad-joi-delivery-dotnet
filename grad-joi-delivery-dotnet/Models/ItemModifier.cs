namespace grad_joi_delivery_dotnet.Models
{
    public class ItemModifier
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public double AdditionalPrice { get; set; }

        public ItemModifier() {}

        public ItemModifier(string name, string value, double additionalPrice = 0.0)
        {
            Name = name;
            Value = value;
            AdditionalPrice = additionalPrice;
        }
    }
}
