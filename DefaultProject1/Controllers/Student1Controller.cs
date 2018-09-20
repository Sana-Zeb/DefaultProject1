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
        /*
        [HttpGet]
          public IActionResult CreateAllStudent()
          {
              return View();

          }

          [HttpPost]
          public IActionResult CreateAllStudent(Student S)
          {
              IList<Student> CreateAllStudent = ORM.Student.ToList<Student>();

              return View(CreateAllStudent);
          }
          */

        public IActionResult CreateAllStudent()
        {
            return View(ORM.Student.ToList<Student>());
        }
       
        public IActionResult Index()
        {
            return View();
        }

         public IActionResult StudentDetail(int ID)
          {
              Student S = ORM.Student.Where(m => m.Id == ID).FirstOrDefault<Student>();
              return View(S);
          }


       
    }
}