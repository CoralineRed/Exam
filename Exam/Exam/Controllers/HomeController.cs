using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Exam.Models;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace Exam.Controllers
{
    public class HomeController : Controller
    {
        static SearchModel searchModel;

        public IActionResult Index() 
        {
            return View();
        }

        public IActionResult Search(string url, string depth)
        {
            var model = new SearchModel();
            model.GetLinks(url, int.Parse(depth));
            searchModel = model;
            return View(model);
        }

        public IActionResult Save()
        {
            Database.Save(searchModel.Results, searchModel.Domain);
            return View();
        }

        public IActionResult ShowResults(string domain)
        {
            var results = Database.SelectByDomain(domain);
            return View(results);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
