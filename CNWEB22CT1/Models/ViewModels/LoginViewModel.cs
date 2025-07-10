using System.ComponentModel.DataAnnotations;

namespace CNWEB22CT1.Models.ViewModels
{
	public class LoginViewModel
	{
		public string Id { get; set; }
		[Required(ErrorMessage = "Vui lòng nhập UserName")]
		public string Username { get; set; }

		[DataType(DataType.Password), Required(ErrorMessage = "Vui lòng nhập password")]
		public string Password { get; set; }
		public string ReturnUrl { get; set; }
	}
}
