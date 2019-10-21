using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MroczekDotDev.Sfira.Services
{
    public abstract class UploadableFile
    {
        public IFormFile formFile { get; set; }
        public string directory { get; set; }
        public string name { get; set; }
        public string extension { get; set; }

        public abstract Task Upload();
    }
}
