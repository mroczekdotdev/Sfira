using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using MroczekDotDev.Sfira.Controllers;
using MroczekDotDev.Sfira.Data;
using MroczekDotDev.Sfira.Models;
using MroczekDotDev.Sfira.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace MroczekDotDev.Sfira.UnitTests.Controllers
{
    public class HomeControllerTests
    {
        private const int postsFeedCount = 3;

        private readonly Mock<IRepository> repository;
        private readonly Mock<UserManager<ApplicationUser>> userManager;
        private readonly Mock<IOptionsMonitor<FeedOptions>> options;
        private readonly HomeController controller;

        public HomeControllerTests()
        {
            repository = new Mock<IRepository>();
            repository.Setup(r => r.GetPostsByFollowerId(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns((string userId, int? count, int? cursor) =>
                {
                    var posts = new List<Post>();
                    for (int i = 0; i < count; i++)
                    {
                        posts.Add(new Post());
                    }
                    if (posts.Count > 0)
                    {
                        posts.Last().Id = cursor.HasValue ? (int)cursor : (int)count;
                    }
                    return posts;
                });

            var userStore = Mock.Of<IUserStore<ApplicationUser>>();
            userManager = new Mock<UserManager<ApplicationUser>>(
                userStore, null, null, null, null, null, null, null, null);
            userManager.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser());

            options = new Mock<IOptionsMonitor<FeedOptions>>();
            options.SetupGet(o => o.CurrentValue)
                .Returns(new FeedOptions() { PostsFeedCount = postsFeedCount });

            controller = new HomeController(repository.Object, userManager.Object, options.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(),
                }
            };
        }

        [Fact]
        public async Task Index_NotAuthenticated_ReturnsViewResultWithoutModel()
        {
            // Arrange
            controller.ControllerContext.HttpContext.User.AddIdentity(new ClaimsIdentity());

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task Index_Authenticated_ReturnsViewResultWithProperModel()
        {
            // Arrange
            controller.ControllerContext.HttpContext.User.AddIdentity(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "Name")
            }, "authenticationType"));

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<PostsFeedLoaderViewModel>(viewResult.ViewData.Model);
            Assert.Equal(postsFeedCount, model.Posts.Count());
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(3, 1)]
        [InlineData(36, 12)]
        public async Task PostsFeed_ByDefault_ReturnsViewComponentResultWithProperArguments(int count, int cursor)
        {
            // Arrange
            controller.ControllerContext.HttpContext.User.AddIdentity(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "Name")
            }, "authenticationType"));

            // Act
            var result = await controller.PostsFeed(count, cursor);

            // Assert
            var viewComponentResult = Assert.IsType<ViewComponentResult>(result);
            var arguments = Assert.IsAssignableFrom<IEnumerable<PostViewModel>>(viewComponentResult.Arguments);
            if (arguments.Any())
            {
                Assert.Equal(count, arguments.Count());
                Assert.Equal(cursor, arguments.Last().Id);
            }
        }
    }
}
