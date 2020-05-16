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
    [Table("ProposalOperations", Schema = "Proposal")]
    public class ProposalOperation : TEntity<Guid>
    {
        public string Title { get; set; }

        public virtual ICollection<ProposalWorkflowHistory> ProposalWorkflowHistories { get; set; }
    }
}
