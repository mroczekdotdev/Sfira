using System.Collections.Generic;
using System.Threading.Tasks;

namespace MroczekDotDev.Sfira.Services.FileUploading
{
    public class FileUploader : IFileUploader
    {
        public async Task Upload(UploadableFile file)
        {
            await file.Upload();
        }

        public async Task Upload(IEnumerable<UploadableFile> files)
        {
            foreach (UploadableFile file in files)
            {
                await file.Upload();
            }
        }
    }
}
