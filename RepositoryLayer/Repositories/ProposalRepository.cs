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
    public class ProposalRepository : GenericRepository<Proposal, Guid>, IProposalRepository
    {
        IProposalKeywordRepository KeywordRepository;
        Lazy<IStudentRepository> StudentRepository;
        IProposalStageRepository StageRepository;
        IProposalStatusRepository StatusRepository;
        public ProposalRepository(IUnitOfWork unitOfWork ,
            IProposalKeywordRepository IProposalKeywordRepository,
            Lazy<IStudentRepository> IStudentRepository,
            IProposalStageRepository IProposalStageRepository,
            IProposalStatusRepository IProposalStatusRepository
            ) : base(unitOfWork)
        {
            KeywordRepository = IProposalKeywordRepository;
            StudentRepository = IStudentRepository;
            StageRepository = IProposalStageRepository;
            StatusRepository = IProposalStatusRepository;
        }

        public bool SubmitProposal(ProposalGeneralInfoJSON ProposalJSON , string Username)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    bool result = true;

                    Student s = StudentRepository.Value.SelectBy(q => q.SocialSecurityNumber == Username).FirstOrDefault();
                    if (s == null)
                        return false;

                    Proposal p = new Proposal();
                    p.ID = Guid.NewGuid();
                    p.CreateDate = DateTime.Now;
                    p.IsFinalApprove = false;
                    p.LatestOperation = "ثبت شده توسط دانشجو";
                    p.LatinName = ProposalJSON.LatinName;
                    p.Name = ProposalJSON.Name;
                    p.ResearchTypeID = ProposalJSON.ReseachTypeID;
                    p.ProposalStageID = StageRepository.SelectBy(w => w.Order == 1).FirstOrDefault().ID;
                    p.ProposalStatusID = StatusRepository.SelectBy(e => e.IsDefault).FirstOrDefault().ID;
                    p.StudentID = s.ID;
                    p.BothJudgesApproved = false;
                    
                    Add(p);

                    result &= KeywordRepository.AddProposalKeyword(ProposalJSON.keywords, p.ID);
                    
                    Commit();
                    dbContextTransaction.Commit();
                    dbContextTransaction.Dispose();
                    
                    return result;
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    dbContextTransaction.Dispose();
                    return false;
                }
            }
        }

    }
}
