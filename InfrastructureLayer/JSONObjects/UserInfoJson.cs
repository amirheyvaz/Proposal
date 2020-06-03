using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.JSONObjects
{
    public class UserInfoJson
    {
        public Guid ID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string SocialSecurityNumber { get; set; }

        public string Role { get; set; }

        public string StudentNumber { get; set; }

        public bool HasProposal { get; set; }

        /// <summary>
        /// درجه استاد
        /// 0 = استاد ساده
        /// 1 = داور
        /// 2 = استاد راهنما
        /// 3 = مدیر گروه
        /// 4 = عضو شورا
        /// 5 = رئیس دانشکده
        /// 6 = رئیس دانشگاه
        /// </summary>
        public int ProfessorDegree { set; get; }

        public string EducationalGroupTitle { get; set; }

        public string UniversityRankTitle { get; set; }

        public string LatestDegree { get; set; }

        public string MainSpecialty { get; set; }

        public string FacultyName { get; set; }

    }
}
