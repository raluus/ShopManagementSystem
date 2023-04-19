﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShopManagementSystem.Models
{
    public class Product
    {
        public int Id { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        [Required]
        [StringLength(10)]
        public string ProductCategory{ get; set; } = string.Empty;

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string? ProductName { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string? ProductImage { get; set; }

        [Range(1, 500)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
    }
}
