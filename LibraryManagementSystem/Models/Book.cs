using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibraryManagementSystem.Extensions;
using LibraryManagementSystem.ViewModels;

namespace LibraryManagementSystem.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Genres { get; set; }

        public byte[]? Cover { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Number of pages must be at least 1.")]
        public int PagesNumber { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Read pages cannot be negative.")]
        public int ReadPagesNumber { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        public Book() {}

        public Book(CreateBookViewModel model, int userId)
        {
            Title = model.Title;
            Author = model.Author;
            Description = model.Description;
            Genres = model.Genres;
            Cover = model.NewCover?.ToByteArray();
            PagesNumber = model.PagesNumber;
            ReadPagesNumber = model.ReadPagesNumber;
            UserId = userId;
        }

        public Book(UpdateBookViewModel model, Book existingBook)
        {
            Title = model.Title;
            Author = model.Author;
            Description = model.Description;
            Genres = model.Genres;
            Cover = (model.NewCover is null) ? model.Cover : model.NewCover.ToByteArray();
            PagesNumber = model.PagesNumber;
            ReadPagesNumber = model.ReadPagesNumber;
            LastUpdatedAt = DateTime.UtcNow;
            CreatedAt = existingBook.CreatedAt;
            UserId = existingBook.UserId;
            Id = existingBook.Id;
        }
    }
}
