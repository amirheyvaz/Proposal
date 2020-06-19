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
    public class ProfessorRepository : GenericRepository<Professor, Guid>, IProfessorRepository
    {
        IProposalRepository ProposalRepository;
        IStudentRepository StudentRepository;
        public ProfessorRepository(IUnitOfWork unitOfWork,
            IProposalRepository IProposalRepository,
            IStudentRepository IStudentRepository
            ) : base(unitOfWork)
        {
            ProposalRepository = IProposalRepository;
            StudentRepository = IStudentRepository;
        }


        public bool AuthenticateProfessor(string SocialNo, string Password)
        {
            Professor s = SelectBy(p => p.SocialSecurityNumber == SocialNo).FirstOrDefault();
            if (s == null)
                return false;
            else
                return s.Password == Password;
        }

        public UserInfoJson GetUserInfo(string username)
        {
            var AllProposals = ProposalRepository.GetAll();
            var AllStudents = StudentRepository.GetAll();
            return SelectBy(s => s.SocialSecurityNumber == username).AsEnumerable()
                .Select(p => new UserInfoJson {
                    ID = p.ID,
                    SocialSecurityNumber = p.SocialSecurityNumber,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Role = "Professor",
                    ProfessorDegree = p.Universities_Manager.Any() ? 6 : (p.Faculties_Manager.Any() ? 5 : (p.IsCouncilMember ? 4 : (p.EducationalGroups_Manager.Any()) ? 3 : (AllStudents.Any(AS => AS.FirstGuidingProfessorID == p.ID) || AllStudents.Any(AS2 => AS2.SecondGuidingProfessorID == p.ID) ? 2 : (AllProposals.Any(AP=>AP.FirstJudgeID == p.ID) || AllProposals.Any(AP2 => AP2.SecondJudgeID == p.ID) ? 1 : 0)))),
                    EducationalGroupTitle = p.EducationalGroup.Name,
                    UniversityRankTitle = p.UniversityRank.Title,
                    LatestDegree = p.LatestDegree,
                    MainSpecialty = p.MainSpecialty,
                    FacultyName = p.Faculty.Name
                }).FirstOrDefault();
        }

        public List<ComboBoxJSON> GetAllProfessors()
        {
            return GetAll().Select(
                p => new ComboBoxJSON {
                    ID = p.ID,
                    Title = p.FirstName + " " + p.LastName
                }
            ).ToList();
        }
    }
}
