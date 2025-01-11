using LibraryManagementSystem.Attributes;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.ViewModels
{
    public class CreateBookViewModel : IValidatableObject
    {
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author is required.")]
        public string Author { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Genres are required.")]
        [StringLength(200, ErrorMessage = "Genres cannot exceed 200 characters.")]
        public string Genres { get; set; }

        [Required(ErrorMessage = "Total pages are required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Total pages must be a positive number.")]
        public int PagesNumber { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Read pages must be a non-negative number.")]
        public int ReadPagesNumber { get; set; }

        [FileExtensionValidation([".jpg", ".jpeg", ".png", ".gif"])]
        public IFormFile? NewCover { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ReadPagesNumber > PagesNumber)
            {
                yield return new ValidationResult(
                    "Read pages cannot exceed total pages.",
                    [nameof(ReadPagesNumber)]);
            }
        }
    }
}
