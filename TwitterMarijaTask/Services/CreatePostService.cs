using Microsoft.EntityFrameworkCore;
using TwitterMarijaTask.Data;

namespace TwitterMarijaTask.Services
{
    public class CreatePostService : ICreatePostService
    {
        private readonly AppDbContext _context;
        public CreatePostService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Post> CreateANewPostAsync(string description, string? imageUrl, int userId)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
            {
                throw new ArgumentException("User with the provided ID does not exist.");
            }
            if (description.Length < 12 || description.Length > 140)
            {
                throw new ArgumentException("Description must be between 12 and 140 characters.");
            }
            var post = new Post
            {
                Description = description,
                ImageUrl = imageUrl,
                UserId = userId,
                CreatedAt = DateTime.Now
            };
             _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return post;

        }
    }
}
