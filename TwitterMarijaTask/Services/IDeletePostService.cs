namespace TwitterMarijaTask.Services
{
    public interface IDeletePostService
    {
        Task DeletePostAsync(int Id, int UserId);
    }
}
