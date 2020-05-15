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
    public class ProfessorRepository : GenericRepository<Professor, Guid>, IProfessorRepository
    {
        public ProfessorRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public bool AuthenticateProfessor(string SocialNo, string Password)
        {
            Professor s = SelectBy(p => p.SocialSecurityNumber == SocialNo).FirstOrDefault();
            if (s == null)
                return false;
            else
                return s.Password == Password;
        }
    }
}
