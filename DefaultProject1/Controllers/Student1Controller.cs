using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DefaultProject1.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DefaultProject1.Controllers
{
    public class Student1Controller : Controller
    {
        private ProjectContext ORM = null;

        IHostingEnvironment _ENV = null;

        public Student1Controller(ProjectContext ORM , IHostingEnvironment ENV)
        {
            this.ORM = ORM;
            _ENV = ENV;
        }

        [HttpGet]
        public IActionResult CreateStudent()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateStudent(Student S , IFormFile PP, IFormFile CVfile)
        {
            string wwwRootPath = _ENV.WebRootPath;
            string FTPPathForPP = wwwRootPath + "/WebData/PP/";

            string UniqueName = Guid.NewGuid().ToString();
            string FileExtension = Path.GetExtension(PP.FileName);

            FileStream FS = new FileStream(FTPPathForPP + UniqueName + FileExtension, FileMode.Create);

            PP.CopyTo(FS);
            FS.Close();

            S.ProfilePicture = "/WebData/PP/" + UniqueName + FileExtension;

            string CVPath = "/WebData/CV/" + Guid.NewGuid().ToString() + Path.GetExtension(CVfile.FileName);
            CVfile.CopyTo(new FileStream(wwwRootPath + CVPath, FileMode.Create));
            S.Cv = CVPath;

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


        public FileResult DownloadCV(string Path)
        {
            if (string.IsNullOrEmpty(Path))
            {
                ViewBag.Message = "Invalid Path";
                return null;
            }
            return File(Path, new MimeSharp.Mime().Lookup(Path), DateTime.Now.ToString("ddMMyyyyhhmmss") + System.IO.Path.GetExtension(Path));
        }





    }
}