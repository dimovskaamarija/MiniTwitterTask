using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterMarijaTask.Components;
using TwitterMarijaTask.Data;
using TwitterMarijaTask.Services;
using Xunit;

namespace TwitterMarijaTask.Tests
{
    public class ComponentTest : TestContext
    {
        [Fact]
        public void NewPostFormComponentContainsTagTest()
        {
            // Arrange
            var mockUserService = new Mock<IGetUserService>();
            mockUserService.Setup(s => s.GetUserByIdAsync(1)).ReturnsAsync(new User
            {
                Id = 40,
                Name = "Test",
                Email="test@gmail.com",
                ImageUrl = "Images/users/user1.jpg"
            });

            Services.AddSingleton(mockUserService.Object);
            Services.AddSingleton(Mock.Of<ICreatePostService>()); 

            // Act
            var renderedComponent = RenderComponent<NewPostForm>();

            // Assert
            var textArea = renderedComponent.Find("textarea[placeholder=\"What's happening?\"]");
            Assert.NotNull(textArea);
            Assert.Equal("What's happening?", textArea.GetAttribute("placeholder"));
        }

    }
}
