using InfrastructureLayer.AbstractModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsLayer.Models
{
    [Table("ProposalFiles" , Schema = "Proposal")]
    public class ProposalFile : TEntity<Guid>
    {
        public byte[] File { get; set; }

        public Guid ProposalID { get; set; }

        [ForeignKey("ProposalID")]
        public virtual Proposal Proposal { get; set; }
    }
}
