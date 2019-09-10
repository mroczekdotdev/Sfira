using MarcinMroczek.Sfira.Data;
using MarcinMroczek.Sfira.Models;
using MarcinMroczek.Sfira.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarcinMroczek.Sfira.ViewComponents
{
    public class AttachmentViewComponent : ViewComponent
    {
        private readonly IDataStorage dataStorage;
        private readonly UserManager<ApplicationUser> userManager;

        public AttachmentViewComponent(IDataStorage dataStorage, UserManager<ApplicationUser> userManager)
        {
            this.dataStorage = dataStorage;
            this.userManager = userManager;
        }

        public IViewComponentResult Invoke(AttachmentViewModel attachment)
        {
            string view;

            switch (Enum.Parse<AttachmentType>(attachment.Type))
            {
                case AttachmentType.image:
                    view = "ImageAttachment";
                    break;
                default:
                    return Content(string.Empty);
            }

            return View(view, attachment);
        }
    }
}
