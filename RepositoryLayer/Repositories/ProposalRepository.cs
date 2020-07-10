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
        Lazy<IProfessorRepository> ProfessorRepository;
        IProposalStageRepository StageRepository;
        IProposalStatusRepository StatusRepository;
        IProposalCommentRepository ProposalCommentRepository;
        IProposalWorkflowHistoryRepository ProposalWorkflowHistoryRepository;
        IProposalFileRepository ProposalFileRepository;
        public ProposalRepository(IUnitOfWork unitOfWork ,
            IProposalKeywordRepository IProposalKeywordRepository,
            Lazy<IStudentRepository> IStudentRepository,
            IProposalStageRepository IProposalStageRepository,
            IProposalStatusRepository IProposalStatusRepository,
            IProposalCommentRepository IProposalCommentRepository,
            IProposalWorkflowHistoryRepository IProposalWorkflowHistoryRepository,
            IProposalFileRepository IProposalFileRepository,
            Lazy<IProfessorRepository> IProfessorRepository
            ) : base(unitOfWork)
        {
            KeywordRepository = IProposalKeywordRepository;
            StudentRepository = IStudentRepository;
            StageRepository = IProposalStageRepository;
            StatusRepository = IProposalStatusRepository;
            ProposalCommentRepository = IProposalCommentRepository;
            ProposalWorkflowHistoryRepository = IProposalWorkflowHistoryRepository;
            ProposalFileRepository = IProposalFileRepository;
            ProfessorRepository = IProfessorRepository;
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

                    //WorkFlow
                    ProposalWorkflowHistory work = new ProposalWorkflowHistory {
                        ID = Guid.NewGuid(),
                        OccuranceDate = DateTime.Now,
                        OccuredByProfessorID = null,
                        OccuredByStudentID = s.ID,
                        ProposalID = p.ID,
                        ProposalOperationID = Guid.Parse("4aaaa946-53ce-45c8-a317-20eba03867e0")
                    };
                    ProposalWorkflowHistoryRepository.Add(work);
                    //

                    ProposalFile pf = ProposalFileRepository.Get(ProposalJSON.FileID);
                    if(pf == null)
                    {
                        return false;
                    }
                    else
                    {
                        pf.ProposalID = p.ID;
                    }


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
            var Editables = new List<int>() { 1 , 4 , 7 , 11 };
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
                WorkflowHistories = ProposalWorkflowHistoryRepository.GetAllHistories(p.ID),
                Deletable = p.ProposalStage.Order == 1,
                Editable = Editables.Contains(p.ProposalStage.Order),
                Sendable = p.ProposalStage.Order == 1 || p.ProposalStage.Order == 4 || p.ProposalStage.Order == 7 || p.ProposalStage.Order == 11
            }).FirstOrDefault();
        }

        public bool DeleteProposal(Guid ProposalID)
        {
            using (var transaction = Context.Database.BeginTransaction()) {
                try
                {

                    var keywords = KeywordRepository.SelectBy(k => k.ProposalID == ProposalID);
                    KeywordRepository.DeleteRange(keywords.ToList());

                    var comments = ProposalCommentRepository.SelectBy(c => c.ProposalID == ProposalID);
                    ProposalCommentRepository.DeleteRange(comments.ToList());

                    var histories = ProposalWorkflowHistoryRepository.SelectBy(h => h.ProposalID == ProposalID);
                    ProposalWorkflowHistoryRepository.DeleteRange(histories.ToList());

                    var proposalFiles = ProposalFileRepository.SelectBy(f => f.ProposalID == ProposalID);
                    ProposalFileRepository.DeleteRange(proposalFiles.ToList());

                    DeleteById(ProposalID);

                    Commit();
                    transaction.Commit();
                    transaction.Dispose();
                    return true;
                }catch(Exception e)
                {
                    transaction.Rollback();
                    transaction.Dispose();
                    return false;
                }

            }
        }

        public List<ProposalJSON> GetAllProfessorProposals(Guid ProfessorID)
        {
            List<Proposal> proposals = new List<Proposal>();
            Professor pro = ProfessorRepository.Value.Get(ProfessorID);

            proposals.AddRange(SelectBy(p => p.Student.FacultyID == pro.FacultyID && (p.ProposalStage.Order == 3 || p.ProposalStage.Order == 5 || p.ProposalStage.Order == 9 || (p.DefenceMeetingTime.HasValue && p.ProposalStage.Order == 6))  && ((p.FirstJudgeID == ProfessorID && !p.FirstJudgeApproved )|| (p.SecondJudgeID == ProfessorID && !p.SecondJudgeApproved))));
            proposals.AddRange(SelectBy(p => p.Student.FacultyID == pro.FacultyID && (p.Student.FirstGuidingProfessorID == ProfessorID || p.Student.SecondGuidingProfessorID == ProfessorID) && (p.ProposalStage.Order == 8 || p.ProposalStage.Order == 12)));
            if (pro.EducationalGroups_Manager.Any())
            {
                List<Guid> FacultyIDs = pro.EducationalGroups_Manager.Select(m => m.FacultyID).ToList();
                proposals.AddRange(SelectBy(p => ((p.ProposalStage.Order == 6 && !p.DefenceMeetingTime.HasValue)||p.ProposalStage.Order == 2) && FacultyIDs.Contains(p.Student.FacultyID)));
            }
            if (pro.IsCouncilMember)
            {
                proposals.AddRange(SelectBy(p => p.ProposalStage.Order == 10 && p.Student.FacultyID == pro.FacultyID));
            }

            return proposals.AsEnumerable().Select((p,index) => new ProposalJSON
             {
                 RowNumber = index + 1,
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
                 ProposalStageTitle = !p.ProposalStage.IsLast ? ("در انتظار اقدام " + p.ProposalStage.Title) : p.ProposalStage.Title,
                 ProposalStageOrder = p.ProposalStage.Order,
                 LatestOperation = p.LatestOperation,
                 ProposalStatusID = p.ProposalStatusID,
                 ProposalStatusTitle = p.ProposalStatus.Title,
                 IsFinalApprove = p.IsFinalApprove,
                 DefenceMeetingTime = p.DefenceMeetingTime.HasValue ? p.DefenceMeetingTime.Value.GregorianToShamsi() : "",
                 FirstJudgeApproved = p.FirstJudgeApproved,
                 SecondJudgeApproved = p.SecondJudgeApproved,
                 Keywords = KeywordRepository.GetProposalKeywords(p.ID),
                 Comments = ProposalCommentRepository.GetAllProposalComments(p.ID),
                 WorkflowHistories = ProposalWorkflowHistoryRepository.GetAllHistories(p.ID),
                 WaitingForJudgeApprovement = /*(!p.FirstJudgeApproved || !p.SecondJudgeApproved) && */ (p.ProposalStage.Order == 3 || p.ProposalStage.Order == 5 || p.ProposalStage.Order == 9),
                 WaitingForJudgeAssignment = p.ProposalStage.Order == 2,
                 WaitingForGuidingProfessorApprovement = (p.ProposalStage.Order == 8 || p.ProposalStage.Order == 12),
                 WaitingForCouncilApprovement = p.ProposalStage.Order == 10,
                 WaitingForDefenceMeetingTiming = p.ProposalStage.Order == 6 && !p.DefenceMeetingTime.HasValue,
                 WaitingForDefenceMeetingJudgement = p.ProposalStage.Order == 6 && p.DefenceMeetingTime.HasValue
            }).ToList();
        }

        public string SendProposal(Guid ID )//, ProposalComment comment)
        {
            
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var proposal = Get(ID);
                    if (proposal == null)
                    {
                        dbContextTransaction.Rollback();
                        dbContextTransaction.Dispose();
                        return "اطلاعات پروپوزال اشتباه است";
                    }
                        
                    if (proposal.ProposalStage.Order != 1 && proposal.ProposalStage.Order != 4 && proposal.ProposalStage.Order != 7 && proposal.ProposalStage.Order != 11)
                    {
                        dbContextTransaction.Rollback();
                        dbContextTransaction.Dispose();
                        return "پروپوزال در دست بررسی این کاربر قرار نیست";
                    }
                        

                    var currentStage = StageRepository.Get(proposal.ProposalStageID);
                    if (currentStage != null)
                    {
                        ProposalStage nextStage = null;
                        if (currentStage.Order == 1 || currentStage.Order == 4 || currentStage.Order == 11)
                            nextStage = StageRepository.SelectBy(p => p.Order == currentStage.Order + 1).FirstOrDefault();
                        else if(currentStage.Order == 7)
                        {
                            if (proposal.BigChangesForJudges.HasValue && proposal.BigChangesForJudges.Value)
                            {
                                nextStage = StageRepository.SelectBy(p => p.Order == 9).FirstOrDefault();
                            }
                            else
                            {
                                nextStage = StageRepository.SelectBy(p => p.Order == 8).FirstOrDefault();
                            }
                        }

                        if (nextStage != null)
                        {
                            
                            proposal.ProposalStageID = nextStage.ID;
                            proposal.LatestOperation = "ارسال شده توسط دانشجو";
                            //WorkFlow
                            ProposalWorkflowHistory work = new ProposalWorkflowHistory
                            {
                                ID = Guid.NewGuid(),
                                OccuranceDate = DateTime.Now,
                                OccuredByProfessorID = null,
                                OccuredByStudentID = proposal.StudentID,
                                ProposalID = proposal.ID,
                                ProposalOperationID = Guid.Parse("B56DBC09-DDFA-4003-89EC-CED81CD31F7C")
                            };
                            ProposalWorkflowHistoryRepository.Add(work);
                            //
                            //Comment
                            //ProposalComment com = new ProposalComment
                            //{
                            //    ID = Guid.NewGuid(),
                            //    ImportanceLevel = comment.ImportanceLevel,
                            //    Content = comment.Content,
                            //    OccuranceDate = DateTime.Now,
                            //    OccuredByProfessorID = null,
                            //    OccuredByStudentID = proposal.StudentID,
                            //    ProposalID = proposal.ID,
                            //    ProposalStageID = currentStage.ID
                            //};
                            //ProposalCommentRepository.Add(com);
                            //
                        }
                    }
                   

                    Commit();
                    dbContextTransaction.Commit();
                    dbContextTransaction.Dispose();

                    return "";
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    dbContextTransaction.Dispose();
                    return "خطا در سرور";
                }
            }
        }

        public string ApproveProposal(Guid ID , Guid ProfessorID , ProposalComment comment)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var proposal = Get(ID);
                    if (proposal == null)
                    {
                        dbContextTransaction.Rollback();
                        dbContextTransaction.Dispose();
                        return "اطلاعات پروپوزال اشتباه است";
                    }
                       
                    if (proposal.ProposalStage.Order != 3 && proposal.ProposalStage.Order != 5 && proposal.ProposalStage.Order != 6 && proposal.ProposalStage.Order != 8 && proposal.ProposalStage.Order != 9 && proposal.ProposalStage.Order != 10 && proposal.ProposalStage.Order != 12)
                    {
                        dbContextTransaction.Rollback();
                        dbContextTransaction.Dispose();
                        return "پروپوزال در دست بررسی این کاربر قرار نیست";
                    }
                       

                    var currentStage = StageRepository.Get(proposal.ProposalStageID);
                    if (currentStage != null)
                    {
                        ProposalStage nextStage = null;
                       
                            
                        //5 ==> 6
                        //9 ==> 10
                        //3 ==> 6
                        //6 ==> 10
                        //8 ==> 10
                        //10 ==> 13 FA
                        //12 ==> 10
                        switch (currentStage.Order)
                        {
                            case 3:
                                {
                                    if(proposal.FirstJudgeID == ProfessorID)
                                    {
                                        proposal.FirstJudgeApproved = true;
                                    }
                                    else if(proposal.SecondJudgeID == ProfessorID)
                                    {
                                        proposal.SecondJudgeApproved = true;
                                    }
                                    if(proposal.SecondJudgeApproved && proposal.FirstJudgeApproved)
                                    {
                                        nextStage = StageRepository.SelectBy(p => p.Order == 6).FirstOrDefault();
                                        proposal.FirstJudgeApproved = false;
                                        proposal.SecondJudgeApproved = false;
                                        proposal.LatestOperation = "تایید شده توسط داوران";
                                    }
                                    else
                                    {
                                        nextStage = currentStage;
                                    }
                                    break;
                                }
                            case 5:
                                {
                                    if (proposal.FirstJudgeID == ProfessorID)
                                    {
                                        proposal.FirstJudgeApproved = true;
                                    }
                                    else if (proposal.SecondJudgeID == ProfessorID)
                                    {
                                        proposal.SecondJudgeApproved = true;
                                    }
                                    if (proposal.SecondJudgeApproved && proposal.FirstJudgeApproved)
                                    {
                                        nextStage = StageRepository.SelectBy(p => p.Order == 6).FirstOrDefault();
                                        proposal.FirstJudgeApproved = false;
                                        proposal.SecondJudgeApproved = false;
                                        proposal.LatestOperation = "تایید شده توسط داوران";
                                    }
                                    else
                                    {
                                        nextStage = currentStage;
                                    }
                                    
                                    break;
                                }
                            case 6:
                                {
                                    if (proposal.FirstJudgeID == ProfessorID)
                                    {
                                        proposal.FirstJudgeApproved = true;
                                    }
                                    else if (proposal.SecondJudgeID == ProfessorID)
                                    {
                                        proposal.SecondJudgeApproved = true;
                                    }
                                    if (proposal.SecondJudgeApproved && proposal.FirstJudgeApproved)
                                    {
                                        nextStage = StageRepository.SelectBy(p => p.Order == 10).FirstOrDefault();
                                        proposal.FirstJudgeApproved = false;
                                        proposal.SecondJudgeApproved = false;
                                        proposal.LatestOperation = "تایید شده توسط داوران در جلسه دفاع";
                                    }
                                    else
                                    {
                                        nextStage = currentStage;
                                    }
                                    
                                    break;
                                }
                            case 8:
                                {
                                    nextStage = StageRepository.SelectBy(p => p.Order == 10).FirstOrDefault();
                                    proposal.LatestOperation = "تایید شده توسط استاد راهنما";
                                    break;
                                }
                            case 9:
                                {
                                    if (proposal.FirstJudgeID == ProfessorID)
                                    {
                                        proposal.FirstJudgeApproved = true;
                                    }
                                    else if (proposal.SecondJudgeID == ProfessorID)
                                    {
                                        proposal.SecondJudgeApproved = true;
                                    }
                                    if (proposal.SecondJudgeApproved && proposal.FirstJudgeApproved)
                                    {
                                        nextStage = StageRepository.SelectBy(p => p.Order == 10).FirstOrDefault();
                                        proposal.FirstJudgeApproved = false;
                                        proposal.SecondJudgeApproved = false;
                                        proposal.LatestOperation = "تایید شده توسط داوران";
                                    }
                                    else
                                    {
                                        nextStage = currentStage;
                                    }
                                    
                                    break;
                                }
                            case 10:
                                {
                                    nextStage = StageRepository.SelectBy(p => p.Order == 13).FirstOrDefault();
                                    proposal.LatestOperation = "تایید شده توسط شورا";
                                    proposal.IsFinalApprove = true;
                                    break;
                                }
                            case 12:
                                {
                                    nextStage = StageRepository.SelectBy(p => p.Order == 10).FirstOrDefault();
                                    proposal.LatestOperation = "تایید شده توسط استاد راهنما";
                                    break;
                                }
                        }
                        if (nextStage != null)
                        {
                            proposal.ProposalStageID = nextStage.ID;
                            //WorkFlow
                            ProposalWorkflowHistory work = new ProposalWorkflowHistory
                            {
                                ID = Guid.NewGuid(),
                                OccuranceDate = DateTime.Now,
                                OccuredByProfessorID = ProfessorID,
                                OccuredByStudentID = null,
                                ProposalID = proposal.ID,
                                ProposalOperationID = Guid.Parse("BF63E692-E4F9-4E7E-BF3D-1DC2C818907F")
                            };
                            ProposalWorkflowHistoryRepository.Add(work);
                            //
                            //Comment
                            ProposalComment com = new ProposalComment
                            {
                                ID = Guid.NewGuid(),
                                ImportanceLevel = comment.ImportanceLevel,
                                Content = comment.Content,
                                OccuranceDate = DateTime.Now,
                                OccuredByProfessorID = ProfessorID,
                                OccuredByStudentID = null,
                                ProposalID = proposal.ID,
                                ProposalStageID = currentStage.ID
                            };
                            ProposalCommentRepository.Add(com);
                            //
                        }
                    }


                    Commit();
                    dbContextTransaction.Commit();
                    dbContextTransaction.Dispose();

                    return "";
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    dbContextTransaction.Dispose();
                    return "خطا در سرور";
                }
            }
        }

        public string RejectProposal(Guid ID, Guid ProfessorID , ProposalComment comment, bool BigChanges = true)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var proposal = Get(ID);
                    if (proposal == null)
                    {
                        dbContextTransaction.Rollback();
                        dbContextTransaction.Dispose();
                        return "اطلاعات پروپوزال اشتباه است";
                    }

                    if (proposal.ProposalStage.Order != 3 && proposal.ProposalStage.Order != 5 && proposal.ProposalStage.Order != 6 && proposal.ProposalStage.Order != 8 && proposal.ProposalStage.Order != 9 && proposal.ProposalStage.Order != 10 && proposal.ProposalStage.Order != 12)
                    {
                        dbContextTransaction.Rollback();
                        dbContextTransaction.Dispose();
                        return "پروپوزال در دست بررسی این کاربر قرار نیست";
                    }


                    var currentStage = StageRepository.Get(proposal.ProposalStageID);
                    if (currentStage != null)
                    {
                        ProposalStage nextStage = null;


                        //5 ==> 4
                        //9 ==> 7
                        //3 ==> 4
                        //6 ==> 7 BigChanges
                        //8 ==> 7
                        //10 ==> 11
                        //12 ==> 11
                        switch (currentStage.Order)
                        {
                            case 3:
                                {
                                    if (proposal.FirstJudgeID == ProfessorID)
                                    {
                                        proposal.FirstJudgeApproved = true;
                                    }
                                    else if (proposal.SecondJudgeID == ProfessorID)
                                    {
                                        proposal.SecondJudgeApproved = true;
                                    }
                                    if (proposal.SecondJudgeApproved && proposal.FirstJudgeApproved)
                                    {
                                        nextStage = StageRepository.SelectBy(p => p.Order == 4).FirstOrDefault();
                                        proposal.FirstJudgeApproved = false;
                                        proposal.SecondJudgeApproved = false;
                                        proposal.LatestOperation = "رد شده توسط داوران";
                                    }
                                    else
                                    {
                                        nextStage = currentStage;
                                    }
                                    break;
                                }
                            case 5:
                                {
                                    if (proposal.FirstJudgeID == ProfessorID)
                                    {
                                        proposal.FirstJudgeApproved = true;
                                    }
                                    else if (proposal.SecondJudgeID == ProfessorID)
                                    {
                                        proposal.SecondJudgeApproved = true;
                                    }
                                    if (proposal.SecondJudgeApproved && proposal.FirstJudgeApproved)
                                    {
                                        nextStage = StageRepository.SelectBy(p => p.Order == 4).FirstOrDefault();
                                        proposal.FirstJudgeApproved = false;
                                        proposal.SecondJudgeApproved = false;
                                        proposal.LatestOperation = "رد شده توسط داوران";
                                    }
                                    else
                                    {
                                        nextStage = currentStage;
                                    }

                                    break;
                                }
                            case 6:
                                {
                                    if (proposal.FirstJudgeID == ProfessorID)
                                    {
                                        proposal.FirstJudgeApproved = true;
                                    }
                                    else if (proposal.SecondJudgeID == ProfessorID)
                                    {
                                        proposal.SecondJudgeApproved = true;
                                    }
                                    if (proposal.SecondJudgeApproved && proposal.FirstJudgeApproved)
                                    {
                                        nextStage = StageRepository.SelectBy(p => p.Order == 7).FirstOrDefault();
                                        proposal.FirstJudgeApproved = false;
                                        proposal.SecondJudgeApproved = false;
                                        proposal.BigChangesForJudges = BigChanges;
                                        proposal.LatestOperation = "رد شده توسط داوران در جلسه دفاع";
                                    }
                                    else
                                    {
                                        nextStage = currentStage;
                                    }

                                    break;
                                }
                            case 8:
                                {
                                    nextStage = StageRepository.SelectBy(p => p.Order == 7).FirstOrDefault();
                                    proposal.LatestOperation = "رد شده توسط استاد راهنما";
                                    break;
                                }
                            case 9:
                                {
                                    if (proposal.FirstJudgeID == ProfessorID)
                                    {
                                        proposal.FirstJudgeApproved = true;
                                    }
                                    else if (proposal.SecondJudgeID == ProfessorID)
                                    {
                                        proposal.SecondJudgeApproved = true;
                                    }
                                    if (proposal.SecondJudgeApproved && proposal.FirstJudgeApproved)
                                    {
                                        nextStage = StageRepository.SelectBy(p => p.Order == 7).FirstOrDefault();
                                        proposal.FirstJudgeApproved = false;
                                        proposal.SecondJudgeApproved = false;
                                        proposal.LatestOperation = "رد شده توسط داوران";
                                    }
                                    else
                                    {
                                        nextStage = currentStage;
                                    }

                                    break;
                                }
                            case 10:
                                {
                                    nextStage = StageRepository.SelectBy(p => p.Order == 11).FirstOrDefault();
                                    proposal.IsFinalApprove = true;
                                    proposal.LatestOperation = "رد شده توسط شورا";
                                    break;
                                }
                            case 12:
                                {
                                    nextStage = StageRepository.SelectBy(p => p.Order == 11).FirstOrDefault();
                                    proposal.LatestOperation = "رد شده توسط استاد راهنما";
                                    break;
                                }
                        }
                        if (nextStage != null)
                        {
                            proposal.ProposalStageID = nextStage.ID;
                            //WorkFlow
                            ProposalWorkflowHistory work = new ProposalWorkflowHistory
                            {
                                ID = Guid.NewGuid(),
                                OccuranceDate = DateTime.Now,
                                OccuredByProfessorID = ProfessorID,
                                OccuredByStudentID = null,
                                ProposalID = proposal.ID,
                                ProposalOperationID = Guid.Parse("73142388-9CE9-465A-AC7E-830B5D5F317C")
                            };
                            ProposalWorkflowHistoryRepository.Add(work);
                            //
                            //Comment
                            ProposalComment com = new ProposalComment {
                                ID = Guid.NewGuid(),
                                ImportanceLevel = comment.ImportanceLevel,
                                Content = comment.Content,
                                OccuranceDate = DateTime.Now,
                                OccuredByProfessorID = ProfessorID,
                                OccuredByStudentID = null,
                                ProposalID = proposal.ID,
                                ProposalStageID = currentStage.ID
                            };
                            ProposalCommentRepository.Add(com);
                            //
                        }
                    }


                    Commit();
                    dbContextTransaction.Commit();
                    dbContextTransaction.Dispose();

                    return "";
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    dbContextTransaction.Dispose();
                    return "خطا در سرور";
                }
            }
        }

        public string AssignJudges(Guid ProposalID, Guid FirstJudgeID, Guid SecondJudgeID)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var proposal = Get(ProposalID);
                    if (proposal == null)
                    {
                        dbContextTransaction.Rollback();
                        dbContextTransaction.Dispose();
                        return "اطلاعات پروپوزال اشتباه است";
                    }

                    var p1 = ProfessorRepository.Value.Get(FirstJudgeID);
                    var p2 = ProfessorRepository.Value.Get(SecondJudgeID);
                    if (p1 == null || p2 == null)
                    {
                        dbContextTransaction.Rollback();
                        dbContextTransaction.Dispose();
                        return "اطلاعات داوران اشتباه است";
                    }

                    proposal.FirstJudgeID = p1.ID;
                    proposal.SecondJudgeID = p2.ID;
                    proposal.ProposalStageID = StageRepository.SelectBy(p => p.Order == 3).FirstOrDefault().ID;
                    proposal.LatestOperation = "تعیین داوران توسط مدیر گروه";

                    Commit();
                    dbContextTransaction.Commit();
                    dbContextTransaction.Dispose();

                    return "";
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    dbContextTransaction.Dispose();
                    return "خطا در سرور";
                }
            }
        }

        public bool AssignDefenceMeetingTime(DateTime date , string Time , Guid ProposalID)
        {
            using (var transaction = Context.Database.BeginTransaction())
            {
                try
                {

                    var Proposal = Get(ProposalID);
                    if (Proposal == null)
                        return false;

                    Proposal.DefenceMeetingHour = Time;
                    Proposal.DefenceMeetingTime = date;
                    Proposal.LatestOperation = "تعیین زمان جلسه دفاع توسط مدیر گروه";
                    


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

        public bool EditProposal(Guid ProposalID, Guid FileID , string com)
        {
            using (var transaction = Context.Database.BeginTransaction())
            {
                try
                {

                    var proposal = Get(ProposalID);
                    var file = ProposalFileRepository.Get(FileID);
                    if (file == null || proposal == null)
                    {
                        return false;
                    }
  
                    file.ProposalID = ProposalID;

                    ProposalWorkflowHistory w = new ProposalWorkflowHistory() {
                        ID = Guid.NewGuid(),
                        OccuredByProfessorID = null,
                        OccuredByStudentID = proposal.StudentID,
                        OccuranceDate = DateTime.Now,
                        ProposalID = proposal.ID,
                        ProposalOperationID = Guid.Parse("bf63e692-e4f9-4e7e-bf3d-1dc2c8189078")
                    };
                    ProposalWorkflowHistoryRepository.Add(w);

                    ProposalComment c = new ProposalComment() {
                        ID = Guid.NewGuid(),
                        OccuredByProfessorID = null,
                        OccuredByStudentID = proposal.StudentID,
                        OccuranceDate = DateTime.Now,
                        Content = com,
                        ProposalID = proposal.ID,
                        ImportanceLevel = null,
                        ProposalStageID = proposal.ProposalStageID
                    };
                    ProposalCommentRepository.Add(c);

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
