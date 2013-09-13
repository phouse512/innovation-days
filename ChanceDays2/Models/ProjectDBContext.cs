using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ChanceDays2.Models;


namespace ChanceDays2.Models
{
    public class ProjectDBContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<ProjectVersion> ProjectVersions { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<LogEntry> LogEntries { get; set; }
        //public DbSet<SearchCriteria> Searches { get; set; }
    }
}