using Proposal.Core;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;


namespace Proposal.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/Authentication")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AuthController : ApiController
    {
        [HttpPost]
        [Route("GetToken/{username}/{password}")]
        public string GetToken(string username, string password)
        {
            if (CheckUser(username, password))
            {
                return JwtManager.GenerateToken(username);
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

        }

        private bool CheckUser(string username, string password)
        {
            // should check in the database
            var StudentRepository = IocConfig.Container.GetInstance<IStudentRepository>();
            var ProfessorRepository = IocConfig.Container.GetInstance<IProfessorRepository>();
            if (StudentRepository.AuthenticateStudent(username, password) || ProfessorRepository.AuthenticateProfessor(username , password))
            {
                return true;
            }
            else
            {
                return false;
            }


        }

    }
}
