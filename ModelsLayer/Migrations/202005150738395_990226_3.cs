namespace ModelsLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _990226_3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Proposal.Universities", "ManagerID", "Proposal.Professors");
            DropIndex("Proposal.Faculties", new[] { "ManagerID" });
            DropIndex("Proposal.Universities", new[] { "ManagerID" });
            AlterColumn("Proposal.Faculties", "ManagerID", c => c.Guid());
            AlterColumn("Proposal.Universities", "ManagerID", c => c.Guid());
            CreateIndex("Proposal.Faculties", "ManagerID");
            CreateIndex("Proposal.Universities", "ManagerID");
            AddForeignKey("Proposal.Universities", "ManagerID", "Proposal.Professors", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("Proposal.Universities", "ManagerID", "Proposal.Professors");
            DropIndex("Proposal.Universities", new[] { "ManagerID" });
            DropIndex("Proposal.Faculties", new[] { "ManagerID" });
            AlterColumn("Proposal.Universities", "ManagerID", c => c.Guid(nullable: false));
            AlterColumn("Proposal.Faculties", "ManagerID", c => c.Guid(nullable: false));
            CreateIndex("Proposal.Universities", "ManagerID");
            CreateIndex("Proposal.Faculties", "ManagerID");
            AddForeignKey("Proposal.Universities", "ManagerID", "Proposal.Professors", "ID", cascadeDelete: true);
        }
    }
}
