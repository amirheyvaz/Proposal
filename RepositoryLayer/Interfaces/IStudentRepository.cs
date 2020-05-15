using ModelsLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface IStudentRepository : IGenericRepository<Student , Guid>
    {
        bool AuthenticateStudent(string SocialNo, string Password);
    }
}
