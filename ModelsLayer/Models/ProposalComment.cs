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
    [Table("ProposalComments", Schema = "Proposal")]
    public class ProposalComment : TEntity<Guid>
    {
        [Required]
        public Guid ProposalID { get; set; }

        [ForeignKey("ProposalID")]
        public virtual Proposal Proposal { get; set; }

        public DateTime OccuranceDate { get; set; }

        public Guid? OccuredByStudentID { get; set; }

        [ForeignKey("OccuredByStudentID")]
        public virtual Student OccuredByStudent { get; set; }

        public Guid? OccuredByProfessorID { get; set; }

        [ForeignKey("OccuredByProfessorID")]
        public virtual Professor OccuredByProfessor { get; set; }

        [Required]
        public Guid ProposalStageID { get; set; }

        [ForeignKey("ProposalStageID")]
        public virtual ProposalStage ProposalStage { get; set; }

        public int? ImportanceLevel { get; set; }
    }
}
