namespace ModelsLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _990226_4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Proposal.Professors", "EducationGradeID", "Proposal.EducationGrades");
            DropIndex("Proposal.EducationalGroups", new[] { "ManagerID" });
            DropIndex("Proposal.Professors", new[] { "EducationGradeID" });
            AlterColumn("Proposal.EducationalGroups", "ManagerID", c => c.Guid());
            AlterColumn("Proposal.Professors", "EducationGradeID", c => c.Guid());
            CreateIndex("Proposal.EducationalGroups", "ManagerID");
            CreateIndex("Proposal.Professors", "EducationGradeID");
            AddForeignKey("Proposal.Professors", "EducationGradeID", "Proposal.EducationGrades", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("Proposal.Professors", "EducationGradeID", "Proposal.EducationGrades");
            DropIndex("Proposal.Professors", new[] { "EducationGradeID" });
            DropIndex("Proposal.EducationalGroups", new[] { "ManagerID" });
            AlterColumn("Proposal.Professors", "EducationGradeID", c => c.Guid(nullable: false));
            AlterColumn("Proposal.EducationalGroups", "ManagerID", c => c.Guid(nullable: false));
            CreateIndex("Proposal.Professors", "EducationGradeID");
            CreateIndex("Proposal.EducationalGroups", "ManagerID");
            AddForeignKey("Proposal.Professors", "EducationGradeID", "Proposal.EducationGrades", "ID", cascadeDelete: true);
        }
    }
}
