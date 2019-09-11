using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MroczekDotDev.Sfira.Data;
using Microsoft.AspNetCore.Mvc;

namespace MroczekDotDev.Sfira.Controllers
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
        public ViewResult Index()
        {
            return View("Home");
        }
    }
}
