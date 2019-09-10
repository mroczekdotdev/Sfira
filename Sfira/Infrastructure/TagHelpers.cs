using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.IO;

namespace MarcinMroczek.Sfira.Infrastructure.TagHelpers
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
            string filePath = @"\media\users\" + UserId + @"\" + Media + ".jpg";

            if (!File.Exists(environment.WebRootPath + filePath))
            {
                filePath = "/media/site/default-" + Media + ".png";
            }
            else
            {
                filePath = "/media/users/" + UserId + "/" + Media + ".jpg";
            }

            output.Attributes.SetAttribute("style", "background: url(" + filePath + ") center / cover no-repeat");
        }
    }
}
