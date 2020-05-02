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
    [Table("EducationalGroups", Schema = "Proposal")]
    public class EducationalGroup : TEntity<Guid>
    {
        [Required]
        public string Name { get; set; }

        public Guid FacultyID { get; set; }

        [ForeignKey("FacultyID")]
        public virtual Faculty Faculty { get; set; }

        [Required]
        public Guid ManagerID { get; set; }

        [ForeignKey("ManagerID")]
        public virtual Professor Manager { get; set; }

        [InverseProperty("EducationalGroup")]
        public virtual ICollection<Professor> Professors { get; set; }

    }
}
