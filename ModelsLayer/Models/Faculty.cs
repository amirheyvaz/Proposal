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
    [Table("Faculties", Schema = "Proposal")]
    public class Faculty : TEntity<Guid>
    {
        [Required]
        public string Name { get; set; }

        public Guid? ManagerID { get; set; }

        [InverseProperty("Faculties_Manager")]
        [ForeignKey("ManagerID")]
        public virtual Professor Manager { get; set; }

        [Required]
        public Guid UniversityID { get; set; }

        [ForeignKey("UniversityID")]
        public virtual University University { get; set; }

        public virtual ICollection<Student> Students { get; set; }

        [InverseProperty("Faculty")]
        public virtual ICollection<Professor> Professors { get; set; }

        public virtual ICollection<EducationalGroup> EducationalGroups { get; set; }
    }
}
