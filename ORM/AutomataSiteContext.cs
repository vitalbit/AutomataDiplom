using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM
{
    public class AutomataSiteContext : DbContext
    {
        public AutomataSiteContext() : base("AutomataSite") 
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<AutomataSiteContext>());
        }

        public DbSet<UniversityInfo> UniversityInfos { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TestType> TestTypes { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Material> Materials { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
