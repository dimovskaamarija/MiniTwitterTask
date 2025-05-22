using System.ComponentModel.DataAnnotations;

namespace TwitterMarijaTask.Data
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        [Required]
        public required string? Email { get; set; }

        [Required]
        public  string? Username { get; set; }

        [Required]
        public DateOnly JoinedOn { get; set; }

        public string? ImageUrl { get; set; }

        public ICollection <Post> Posts { get;  } = new List<Post>();

    }
}
