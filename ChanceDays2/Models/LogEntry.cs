using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChanceDays2.Models
{
    public class LogEntry
    {
        public virtual int LogEntryId { get; set; }
        public virtual User PostedBy { get; set; }
        public virtual DateTime PostedOn { get; set; }
        public virtual int ParentProject { get; set; }
        public virtual string Content { get; set; }
    }

    public class CreateLogEntry
    {
        public virtual string posterName { get; set; }
        public virtual int parentProject { get; set; }
        public virtual string content { get; set; }
    }
}