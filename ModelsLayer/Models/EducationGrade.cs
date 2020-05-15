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
    [Table("EducationGrades", Schema = "Proposal")]
    public class EducationGrade : TEntity<Guid>
    {
        [Required]
        public string Title { get; set; }

        public virtual ICollection<Professor> Professors { get; set; }
    }
}
