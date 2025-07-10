using System.ComponentModel.DataAnnotations;

namespace CNWEB22CT1.Models
{
    public class CategoryModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập tên danh mục")]
        public string Description { get; set; }
		[Required(ErrorMessage = "Yêu cầu nhập tên danh mục")]
		public string Name { get; set; }
       
        public string Slug { get; set; }

        public int Status { get; set; }
       
    }
}
