namespace ModelsLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _990305_4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Proposal.ProposalStages", "ApproveType", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("Proposal.ProposalStages", "ApproveType");
        }
    }
}
