using System.Threading.Tasks;

namespace MroczekDotDev.Sfira.Services.FileUploading
{
    public abstract class UploadableFile
    {
        public string Name { get; set; }
        public readonly string Extension;

        public abstract Task Upload();

        public UploadableFile(string extension)
        {
            Extension = extension;
        }
    }
}
