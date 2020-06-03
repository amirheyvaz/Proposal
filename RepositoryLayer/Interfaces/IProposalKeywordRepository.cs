using ModelsLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface IProposalKeywordRepository : IGenericRepository<ProposalKeyword , Guid>
    {
        bool AddProposalKeyword(List<string> keys, Guid ProposalID);
        List<string> GetProposalKeywords(Guid ProposalID);
    }
}
