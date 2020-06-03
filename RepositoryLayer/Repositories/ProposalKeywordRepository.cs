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
    public class ProposalKeywordRepository : GenericRepository<ProposalKeyword, Guid>, IProposalKeywordRepository
    {
        public ProposalKeywordRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public bool AddProposalKeyword(List<string> keys , Guid ProposalID)
        {
            
                try
                {
                    foreach(string a in keys)
                    {
                        if (String.IsNullOrWhiteSpace(a))
                        {
                            continue;
                        }
                        ProposalKeyword k = new ProposalKeyword();
                        k.ID = Guid.NewGuid();
                        k.ProposalID = ProposalID;
                        k.Title = a;
                        char ch = a.ToCharArray()[0];
                        if((ch >= 65 && ch <= 90) || (ch >= 97 && ch <= 122))
                        {
                            k.isLatin = true;
                        }
                        else
                        {
                            k.isLatin = false;
                        }
                        Add(k);
                    }
                    Commit();
        

                    return true;
                }
                catch (Exception e)
                {
                  
                    return false;
                }
            

        }

        public List<string> GetProposalKeywords(Guid ProposalID)
        {
            return SelectBy(p => p.ProposalID == ProposalID).Select(q => q.Title).ToList();
        }
    }
}
