using NorthwindApiClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindApiClient.Services
{
    public interface INorthwindApiService
    {
        Task<PagedResponse<ProductDto>> GetProductsAsync(int pageNumber = 1, int pageSize = 10, int? categoryId = null);
        Task<ProductDto> GetProductAsync(int id);
        Task<ProductDto> CreateProductAsync(ProductDto product);
        Task UpdateProductAsync(int id, ProductDto product);
        Task DeleteProductAsync(int id);
    }
}
