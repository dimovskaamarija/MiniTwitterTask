
using Microsoft.EntityFrameworkCore;
using TwitterMarijaTask.Data;

namespace TwitterMarijaTask.Services
{
    public class DeletePostService : IDeletePostService
    

    {
        private readonly AppDbContext _context;
        public DeletePostService(AppDbContext context)
        {
            _context = context;
        }
        public async Task DeletePostAsync(int Id, int UserId)
        {
             var post= await _context.Posts.FirstOrDefaultAsync(p => p.Id == Id && p.UserId == UserId);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();

            }
             

        }
    
    }
}
