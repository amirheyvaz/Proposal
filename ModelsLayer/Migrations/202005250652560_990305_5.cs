namespace ModelsLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _990305_5 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Proposal.ProposalStages", "ApproveType", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("Proposal.ProposalStages", "ApproveType", c => c.Boolean());
        }
    }
}
