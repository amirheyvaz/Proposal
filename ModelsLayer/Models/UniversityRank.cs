using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using InfrastructureLayer.AbstractModels;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace ModelsLayer.Models
{
    [Table("UniversityRanks", Schema = "Proposal")]
    public class UniversityRank : TEntity<Guid>
    {
        [Required]
        public string Title { get; set; }

        public virtual ICollection<Professor> Professors { get; set; }
    }
}
