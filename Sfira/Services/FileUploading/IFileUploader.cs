using System.Collections.Generic;
using System.Threading.Tasks;

namespace MroczekDotDev.Sfira.Services.FileUploading
{
    public interface IFileUploader
    {
        Task Upload(UploadableFile file);

        Task Upload(IEnumerable<UploadableFile> files);

        UploadableImageFile NewUploadableImageFile();
    }
}
