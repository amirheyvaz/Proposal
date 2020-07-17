using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Proposal.Core;
using RepositoryLayer.Interfaces;
using InfrastructureLayer.JSONObjects;
using ModelsLayer.Models;

namespace Test.Proposal
{
    [TestFixture]
    public class ProposalTest
    {
        [TestCase(null ,"0020881029" , false ,Description = "Null Input")]
        [TestCase(null , "1235556230", false, Description = "Wrong Username")]
        [Order(1)]
        public void Proposal_SubmitProposal(ProposalGeneralInfoJSON p, string Username , bool expect)
        {
            var ProposalRep = IocConfig.Container.GetInstance<IProposalRepository>();
            Assert.DoesNotThrow(() => ProposalRep.SubmitProposal(p , Username));
            Assert.That(ProposalRep.SubmitProposal(p, Username) , Is.EqualTo(expect));
        }


        [TestCase]
        [Order(2)]
        public void Proposal_Submit_ResearchTypeIsInvalid()
        {
            var ProposalRep = IocConfig.Container.GetInstance<IProposalRepository>();
            ProposalGeneralInfoJSON p = new ProposalGeneralInfoJSON()
            {
                Name = "Unit test proposal",
                LatinName = "Unit Test Proposal",
                ReseachTypeID = Guid.Parse("F4234BCF-79AE-48EA-B7BE-C4BA0EF7E37B"),
                keywords = new List<string>() { "test1", "test2" },
                FileID = Guid.Parse("F4234BCF-79AE-48EA-B7BE-C4BA0EF7E37B")
            };
            Assert.DoesNotThrow(() => {
                Assert.That(ProposalRep.SubmitProposal(p, "0020881029"), Is.EqualTo(false));
            });
        }

        [TestCase]
        [Order(3)]
        public void Proposal_Submit_ProposalFileIsInvalid()
        {
            var ProposalRep = IocConfig.Container.GetInstance<IProposalRepository>();
            ProposalGeneralInfoJSON p = new ProposalGeneralInfoJSON()
            {
                Name = "Unit test proposal",
                LatinName = "Unit Test Proposal",
                ReseachTypeID = Guid.Parse("232B3133-79BF-4A9B-BF58-ABE941C05860"),
                keywords = new List<string>() { "test1", "test2" },
                FileID = Guid.Parse("232B3133-79BF-4A9B-BF58-ABE941C05860")
            };
            Assert.DoesNotThrow(() => {
                Assert.That(ProposalRep.SubmitProposal(p, "0020881029"), Is.EqualTo(false));
            });
        }

        [TestCase]
        [Order(4)]
        public void Proposal_RealSubmit()
        {
            var ProposalRep = IocConfig.Container.GetInstance<IProposalRepository>();
            ProposalGeneralInfoJSON p = new ProposalGeneralInfoJSON()
            {
                Name = "Unit test proposal" + DateTime.Now.ToString(),
                LatinName = "Unit Test Proposal" + DateTime.Now.ToString(),
                ReseachTypeID = Guid.Parse("232B3133-79BF-4A9B-BF58-ABE941C05860"),
                keywords = new List<string>() { "test1" , "test2"},
                FileID = Guid.Parse("F4234BCF-79AE-48EA-B7BE-C4BA0EF7E37B")
            };
            Assert.DoesNotThrow(() => {
                Assert.That(ProposalRep.SubmitProposal(p, "0020881029"), Is.EqualTo(true));
            });
        }

        [TestCase]
        [Order(5)]
        public void Proposal_ApproveProposal()
        {
            var ProposalRep = IocConfig.Container.GetInstance<IProposalRepository>();
            ProposalComment com = new ProposalComment() {
                Content = "Unit Test",
                ImportanceLevel = null
            };
            //Invalid Proposal
            Assert.IsNotEmpty(ProposalRep.ApproveProposal(Guid.Parse("8ff58299-98e4-4216-93de-0fd31b549ee0"), Guid.Parse("8ff58299-98e4-4216-93de-0fd31b549ee0"), com));
            //Invalid Judge
            Assert.IsNotEmpty(ProposalRep.ApproveProposal(Guid.Parse("47b867b9-2111-44a0-9754-a75696b7ed5f"), Guid.Parse("47b867b9-2111-44a0-9754-a75696b7ed5f"), com));
            //Stage 3
            Assert.DoesNotThrow(() => {
                Assert.IsEmpty(ProposalRep.ApproveProposal(Guid.Parse("de7a36a7-d4a7-4edf-927a-f34e8218a119"), Guid.Parse("65fa2183-6839-4c53-981d-f1c733551a71"), com));
            });
            //Stage 5
            Assert.DoesNotThrow(() => {
                Assert.IsEmpty(ProposalRep.ApproveProposal(Guid.Parse("49af2112-fe58-4f3c-be9b-2084e5cdaf86"), Guid.Parse("65fa2183-6839-4c53-981d-f1c733551a71"), com));
            });
            //Stage 6
            Assert.DoesNotThrow(() => {
                Assert.IsEmpty(ProposalRep.ApproveProposal(Guid.Parse("ea8ad633-8da8-4aeb-a1f3-071aca2bee55"), Guid.Parse("65fa2183-6839-4c53-981d-f1c733551a71"), com));
            });
            //Stage 8
            Assert.DoesNotThrow(() => {
                Assert.IsEmpty(ProposalRep.ApproveProposal(Guid.Parse("659f8aa7-3542-47c0-bc13-b58b17a8ddf9"), Guid.Parse("65FA2183-6839-4C53-981D-F1C733551A71"), com));
            });
            //Stage 9
            Assert.DoesNotThrow(() => {
                Assert.IsEmpty(ProposalRep.ApproveProposal(Guid.Parse("d7597512-6e08-4f72-a724-7d1b13001496"), Guid.Parse("65fa2183-6839-4c53-981d-f1c733551a71"), com));
            });
            //Stage 10
            Assert.DoesNotThrow(() => {
                Assert.IsEmpty(ProposalRep.ApproveProposal(Guid.Parse("a8cdb3c3-0663-4a06-a010-9f716496cf54"), Guid.Parse("65fa2183-6839-4c53-981d-f1c733551a71"), com));
            });
            //Stage 12
            Assert.DoesNotThrow(() => {
                Assert.IsEmpty(ProposalRep.ApproveProposal(Guid.Parse("ac221616-793c-480e-a7fd-a1a0a8e08781"), Guid.Parse("65FA2183-6839-4C53-981D-F1C733551A71"), com));
            });
        }
    }
}
