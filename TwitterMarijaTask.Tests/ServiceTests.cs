using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using TwitterMarijaTask.Data;
using TwitterMarijaTask.Services;

namespace TwitterMarijaTask.Tests
{
    public class ServiceTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }
        [Fact]
        public async Task GetAllPostsAsync_WhenCalled_ReturnsAllPosts()
        {
            //Arrange
            var ctx = GetInMemoryDbContext();
            ctx.Users.Add(new User { Id = 14, Name = "Test14", Email = "test14@gmail.com", Username = "test14", ImageUrl = "Images/users/user1.jpg", JoinedOn = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)) });
            ctx.Users.Add(new User { Id = 16, Name = "Test16", Email = "test16@gmail.com", Username = "test16", ImageUrl = "Images/users/user1.jpg", JoinedOn = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)) });
            ctx.Posts.Add(new Post { Id = 14, Description = "Post14 test description", CreatedAt = DateTime.Now.AddMinutes(-1), UserId = 14 });
            ctx.Posts.Add(new Post { Id = 16, Description = "Post16 test description", CreatedAt = DateTime.Now.AddMinutes(-1), UserId = 16 });
            await ctx.SaveChangesAsync();
            var GetAllPostsService = new GetPostService(ctx);
            //Act
            var posts = await GetAllPostsService.GetAllPostsAsync();
            //Assert
            var result= await ctx.Posts.OrderByDescending(p => p.CreatedAt).Include(p => p.User).ToListAsync();
            Assert.NotNull(posts);
            Assert.NotNull(result);
            Assert.Equal(posts.Count, result.Count);
            Assert.Equal(result[1].Description, posts[1].Description);
            Assert.Equal(result[0].Description, posts[0].Description);
            Assert.Equal(result[1].Id, posts[1].Id);
            Assert.Equal(result[0].Id, posts[0].Id);
            Assert.Equal(result[1].UserId, posts[1].UserId);
            Assert.Equal(result[0].UserId, posts[0].UserId);
        }

        [Fact]
        public async Task GetPostsByUsernameAsyncUsernameExists_ReturnsMatchingPosts()
        {
            //Arrange
            var ctx = GetInMemoryDbContext();
            ctx.Users.Add(new User { Id = 12, Name = "Test1", Email = "test@gmail.com", Username = "test12", ImageUrl = "Images/users/user1.jpg", JoinedOn = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)) });
            ctx.Users.Add(new User { Id = 13, Name = "Test2", Email = "test2@gmail.com", Username = "test2", ImageUrl = "Images/users/user1.jpg", JoinedOn = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)) });
            ctx.Posts.Add(new Post { Id = 12, Description = "Post1 test description", CreatedAt = DateTime.Now.AddMinutes(-1), UserId = 12 });
            ctx.Posts.Add(new Post { Id = 13, Description = "Post2 description", CreatedAt = DateTime.Now.AddMinutes(-1), UserId = 13 });
            await ctx.SaveChangesAsync();
            var GetPostsService = new GetPostService(ctx);
            //Act
            var PostsByUsername = await GetPostsService.GetPostsByUsernameAsync("test12");
            //Assert
            var result = await ctx.Posts.Where(p=> p.User.Username=="test12").ToListAsync();
            Assert.NotNull(result);
            Assert.NotNull(PostsByUsername);
            Assert.Equal(result[0].Description, PostsByUsername[0].Description);
            Assert.Equal(result[0].Id, PostsByUsername[0].Id);
            Assert.Equal( result[0].UserId, PostsByUsername[0].UserId);

        }

        [Fact]
        public async Task GetPostsByUsernameAsync_UsernameDoesNotExist_ReturnsEmptyList()
        {
            //Arrange
            var ctx = GetInMemoryDbContext();
            var GetPostService = new GetPostService(ctx);
            //Act
            var PostsByUsername = await GetPostService.GetPostsByUsernameAsync("userdoesnotexist");
            //Assert
            Assert.Empty(PostsByUsername);
            Assert.Empty(ctx.Posts);
        }
      
        [Fact]
        public async Task CreateANewPostAsync_DescriptionIsMinimumLength_CreatesPostSuccessfully()
        {
            //Arrange
            var ctx = GetInMemoryDbContext();
            ctx.Users.Add(new User { Id = 10, Name = "Test1", Email = "test@gmail.com", Username = "test", ImageUrl = "Images/users/user1.jpg", JoinedOn = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)) });
            var CreatePostService = new CreatePostService(ctx);
           await  ctx.SaveChangesAsync();
            //Act
            var post = await CreatePostService.CreateANewPostAsync("Test success", "Images/posts/post1.jpg", 10);
            //Assert
            var result = await ctx.Posts.FindAsync(post.Id);
            Assert.NotNull(result);
            Assert.Equal(post.Description, result.Description);
            Assert.Equal(post.ImageUrl, result.ImageUrl);
            Assert.Equal(post.UserId ,result.UserId);
            
        }

        [Fact]
        public async Task CreateANewPostAsync_CreatesPostSuccessfully()
        {
            var ctx = GetInMemoryDbContext();
            ctx.Users.Add(new User { Id = 10, Name = "Test1", Email = "test@gmail.com", Username = "test", ImageUrl = "Images/users/user1.jpg", JoinedOn = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)) });
            var CreatePostService = new CreatePostService(ctx);
            await ctx.SaveChangesAsync();
            //Act
            var post = await CreatePostService.CreateANewPostAsync("Test success", "Images/posts/post1.jpg", 10);
            //Assert
            var result = await ctx.Posts.FindAsync(post.Id);
            Assert.NotNull(result);
            Assert.Equal("Test success", result.Description);
            Assert.Equal("Images/posts/post1.jpg", result.ImageUrl);
            Assert.Equal(10, result.UserId);
            Assert.Single(ctx.Posts);
        }


            [Fact]
        public async Task CreateANewPostAsync_DescriptionIsMaximumLength_CreatesPostSuccessfully()
        {
            //Arrange
            var ctx = GetInMemoryDbContext();
            ctx.Users.Add(new User { Id = 20, Name = "Test2", Email = "test2@gmail.com", Username = "test2", ImageUrl = "Images/users/user2.jpg", JoinedOn = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)) });
            var CreatePostService = new CreatePostService(ctx);
           await ctx.SaveChangesAsync();
            //Act
            var post = await CreatePostService.CreateANewPostAsync("Test success Test success Test success Test success Test success Test success Test success Test success Test success Test success Test succe", "Images/posts/post2.jpg", 20);
            //Assert
            var result = await ctx.Posts.FindAsync(post.Id);
            Assert.NotNull(result);
            Assert.Equal(post.Description, result.Description);
            Assert.Equal(post.ImageUrl, result.ImageUrl);
            Assert.Equal(post.UserId, result.UserId);
 
        }
        
        [Fact]
        public async Task CreateANewPostAsync_DescriptionExceedsMaximumLength_ThrowsArgumentException()
        {
            //Arrange
            var ctx = GetInMemoryDbContext();
            ctx.Users.Add(new User { Id = 10, Name = "Test1", Email = "test@gmail.com", Username = "test", ImageUrl = "Images/users/user1.jpg", JoinedOn = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)) });
            var CreatePostService = new CreatePostService(ctx);
           await  ctx.SaveChangesAsync();
            //Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => CreatePostService.CreateANewPostAsync("Test post description extends range Test post description extends range Test post description extends range Test post description extends Tes", "Images/posts/post1.jpg", 10));
            Assert.Equal("Description must be between 12 and 140 characters.", exception.Message);
            Assert.Empty(ctx.Posts);
        }
        // Description = 11 characters 
        [Fact]
        public async Task CreateANewPostAsync_DescriptionBelowMinimumLength_ThrowsArgumentException()
        {
            //Arrange
            var ctx = GetInMemoryDbContext();
            ctx.Users.Add(new User { Id = 10, Name = "Test1", Email = "test@gmail.com", Username = "test", ImageUrl = "Images/users/user1.jpg", JoinedOn = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)) });
            var CreatePostService = new CreatePostService(ctx);
            await ctx.SaveChangesAsync();
            //Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => CreatePostService.CreateANewPostAsync("Test under1", "Images/posts/post1.jpg", 10));
            Assert.Equal("Description must be between 12 and 140 characters.", exception.Message);
            Assert.Empty(ctx.Posts);
        }

        [Fact]
        public async Task CreateANewPostAsync_UserDoesNotExist_ThrowsArgumentException()
        {
            //Arrange
            var ctx = GetInMemoryDbContext();
            var CreatePostService = new CreatePostService(ctx);
            //Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => CreatePostService.CreateANewPostAsync("Test for user does not exist", "Images/posts/post1.jpg", 20));
            Assert.Equal("User with the provided ID does not exist.", exception.Message);
            Assert.Empty(ctx.Posts);

        }

        [Fact]
        public async Task GetUserByIdAsync_UserExists_ReturnsUser()
        {
            //Arrange
            var ctx = GetInMemoryDbContext();
            ctx.Users.Add(new User { Id = 15, Name = "Test1", Email = "test@gmail.com", Username = "test", ImageUrl = "Images/users/user1.jpg", JoinedOn = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)) });
            await ctx.SaveChangesAsync();
            var GetUserService = new GetUserService(ctx);
            //Act
            var user = await GetUserService.GetUserByIdAsync(15);
            //Assert
            var result= await ctx.Users.FindAsync(15);
            Assert.NotNull(result);
            Assert.Equal(user.Username, result.Username);
            Assert.Equal(user.Name, result.Name);
            Assert.Equal(user.Email, result.Email);
            Assert.Equal(user.ImageUrl, result.ImageUrl);
            Assert.Equal(user.JoinedOn, result.JoinedOn);
            Assert.Equal(user.Id, result.Id);
        }

        [Fact]
        public async Task GetUserByIdAsync_UserDoesNotExist_ThrowsArgumentException()
        {
            //Arrange
            var ctx = GetInMemoryDbContext();
            var GetUserService = new GetUserService(ctx);
            //Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => GetUserService.GetUserByIdAsync(30));
            Assert.Equal("User with the provided ID does not exist.", exception.Message);
            Assert.Empty(ctx.Users);
        }

        [Fact]
        public async Task DeletePostAsync_PostExistsAndUserMatches_DeletesPost()
        {
            //Arrange
            var ctx = GetInMemoryDbContext();
            var user = new User { Id = 10, Name = "Test1", Email = "test@gmail.com", Username = "test", ImageUrl = "Images/users/user1.jpg", JoinedOn = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)) };
            var post = new Post { Id = 10, Description = "Post1 test description", CreatedAt = DateTime.Now.AddMinutes(-1), UserId = 10 };
            ctx.Posts.Add(post);
            ctx.Users.Add(user);
            await ctx.SaveChangesAsync();
            var DeletePostService = new DeletePostService(ctx);
            //Act
            await DeletePostService.DeletePostAsync(10, 10);
            //Assert
            var result = await ctx.Posts.FindAsync(10);
            Assert.Null(result);

        }

        [Fact]
        public async Task DeletePostAsync_PostIdDoesNotMatchUserId_DoesNotDeletePost()
        {
            //Arrange
            var ctx = GetInMemoryDbContext();
            var user = new User { Id = 10, Name = "Test1", Email = "test@gmail.com", Username = "test", ImageUrl = "Images/users/user1.jpg", JoinedOn = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)) };
            var post = new Post { Id = 10, Description = "Post1 test description", CreatedAt = DateTime.Now.AddMinutes(-1), UserId = 10 };
            var post1 = new Post { Id = 100, Description = "Post1 test description", CreatedAt = DateTime.Now.AddMinutes(-1), UserId = 10 };
            ctx.Users.Add(user);
            ctx.Posts.Add(post);
            await ctx.SaveChangesAsync();
            var DeletePostService = new DeletePostService(ctx);
            //Act
            await DeletePostService.DeletePostAsync(100, 10);
            //Assert
            var result = await ctx.Posts.FindAsync(10);
            Assert.NotNull(result);

        }
    }
}
