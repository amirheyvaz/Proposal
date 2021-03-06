﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Proposal.Filters;
using RepositoryLayer.Interfaces;
using Proposal.Core;
using InfrastructureLayer.JSONObjects;
using ModelsLayer.Models;
using System.Threading.Tasks;
using System.Web;
using System.IO;

namespace Proposal.Controllers
{
    
    [JwtAuthentication]
    [RoutePrefix("api/Proposal")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProposalController : ApiController
    {
        [HttpGet]
        [Route("GetUserInfo/{username}")]
        public UserInfoJson GetUserInfo(string username)
        {
            var StudentRepository = IocConfig.Container.GetInstance<IStudentRepository>();
            var ProfessorRepository = IocConfig.Container.GetInstance<IProfessorRepository>();

            bool isStudent = StudentRepository.SelectBy(s => s.SocialSecurityNumber == username).Any();
            bool isProfessor = ProfessorRepository.SelectBy(s => s.SocialSecurityNumber == username).Any();


            if (!isProfessor && !isStudent)
                return null;
            else if (isStudent)
            {
                return StudentRepository.GetUserInfo(username);
            }
            else
            {
                return ProfessorRepository.GetUserInfo(username);
            }
        }

        [HttpGet]
        [Route("GetAllResearchTypes")]
        public List<ResearchType> GetAllResearchTypes()
        {
            var ReseachTypesRep = IocConfig.Container.GetInstance<IResearchTypeRepository>();
            if(ReseachTypesRep != null)
            {
                return ReseachTypesRep.GetAll().ToList();
            }
            else
            {
                return new List<ResearchType>();
            }
        }

        [HttpPost]
        [Route("UploadProposal/{username}")]
        public bool UploadProposal(string username , [FromBody] ProposalGeneralInfoJSON Proposal)
        {
            try
            {
                var ProposalRep = IocConfig.Container.GetInstance<IProposalRepository>();
                return ProposalRep.SubmitProposal(Proposal, username);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        [HttpGet]
        [Route("GetProposal/{username}")]
        public ProposalJSON GetProposal(string username)
        {
            var ProposalRep = IocConfig.Container.GetInstance<IProposalRepository>();
            var StudentRepository = IocConfig.Container.GetInstance<IStudentRepository>();
            var student = StudentRepository.SelectBy(s => s.SocialSecurityNumber == username).FirstOrDefault();
            if (student == null)
                return null;
            else
                return ProposalRep.GetProposalByStudentID(student.ID);
        }

        [HttpGet]
        [Route("DeleteProposal/{ID}")]
        public bool DeleteProposal(string ID)
        {
            try
            {
                var proposalRep = IocConfig.Container.GetInstance<IProposalRepository>();
                return proposalRep.DeleteProposal(Guid.Parse(ID));
            }
            catch(Exception e)
            {
                return false;
            }
        }


        [HttpGet]
        [Route("GetWaitingForActionProposals/{ProfessorUsername}")]
        public List<ProposalJSON> GetWaitingForActionProposals(string ProfessorUsername)
        {
            var ProfessorRepository = IocConfig.Container.GetInstance<IProfessorRepository>();
            var ProposalRepository = IocConfig.Container.GetInstance<IProposalRepository>();

            var Professor = ProfessorRepository.SelectBy(p => p.SocialSecurityNumber == ProfessorUsername).FirstOrDefault();
            if (Professor == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return ProposalRepository.GetAllProfessorProposals(Professor.ID);
        }

        [HttpGet]
        [Route("SendProposal/{ProposalID}")]
        public string SendProposal (Guid ProposalID)// , [FromBody] ProposalCommentJSON comment)
        {
            var ProposalRepository = IocConfig.Container.GetInstance<IProposalRepository>();
            //ProposalComment p = new ProposalComment {
            //    Content = comment.Content,
            //    ImportanceLevel = comment.ImportanceLevel
            //};
            return ProposalRepository.SendProposal(ProposalID);//, p);
        }

        [HttpPost]
        [Route("ApproveProposal/{ProposalID}/{ProfessorID}")]
        public string ApproveProposal (Guid ProposalID , Guid ProfessorID, [FromBody] ProposalCommentJSON comment)
        {
            var ProposalRepository = IocConfig.Container.GetInstance<IProposalRepository>();
            ProposalComment p = new ProposalComment
            {
                Content = comment.Content,
                ImportanceLevel = comment.ImportanceLevel
            };
            return ProposalRepository.ApproveProposal(ProposalID , ProfessorID , p);
        }

        [HttpPost]
        [Route("RejectProposal/{ProposalID}/{ProfessorID}/{BigChanges:bool?}")]
        public string RejectProposal(Guid ProposalID, Guid ProfessorID, [FromBody] ProposalCommentJSON comment , bool BigChanges = true )
        {
            var ProposalRepository = IocConfig.Container.GetInstance<IProposalRepository>();
            ProposalComment p = new ProposalComment
            {
                Content = comment.Content,
                ImportanceLevel = comment.ImportanceLevel
            };
            return ProposalRepository.RejectProposal(ProposalID, ProfessorID, p , BigChanges);
        }


        [HttpGet]
        [Route("GetAllProfessors")]
        public List<ComboBoxJSON> GetAllProfessors()
        {
            var ProfessorRepository = IocConfig.Container.GetInstance<IProfessorRepository>();
            return ProfessorRepository.GetAllProfessors();
        }

        [HttpGet]
        [Route("AsignJudges/{ProposalID}/{FirstJudgeID}/{SecondJudgeID}")]
        public string AssignJudges(Guid ProposalID , Guid FirstJudgeID , Guid SecondJudgeID)
        {
            var ProposalRepository = IocConfig.Container.GetInstance<IProposalRepository>();
            return ProposalRepository.AssignJudges(ProposalID , FirstJudgeID , SecondJudgeID);
        }

        [HttpGet]
        [Route("AssignDefenceMeetingTime/{ProposalID}/{date}/{time}")]
        public bool AssignDefenceMeetingTime(string date, string time, Guid ProposalID)
        {
            var ProposalRepository = IocConfig.Container.GetInstance<IProposalRepository>();

            DateTime dateTime;
            if(!DateTime.TryParse(date , out dateTime))
            {
                return false;
            }

            return ProposalRepository.AssignDefenceMeetingTime(dateTime,time,ProposalID);
        }

        [HttpPost]
        [Route("UploadPRoposalFile")]
        public Guid UploadPRoposalFile()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var file = HttpContext.Current.Request.Files.Count > 0 ?
                        HttpContext.Current.Request.Files[0] : null;

            if (file == null) {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
                
            byte[] fileData = null;
            using (var binaryReader = new BinaryReader(HttpContext.Current.Request.Files[0].InputStream))
            {
                fileData = binaryReader.ReadBytes(HttpContext.Current.Request.Files[0].ContentLength);
            }


            var FileRep = IocConfig.Container.GetInstance<IProposalFileRepository>();
            ProposalFile f = new ProposalFile() {
                ID = Guid.NewGuid(),
                File = fileData,
                ProposalID = null
            };
            FileRep.Add(f, true);

            return f.ID;

        }

        [HttpGet]
        [Route("DownloadProposalFile/{id}")]
        [AllowAnonymous]
        public HttpResponseMessage DownloadProposalFile(Guid id)
        {
            HttpResponseMessage result = null;
            try
            {

                var FileRep = IocConfig.Container.GetInstance<IProposalFileRepository>();
                var PropRep = IocConfig.Container.GetInstance<IProposalRepository>();
                var proposal = PropRep.Get(id);
                var file = FileRep.SelectBy(p => p.ProposalID == id).FirstOrDefault();
                if (file == null || proposal == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }


                // sendo file to client
                byte[] bytes = file.File;



                    result = Request.CreateResponse(HttpStatusCode.OK);
                    result.Content = new ByteArrayContent(bytes);
                    result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = proposal.Name + ".pdf";
                

                return result;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.Gone);
            }
        }

        [HttpPost]
        [Route("EditProposal/{ProposalID}/{FileID}")]
        public bool EditProposal (Guid ProposalID , Guid FileID , [FromBody] ProposalCommentJSON comment)
        {
            var PropRep = IocConfig.Container.GetInstance<IProposalRepository>();
            var ComRep = IocConfig.Container.GetInstance<IProposalFileRepository>();
            if (!ComRep.DeleteFilesByPID(ProposalID))
            {
                return false;
            }
            return PropRep.EditProposal(ProposalID , FileID , comment.Content);

        }
    }
}
