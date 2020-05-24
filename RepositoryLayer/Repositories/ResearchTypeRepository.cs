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
    public class ResearchTypeRepository : GenericRepository<ResearchType, Guid>, IResearchTypeRepository
    {
        public ResearchTypeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

    }
}
