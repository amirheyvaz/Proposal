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

        public bool DeleteFilesByPID(Guid ProposalID)
        {
            using (var transaction = Context.Database.BeginTransaction())
            {
                try
                {

                    var OtherFile = SelectBy(p => p.ProposalID == ProposalID).AsEnumerable().ToList();

                    foreach(var f in OtherFile)
                    {
                        DeleteById(f.ID);
                    }

                    Commit();
                    transaction.Commit();
                    transaction.Dispose();
                    return true;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    transaction.Dispose();
                    return false;
                }

            }
        }
    }
}
