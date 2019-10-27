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
        public ImageFormat format { get; set; }
        public long quality { get; set; }
        public int maxWidth { get; set; }
        public int maxHeight { get; set; }

        public bool hasThumbnail { get; set; }
        public int thumbWidth { get; set; }
        public int thumbHeight { get; set; }

        public override async Task Upload()
        {
            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);
                Image image = Image.FromStream(memoryStream);

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

                if (hasThumbnail)
                {
                    images.Add((GenerateThumbnail(image), name + "-thumb." + extension));
                }

                Save(images, directory, format, quality);
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

            Directory.CreateDirectory(directory);

            foreach (var image in images)
            {
                image.image.Save(directory + image.fileName, imageCodecInfo, encoderParameters);
            }
        }

        private Image GenerateThumbnail(Image image)
        {
            double scale = Math.Max(thumbWidth / (double)image.Width, thumbHeight / (double)image.Height);
            int newWidth = (int)Math.Round(image.Width * scale);
            int newHeight = (int)Math.Round(image.Height * scale);

            int cropX = -(int)Math.Ceiling((newWidth - thumbWidth) / 2D);
            int cropY = -(int)Math.Ceiling((newHeight - thumbHeight) / 2D);

            Bitmap thumbnail = new Bitmap(thumbWidth, thumbHeight);

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
