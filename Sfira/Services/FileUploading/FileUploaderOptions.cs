using MroczekDotDev.Sfira.Models;
using System.Drawing.Imaging;

namespace MroczekDotDev.Sfira.Services.FileUploading
{
    public class FileUploaderOptions
    {
        public string ImageFileExentsion { get; } = FilenameExtension.jpg.ToString();
        public long ImageQuality { get; set; } = 40;
        public ImageFormat Format { get; } = ImageFormat.Jpeg;
    }
}
