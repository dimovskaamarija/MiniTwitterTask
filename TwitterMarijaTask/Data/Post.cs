using System.ComponentModel.DataAnnotations;

namespace TwitterMarijaTask.Data
{
    public class Post
    {
        public int Id { get; set; }

        [Required]
        public  DateTime CreatedAt { get; set; }

        public string? ImageUrl { get; set; }

        [StringLength(140, MinimumLength = 12)]

        [Required]
        public  string? Description { get; set; }

        public int UserId { get; set; }

        public User User { get; set; } = null!;
    }
}
