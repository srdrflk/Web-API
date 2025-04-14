namespace WebAPI.Models.DTOs
{
    public class ProductQueryDTO
    {
        public int PageNumber { get; set; } = 1; // Using 1-based page numbering
        public int PageSize { get; set; } = 10;
        public int? CategoryId { get; set; } = null;
    }
}
