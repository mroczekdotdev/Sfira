using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarcinMroczek.Sfira.Data;
using Microsoft.AspNetCore.Mvc;

namespace MarcinMroczek.Sfira.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDataStorage dataStorage;

        public HomeController(IDataStorage dataStorage)
        {
            this.dataStorage = dataStorage;
        }

        [Route("")]
        [Route("Home")]
        public IActionResult Index()
        {
            return View("Home");
        }
    }
}
