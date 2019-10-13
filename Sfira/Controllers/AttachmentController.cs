using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MroczekDotDev.Sfira.Data;
using MroczekDotDev.Sfira.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MroczekDotDev.Sfira.Controllers
{
    public class AttachmentController : Controller
    {
        private readonly IHostingEnvironment environment;
        private readonly IDataStorage dataStorage;
        private readonly UserManager<ApplicationUser> userManager;

        public AttachmentController(IHostingEnvironment environment, IDataStorage dataStorage,
            UserManager<ApplicationUser> userManager)
        {
            this.environment = environment;
            this.dataStorage = dataStorage;
            this.userManager = userManager;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(IFormFile file)
        {
            const int maxSizeInBytes = 25165824;

            if (file.Length == 0 || file.Length > maxSizeInBytes)
            {
                return BadRequest();
            }

            ApplicationUser currentUser = await userManager.FindByNameAsync(User.Identity.Name);
            string type;
            string directory = Path.Combine(new[] {
                environment.WebRootPath, "media", "user", currentUser.Id + Path.DirectorySeparatorChar });
            string name = Guid.NewGuid().ToString();
            string extension;

            async Task AttachmentToImage()
            {
                type = "image";
                extension = FilenameExtension.jpg.ToString();
                ImageFormat imageFormat = ImageFormat.Jpeg;
                long imageQuality = 40L;

                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    Image image = Image.FromStream(memoryStream);
                    IEnumerable<(Image, string fileName)> images = ProcessImage(image, name, extension);
                    SaveImages(images, directory, imageFormat, imageQuality);
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

            var attachmentInfo = new
            {
                type,
                name,
                extension,
            };

            return Ok(attachmentInfo);
        }

        [NonAction]
        private IEnumerable<(Image, string fileName)> ProcessImage(Image image, string name, string extension)
        {
            const int maxWidth = 1920;
            const int maxHeight = 1080;

            Image thumb = GenerateThumbnail(image);

            if (image.Width > maxWidth || image.Height > maxHeight)
            {
                double scale = Math.Min(maxWidth / (double)image.Width, maxHeight / (double)image.Height);
                int newWidth = (int)(image.Width * scale);
                int newHeight = (int)(image.Height * scale);

                Bitmap newImage = new Bitmap(newWidth, newHeight);

                using (Graphics graphics = Graphics.FromImage(newImage))
                {
                    graphics.CompositingQuality = CompositingQuality.HighSpeed;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.DrawImage(image, 0, 0, newWidth, newHeight);
                }

                image = newImage;
            }

            var images = new List<(Image, string fileName)>();
            images.Add((image, name + "." + extension));
            images.Add((thumb, name + "-thumb." + extension));

            return images;
        }

        [NonAction]
        private void SaveImages(IEnumerable<(Image image, string fileName)> images, string directory,
            ImageFormat imageFormat, long imageQuality)
        {
            ImageCodecInfo imageCodecInfo = ImageCodecInfo.GetImageDecoders()
                .SingleOrDefault(c => c.FormatID == imageFormat.Guid);

            EncoderParameters encoderParameters = new EncoderParameters();
            encoderParameters.Param = new[]
            {
                new EncoderParameter(Encoder.Quality, imageQuality),
            };

            foreach (var image in images)
            {
                image.image.Save(directory + image.fileName, imageCodecInfo, encoderParameters);
            }
        }

        [NonAction]
        private Image GenerateThumbnail(Image image)
        {
            const int thumbWidth = 512;
            const int thumbHeight = 512;

            double scale = Math.Max(thumbWidth / (double)image.Width, thumbHeight / (double)image.Height);
            int newWidth = (int)Math.Round(image.Width * scale);
            int newHeight = (int)Math.Round(image.Height * scale);

            int cropX = -(int)Math.Ceiling((newWidth - thumbWidth) / 2D);
            int cropY = -(int)Math.Ceiling((newHeight - thumbHeight) / 2D);

            Bitmap thumbImage = new Bitmap(thumbWidth, thumbHeight);

            using (Graphics graphics = Graphics.FromImage(thumbImage))
            {
                graphics.CompositingQuality = CompositingQuality.HighSpeed;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.DrawImage(image, cropX, cropY, newWidth + 1, newHeight + 1);
            }

            return thumbImage;
        }
    }
}
