namespace ModelsLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _990305 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Proposal.ProposalFiles",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        File = c.Binary(),
                        ProposalID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("Proposal.Proposals", t => t.ProposalID, cascadeDelete: true)
                .Index(t => t.ProposalID);
            
            AddColumn("Proposal.ProposalStages", "Order", c => c.Int(nullable: false));
            AddColumn("Proposal.ProposalStages", "IsFirst", c => c.Boolean(nullable: false));
            AddColumn("Proposal.ProposalStages", "IsLast", c => c.Boolean(nullable: false));
            AlterColumn("Proposal.Proposals", "DefenceMeetingTime", c => c.DateTime());
            DropColumn("Proposal.Proposals", "ProposalFile");
        }
        
        public override void Down()
        {
            AddColumn("Proposal.Proposals", "ProposalFile", c => c.Binary(nullable: false));
            DropForeignKey("Proposal.ProposalFiles", "ProposalID", "Proposal.Proposals");
            DropIndex("Proposal.ProposalFiles", new[] { "ProposalID" });
            AlterColumn("Proposal.Proposals", "DefenceMeetingTime", c => c.DateTime(nullable: false));
            DropColumn("Proposal.ProposalStages", "IsLast");
            DropColumn("Proposal.ProposalStages", "IsFirst");
            DropColumn("Proposal.ProposalStages", "Order");
            DropTable("Proposal.ProposalFiles");
        }
    }
}
