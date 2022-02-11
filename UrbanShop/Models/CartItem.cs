using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UrbanShop.ApiModels;

namespace UrbanShop.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public int ProductId { get; set; }

        public Product Product { get; set; }

        [Required]
        [Range(1, 99, ErrorMessage ="Value for {0} must be between {1} and {2}.")]
        public int Quantity { get; set; }
    }
}
