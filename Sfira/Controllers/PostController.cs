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
using System.IO;
using System.Threading.Tasks;

namespace MroczekDotDev.Sfira.Controllers
{
    public class PostController : Controller
    {
        private readonly IWebHostEnvironment env;
        private readonly IRepository repository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IFileUploader fileUploader;

        public static string Name { get; } = nameof(PostController)
            .Substring(0, nameof(PostController).LastIndexOf(nameof(Controller)));

        public PostController(
            IWebHostEnvironment env,
            IRepository repository,
            UserManager<ApplicationUser> userManager,
            IFileUploader fileUploader)
        {
            this.env = env;
            this.repository = repository;
            this.userManager = userManager;
            this.fileUploader = fileUploader;
        }

        [Authorize]
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] PostViewModel post, IFormFile formFile)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser currentUser = await userManager.FindByNameAsync(User.Identity.Name);
                post.Author = currentUser;

                if (formFile != null && formFile.Length > 0)
                {
                    string userMediaPath = Path.Combine(new[] {
                        env.WebRootPath, "media", "user", currentUser.Id + Path.DirectorySeparatorChar });

                    UploadableImageFile file = fileUploader.NewUploadableImageFile();
                    file.FormFile = formFile;
                    file.Directory = userMediaPath;
                    file.Name = Guid.NewGuid().ToString();
                    file.MaxWidth = 1920;
                    file.MaxHeight = 1080;
                    file.HasThumbnail = true;
                    file.ThumbWidth = 512;
                    file.ThumbHeight = 512;

                    await fileUploader.Upload(file);

                    post.Attachment = new AttachmentViewModel
                    {
                        Owner = currentUser,
                        Type = FileType.image.ToString(),
                        Name = file.Name,
                        Extension = file.Extension,
                    };
                }

                repository.AddPost(post);

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
            UserPost userPost = repository.MarkPost(currentUser.Id, postId, interaction);

            if (userPost != null)
            {
                PostViewModel post = repository.GetPostById(postId).ToViewModel;

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
