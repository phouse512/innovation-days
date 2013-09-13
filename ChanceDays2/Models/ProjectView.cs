using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChanceDays2.Models
{
    public class ProjectView
    {
        public virtual int ProjectId { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual DateTime CreatedOn { get; set; }
        public virtual int VersionId { get; set; }
        public virtual DateTime EditedOn { get; set; }
        public virtual User EditedBy { get; set; }
        public virtual string ProjectName { get; set; }
        public virtual string ChanceDay { get; set; }
        public virtual string ProcessStatus { get; set; }
        public virtual string FeedbackStatus { get; set; }
        public virtual User Owner { get; set; }
        public virtual User Presenter { get; set; }
        public virtual User Lead { get; set; }
        public virtual string PitchUrl { get; set; }
        public virtual string ProblemDescription { get; set; }
        public virtual string ProposedSolution { get; set; }
        public virtual string ProjectScope { get; set; }
        public virtual ICollection<User> Members { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
    }
}