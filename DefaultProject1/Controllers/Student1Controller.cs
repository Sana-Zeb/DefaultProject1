using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DefaultProject1.Models;
using Microsoft.AspNetCore.Mvc;

namespace DefaultProject1.Controllers
{
    public class Student1Controller : Controller
    {
        private ProjectContext ORM = null;

        public Student1Controller(ProjectContext ORM)
        {
            this.ORM = ORM;
        }

        [HttpGet]
        public IActionResult CreateStudent()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateStudent(Student S)
        {
            ORM.Add(S);

            ORM.SaveChanges();
            ViewBag.Message = "Registration Done Successfully";
            ModelState.Clear();
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}