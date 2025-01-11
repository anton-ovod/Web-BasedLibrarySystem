using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Attributes
{
    public class FileExtensionValidationAttribute(string[] allowedExtensions) : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName)?.ToLower();
                if (!allowedExtensions.Contains(extension))
                {
                    return new ValidationResult($"Please upload a valid image ({string.Join(", ", allowedExtensions)}).");
                }
            }

            return ValidationResult.Success;
        }
    }

}
