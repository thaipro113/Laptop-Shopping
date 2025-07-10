using CNWEB22CT1.Repository.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CNWEB22CT1.Models
{
    public class ProductModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập tên Sản phẩm")]
        [MinLength(4, ErrorMessage = "Tên sản phẩm phải có ít nhất 4 ký tự")]
        public string Name { get; set; }

        public string Slug { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập mô tả Sản phẩm")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập giá Sản phẩm")]
        //[Range(1, double.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Chọn 1 Thương hiệu")]
        [ForeignKey("BrandId")]
        public int BrandId { get; set; }

        [Required(ErrorMessage = "Chọn 1 Danh mục")]
        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }

        public CategoryModel Category { get; set; }
        public BrandModel Brand { get; set; }

        public string Image { get; set; }

        [NotMapped] 
        [FileExtension]
        public IFormFile? ImageUpload { get; set; }
    }
}
