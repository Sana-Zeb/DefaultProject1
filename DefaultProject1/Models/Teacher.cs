using System;
using System.Collections.Generic;

namespace DefaultProject1.Models
{
    public partial class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }
        public string ProfilePicture { get; set; }
        public string Cv { get; set; }
    }
}
