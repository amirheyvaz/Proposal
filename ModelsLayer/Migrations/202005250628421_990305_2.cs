namespace ModelsLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _990305_2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Proposal.Proposals", "BothJudgesApproved", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("Proposal.Proposals", "BothJudgesApproved", c => c.Boolean());
        }
    }
}
