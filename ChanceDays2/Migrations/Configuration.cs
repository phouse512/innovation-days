namespace ChanceDays2.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using ChanceDays2.Models;
    

    internal sealed class Configuration : DbMigrationsConfiguration<ChanceDays2.Models.ProjectDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ChanceDays2.Models.ProjectDBContext context)
        {
            context.Users.AddOrUpdate(d => d.FirstName,
                new User() { FirstName = "Phil", LastName = "House", Email = "philiphouse2015@u.northwestern.edu", Department = "PD", Title = "Software Engineer Intern", Office = "Solon", Username = "MRISOFTWARE\\PHouse" },
                new User() { FirstName = "Bob", LastName = "Schmoe", Email = "bobschmoe@mrisoftware.com", Department = "PD", Title = "Senior Software Engineer", Office = "Solon", Username = "hi" },
                new User() { FirstName = "Joe", LastName = "Mansion", Email = "joemansion@mrisoftware.com", Department = "PM", Title = "Project Manager", Office = "Solon", Username = "oh" }
            );


          
        }
    }
}
