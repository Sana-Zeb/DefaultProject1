using System;
using System.Collections.Generic;

namespace DefaultProject1.Models
{
    public partial class LoginUser
    {
        public int Id { get; set; }
        public string FName { get; set; }
        public string LName{ get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string DOB{ get; set; }
        public string country { get; set; }
    }
}
