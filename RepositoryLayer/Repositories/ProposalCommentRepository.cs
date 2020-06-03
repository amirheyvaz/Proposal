using ModelsLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfrastructureLayer.Interfaces;
using RepositoryLayer.Interfaces;
using InfrastructureLayer.JSONObjects;
using InfrastructureLayer.Utilities;

namespace RepositoryLayer.Repositories
{
    public class ProposalCommentRepository : GenericRepository<ProposalComment, Guid>, IProposalCommentRepository
    {
        public ProposalCommentRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public List<ProposalCommentJSON> GetAllProposalComments(Guid ProposalID)
        {
            return SelectBy(q => q.ProposalID == ProposalID)
                .AsEnumerable()
                .Select(p => new ProposalCommentJSON {
                    ID = p.ID,
                    ProposalID = p.ProposalID,
                    ProposalTitle = p.Proposal.Name,
                    OccuredByPersonID = p.OccuredByStudentID.HasValue ? p.OccuredByStudentID.Value : p.OccuredByProfessorID.Value,
                    OccuredByPersonTitle = p.OccuredByStudentID.HasValue ? p.OccuredByStudent.FirstName + " " + p.OccuredByStudent.LastName : p.OccuredByProfessor.FirstName + " " + p.OccuredByProfessor.LastName,
                    OccuredByStudent = p.OccuredByStudentID.HasValue,
                    ImportanceLevel = p.ImportanceLevel.HasValue ? p.ImportanceLevel.Value : 0,
                    OccuranceDate = p.OccuranceDate.GregorianToShamsi(),
                    StageID = p.ProposalStageID,
                    StageTitle = p.ProposalStage.Title
                }).ToList();
        }
    }
}
