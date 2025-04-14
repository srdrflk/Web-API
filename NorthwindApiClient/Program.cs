using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NorthwindApiClient.Models;
using NorthwindApiClient.Services;


    var configuration = new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("C:\\Users\\Serdar_Filik\\Desktop\\DotNetBasicMentorProjects\\WebAPI\\NorthwindApiClient\\appsettings.json", false)
        .Build();

    var serviceProvider = new ServiceCollection()
        .AddHttpClient()
        .AddSingleton<IConfiguration>(configuration)
        .AddSingleton<INorthwindApiService, NorthwindApiService>()
        .BuildServiceProvider();

    var apiService = serviceProvider.GetService<INorthwindApiService>();

    try
    {
        await RunProductMenu(apiService);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
    }

    Console.WriteLine("\nPress any key to exit...");
    Console.ReadKey();


static async Task RunProductMenu(INorthwindApiService apiService)
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("Northwind API Client");
        Console.WriteLine("====================");
        Console.WriteLine("1. List Products");
        Console.WriteLine("2. Get Product by ID");
        Console.WriteLine("3. Create Product");
        Console.WriteLine("4. Update Product");
        Console.WriteLine("5. Delete Product");
        Console.WriteLine("6. Exit");
        Console.Write("\nSelect an option: ");

        var option = Console.ReadLine();

        try
        {
            switch (option)
            {
                case "1":
                    await ListProducts(apiService);
                    break;
                case "2":
                    await GetProduct(apiService);
                    break;
                case "3":
                    await CreateProduct(apiService);
                    break;
                case "4":
                    await UpdateProduct(apiService);
                    break;
                case "5":
                    await DeleteProduct(apiService);
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Invalid option. Try again.");
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
}

static async Task ListProducts(INorthwindApiService apiService)
{
    Console.Write("\nPage number (default 1): ");
    var pageNumberInput = Console.ReadLine();
    int.TryParse(pageNumberInput, out var pageNumber);
    pageNumber = pageNumber == 0 ? 1 : pageNumber;

    Console.Write("Page size (default 10): ");
    var pageSizeInput = Console.ReadLine();
    int.TryParse(pageSizeInput, out var pageSize);
    pageSize = pageSize == 0 ? 10 : pageSize;

    Console.Write("Category ID (optional): ");
    var categoryIdInput = Console.ReadLine();
    int.TryParse(categoryIdInput, out var categoryId);

    var response = await apiService.GetProductsAsync(pageNumber, pageSize,
        string.IsNullOrEmpty(categoryIdInput) ? null : (int?)categoryId);

    Console.WriteLine($"\nPage {response.PageNumber} of {response.TotalPages} (Total: {response.TotalRecords})");
    Console.WriteLine("------------------------------------------------");
    foreach (var product in response.Data)
    {
        Console.WriteLine($"{product.ProductId}: {product.ProductName} | " +
            $"Category: {product.CategoryName} | " +
            $"Price: {product.UnitPrice:C} | " +
            $"Stock: {product.UnitsInStock}");
    }
}

static async Task GetProduct(INorthwindApiService apiService)
{
    Console.Write("\nEnter Product ID: ");
    var id = int.Parse(Console.ReadLine());
    var product = await apiService.GetProductAsync(id);

    Console.WriteLine("\nProduct Details:");
    Console.WriteLine("----------------");
    Console.WriteLine($"ID: {product.ProductId}");
    Console.WriteLine($"Name: {product.ProductName}");
    Console.WriteLine($"Category: {product.CategoryName} (ID: {product.CategoryId})");
    Console.WriteLine($"Quantity Per Unit: {product.QuantityPerUnit}");
    Console.WriteLine($"Unit Price: {product.UnitPrice:C}");
    Console.WriteLine($"Units In Stock: {product.UnitsInStock}");
    Console.WriteLine($"Discontinued: {product.Discontinued}");
}

static async Task CreateProduct(INorthwindApiService apiService)
{
    var product = new ProductDto();

    Console.WriteLine("\nCreate New Product");
    Console.WriteLine("------------------");
    Console.Write("Product Name: ");
    product.ProductName = Console.ReadLine();

    Console.Write("Category ID: ");
    product.CategoryId = int.Parse(Console.ReadLine());

    Console.Write("Quantity Per Unit: ");
    product.QuantityPerUnit = Console.ReadLine();

    Console.Write("Unit Price: ");
    product.UnitPrice = decimal.Parse(Console.ReadLine());

    Console.Write("Units In Stock: ");
    product.UnitsInStock = short.Parse(Console.ReadLine());

    Console.Write("Discontinued (true/false): ");
    product.Discontinued = bool.Parse(Console.ReadLine());

    var createdProduct = await apiService.CreateProductAsync(product);
    Console.WriteLine($"\nProduct created with ID: {createdProduct.ProductId}");
}

static async Task UpdateProduct(INorthwindApiService apiService)
{
    Console.Write("\nEnter Product ID to update: ");
    var id = int.Parse(Console.ReadLine());

    var existingProduct = await apiService.GetProductAsync(id);

    Console.WriteLine("\nCurrent Values:");
    Console.WriteLine($"1. Name: {existingProduct.ProductName}");
    Console.WriteLine($"2. Category ID: {existingProduct.CategoryId}");
    Console.WriteLine($"3. Quantity Per Unit: {existingProduct.QuantityPerUnit}");
    Console.WriteLine($"4. Unit Price: {existingProduct.UnitPrice}");
    Console.WriteLine($"5. Units In Stock: {existingProduct.UnitsInStock}");
    Console.WriteLine($"6. Discontinued: {existingProduct.Discontinued}");

    Console.Write("\nEnter field number to update (1-6): ");
    var field = Console.ReadLine();

    switch (field)
    {
        case "1":
            Console.Write("New Product Name: ");
            existingProduct.ProductName = Console.ReadLine();
            break;
        case "2":
            Console.Write("New Category ID: ");
            existingProduct.CategoryId = int.Parse(Console.ReadLine());
            break;
        case "3":
            Console.Write("New Quantity Per Unit: ");
            existingProduct.QuantityPerUnit = Console.ReadLine();
            break;
        case "4":
            Console.Write("New Unit Price: ");
            existingProduct.UnitPrice = decimal.Parse(Console.ReadLine());
            break;
        case "5":
            Console.Write("New Units In Stock: ");
            existingProduct.UnitsInStock = short.Parse(Console.ReadLine());
            break;
        case "6":
            Console.Write("New Discontinued (true/false): ");
            existingProduct.Discontinued = bool.Parse(Console.ReadLine());
            break;
        default:
            Console.WriteLine("Invalid field number.");
            return;
    }

    await apiService.UpdateProductAsync(id, existingProduct);
    Console.WriteLine("\nProduct updated successfully.");
}

static async Task DeleteProduct(INorthwindApiService apiService)
{
    Console.Write("\nEnter Product ID to delete: ");
    var id = int.Parse(Console.ReadLine());

    Console.Write($"Are you sure you want to delete product {id}? (y/n): ");
    var confirm = Console.ReadLine();

    if (confirm?.ToLower() == "y")
    {
        await apiService.DeleteProductAsync(id);
        Console.WriteLine("Product deleted successfully.");
    }
    else
    {
        Console.WriteLine("Deletion cancelled.");
    }
}