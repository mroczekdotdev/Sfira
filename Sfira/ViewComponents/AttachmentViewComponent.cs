using Microsoft.AspNetCore.Mvc;
using MroczekDotDev.Sfira.Models;
using MroczekDotDev.Sfira.ViewModels;
using System;
using System.Threading.Tasks;

namespace MroczekDotDev.Sfira.ViewComponents
{
    public class AttachmentViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(AttachmentViewModel attachment)
        {
            string view;

            switch (Enum.Parse<FileType>(attachment.Type, true))
            {
                case FileType.image:
                    view = "ImageAttachment";
                    break;

                default:
                    return Content(string.Empty);
            }

            return View(view, attachment);
        }
    }
}
