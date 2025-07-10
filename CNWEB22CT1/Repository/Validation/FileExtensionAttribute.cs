using System.ComponentModel.DataAnnotations;

namespace CNWEB22CT1.Repository.Validation
{
    public class FileExtensionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant(); // Lấy phần mở rộng và chuyển về chữ thường
                string[] allowedExtensions = { ".png", ".jpg", ".jpeg", ".webp" }; // Bao gồm dấu chấm
                if (!allowedExtensions.Contains(extension))
                {
                    return new ValidationResult("Chỉ cho phép sử dụng các hình ảnh chứa đuôi .png, .jpg, .jpeg, .webp");
                }
            }
            return ValidationResult.Success;
        }
    }
}
