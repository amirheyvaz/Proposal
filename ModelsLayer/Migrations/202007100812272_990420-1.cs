namespace ModelsLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _9904201 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Proposal.ProposalFiles", "ProposalID", "Proposal.Proposals");
            DropIndex("Proposal.ProposalFiles", new[] { "ProposalID" });
            AlterColumn("Proposal.ProposalFiles", "ProposalID", c => c.Guid());
            CreateIndex("Proposal.ProposalFiles", "ProposalID");
            AddForeignKey("Proposal.ProposalFiles", "ProposalID", "Proposal.Proposals", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("Proposal.ProposalFiles", "ProposalID", "Proposal.Proposals");
            DropIndex("Proposal.ProposalFiles", new[] { "ProposalID" });
            AlterColumn("Proposal.ProposalFiles", "ProposalID", c => c.Guid(nullable: false));
            CreateIndex("Proposal.ProposalFiles", "ProposalID");
            AddForeignKey("Proposal.ProposalFiles", "ProposalID", "Proposal.Proposals", "ID", cascadeDelete: true);
        }
    }
}
