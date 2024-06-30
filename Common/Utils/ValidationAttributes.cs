using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Common.Utils
{
    public class NotNullOrEmptyOrWhitespaceAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string str)
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    return new ValidationResult(ErrorMessage ?? "The field cannot be null, empty, or whitespace.");
                }
            }

            return ValidationResult.Success;
        }
    }
    public class ImageValidationAttribute : ValidationAttribute
    {
        private readonly string[] _permittedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        private const long _maxFileSize = 2 * 1024 * 1024; // 2MB

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (string.IsNullOrEmpty(ext) || !_permittedExtensions.Contains(ext))
                {
                    return new ValidationResult("The Image must be a valid image file (.jpg, .jpeg, .png, .gif).");
                }

                if (file.Length > _maxFileSize)
                {
                    return new ValidationResult("The Image must be less than 2MB in size.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
