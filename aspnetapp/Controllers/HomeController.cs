using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using aspnetapp.Models;
using MySql.Data.MySqlClient;  
using aspnetapp.Utils;
using aspnetapp.Dao;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace aspnetapp.Controllers
{
    public class HomeController : Controller
    {
        public string title = ".Netcore Demo For Rainbond";
        public IActionResult Index()
        {
            ViewData["Title"] = title;
            return View();
        }

        [Route("mysqldemo")]
        public IActionResult MysqlDemo()
        {
            ViewData["Title"] = title;
            ViewData["MYSQL_HOST"] = Environment.GetEnvironmentVariable("MYSQL_HOST");
            ViewData["MYSQL_PORT"] = Environment.GetEnvironmentVariable("MYSQL_PORT");
            ViewData["MYSQL_USER"] = Environment.GetEnvironmentVariable("MYSQL_USER");
            ViewData["MYSQL_PASSWORD"] = Environment.GetEnvironmentVariable("MYSQL_PASSWORD");
            ViewData["MYSQL_DATABASE"] = Environment.GetEnvironmentVariable("MYSQL_DATABASE");
            return View();
        }
        [Route("apidemo")]
        public IActionResult APIDemo()
        {
            ViewData["Title"] = title;
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
