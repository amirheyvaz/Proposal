using InfrastructureLayer.Interfaces;
using ModelsLayer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ModelsLayer.DataContext
{
    public class DataContext : DbContext, IUnitOfWork
    {
        
        //DB SETS
        public DbSet<Student> Students { set; get; }
        public DbSet<PeriodType> PeriodTypes { set; get; }
        public DbSet<EducationGrade> EducationGrades { set; get; }
        public DbSet<Professor> Professors { set; get; }
        public DbSet<UniversityRank> UniversityRanks { set; get; }
        public DbSet<University> Universities { set; get; }
        public DbSet<Faculty> Faculties { set; get; }
        public DbSet<EducationalGroup> EducationalGroups { set; get; }

        //

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing); //فقط تعريف شده تا يك برك پوينت در اينجا قرار داده شود براي آزمايش فراخواني آن
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the current object
        /// </summary>
        public new void Dispose()
        {
            Dispose(true);
        }



        public DataContext()
            : base("ProposalConnectionString")
        {
            Configuration.LazyLoadingEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Faculty>()
                .HasRequired<Professor>(f => f.Manager)
                .WithMany(g => g.Faculties_Manager)
                .HasForeignKey<Guid>(a => a.ManagerID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Professor>()
                .HasRequired<Faculty>(p => p.Faculty)
                .WithMany(q => q.Professors)
                .HasForeignKey<Guid>(w => w.FacultyID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Professor>()
                .HasRequired<EducationalGroup>(e => e.EducationalGroup)
                .WithMany(w => w.Professors)
                .HasForeignKey<Guid>(q => q.EducationalGroupID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EducationalGroup>()
                .HasRequired<Professor>(p => p.Manager)
                .WithMany(r => r.EducationalGroups_Manager)
                .HasForeignKey<Guid>(q => q.ManagerID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Student>()
                .HasRequired<Professor>(s => s.FirstGuidingProfessor)
                .WithMany(d => d.Students_FirstGuidingProfessor)
                .HasForeignKey<Guid>(f => f.FirstGuidingProfessorID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Student>()
                .HasRequired<Professor>(s => s.SecondGuidingProfessor)
                .WithMany(d => d.Students_SecondGuidingProfessor)
                .HasForeignKey<Guid>(f => f.SecondGuidingProfessorID)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
