using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChanceDays2.Models
{
    public class PaginatedProjectSearchResults
    {
        public virtual int totalPages { get; set; }
        public virtual int currentPage { get; set; }
        public virtual List<ProjectView> projectResults { get; set; }
    }
}