using InfrastructureLayer.AbstractModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLayer.Models
{
    [Table("PeriodTypes", Schema = "Proposal")]
    public class PeriodType : TEntity<Guid>
    {
        [Required]
        public string Title { get; set; }

        public virtual ICollection<Student> Students { get; set; }
    }
}
