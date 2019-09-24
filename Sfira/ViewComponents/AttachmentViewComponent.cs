﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MroczekDotDev.Sfira.Data;
using MroczekDotDev.Sfira.Models;
using MroczekDotDev.Sfira.ViewModels;
using System;

namespace MroczekDotDev.Sfira.ViewComponents
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
