using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InfrastructureLayer.AbstractModels;
namespace ModelsLayer.Models
{
    [Table("ResearchTypes", Schema = "Proposal")]
    public class ResearchType : TEntity<Guid>
    {
        public string Title { get; set; }

        public virtual ICollection<Student> Students { get; set; }
    }
}
