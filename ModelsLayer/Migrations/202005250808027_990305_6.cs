namespace ModelsLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _990305_6 : DbMigration
    {
        public override void Up()
        {
            DropIndex("Proposal.Proposals", new[] { "FirstJudgeID" });
            DropIndex("Proposal.Proposals", new[] { "SecondJudgeID" });
            AlterColumn("Proposal.Proposals", "FirstJudgeID", c => c.Guid());
            AlterColumn("Proposal.Proposals", "SecondJudgeID", c => c.Guid());
            CreateIndex("Proposal.Proposals", "FirstJudgeID");
            CreateIndex("Proposal.Proposals", "SecondJudgeID");
        }
        
        public override void Down()
        {
            DropIndex("Proposal.Proposals", new[] { "SecondJudgeID" });
            DropIndex("Proposal.Proposals", new[] { "FirstJudgeID" });
            AlterColumn("Proposal.Proposals", "SecondJudgeID", c => c.Guid(nullable: false));
            AlterColumn("Proposal.Proposals", "FirstJudgeID", c => c.Guid(nullable: false));
            CreateIndex("Proposal.Proposals", "SecondJudgeID");
            CreateIndex("Proposal.Proposals", "FirstJudgeID");
        }
    }
}
