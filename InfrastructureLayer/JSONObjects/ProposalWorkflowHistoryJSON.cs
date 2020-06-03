using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.JSONObjects
{
    public class ProposalWorkflowHistoryJSON
    {
        public Guid ID { get; set; }

        public Guid ProposalID { get; set; }

        public string ProposalName { get; set; }

        public Guid OperationID { get; set; }

        public string OperationTitle { get; set; }

        public bool OccuredByStudent { get; set; }

        public Guid OccuredByPersonID { get; set; }

        public string OccuredByPersonName { get; set; }
    }
}
