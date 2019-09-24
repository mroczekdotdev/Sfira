using Microsoft.AspNetCore.Mvc;
using MroczekDotDev.Sfira.Data;

namespace MroczekDotDev.Sfira.Controllers
{
    public class MessagesController : Controller
    {
        private readonly IDataStorage dataStorage;

        public MessagesController(IDataStorage dataStorage)
        {
            this.dataStorage = dataStorage;
        }

        public IActionResult Index()
        {
            return View("Messages");
        }
    }
}
