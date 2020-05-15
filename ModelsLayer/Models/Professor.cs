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
    [Table("Professors", Schema = "Proposal")]
    public class Professor : TEntity<Guid>
    {
        [InverseProperty("FirstGuidingProfessor")]
        public virtual ICollection<Student> Students_FirstGuidingProfessor { get; set; }

        [InverseProperty("SecondGuidingProfessor")]
        public virtual ICollection<Student> Students_SecondGuidingProfessor { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        public string Password { get; set; }

        public DateTime DateOfBirth { get; set; }

        [Required]
        public string SocialSecurityNumber { get; set; }

        public string BirthCertificateNumber { get; set; }

        public string Email { get; set; }

        public string LatestDegree { get; set; }

        public string MainSpecialty { get; set; }

        [Required]
        public bool IsCouncilMember { get; set; }

        public string GoogleCalendarID { get; set; }

        public Guid UniversityRankID { get; set; }

        [ForeignKey("UniversityRankID")]
        public virtual UniversityRank UniversityRank { get; set; }

        public virtual ICollection<University> Universities_Manager { get; set; }

        [InverseProperty("Manager")]    
        public virtual ICollection<Faculty> Faculties_Manager { get; set; }

        [Required]
        public Guid FacultyID { get; set; }

        [ForeignKey("FacultyID")]
        public virtual Faculty Faculty { get; set; }

        [InverseProperty("Manager")]
        public virtual ICollection<EducationalGroup> EducationalGroups_Manager { get; set; }

        [Required]
        public Guid EducationalGroupID { get; set; }

        [InverseProperty("Professors")]
        [ForeignKey("EducationalGroupID")]
        public virtual EducationalGroup EducationalGroup { get; set; }
    }
}
