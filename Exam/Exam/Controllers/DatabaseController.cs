using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exam.Models;
using Microsoft.AspNetCore.Mvc;

namespace Exam.Controllers
{
    public class DatabaseController : Controller
    {
        public IActionResult Save(SearchModel model)
        {
            Database.Save(model.Results, model.Domain);
            return View();
        }
    }
}