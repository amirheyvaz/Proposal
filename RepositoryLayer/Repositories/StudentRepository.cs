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
    public class StudentRepository : GenericRepository<Student, Guid>, IStudentRepository
    {
        public StudentRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public bool AuthenticateStudent(string SocialNo , string Password)
        {
            Student s = SelectBy(p => p.SocialSecurityNumber == SocialNo).FirstOrDefault();
            if (s == null)
                return false;
            else
                return s.Password == Password;
        }

        public UserInfoJson GetUserInfo(string username)
        {
            Student s = SelectBy(p => p.SocialSecurityNumber == username).FirstOrDefault();
            if (s == null)
                return null;

            return new UserInfoJson {
                ID = s.ID,
                FirstName = s.FirstName,
                LastName = s.LastName,
                StudentNumber = s.StudentNumber,
                SocialSecurityNumber = s.SocialSecurityNumber,
                Role = "Student"
            };
        }
    }
}
