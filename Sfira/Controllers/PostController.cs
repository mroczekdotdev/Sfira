using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MroczekDotDev.Sfira.Data;
using MroczekDotDev.Sfira.Models;
using MroczekDotDev.Sfira.Services.FileUploading;
using MroczekDotDev.Sfira.ViewModels;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace MroczekDotDev.Sfira.Controllers
{
    public class PostController : Controller
    {
        private readonly IHostingEnvironment environment;
        private readonly IDataStorage dataStorage;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IFileUploader fileUploader;

        public PostController(
            IHostingEnvironment environment,
            IDataStorage dataStorage,
            UserManager<ApplicationUser> userManager,
            IFileUploader fileUploader)
        {
            this.environment = environment;
            this.dataStorage = dataStorage;
            this.userManager = userManager;
            this.fileUploader = fileUploader;
        }

        [Authorize]
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create(PostViewModel post, IFormFile formFile)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser currentUser = await userManager.FindByNameAsync(User.Identity.Name);
                post.Author = currentUser;

                if (formFile != null && formFile.Length > 0)
                {
                    string userMediaPath = Path.Combine(new[] {
                        environment.WebRootPath, "media", "user", currentUser.Id + Path.DirectorySeparatorChar });

                    var file = new UploadableImageFile
                    {
                        formFile = formFile,
                        directory = userMediaPath,
                        name = Guid.NewGuid().ToString(),
                        extension = FilenameExtension.jpg.ToString(),
                        format = ImageFormat.Jpeg,
                        quality = 40L,
                        maxWidth = 1920,
                        maxHeight = 1080,
                        hasThumbnail = true,
                        thumbWidth = 512,
                        thumbHeight = 512,
                    };
                    await fileUploader.Upload(file);

                    post.Attachment = new AttachmentViewModel
                    {
                        Owner = currentUser,
                        Type = FileType.image.ToString(),
                        Name = file.name,
                        Extension = file.extension,
                    };
                }

                dataStorage.AddPost(post);

                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [Authorize]
        public async Task<IActionResult> Mark(int postId, string interaction)
        {
            ApplicationUser currentUser = await userManager.FindByNameAsync(User.Identity.Name);
            UserPost userPost = dataStorage.MarkPost(currentUser.Id, postId, interaction);

            if (userPost != null)
            {
                PostViewModel post = dataStorage.GetPostById(postId).ToViewModel;

                var postCounters = new
                {
                    likescount = post.LikesCount,
                    favoritescount = post.FavoritesCount,
                };

                return Ok(postCounters);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
