using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChanceDays2.Models
{
    public class ProjectVersion
    {
        public virtual int ProjectVersionId { get; set; }
        public virtual DateTime EditedOn { get; set; }
        public virtual User EditedBy { get; set; }
        public virtual string ProjectName { get; set; }
        public virtual string ChanceDay { get; set; }
        public virtual string ProcessStatus { get; set; }
        public virtual string FeedbackStatus { get; set; }
        public virtual User Owner { get; set; }
        public virtual User Presenter { get; set; }
        public virtual User Lead { get; set; }
        public virtual string PitchUrl { get; set;} 
        public virtual string ProblemDescription { get; set; }
        public virtual string ProposedSolution { get; set; }
        public virtual string ProjectScope { get; set; }
        public virtual string ChangeDescription { get; set; }
        public virtual int parentId { get; set; }
    }

    public class PaginatedEditHistory
    {
        public virtual int ParentId { get; set; }
        public virtual int currentPage { get; set; }
        public virtual int totalPages { get; set; }
        public virtual List<ProjectVersion> results { get; set; }
    }
}