using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Models
{
    public class UserSession
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string SessionId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required] 
        public string Device { get; set; }

        [Required]
        public string IpAddress { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public TimeSpan ExpireTimeSpan { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [NotMapped]
        public DateTime ExpiresAt => CreatedAt + ExpireTimeSpan;

    }
}
