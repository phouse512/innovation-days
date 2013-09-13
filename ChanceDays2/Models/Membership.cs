using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChanceDays2.Models
{
    public class Membership
    {
        public virtual int MembershipId { get; set; }
        public virtual Project Project { get; set; }
        public virtual User Member { get; set; }
    }
}