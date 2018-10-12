using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
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
        public IActionResult CreateStudent(Student S ,  IFormFile CVfile)
        {
            String CVPath = _ENV.WebRootPath + "/WebData/CV/";
            String CVName = Guid.NewGuid().ToString();
            String CVExtension = Path.GetExtension(CVfile.FileName);

            FileStream FS = new FileStream(CVPath + CVName + CVExtension, FileMode.Create);
            CVfile.CopyTo(FS);
            FS.Close();

            S.Cv = "/WebData/CV/" + CVName + CVExtension;



            //send Email

            MailMessage sEmail = new MailMessage();
            sEmail.From = new MailAddress("sanazeb110@gmail.com");
            sEmail.To.Add(new MailAddress(S.Email));
            sEmail.CC.Add(new MailAddress("sanazeb110@gmail.com"));
            sEmail.Subject = "Welcome to student registration form";
            sEmail.Body = "Respected " + S.Name + ",<br><br>" +
                "Thanks for registering with student registration form,we welcome you to our institution" +
                "<br><br>" +
                "<b>Regards</b>,<br>xyz Team";
            sEmail.IsBodyHtml = true;
             if (!string.IsNullOrEmpty(S.Cv))
             {
                 sEmail.Attachments .Add(new Attachment(_ENV.WebRootPath + S.Cv));
             }

            //smtp 
            SmtpClient oSMTP = new SmtpClient();
            oSMTP.Host = "smtp.gmail.com";
            oSMTP.Port = 587;
            oSMTP.EnableSsl = true;
            oSMTP.Credentials = new System.Net.NetworkCredential("sanazeb110@gmail.com", "sanazeb789");

            try
            {
                oSMTP.Send(sEmail);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Mail has sent successfully";
            }

            ORM.Add(S);
            ORM.SaveChanges();

             string APIURL = "http://bulksms.com.pk/api/sms.php?username=923478543050&password=4932&sender=BrandName&mobile=923177371912&message=registrationdone";
        
              using (var APIClient = new HttpClient())
              {
                  Task<HttpResponseMessage> RM = APIClient.GetAsync(APIURL);
                  Task<string> FinalRespone = RM.Result.Content.ReadAsStringAsync();
              }
            ViewBag.Message = "Registration Done Successfully";
            ModelState.Clear();
            return View();
        }
        
          [HttpGet]
        public IActionResult CreateAllStudent()
        {
            return View(ORM.Student.ToList<Student>());
        }
        [HttpPost]
        public IActionResult CreateAllStudent(string SearchByName, string SearchByRollNo, string SearchByDepartment)
        {
            IList<Student> CreateAllStudent = ORM.Student.Where(m => m.Name.Contains(SearchByName) || m.RollNo.Contains(SearchByRollNo) || m.Department.Contains(SearchByDepartment)).ToList<Student>();
            return View(CreateAllStudent);
           // return View(ORM.Student.ToList<Student>());
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

        public IActionResult DeleteStudent1(Student student)
        {
            ORM.Student.Remove(student);
            ORM.SaveChanges();
            return RedirectToAction(nameof(Student1Controller.CreateAllStudent));
        }
        public string deletestudentajax(Student S)
        {
            string result = "";
            try
            {
                ORM.Student.Remove(S);
                ORM.SaveChanges();
                result = "Yes";
            }
            catch (Exception ex)
            {
                result = "No";
            }
            return result;
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


       public string GetStudentsNames()
        {
            string Result = "";
            var r = Request;
            IList<Student> All = ORM.Student.ToList<Student>();
            Result += "<h1 class='alert alert-success'>Total Students: " + All.Count + "</h1>";
            
            foreach (Student S in All)
            {
                Result += "<a href='/Student1/StudentDetail?Id=" + S.Id + "'><p><span class='glyphicon glyphicon-user'></span> " + S.Name + "</p></a> <a href='/student1/Deletestudent?id=" + S.Id + "'>Delete</a>";
            }
            return Result;
     }

        public string ShowAd()
        {
            string Ad = "";
            Ad = "<img class='img img-responsive' src='http://lorempixel.com/400/200/sports/Dummy-Text/'/>";
            return Ad;
        }

        public string showstudentdetail()
        {
            string tb = "";
            var r = Request;
            IList<Student> All = ORM.Student.ToList<Student>();
            tb += "<h1 class='alert alert-success'>Total Students: " + All.Count + "</h1>";

            foreach (Student S in All)
            {
                tb += "<a href='/Student1/StudentDetail?Id=" + S.Id + "'><p><span class='glyphicon glyphicon-user'></span>" + S.Phone_no + " + " + S.Email + "</p></a> <a href='/student1/StudentDetail?id=" + S.Id + "'>Detail</a>";
            }

            return tb;
        }

    }
}