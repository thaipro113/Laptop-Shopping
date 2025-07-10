namespace CNWEB22CT1.Models
{
	public class WishlistModel
	{
		public int Id { get; set; }
		public int ProductId { get; set; }
		public string UserName { get; set; } // Email hoặc username của người dùng
		public DateTime AddedDate { get; set; } = DateTime.Now;

		public ProductModel Product { get; set; } // Thông tin sản phẩm
	}
}
