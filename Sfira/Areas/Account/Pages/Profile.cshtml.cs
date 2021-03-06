﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MroczekDotDev.Sfira.Models;
using MroczekDotDev.Sfira.Services.FileUploading;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace MroczekDotDev.Sfira.Areas.Account.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly IWebHostEnvironment env;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IFileUploader fileUploader;

        public ProfileModel(
            IWebHostEnvironment env,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IFileUploader fileUploader)
        {
            this.env = env;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.fileUploader = fileUploader;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public ProfileInputModel Input { get; set; }

        public class ProfileInputModel
        {
            [Required]
            [StringLength(24, ErrorMessage = "{0} length must be between {2} and {1} characters.", MinimumLength = 3)]
            [RegularExpression(@"^[ -~]+$", ErrorMessage = "Name can contain only basic characters.")]
            public string Name { get; set; }

            [StringLength(240, ErrorMessage = "{0} can't be longer than {1} characters.")]
            public string Description { get; set; }

            [StringLength(36, ErrorMessage = "{0} can't be longer than {1} characters.")]
            public string Location { get; set; }

            [StringLength(72, ErrorMessage = "URL can't be longer than {1} characters.")]
            [RegularExpression(
                @"^(?:https?://)?[A-Za-z0-9][A-Za-z0-9-]*[A-Za-z0-9]+\.[A-Za-z]{2,}[\w!#\$%&'\(\)\*\+,\-\./:;=?@\[\]~]*$",
                ErrorMessage = "Must be valid web address.")]
            public string Website { get; set; }

            [BindProperty]
            public IFormFile Avatar { get; set; }

            [BindProperty]
            public IFormFile Cover { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            var userName = await userManager.GetUserNameAsync(user);
            var name = user.Name;
            var description = user.Description;
            var location = user.Location;
            var website = user.Website;

            Username = userName;

            Input = new ProfileInputModel
            {
                Name = name,
                Description = description,
                Location = location,
                Website = website,
                Avatar = null,
                Cover = null,
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            if (Input.Name != user.Name)
            {
                user.Name = Input.Name;
            }

            if (Input.Description != user.Description)
            {
                user.Description = Input.Description;
            }

            if (Input.Location != user.Location)
            {
                user.Location = Input.Location;
            }

            if (Input.Website != user.Website)
            {
                user.Website = Input.Website;
            }

            if (HttpContext.Request.Form.Files.Count > 0)
            {
                string userMediaPath = Path.Combine(new[] {
                    env.WebRootPath, "media", "user", user.Id + Path.DirectorySeparatorChar });

                var files = new List<UploadableFile>();

                if (Input.Avatar != null && Input.Avatar.Length > 0)
                {
                    UploadableImageFile file = fileUploader.NewUploadableImageFile();
                    file.FormFile = Input.Avatar;
                    file.Directory = userMediaPath;
                    file.Name = Guid.NewGuid().ToString();
                    file.MaxWidth = 512;
                    file.MaxHeight = 512;

                    files.Add(file);

                    if (user.AvatarImage != null)
                    {
                        System.IO.File.Delete(userMediaPath + user.AvatarImage);
                    }

                    user.AvatarImage = file.Name + "." + file.Extension;
                }

                if (Input.Cover != null && Input.Cover.Length > 0)
                {
                    UploadableImageFile file = fileUploader.NewUploadableImageFile();
                    file.FormFile = Input.Cover;
                    file.Directory = userMediaPath;
                    file.Name = Guid.NewGuid().ToString();
                    file.MaxWidth = 1920;
                    file.MaxHeight = 1080;

                    files.Add(file);

                    if (user.CoverImage != null)
                    {
                        System.IO.File.Delete(userMediaPath + user.CoverImage);
                    }

                    user.CoverImage = file.Name + "." + file.Extension;
                }

                await fileUploader.Upload(files);
            }

            await userManager.UpdateAsync(user);
            await signInManager.RefreshSignInAsync(user);

            StatusMessage = "Your profile has been updated";

            return RedirectToPage();
        }
    }
}
