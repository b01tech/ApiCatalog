using System.ComponentModel.DataAnnotations;

namespace ApiCatalog.DTOs
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        [Required]
        [StringLength(80)]
        public string? Name { get; set; }
        [StringLength(300)]
        public string? Description { get; set; }
        [Required]
        public decimal? Price { get; set; }
        [StringLength(300)]
        public string? ImageUrl { get; set; }
        public float? Amount { get; set; }
        public DateTime RegistrationDate { get; set; }

        public int CategoryId { get; set; }

    }
}
