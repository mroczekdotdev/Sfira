using Microsoft.AspNetCore.Mvc;
using MroczekDotDev.Sfira.Data;
using MroczekDotDev.Sfira.Infrastructure;

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
            return View("Tag", dataStorage.GetPostsByTag(tagName).ToViewModels());
        }
    }
}
