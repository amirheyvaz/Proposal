using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Proposal.Core;
using RepositoryLayer.Interfaces;

namespace Test.Student
{
    [TestFixture]
    public class StudentTest
    {
        [TestCase("0020881029", true, Description = "Exists")]
        [TestCase("0024455985", false, Description = "Doesn't Exists")]
        public void Student_GetUserInfo(string u , bool e){
            var studentRep = IocConfig.Container.GetInstance<IStudentRepository>();
            if (e)
                Assert.IsNotNull(studentRep.GetUserInfo(u));
            Assert.DoesNotThrow(() => studentRep.GetUserInfo(u));
        }

        [TestCase("0020881029", "123456", true, Description = "Correct-Password")]
        [TestCase("0020881029", "751456", false, Description = "Incorrect-Password")]
        public void Student_AuthenticateStudent(string u, string p, bool e)
        {
            var studentRep = IocConfig.Container.GetInstance<IStudentRepository>();
            Assert.DoesNotThrow(() => studentRep.AuthenticateStudent(u, p));
            bool Actual = studentRep.AuthenticateStudent(u, p);
            Assert.That(Actual, Is.EqualTo(e));
        }
    }
}
