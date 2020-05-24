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
                //TODO
                return null;
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
    }
}
