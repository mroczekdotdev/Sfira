using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MroczekDotDev.Sfira.Services.FileUploading
{
    public class FileUploader : IFileUploader
    {
        private readonly FileUploaderOptions options;

        public FileUploader(IOptionsMonitor<FileUploaderOptions> optionsAccessor)
        {
            options = optionsAccessor.CurrentValue;
        }

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

        public UploadableImageFile NewUploadableImageFile()
        {
            return new UploadableImageFile(options.Format, options.ImageFileExentsion, options.ImageQuality);
        }
    }
}
