using System;
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

    }
}
