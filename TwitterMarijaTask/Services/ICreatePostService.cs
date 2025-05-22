using TwitterMarijaTask.Data;

namespace TwitterMarijaTask.Services
{
    public interface ICreatePostService
    {
        Task<Post> CreateANewPostAsync(string description, string? imageUrl, int userId);
    }
}
