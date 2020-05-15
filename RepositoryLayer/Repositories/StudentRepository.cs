using ModelsLayer.Models;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfrastructureLayer.Interfaces;

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
    }
}
