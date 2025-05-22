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
        public async Task GetAllPostsAsyncSuccessTest()
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
            Assert.NotNull(posts);
            Assert.Equal(2, posts.Count);
            Assert.Equal("Post14 test description", posts[1].Description);
            Assert.Equal("Post16 test description", posts[0].Description);
            Assert.Equal(14, posts[1].Id);
            Assert.Equal(16, posts[0].Id);
            Assert.Equal(14, posts[1].UserId);
            Assert.Equal(16, posts[0].UserId);
        }

        [Fact]
        public async Task GetPostsByUsernameAsyncSuccessTest()
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
            Assert.NotNull(PostsByUsername);
            Assert.Single(PostsByUsername);
            Assert.Equal("Post1 test description", PostsByUsername[0].Description);
            Assert.Equal(12, PostsByUsername[0].Id);

        }

        [Fact]
        public async Task GetPostsByUsernameAsyncFailTest()
        {
            //Arrange
            var ctx = GetInMemoryDbContext();
            var GetPostService = new GetPostService(ctx);
            //Act
            var PostsByUsername = await GetPostService.GetPostsByUsernameAsync("userdoesnotexist");
            //Assert
            Assert.Empty(PostsByUsername);
        }

        [Fact]
        public async Task CreateANewPostAsyncSuccessTest()
        {
            //Arrange
            var ctx = GetInMemoryDbContext();
            ctx.Users.Add(new User { Id = 10, Name = "Test1", Email = "test@gmail.com", Username = "test", ImageUrl = "Images/users/user1.jpg", JoinedOn = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)) });
            var CreatePostService = new CreatePostService(ctx);
            ctx.SaveChanges();
            //Act
            var post = await CreatePostService.CreateANewPostAsync("Test post successfully created", "Images/posts/post1.jpg", 10);
            //Assert
            Assert.NotNull(post);
            Assert.Equal("Test post successfully created", post.Description);
            Assert.Equal("Images/posts/post1.jpg", post.ImageUrl);
            Assert.Equal(10, post.UserId);
            Assert.Single(ctx.Posts);
        }

        [Fact]
        public async Task CreateANewPostAsyncDescriptionExtendsRangeTest()
        {
            //Arrange
            var ctx = GetInMemoryDbContext();
            ctx.Users.Add(new User { Id = 10, Name = "Test1", Email = "test@gmail.com", Username = "test", ImageUrl = "Images/users/user1.jpg", JoinedOn = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)) });
            var CreatePostService = new CreatePostService(ctx);
            ctx.SaveChanges();
            //Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => CreatePostService.CreateANewPostAsync("Test post description bigger than maximum length.Test post description bigger than maximum length.Test post description bigger than maximum length.Test post Test post description bigger than maximum length.Test post description bigger than maximum length.Test post description bigger than maximum length.Test post description bigger than maximum length.Test post description bigger than maximum length.Test post description bigger than maximum length.Test post description bigger than maximum length.Test post description bigger than maximum length.Test post description bigger than maximum length.Test post description bigger than maximum length.Test post description bigger than maximum length.Test post description bigger than maximum length.Test post description bigger than maximum length.Test post description bigger than maximum length.Test post description bigger than maximum length.Test post description bigger than maximum length.Test post description bigger than maximum length.Test post description bigger than maximum length.Test post description bigger than maximum length. bigger than maximum length. ", "Images/posts/post1.jpg", 10));
            Assert.Equal("Description must be between 12 and 140 characters.", exception.Message);
        }

        [Fact]
        public async Task CreateANewPostAsyncDescriptionUnderRangeTest()
        {
            //Arrange
            var ctx = GetInMemoryDbContext();
            ctx.Users.Add(new User { Id = 10, Name = "Test1", Email = "test@gmail.com", Username = "test", ImageUrl = "Images/users/user1.jpg", JoinedOn = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)) });
            var CreatePostService = new CreatePostService(ctx);
            ctx.SaveChanges();
            //Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => CreatePostService.CreateANewPostAsync("Test", "Images/posts/post1.jpg", 10));
            Assert.Equal("Description must be between 12 and 140 characters.", exception.Message);
        }

        [Fact]
        public async Task CreateANewPostAsyncInvalidUserTest()
        {
            //Arrange
            var ctx = GetInMemoryDbContext();
            var CreatePostService = new CreatePostService(ctx);
            //Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => CreatePostService.CreateANewPostAsync("Test for user does not exist", "Images/posts/post1.jpg", 20));
            Assert.Equal("User with the provided ID does not exist.", exception.Message);

        }

        [Fact]
        public async Task GetUserByIdSuccessTest()
        {
            //Arrange
            var ctx = GetInMemoryDbContext();
            ctx.Users.Add(new User { Id = 15, Name = "Test1", Email = "test@gmail.com", Username = "test", ImageUrl = "Images/users/user1.jpg", JoinedOn = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)) });
            await ctx.SaveChangesAsync();
            var GetUserService = new GetUserService(ctx);
            //Act
            var user = await GetUserService.GetUserByIdAsync(15);
            //Assert
            Assert.NotNull(user);
            Assert.Equal("test", user.Username);
            Assert.Equal("Test1", user.Name);
            Assert.Equal("test@gmail.com", user.Email);
            Assert.Equal("Images/users/user1.jpg", user.ImageUrl);
            Assert.Equal(DateOnly.FromDateTime(DateTime.Now.AddDays(-1)), user.JoinedOn);
            Assert.Equal(15, user.Id);
        }

        [Fact]
        public async Task GetUserByIdFailTest()
        {
            //Arrange
            var ctx = GetInMemoryDbContext();
            var GetUserService = new GetUserService(ctx);
            //Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => GetUserService.GetUserByIdAsync(30));
            Assert.Equal("User with the provided ID does not exist.", exception.Message);
        }

        [Fact]
        public async Task DeletePostSuccessTest()
        {
            //Arrange
            var ctx = GetInMemoryDbContext();
            var user = new User { Id = 10, Name = "Test1", Email = "test@gmail.com", Username = "test", ImageUrl = "Images/users/user1.jpg", JoinedOn = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)) };
            var post = new Post { Id = 10, Description = "Post1 test description", CreatedAt = DateTime.Now.AddMinutes(-1), UserId = 10 };
            ctx.Posts.Add(post);
            await ctx.SaveChangesAsync();
            var DeletePostService = new DeletePostService(ctx);
            //Act
            await DeletePostService.DeletePostAsync(10, 10);
            //Assert
            var result = await ctx.Posts.FindAsync(10);
            Assert.Null(result);

        }

        [Fact]
        public async Task DeletePostFailTest()
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
