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
    public class ProposalStatusRepository : GenericRepository<ProposalStatus, Guid>, IProposalStatusRepository
    {
        public ProposalStatusRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
