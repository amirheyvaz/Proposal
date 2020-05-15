using ModelsLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface IProfessorRepository : IGenericRepository<Professor, Guid>
    {
        bool AuthenticateProfessor(string SocialNo, string Password);
    }
}
