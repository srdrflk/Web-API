using WebAPI.Models.DTOs;

namespace WebAPI.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetCategories();
        Task<CategoryDTO> GetCategoryById(int id);
        Task<CategoryDTO> CreateCategory(CreateCategoryDTO categoryDto);
        Task UpdateCategory(int id, CategoryDTO categoryDto);
        Task DeleteCategory(int id);
    }
}
