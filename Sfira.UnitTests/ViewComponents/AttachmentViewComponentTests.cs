using Microsoft.AspNetCore.Mvc.ViewComponents;
using MroczekDotDev.Sfira.Models;
using MroczekDotDev.Sfira.ViewComponents;
using MroczekDotDev.Sfira.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MroczekDotDev.Sfira.UnitTests.ViewComponents
{
    public class AttachmentViewComponentTests
    {
        public static List<object[]> fileTypes;

        static AttachmentViewComponentTests()
        {
            fileTypes = new List<object[]>();
            foreach (var name in Enum.GetNames(typeof(FileType)))
            {
                fileTypes.Add(new object[] { name });
            }
        }

        private const string viewNamePostfix = "Attachment";

        private readonly AttachmentViewComponent component;

        public AttachmentViewComponentTests()
        {
            component = new AttachmentViewComponent();
        }

        [Fact]
        public async Task InvokeAsync_UndefinedFileType_ThrowsArgumentException()
        {
            // Arrange
            const string fileType = "undefined";
            var attachment = new AttachmentViewModel() { Type = fileType };

            // Act
            Task result() => component.InvokeAsync(attachment);

            // Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(result);
            string expectedMessage = "Requested value '" + fileType + "' was not found.";
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Theory]
        [MemberData(nameof(fileTypes))]
        public async Task InvokeAsync_DefinedFileType_ReturnsViewViewComponentResultWithProperViewName(string fileType)
        {
            // Arrange
            var attachment = new AttachmentViewModel() { Type = fileType };

            // Act
            var result = await component.InvokeAsync(attachment);

            // Assert
            var viewViewComponentResult = Assert.IsType<ViewViewComponentResult>(result);
            string expectedViewName = fileType + viewNamePostfix;
            Assert.Equal(expectedViewName, viewViewComponentResult.ViewName, StringComparer.OrdinalIgnoreCase);
        }
    }
}
