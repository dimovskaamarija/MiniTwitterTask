using Microsoft.EntityFrameworkCore;
using TwitterMarijaTask.Data;

namespace TwitterMarijaTask.Services
{
    public class GetUserService : IGetUserService
    {
        private readonly AppDbContext _context;

        public GetUserService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<User> GetUserByIdAsync(int id)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == id);
            if (!userExists)
            {
                throw new ArgumentException("User with the provided ID does not exist.");
             }
            return await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
        }
    }
}
