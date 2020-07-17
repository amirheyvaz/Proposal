using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Proposal.Core;
using RepositoryLayer.Interfaces;

namespace Test.Professor
{
    [TestFixture]
    public class ProfessorRepositoryTest
    {
        [TestCase("1234567890", "123456", true, Description = "Correct-Password")]
        [TestCase("1234567890", "751456", false, Description = "Incorrect-Password")]
        public void Professor_AuthenticateProfessor(string u, string p, bool e)
        {
            var ProfessorRep = IocConfig.Container.GetInstance<IProfessorRepository>();
            Assert.DoesNotThrow(() => ProfessorRep.AuthenticateProfessor(u, p));
            bool Actual = ProfessorRep.AuthenticateProfessor(u, p);
            Assert.That(Actual, Is.EqualTo(e));
        }

        [TestCase("1234567890",true , Description = "Exists")]
        [TestCase("1234658890", false , Description = "Doesn't Exists")]
        public void Professor_GetUserInfo(string u , bool e)
        {
            var ProfessorRep = IocConfig.Container.GetInstance<IProfessorRepository>();
            if(e)
                Assert.IsNotNull(ProfessorRep.GetUserInfo(u));
            Assert.DoesNotThrow(() => ProfessorRep.GetUserInfo(u));
        }

        [TestCase]
        public void Professor_GetAllProfessors()
        {
            var ProfessorRep = IocConfig.Container.GetInstance<IProfessorRepository>();
            Assert.IsNotNull(ProfessorRep.GetAllProfessors());
            Assert.DoesNotThrow(() => ProfessorRep.GetAllProfessors());
        }
    }
}
