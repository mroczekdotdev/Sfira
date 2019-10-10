using Microsoft.AspNetCore.Mvc;
using MroczekDotDev.Sfira.Data;
using MroczekDotDev.Sfira.Infrastructure;
using MroczekDotDev.Sfira.ViewModels;
using System.Collections.Generic;

namespace MroczekDotDev.Sfira.Controllers
{
    public class TagController : Controller
    {
        private readonly IDataStorage dataStorage;

        public TagController(IDataStorage dataStorage)
        {
            this.dataStorage = dataStorage;
        }

        public IActionResult Index(string tagName)
        {
            IEnumerable<PostViewModel> result = dataStorage.GetPostsByTag(tagName).ToViewModels();
            return View("Tag", result);
        }
    }
}
