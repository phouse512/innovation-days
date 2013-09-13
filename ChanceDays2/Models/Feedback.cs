using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChanceDays2.Models
{
    public class Feedback
    {
        public virtual int FeedbackId { get; set; }
        public virtual Project owner { get; set; }
        public virtual string Comment { get; set; }
        public virtual User Poster { get; set; }
        public virtual DateTime insertDate { get; set; }
    }

    public class NewFeedback
    {
        public virtual int parentProject { get; set; }
        public virtual string poster { get; set; }
        public virtual string comment { get; set; }
    }
}