using WebAPI.Data;
using WebAPI.Models.DTOs;
using WebAPI.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly NorthwindContext _context;
        private readonly IMapper _mapper;

        public CategoryService(NorthwindContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDTO>> GetCategories()
        {
            var categories = await _context.Categories
                .Where(c => c.CategoryName != null) 
                .ToListAsync();

            return _mapper.Map<IEnumerable<CategoryDTO>>(categories)
                   ?? Enumerable.Empty<CategoryDTO>();
        }

        public async Task<CategoryDTO> GetCategoryById(int id)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryId == id && c.CategoryName != null);

            if (category == null)
            {
                throw new KeyNotFoundException($"Category with ID {id} not found or has null name");
            }

            return _mapper.Map<CategoryDTO>(category);
        }


        public async Task<CategoryDTO> CreateCategory(CreateCategoryDTO categoryDto)
        {
            if (await _context.Categories.AnyAsync(c => c.CategoryName == categoryDto.CategoryName))
            {
                throw new InvalidOperationException("Category name already exists");
            }

            var category = _mapper.Map<Category>(categoryDto);
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return _mapper.Map<CategoryDTO>(category);
        }

        public async Task UpdateCategory(int id, CategoryDTO categoryDto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                throw new Exception("Category not found");
            }

            _mapper.Map(categoryDto, category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                throw new Exception("Category not found");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
