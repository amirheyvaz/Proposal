namespace ModelsLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _990330 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Proposal.ProposalComments", "Content", c => c.String(nullable: false));
            AddColumn("Proposal.Proposals", "BigChangesForJudges", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("Proposal.Proposals", "BigChangesForJudges");
            DropColumn("Proposal.ProposalComments", "Content");
        }
    }
}
