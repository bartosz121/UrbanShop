using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UrbanShop.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Column(TypeName = "decimal(19, 2)")]
        public decimal Price { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public List<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

        [Required]
        public bool IsVisible { get; set; }

        public string GetThumbnailUrl()
        {
            string thumbnailUrl = "/product_images/default.jpg";

            if (ProductImages.Count > 0) {
                thumbnailUrl = $"/product_images/productId_{Id}/{ProductImages[0].ImageUrl}";
            }

            return thumbnailUrl;
        }

        public List<string> GetImagesUrls()
        {
            List<string> result = new List<string>();

            if(ProductImages.Count < 1)
            {
                result.Add("/product_images/default.jpg");
                return result;
            }

            foreach (var img in ProductImages)
            {
                result.Add($"/product_images/productId_{Id}/{img.ImageUrl}");
            }

            result.Reverse();

            return result;
        }
    }

}
