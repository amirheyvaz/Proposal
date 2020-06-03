namespace ModelsLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _990313 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Proposal.Proposals", "FirstJudgeApproved", c => c.Boolean());
            AddColumn("Proposal.Proposals", "SecondJudgeApproved", c => c.Boolean());
            DropColumn("Proposal.Proposals", "BothJudgesApproved");
        }
        
        public override void Down()
        {
            AddColumn("Proposal.Proposals", "BothJudgesApproved", c => c.Boolean(nullable: false));
            DropColumn("Proposal.Proposals", "SecondJudgeApproved");
            DropColumn("Proposal.Proposals", "FirstJudgeApproved");
        }
    }
}
