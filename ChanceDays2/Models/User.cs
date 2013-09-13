using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChanceDays2.Models
{
    public class User
    {
        public virtual int UserId { get; set; }
        public virtual string Username { get; set; }
        public virtual string Email { get; set; }
        public virtual string Bio { get; set; }
        public virtual string Department { get; set; }
        public virtual string Title { get; set; }
        public virtual string SIPaddress { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Workphone { get; set; }
        public virtual string Office { get; set; }
        public virtual string Website { get; set; }
        public virtual string Mobilephone { get; set; }
    }
}