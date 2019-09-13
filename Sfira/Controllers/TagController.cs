using MroczekDotDev.Sfira.Data;
using Microsoft.AspNetCore.Mvc;

namespace MroczekDotDev.Sfira.Controllers
{
    public class TagController : Controller
    {
        private readonly IDataStorage dataStorage;

        public TagController(IDataStorage dataStorage)
        {
            this.dataStorage = dataStorage;
        }

        public IActionResult GetPostsByTag(string id)
        {
            return View("Tag", dataStorage.GetPostsVmByTag(id));
        }
    }
}
