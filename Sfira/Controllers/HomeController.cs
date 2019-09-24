﻿using Microsoft.AspNetCore.Mvc;
using MroczekDotDev.Sfira.Data;

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
