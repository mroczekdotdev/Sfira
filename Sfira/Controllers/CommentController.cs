using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MroczekDotDev.Sfira.Data;
using MroczekDotDev.Sfira.Models;
using MroczekDotDev.Sfira.ViewComponents;
using MroczekDotDev.Sfira.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MroczekDotDev.Sfira.Controllers
{
    public class CommentController : Controller
    {
        private readonly IRepository repository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly FeedOptions feedOptions;
        private readonly int commentsFeedCount;

        public static string Name { get; } = nameof(CommentController)
            .Substring(0, nameof(CommentController).LastIndexOf(nameof(Controller)));

        public CommentController(
            IRepository repository,
            UserManager<ApplicationUser> userManager,
            IOptionsMonitor<FeedOptions> feedOptionsAccessor)
        {
            this.repository = repository;
            this.userManager = userManager;
            feedOptions = feedOptionsAccessor.CurrentValue;
            commentsFeedCount = feedOptions.CommentsFeedCount;
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

        public IActionResult Comments(int postId, bool getCount)
        {
            IEnumerable<CommentViewModel> comments =
                repository.GetCommentsByPostId(postId, commentsFeedCount).ToViewModels();

            var commentsFeedLoader = new CommentsFeedLoaderViewModel
            {
                Comments = comments
            };

            if (comments.Any())
            {
                if (comments.Count() == commentsFeedCount)
                {
                    commentsFeedLoader.HasLoader = true;
                    commentsFeedLoader.LoaderLink = postId + "/CommentsFeed/";
                    commentsFeedLoader.LoaderCount = commentsFeedCount;
                    commentsFeedLoader.LoaderCursor = comments.Last().Id;
                }

                if (getCount)
                {
                    int commentsCount = repository.GetCommentsCountByPostId(postId);
                    Response.Headers.Add("Comments-Count", commentsCount.ToString());
                }
            }

            return PartialView("_CommentsFeedLoaderPartial", commentsFeedLoader);
        }

        public IActionResult CommentsFeed(int postId, int count, int cursor)
        {
            IEnumerable<CommentViewModel> comments = repository.GetCommentsByPostId(postId, count, cursor).ToViewModels();
            return ViewComponent(typeof(CommentsFeedViewComponent), comments);
        }
    }
}
