using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MroczekDotDev.Sfira.Data;
using MroczekDotDev.Sfira.Models;
using MroczekDotDev.Sfira.ViewModels;
using System;

namespace MroczekDotDev.Sfira.ViewComponents
{
    public class AttachmentViewComponent : ViewComponent
    {
        private readonly IRepository repository;
        private readonly UserManager<ApplicationUser> userManager;

        public AttachmentViewComponent(IRepository repository, UserManager<ApplicationUser> userManager)
        {
            this.repository = repository;
            this.userManager = userManager;
        }

        public IViewComponentResult Invoke(AttachmentViewModel attachment)
        {
            string view;

            switch (Enum.Parse<FileType>(attachment.Type))
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
