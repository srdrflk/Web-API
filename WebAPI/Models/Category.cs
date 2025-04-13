using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(15)]
        public string CategoryName { get; set; } = string.Empty;

        [Column(TypeName = "ntext")]
        public string? Description { get; set; }

        public byte[]? Picture { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new List<Product>(); // Initialize collection
    }
}
