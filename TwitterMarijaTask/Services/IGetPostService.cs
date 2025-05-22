using TwitterMarijaTask.Data;

namespace TwitterMarijaTask.Services
{
    public interface IGetPostService
    {
        Task<List<Post>> GetAllPostsAsync();
        Task<List<Post>> GetPostsByUsernameAsync(String username);
    }
}
