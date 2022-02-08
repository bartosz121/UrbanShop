using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UrbanShop.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(80)]
        public string Name { get; set; }

        [Required]
        [MaxLength(80)]
        public string DisplayName { get; set; }

        public List<Product> Products { get; set; } = new List<Product>();

        [Required]
        public bool IsVisible { get; set; }
    }
}
