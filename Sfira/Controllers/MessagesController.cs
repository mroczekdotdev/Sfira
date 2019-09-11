using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MroczekDotDev.Sfira.Data;
using Microsoft.AspNetCore.Mvc;

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
