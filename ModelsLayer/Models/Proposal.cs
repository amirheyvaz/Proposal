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
    [Table("Proposals" , Schema = "Proposal")]
    public class Proposal : TEntity<Guid>
    {
        public string Name { get; set; }

        public DateTime CreateDate { get; set; }

        [Required]
        public Guid StudentID { get; set; }

        [ForeignKey("StudentID")]
        public virtual Student Student { get; set; }

        public string LatinName { get; set; }

        public Guid ResearchTypeID { get; set; }

        [ForeignKey("ResearchTypeID")]
        public virtual ResearchType ResearchType { get; set; }

        public Guid? FirstJudgeID { get; set; }

        [ForeignKey("FirstJudgeID")]
        public virtual Professor FirstJudge { get; set; }

        public Guid? SecondJudgeID { get; set; }

        [ForeignKey("SecondJudgeID")]
        public virtual Professor SecondJudge { get; set; }

        public Guid ProposalStageID { get; set; }

        [ForeignKey("ProposalStageID")]
        public virtual ProposalStage ProposalStage { get; set; }

        public string LatestOperation { get; set; }

        public Guid ProposalStatusID { get; set; }

        [ForeignKey("ProposalStatusID")]
        public virtual ProposalStatus ProposalStatus { get; set; }

        [Required]
        public bool IsFinalApprove { get; set; }

        public DateTime? DefenceMeetingTime { get; set; }

        public string DefenceMeetingHour { get; set; }

        public virtual ICollection<ProposalWorkflowHistory> WorkflowHistories { get; set; }

        public virtual ICollection<ProposalKeyword> ProposalKeywords { get; set; }

        public virtual ICollection<ProposalFile> ProposalFiles { get; set; }

        public virtual ICollection<ProposalComment> ProposalComments { get; set; }

        [Required]
        public bool FirstJudgeApproved { get; set; }

        [Required]
        public bool SecondJudgeApproved { get; set; }

        /// <summary>
        /// تغییرات کلی را برای بررسی داوران مشخص می کند
        /// در غیر این صورت در دست بررسی استاد راهنما قرار می گیرد
        /// </summary>
        public bool? BigChangesForJudges { get; set; }
    }
}
