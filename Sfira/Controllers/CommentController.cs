using MroczekDotDev.Sfira.Data;
using MroczekDotDev.Sfira.Models;
using MroczekDotDev.Sfira.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<JsonResult> Create([FromBody] CommentViewModel comment, int postId)
        {
            if (ModelState.IsValid)
            {
                comment.Author = await userManager.FindByNameAsync(User.Identity.Name);
                comment.ParentId = postId;
                dataStorage.AddComment(comment);
            }
            else
            {
                return Json("error");
            }
            return Json("success");
        }

        public PartialViewResult GetCommentsByPostId(int postId)
        {
            IEnumerable<CommentViewModel> result = dataStorage.GetCommentsVmByPostId(postId);
            return PartialView("_CommentsPartial", result);
        }
    }
}
