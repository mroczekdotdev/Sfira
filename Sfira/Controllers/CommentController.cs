using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MroczekDotDev.Sfira.Data;
using MroczekDotDev.Sfira.Models;
using MroczekDotDev.Sfira.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MroczekDotDev.Sfira.Controllers
{
    public class CommentController : Controller
    {
        private readonly IRepository repository;
        private readonly UserManager<ApplicationUser> userManager;

        public static string Name { get; } = nameof(CommentController)
            .Substring(0, nameof(CommentController).LastIndexOf(nameof(Controller)));

        public CommentController(IRepository repository, UserManager<ApplicationUser> userManager)
        {
            this.repository = repository;
            this.userManager = userManager;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CommentViewModel comment, int postId)
        {
            if (ModelState.IsValid)
            {
                comment.Author = await userManager.FindByNameAsync(User.Identity.Name);
                comment.ParentId = postId;
                repository.AddComment(comment);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        public PartialViewResult Feed(int postId)
        {
            IEnumerable<CommentViewModel> comments = repository.GetCommentsByPostId(postId).ToViewModels();

            return PartialView("_CommentsFeedPartial", comments);
        }
    }
}
