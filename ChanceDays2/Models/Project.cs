using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChanceDays2.Models
{
    public class Project
    {
        public virtual int ProjectId { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual ProjectVersion CurrentVersion { get; set; }
        public virtual DateTime CreatedOn { get; set; }
    }

    public class ProjectCreateView
    {
        public virtual string creatorName { get; set; }
        public virtual string PitchUrl { get; set; }
        public virtual string ProblemDescription { get; set; }
        public virtual string ProposedSolution { get; set; }
        public virtual string ProjectScope { get; set; }
        public virtual string ProjectName { get; set; }
        public virtual string ChanceDay { get; set; }
    }

    public class ProjectEditView
    {
        public virtual int ProjectId { get; set; }
        public virtual string editorName { get; set; }
        public virtual string PitchUrl { get; set; }
        public virtual string ProblemDescription { get; set; }
        public virtual string ProposedSolution { get; set; }
        public virtual string ProjectScope { get; set; }
        public virtual string ProjectName { get; set; }
        public virtual string ChanceDay { get; set; }
        public virtual string ProcessStatus { get; set; }
        public virtual string FeedbackStatus { get; set; }
    }
}