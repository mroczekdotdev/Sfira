using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MroczekDotDev.Sfira.Data;
using MroczekDotDev.Sfira.Models;
using MroczekDotDev.Sfira.Services;
using MroczekDotDev.Sfira.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        private readonly FileUpload fileUpload;

        public PostController(IHostingEnvironment environment, IDataStorage dataStorage,
            UserManager<ApplicationUser> userManager, FileUpload fileUpload)
        {
            this.environment = environment;
            this.dataStorage = dataStorage;
            this.userManager = userManager;
            this.fileUpload = fileUpload;
        }

        [Authorize]
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create(PostViewModel post, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser currentUser = await userManager.FindByNameAsync(User.Identity.Name);
                post.Author = currentUser;

                if (file != null && file.Length > 0)
                {
                    string fileType;
                    string directory = Path.Combine(new[] {
                        environment.WebRootPath, "media", "user", currentUser.Id + Path.DirectorySeparatorChar });
                    string fileName = Guid.NewGuid().ToString();
                    string fileExtension;

                    async Task AttachmentToImage()
                    {
                        fileType = "image";
                        fileExtension = FilenameExtension.jpg.ToString();
                        ImageFormat imageFormat = ImageFormat.Jpeg;
                        long imageQuality = 40L;

                        using (var memoryStream = new MemoryStream())
                        {
                            await file.CopyToAsync(memoryStream);
                            Image image = Image.FromStream(memoryStream);
                            IEnumerable<(Image, string fileName)> images = fileUpload.ProcessImage(image, fileName,
                                fileExtension);
                            fileUpload.SaveImages(images, directory, imageFormat, imageQuality);
                        }
                    }

                    switch (file.ContentType.ToLower())
                    {
                        case "image/jpeg":
                        case "image/png":
                            Directory.CreateDirectory(directory);
                            await AttachmentToImage();
                            break;

                        default:
                            return BadRequest();
                    }

                    post.Attachment = new AttachmentViewModel
                    {
                        Owner = currentUser,
                        Type = fileType,
                        Name = fileName,
                        Extension = fileExtension,
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
                PostViewModel post = dataStorage.GetPostById(postId).ToViewModel();

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
