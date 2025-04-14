using WebAPI.Models.DTOs;

namespace WebAPI.Services
{
    public interface IProductService
    {
        Task<PagedResponseDTO<ProductDTO>> GetProducts(ProductQueryDTO query);
        Task<ProductDTO> GetProductById(int id);
        Task<ProductDTO> CreateProduct(CreateProductDTO productDto);
        Task UpdateProduct(int id, UpdateProductDTO productDto);
        Task DeleteProduct(int id);
    }
}
