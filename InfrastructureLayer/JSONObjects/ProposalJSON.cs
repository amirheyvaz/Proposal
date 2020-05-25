using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.JSONObjects
{
    public class ProposalJSON
    {
        public string Name { get; set; }

        public string LatinName { get; set; }

        public string CreateDate { get; set; }

        public string StudentFullName { get; set; }

        public Guid StudentID { get; set; }

        public string StudentSocialSecurityNumber { get; set; }

        public Guid ReseachTypeID { get; set; }

        public string ReseachTypeTitle { get; set; }

        public string FirstJudgeFullName { get; set; }

        public Guid FirstJudgeID { get; set; }

        public string FirstJudgeSocialSecurityNumber { get; set; }

        public string SecondJudgeFullName { get; set; }

        public Guid SecondJudgeID { get; set; }

        public string SecondJudgeSocialSecurityNumber { get; set; }

        public Guid ProposalStageID { get; set; }

        public string ProposalStageTitle { get; set; }

        public string LatestOperation { get; set; }

        public Guid ProposalStatusID { get; set; }

        public string ProposalStatusTitle { get; set; }

        public bool IsFinalApprove { get; set; }

        public string DefenceMeetingTime { get; set; }

        public List<string> Keywords { get; set; }

        public bool BothJudgesApproved { get; set; }

        public List<ProposalWorkflowHistoryJSON> WorkflowHistories { get; set; }

        public List<ProposalCommentJSON> Comments { get; set; }
    }
}
