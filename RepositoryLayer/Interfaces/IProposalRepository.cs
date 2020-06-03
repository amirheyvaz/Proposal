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
    }
}
