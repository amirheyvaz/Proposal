using InfrastructureLayer.JSONObjects;
using ModelsLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface IProposalWorkflowHistoryRepository : IGenericRepository<ProposalWorkflowHistory , Guid>
    {
        List<ProposalWorkflowHistoryJSON> GetAllHistories(Guid ProposalID);
    }
}
