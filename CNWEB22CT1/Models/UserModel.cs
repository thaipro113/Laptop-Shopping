using System.ComponentModel.DataAnnotations;

namespace CNWEB22CT1.Models
{
    public class UserModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập UserName")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Email"), EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password), Required(ErrorMessage = "Vui lòng nhập password")]
        public string Password { get; set; }

        [Phone(ErrorMessage = "Vui lòng nhập số điện thoại hợp lệ")]
        public string PhoneNumber { get; set; } // Thêm trường mới
    }
}
