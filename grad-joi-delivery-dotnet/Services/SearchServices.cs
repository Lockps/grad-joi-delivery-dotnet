
using grad_joi_delivery_dotnet.Models;

namespace grad_joi_delivery_dotnet.Services
{

    public class SearchItemsServices
    {
        public static IEnumerable<Item> SearchItem(string searchText, Category? category)
        {
            List<Item> allItems = StaticData.StaticData.Items;
            
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return allItems;
            }

            return allItems.Where(x => x.Name.ToLower().Contains(searchText)).Where(x => x.Category == category);
        }
    }
}
