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
    [Table("ProposalStages", Schema = "Proposal")]
    public class ProposalStage : TEntity<Guid>
    {
        public string Title { get; set; }

        public int Order { get; set; }

        public bool IsFirst { get; set; }

        public bool IsLast { get; set; }

        public bool ApproveType { get; set; }

        public virtual ICollection<Proposal> Proposals { get; set; }

        public virtual ICollection<ProposalComment> ProposalComments { get; set; }
    }
}
