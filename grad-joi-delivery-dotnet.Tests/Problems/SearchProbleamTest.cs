using grad_joi_delivery_dotnet.Models;
using grad_joi_delivery_dotnet.Problems;
using grad_joi_delivery_dotnet.Services;
using Xunit;

namespace grad_joi_delivery_dotnet.Tests.Problems;

public class SearchProbleamTest
{
    [Fact]
    public void SearchData_WhenItemFound_ReturnsItem()
    {
        // Given
        string searchtext = "milk";
        Category category = Category.DAIRY;

        // When
        IEnumerable<Item> searchedItems = SearchItemsServices.SearchItem(searchtext, category);

        List<Item> expectedResult = new List<Item>
        {
            new Item("5", "Amul Full Cream Milk", "Milk Product - Milk", 60, Category.DAIRY),
        };

        if(searchedItems == null)
        {
            Console.WriteLine("Search items should not be null");
        }

        // Then
        Assert.Equal(expectedResult, searchedItems);
    }

}
