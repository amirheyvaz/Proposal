using InfrastructureLayer.JSONObjects;
using ModelsLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface IProposalRepository : IGenericRepository<Proposal,Guid>
    {
        bool SubmitProposal(ProposalGeneralInfoJSON ProposalJSON, string Username);
        ProposalJSON GetProposalByStudentID(Guid StudentID);
        bool DeleteProposal(Guid ProposalID);
        List<ProposalJSON> GetAllProfessorProposals(Guid ProfessorID);
        string SendProposal(Guid ID, ProposalComment comment);
        string ApproveProposal(Guid ID, Guid ProfessorID, ProposalComment comment);
        string RejectProposal(Guid ID, Guid ProfessorID, ProposalComment comment, bool BigChanges = true);
        string AssignJudges(Guid ProposalID, Guid FirstJudgeID, Guid SecondJudgeID);
        bool AssignDefenceMeetingTime(DateTime date, string Time, Guid ProposalID);
    }
}
