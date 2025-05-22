using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TwitterMarijaTask.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) { }
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Name = "Marija", Email = "mda@mca.dev", Username = "dimovskaamarija", ImageUrl = "Images/users/user1.jpg", JoinedOn = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)) },
            new User { Id = 2, Name = "Sephora", Email = "sephora@support.com", Username = "sephora.official", ImageUrl = "Images/users/user3.png", JoinedOn = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)) },
            new User { Id = 3, Name ="Sports Master", Email = "sports-master@support.com", Username = "sports_master_official_store", ImageUrl="Images/users/user2.jpg" , JoinedOn = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)) });
        modelBuilder.Entity<Post>().HasData(
       new Post { Id = 1, CreatedAt = DateTime.Now.AddMinutes(-28), UserId = 1, Description = "Marija's  test post.", ImageUrl="Images/posts/post1.jpg"},
       new Post { Id = 2, CreatedAt = DateTime.Now.AddHours(-5), UserId = 1, Description = "Marija's  test post.", ImageUrl="Images/posts/post2.jpg"},
       new Post { Id = 3, CreatedAt = DateTime.Today.AddDays(-50), UserId = 1, Description = "Marija's  test post.", ImageUrl = "Images/posts/post3.jpg" },
       new Post { Id = 4, CreatedAt = DateTime.Now.AddMinutes(-34), UserId = 1, Description = "Marija's  test post." },
       new Post { Id = 5, CreatedAt = DateTime.Now.AddDays(-5), UserId = 1, Description = "Marija's  test post" ,ImageUrl = "Images/posts/post4.jpg" },
       new Post { Id = 6, CreatedAt = DateTime.Now.AddMinutes(-22), UserId = 2, Description = "Sephora's  test post" ,ImageUrl = "Images/posts/post5.jpeg" },
       new Post { Id = 7, CreatedAt = DateTime.Now.AddDays(-7), UserId = 2, Description = "Sephora's  test post" ,ImageUrl = "Images/posts/post6.png" },
       new Post { Id = 8, CreatedAt = DateTime.Now.AddDays(-1), UserId = 3, Description = "Sports Master's  test post." ,ImageUrl = "Images/posts/post7.jpg" },
       new Post { Id = 9, CreatedAt = DateTime.Now.AddHours(-4), UserId = 3, Description = "Sports Master's  test post." ,ImageUrl = "Images/posts/post8.jpg" }
       
       );

    }
}
