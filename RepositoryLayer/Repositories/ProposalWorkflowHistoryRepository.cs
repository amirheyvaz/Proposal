using ModelsLayer.Models;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfrastructureLayer.Interfaces;
using InfrastructureLayer.JSONObjects;

namespace RepositoryLayer.Repositories
{
    public class ProposalWorkflowHistoryRepository : GenericRepository<ProposalWorkflowHistory, Guid>, IProposalWorkflowHistoryRepository
    {
        public ProposalWorkflowHistoryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public List<ProposalWorkflowHistoryJSON> GetAllHistories(Guid ProposalID)
        {
            return SelectBy(q => q.ProposalID == ProposalID).Select(p => new ProposalWorkflowHistoryJSON {
                ProposalID = p.ProposalID,
                ID = p.ID,
                ProposalName = p.Proposal.Name,
                OccuredByPersonID = p.OccuredByProfessorID.HasValue ? p.OccuredByProfessorID.Value : p.OccuredByStudentID.Value,
                OccuredByPersonName = p.OccuredByProfessorID.HasValue ? p.OccuredByProfessor.FirstName + " " + p.OccuredByProfessor.LastName : p.OccuredByStudent.FirstName + " " + p.OccuredByStudent.LastName,
                OccuredByStudent = p.OccuredByStudentID.HasValue,
                OperationID = p.ProposalOperationID,
                OperationTitle = p.ProposalOperation.Title 
            }).ToList();
        }

    }
}
