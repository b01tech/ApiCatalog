﻿using System.ComponentModel.DataAnnotations;

namespace ApiCatalog.Models;

public class Category
{
    [Key]
    public int CategoryId { get; set; }
    [Required]
    [StringLength(80)]
    public string? Name { get; set; }
    [StringLength(300)]
    public string? ImageUrl { get; set; }
    public ICollection<Product>? Products { get; set; }

    public Category()
    {
        Products = new List<Product>();
    }
}
