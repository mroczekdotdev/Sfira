using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MroczekDotDev.Sfira.Models;
using System.IO;

namespace MroczekDotDev.Sfira.TagHelpers
{
    [HtmlTargetElement("div", Attributes = "sfira-user, sfira-media")]
    public class UserMediaTagHelper : TagHelper
    {
        [HtmlAttributeName("sfira-user")]
        public IHasUserMedia User { get; set; }

        [HtmlAttributeName("sfira-media")]
        public string Media { get; set; }

        private readonly IHostingEnvironment environment;

        public UserMediaTagHelper(IHostingEnvironment environment)
        {
            this.environment = environment;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string filePath;

            switch (Media)
            {
                case "avatar":
                    if (User.AvatarImage != null
                        && File.Exists(Path.Combine(new[] {
                            environment.WebRootPath, "media", "user", User.Id, User.AvatarImage })))
                    {
                        filePath = "/media/user/" + User.Id + "/" + User.AvatarImage;
                        break;
                    }
                    else
                    {
                        goto default;
                    }
                case "cover":
                    if (User.CoverImage != null
                        && File.Exists(Path.Combine(new[] {
                            environment.WebRootPath, "media", "user", User.Id, User.CoverImage })))
                    {
                        filePath = "/media/user/" + User.Id + "/" + User.CoverImage;
                        break;
                    }
                    else
                    {
                        goto default;
                    }
                default:
                    filePath = "/media/site/default-" + Media + ".png";
                    break;
            }

            output.Attributes.SetAttribute("style", "background: url(" + filePath + ") center / cover no-repeat");
        }
    }
}
