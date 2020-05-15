namespace ModelsLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _990226_2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Proposal.Students", "EducationGradeID", "Proposal.EducationGrades");
            DropIndex("Proposal.Students", new[] { "EducationGradeID" });
            AddColumn("Proposal.Professors", "EducationGradeID", c => c.Guid(nullable: false));
            CreateIndex("Proposal.Professors", "EducationGradeID");
            AddForeignKey("Proposal.Professors", "EducationGradeID", "Proposal.EducationGrades", "ID", cascadeDelete: true);
            DropColumn("Proposal.Students", "EducationGradeID");
        }
        
        public override void Down()
        {
            AddColumn("Proposal.Students", "EducationGradeID", c => c.Guid(nullable: false));
            DropForeignKey("Proposal.Professors", "EducationGradeID", "Proposal.EducationGrades");
            DropIndex("Proposal.Professors", new[] { "EducationGradeID" });
            DropColumn("Proposal.Professors", "EducationGradeID");
            CreateIndex("Proposal.Students", "EducationGradeID");
            AddForeignKey("Proposal.Students", "EducationGradeID", "Proposal.EducationGrades", "ID", cascadeDelete: true);
        }
    }
}
