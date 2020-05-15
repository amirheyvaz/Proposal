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
    [Table("Universities", Schema = "Proposal")]
    public class University : TEntity<Guid>
    {
        [Required]
        public string Name { get; set; }

        public Guid? ManagerID { get; set; }

        [ForeignKey("ManagerID")]
        public virtual Professor Manager { get; set; }

        public virtual ICollection<Faculty> Faculties { get; set; }
    }
}
