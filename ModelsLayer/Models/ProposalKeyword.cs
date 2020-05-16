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
    [Table("ProposalKeywords", Schema = "Proposal")]
    public class ProposalKeyword : TEntity<Guid>
    {
        public string Title { get; set; }

        [Required]
        public bool isLatin { get; set; }

        public Guid ProposalID { get; set; }

        [ForeignKey("ProposalID")]
        public virtual Proposal Proposal { get; set; }
    }
}
