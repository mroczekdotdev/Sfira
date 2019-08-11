using MarcinMroczek.Sfira.Data;
using Microsoft.AspNetCore.Mvc;

namespace MarcinMroczek.Sfira.Controllers
{
    public class TagController : Controller
    {
        private readonly IDataStorage dataStorage;

        public TagController(IDataStorage dataStorage)
        {
            this.dataStorage = dataStorage;
        }

        public IActionResult Index(string id)
        {
            return View("Tag", dataStorage.GetPostsByTag(id));
        }
    }
}
