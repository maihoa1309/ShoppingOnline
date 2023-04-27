using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ShoppingOnline.Models.Entity
{
    public class Product
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(Category.Id))]
        public int CategoryId { get; set; }
        public virtual Category? CategoryEntity { get; set; }

        [Required(ErrorMessage ="Name is required")]
        [MinLength(10)]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Code is required")]
        [RegularExpression(@"^[A-Z]{2}[0-9]{4}$",ErrorMessage ="Code is invalid")]
        public string? Code { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        public int? Quantity { get; set; }

        [Required(ErrorMessage = "Price is required")]
        public decimal? UnitPrice { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
    }
}
