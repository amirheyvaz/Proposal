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
    public class ProposalFileRepository : GenericRepository<ProposalFile, Guid>, IProposalFileRepository
    {
        public ProposalFileRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
