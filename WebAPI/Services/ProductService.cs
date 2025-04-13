using AutoMapper;
using WebAPI.Data;
using WebAPI.Models.DTOs;
using WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly NorthwindContext _context;
        private readonly IMapper _mapper;

        public ProductService(NorthwindContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDTO>> GetProducts()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.ProductName != null) // Filter out null product names
                .ToListAsync();

            return _mapper.Map<IEnumerable<ProductDTO>>(products) ?? Enumerable.Empty<ProductDTO>();
        }

        public async Task<ProductDTO> GetProductById(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == id);
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<ProductDTO> CreateProduct(CreateProductDTO productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Reload with category to include in response
            await _context.Entry(product).Reference(p => p.Category).LoadAsync();
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task UpdateProduct(int id, UpdateProductDTO productDto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                throw new Exception("Product not found");
            }

            _mapper.Map(productDto, product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                throw new Exception("Product not found");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
