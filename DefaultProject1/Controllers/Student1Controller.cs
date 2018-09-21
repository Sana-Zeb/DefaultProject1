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

         public IActionResult StudentDetail(int Id)
          {
              Student S = ORM.Student.Where(m => m.Id == Id).FirstOrDefault<Student>();
              return View(S);
          }

       
        [HttpGet]
        public IActionResult EditStudent(int Id)
        {

            Student S = ORM.Student.Where(m => m.Id == Id).FirstOrDefault<Student>();
            return View(S);
        }
        [HttpPost]
        public IActionResult EditStudent(Student S)
        {
            ORM.Student.Update(S);
            ORM.SaveChanges();      
            return RedirectToAction("CreateAllStudent");
        }

        public IActionResult DeleteStudent(Student S)
        {
            ORM.Student.Remove(S);
            ORM.SaveChanges();
            return RedirectToAction("CreateAllStudent");
        }







    }
}