using ModelsLayer.Models;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfrastructureLayer.Interfaces;

namespace RepositoryLayer.Repositories
{
    public class ProposalRepository : GenericRepository<Proposal, Guid>, IProposalRepository
    {
        public ProposalRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
