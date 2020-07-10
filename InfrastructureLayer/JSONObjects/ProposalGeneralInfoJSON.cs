using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.JSONObjects
{
    public class ProposalGeneralInfoJSON
    {
        //public string File { get; set; }

        public Guid FileID { get; set; }

        public string Name { get; set; }

        public string LatinName { get; set; }

        public List<string> keywords { get; set; }

        public Guid ReseachTypeID { get; set; }
    }
}
