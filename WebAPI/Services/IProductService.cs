using WebAPI.Models.DTOs;

namespace WebAPI.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetProducts();
        Task<ProductDTO> GetProductById(int id);
        Task<ProductDTO> CreateProduct(CreateProductDTO productDto);
        Task UpdateProduct(int id, UpdateProductDTO productDto);
        Task DeleteProduct(int id);
    }
}
