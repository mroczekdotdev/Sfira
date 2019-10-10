using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MroczekDotDev.Sfira.Data;
using MroczekDotDev.Sfira.Infrastructure;
using MroczekDotDev.Sfira.Models;
using MroczekDotDev.Sfira.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MroczekDotDev.Sfira.Controllers
{
    public class CommentController : Controller
    {
        private readonly IDataStorage dataStorage;
        private readonly UserManager<ApplicationUser> userManager;

        public CommentController(IDataStorage dataStorage, UserManager<ApplicationUser> userManager)
        {
            this.dataStorage = dataStorage;
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
                dataStorage.AddComment(comment);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        public PartialViewResult Comments(int postId)
        {
            IEnumerable<CommentViewModel> result = dataStorage.GetCommentsByPostId(postId).ToViewModels();
            return PartialView("_CommentsPartial", result);
        }
    }
}
