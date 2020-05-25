using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.JSONObjects
{
    public class KeywordJSON
    {
        public string Title { get; set; }

        public Guid ID { get; set; }

        public Guid ProposalID { get; set; }

        public bool IsLatin { get; set; }

    }
}
