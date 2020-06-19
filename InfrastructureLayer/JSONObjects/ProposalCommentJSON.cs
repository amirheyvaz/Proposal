using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.JSONObjects
{
    public class ProposalCommentJSON
    {
        public Guid ID { get; set; }

        public Guid ProposalID { get; set; }

        public string ProposalTitle { get; set; }

        public string OccuranceDate { get; set; }

        public Guid OccuredByPersonID { get; set; }

        public string OccuredByPersonTitle { get; set; }

        public bool OccuredByStudent { get; set; }

        public Guid StageID { get; set; }

        public string StageTitle { get; set; }

        public int ImportanceLevel { get; set; }

        public string Content { get; set; }
    }
}
