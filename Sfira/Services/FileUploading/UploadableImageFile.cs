using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MroczekDotDev.Sfira.Services.FileUploading
{
    public class UploadableImageFile : UploadableFile
    {
        private readonly ImageFormat format;
        private readonly long quality;

        public IFormFile FormFile { get; set; }
        public string Directory { get; set; }

        public int MaxWidth { get; set; }
        public int MaxHeight { get; set; }

        public bool HasThumbnail { get; set; }
        public int ThumbWidth { get; set; }
        public int ThumbHeight { get; set; }

        public UploadableImageFile(ImageFormat format, string extension, long quality) : base(extension)
        {
            this.quality = quality;
            this.format = format;
        }

        public override async Task Upload()
        {
            using (var memoryStream = new MemoryStream())
            {
                await FormFile.CopyToAsync(memoryStream);
                Image image = Image.FromStream(memoryStream);

                if (image.Width > MaxWidth || image.Height > MaxHeight)
                {
                    double scale = Math.Min(MaxWidth / (double)image.Width, MaxHeight / (double)image.Height);
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
                images.Add((image, Name + "." + Extension));

                if (HasThumbnail)
                {
                    images.Add((GenerateThumbnail(image), Name + "-thumb." + Extension));
                }

                Save(images, Directory, format, quality);
            }
        }

        private void Save(
            IEnumerable<(Image image, string fileName)> images,
            string directory,
            ImageFormat format,
            long quality)
        {
            ImageCodecInfo imageCodecInfo = ImageCodecInfo.GetImageDecoders()
                .SingleOrDefault(c => c.FormatID == format.Guid);

            EncoderParameters encoderParameters = new EncoderParameters();
            encoderParameters.Param = new[]
            {
                new EncoderParameter(Encoder.Quality, quality),
            };

            System.IO.Directory.CreateDirectory(directory);

            foreach (var image in images)
            {
                image.image.Save(directory + image.fileName, imageCodecInfo, encoderParameters);
            }
        }

        private Image GenerateThumbnail(Image image)
        {
            double scale = Math.Max(ThumbWidth / (double)image.Width, ThumbHeight / (double)image.Height);
            int newWidth = (int)Math.Round(image.Width * scale);
            int newHeight = (int)Math.Round(image.Height * scale);

            int cropX = -(int)Math.Ceiling((newWidth - ThumbWidth) / 2D);
            int cropY = -(int)Math.Ceiling((newHeight - ThumbHeight) / 2D);

            Bitmap thumbnail = new Bitmap(ThumbWidth, ThumbHeight);

            using (Graphics graphics = Graphics.FromImage(thumbnail))
            {
                graphics.CompositingQuality = CompositingQuality.HighSpeed;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.DrawImage(image, cropX, cropY, newWidth + 1, newHeight + 1);
            }

            return thumbnail;
        }
    }
}
