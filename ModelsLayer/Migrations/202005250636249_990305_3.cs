namespace ModelsLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _990305_3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Proposal.ProposalStatuses", "IsDefault", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Proposal.ProposalStatuses", "IsDefault");
        }
    }
}
