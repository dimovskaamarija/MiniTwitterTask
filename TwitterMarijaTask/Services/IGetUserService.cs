using TwitterMarijaTask.Data;

namespace TwitterMarijaTask.Services
{
    public interface IGetUserService
    {
        Task<User> GetUserByIdAsync(int id);
    }
}
