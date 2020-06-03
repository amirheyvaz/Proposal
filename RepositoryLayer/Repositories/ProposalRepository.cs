using ModelsLayer.Models;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfrastructureLayer.Interfaces;
using InfrastructureLayer.JSONObjects;
using InfrastructureLayer.Utilities;

namespace RepositoryLayer.Repositories
{
    public class ProposalRepository : GenericRepository<Proposal, Guid>, IProposalRepository
    {
        IProposalKeywordRepository KeywordRepository;
        Lazy<IStudentRepository> StudentRepository;
        IProposalStageRepository StageRepository;
        IProposalStatusRepository StatusRepository;
        IProposalCommentRepository ProposalCommentRepository;
        IProposalWorkflowHistoryRepository ProposalWorkflowHistoryRepository;
        public ProposalRepository(IUnitOfWork unitOfWork ,
            IProposalKeywordRepository IProposalKeywordRepository,
            Lazy<IStudentRepository> IStudentRepository,
            IProposalStageRepository IProposalStageRepository,
            IProposalStatusRepository IProposalStatusRepository,
            IProposalCommentRepository IProposalCommentRepository,
            IProposalWorkflowHistoryRepository IProposalWorkflowHistoryRepository
            ) : base(unitOfWork)
        {
            KeywordRepository = IProposalKeywordRepository;
            StudentRepository = IStudentRepository;
            StageRepository = IProposalStageRepository;
            StatusRepository = IProposalStatusRepository;
            ProposalCommentRepository = IProposalCommentRepository;
            ProposalWorkflowHistoryRepository = IProposalWorkflowHistoryRepository;
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
                    p.FirstJudgeApproved = false;
                    p.SecondJudgeApproved = false;

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

        public ProposalJSON GetProposalByStudentID(Guid StudentID)
        {
            return SelectBy(s => s.StudentID == StudentID).AsEnumerable().Select(p => new ProposalJSON {
                ID = p.ID,
                Name = p.Name,
                LatinName = p.LatinName,
                CreateDate = p.CreateDate.GregorianToShamsi(),
                StudentFullName = p.Student.FirstName + " " + p.Student.LastName,
                StudentID = p.StudentID,
                StudentSocialSecurityNumber = p.Student.SocialSecurityNumber,
                ReseachTypeID = p.ResearchTypeID,
                ReseachTypeTitle = p.ResearchType.Title,
                FirstJudgeID = p.FirstJudgeID.HasValue ? p.FirstJudgeID.Value : Guid.Empty,
                FirstJudgeFullName = p.FirstJudgeID.HasValue ? p.FirstJudge.FirstName + " " + p.FirstJudge.LastName : "",
                FirstJudgeSocialSecurityNumber = p.FirstJudgeID.HasValue ? p.FirstJudge.SocialSecurityNumber : "",
                SecondJudgeID = p.SecondJudgeID.HasValue ? p.SecondJudgeID.Value : Guid.Empty,
                SecondJudgeFullName = p.SecondJudgeID.HasValue ? p.SecondJudge.FirstName + " " + p.SecondJudge.LastName : "",
                SecondJudgeSocialSecurityNumber = p.SecondJudgeID.HasValue ? p.SecondJudge.SocialSecurityNumber : "",
                ProposalStageID = p.ProposalStageID,
                ProposalStageTitle = p.ProposalStage.Title,
                LatestOperation = p.LatestOperation,
                ProposalStatusID = p.ProposalStatusID,
                ProposalStatusTitle = p.ProposalStatus.Title,
                IsFinalApprove = p.IsFinalApprove,
                DefenceMeetingTime = p.DefenceMeetingTime.HasValue ? p.DefenceMeetingTime.Value.GregorianToShamsi() : "",
                FirstJudgeApproved = p.FirstJudgeApproved,
                SecondJudgeApproved = p.SecondJudgeApproved,
                Keywords = KeywordRepository.GetProposalKeywords(p.ID),
                Comments = ProposalCommentRepository.GetAllProposalComments(p.ID),
                WorkflowHistories = ProposalWorkflowHistoryRepository.GetAllHistories(p.ID)
            }).FirstOrDefault();
        }
    }
}
