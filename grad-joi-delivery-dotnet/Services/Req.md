Problem Statement: Search and Filter Products
Implement Product Search with Optional Category.
Implement a method to search products by name (case-insensitive).
Support partial matches (e.g., searching "milk" should return "Amul Full Cream Milk").

Description:
As a storefront user,
I want to search for products by name,
and optionally filter them by category or price range,
so that I can easily find the items I need from the product catalog.
Acceptance Criteria:
Search Functionality:
Given a list of products and a keyword, return all products where the keyword is a substring (case-insensitive) of the product name matches.
Filter by Category:
Given a category filter, return only products from that category.
Empty Results:
If no products match the query or filters, return an empty list, not an error.


Sample Item:

{
  "id": "P1001",
  "name": "Amul Full Cream Milk",
  "category": "Dairy",
  "price": 60,
  “Description”: “Milk Product - Milk”
}

Input: 

{
  "searchText": "milk",
  "category": "dairy"
}

Expected Output:

[
   {
      "id": "P1001",
      "name": "Amul Full Cream Milk",
      "category": "Dairy",
      "price": 60,
      “Description”: “Milk Product - Milk”
   }
]