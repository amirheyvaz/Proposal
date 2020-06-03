namespace ModelsLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _990313_1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Proposal.Proposals", "FirstJudgeApproved", c => c.Boolean(nullable: false));
            AlterColumn("Proposal.Proposals", "SecondJudgeApproved", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("Proposal.Proposals", "SecondJudgeApproved", c => c.Boolean());
            AlterColumn("Proposal.Proposals", "FirstJudgeApproved", c => c.Boolean());
        }
    }
}
