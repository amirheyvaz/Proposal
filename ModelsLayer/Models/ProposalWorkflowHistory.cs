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
    [Table("ProposalWorkflowHistories", Schema = "Proposal")]
    public class ProposalWorkflowHistory : TEntity<Guid>
    {
        public Guid ProposalID { get; set; }

        [ForeignKey("ProposalID")]
        public virtual Proposal Proposal { get; set; }

        public Guid ProposalOperationID { get; set; }

        [ForeignKey("ProposalOperationID")]
        public virtual ProposalOperation ProposalOperation { get; set; }

        public DateTime OccuranceDate { get; set; }

        public Guid? OccuredByProfessorID { get; set; }

        [ForeignKey("OccuredByProfessorID")]
        public virtual Professor OccuredByProfessor { get; set; }

        public Guid? OccuredByStudentID { get; set; }

        [ForeignKey("OccuredByStudentID")]
        public virtual Student OccuredByStudent { get; set; }
    }
}
