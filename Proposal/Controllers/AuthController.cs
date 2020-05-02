using Proposal.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;


namespace Proposal.Controllers
{
    [RoutePrefix("api/Authentication")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AuthController : ApiController
    {
        [Route("GetToken/{username}/{password}")]
        [HttpPost]
        [AllowAnonymous]
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
            //var UserRep = IocConfig.Container.GetInstance<IUserRepository>();

            //if (UserRep.SelectBy(x => x.UserName == username && x.Password == password).Any())
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}

            return true;

        }
    }
}
