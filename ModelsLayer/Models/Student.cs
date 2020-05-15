using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using InfrastructureLayer.AbstractModels;
using System.ComponentModel.DataAnnotations;

namespace ModelsLayer.Models
{
    [Table("Students", Schema = "Proposal")]
    public class Student : TEntity<Guid>
    {
        [Required]
        public string StudentNumber { get; set; }

        [Required]
        public string Password { get; set; }

        public int EnteranceYear { get; set; }

        public string Major { get; set; }

        public string Orientation { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        [Required]
        public string SocialSecurityNumber { get; set; }

        public string BirthCertificateNumber { get; set; }

        public string Email { get; set; }

        public Guid PeriodTypeID { get; set; }

        [ForeignKey("PeriodTypeID")]
        public virtual PeriodType PeriodType { get; set; }

        [Required]
        public Guid FirstGuidingProfessorID { get; set; }

        [ForeignKey("FirstGuidingProfessorID")]
        public virtual Professor FirstGuidingProfessor { get; set; }

        [Required]
        public Guid SecondGuidingProfessorID { get; set; }

        [ForeignKey("SecondGuidingProfessorID")]
        public virtual Professor SecondGuidingProfessor { get; set; }

        public Guid FacultyID { get; set; }

        [ForeignKey("FacultyID")]
        public virtual Faculty Faculty { get; set; }

    }
}
