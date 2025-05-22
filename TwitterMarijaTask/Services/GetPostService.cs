using Microsoft.EntityFrameworkCore;
using TwitterMarijaTask.Data;

namespace TwitterMarijaTask.Services
{
    public class GetPostService : IGetPostService
    {

        private readonly AppDbContext _context;

        public GetPostService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Post>> GetAllPostsAsync() =>
            await _context.Posts.OrderByDescending(p => p.CreatedAt).Include(p=>p.User).ToListAsync();

        public async Task<List<Post>> GetPostsByUsernameAsync(string username)
        {
            return await _context.Posts
                .Where(p => p.User != null && p.User.Username == username)
                .Include(p => p.User)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }
    }
}
