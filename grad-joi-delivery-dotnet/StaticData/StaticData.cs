using grad_joi_delivery_dotnet.Models;

namespace grad_joi_delivery_dotnet.StaticData
{
    public class StaticData
    {
        public const string ZONEA = "ZoneA";
        public const string ZONEB = "ZoneB";
        public const string ZONEC = "ZoneC";

        public static List<DistanceMap> DistanceMap = new List<DistanceMap>
        {
            new DistanceMap(ZONEA, ZONEA, 0),
            new DistanceMap(ZONEA, ZONEB, 3),
            new DistanceMap(ZONEA, ZONEC, 6),
            new DistanceMap(ZONEB, ZONEC, 3),
            new DistanceMap(ZONEB, ZONEB, 0),
            new DistanceMap(ZONEB, ZONEC, 8),
            new DistanceMap(ZONEC, ZONEC, 0),
        };

        public static List<Store> Stores = new List<Store>
        {
            new Store("1", ZONEA, new List<string> { "Milk", "Eggs", "Bread" }),
            new Store("2", ZONEB, new List<string> { "Bread", "Milks" }),
            new Store("3", ZONEC, new List<string> { "Juice", "Bread" }),
        };

        public static List<Item> Items = new List<Item>
        {
            new Item("1", "Notebook", "", 15),
            new Item("2", "Keyboard", "", 50),
            new Item("3", "Mouse", "", 25),
            new Item("4", "Monitor", "", 75),
        };
    }
}
