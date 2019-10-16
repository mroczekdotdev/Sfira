using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.IO;

namespace MroczekDotDev.Sfira.TagHelpers
{
    [HtmlTargetElement("div", Attributes = "sfira-id, sfira-media")]
    public class UserMediaTagHelper : TagHelper
    {
        [HtmlAttributeName("sfira-id")]
        public string UserId { get; set; }

        [HtmlAttributeName("sfira-media")]
        public string Media { get; set; }

        private readonly IHostingEnvironment environment;

        public UserMediaTagHelper(IHostingEnvironment environment)
        {
            this.environment = environment;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string filePath = Path.Combine(new[] { "media", "user", UserId, Media + ".jpg" });

            if (!File.Exists(Path.Combine(environment.WebRootPath, filePath)))
            {
                filePath = "/media/site/default-" + Media + ".png";
            }
            else
            {
                filePath = "/media/user/" + UserId + "/" + Media + ".jpg";
            }

            output.Attributes.SetAttribute("style", "background: url(" + filePath + ") center / cover no-repeat");
        }
    }
}
