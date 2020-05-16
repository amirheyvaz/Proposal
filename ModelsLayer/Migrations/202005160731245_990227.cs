namespace ModelsLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _990227 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Proposal.Professors", "EducationGradeID", "Proposal.EducationGrades");
            DropIndex("Proposal.Professors", new[] { "EducationGradeID" });
            CreateTable(
                "Proposal.ProposalComments",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        ProposalID = c.Guid(nullable: false),
                        OccuranceDate = c.DateTime(nullable: false),
                        OccuredByStudentID = c.Guid(),
                        OccuredByProfessorID = c.Guid(),
                        ProposalStageID = c.Guid(nullable: false),
                        ImportanceLevel = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("Proposal.Professors", t => t.OccuredByProfessorID)
                .ForeignKey("Proposal.Students", t => t.OccuredByStudentID)
                .ForeignKey("Proposal.Proposals", t => t.ProposalID, cascadeDelete: true)
                .ForeignKey("Proposal.ProposalStages", t => t.ProposalStageID, cascadeDelete: true)
                .Index(t => t.ProposalID)
                .Index(t => t.OccuredByStudentID)
                .Index(t => t.OccuredByProfessorID)
                .Index(t => t.ProposalStageID);
            
            CreateTable(
                "Proposal.Proposals",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        StudentID = c.Guid(nullable: false),
                        LatinName = c.String(),
                        ResearchTypeID = c.Guid(nullable: false),
                        FirstJudgeID = c.Guid(nullable: false),
                        SecondJudgeID = c.Guid(nullable: false),
                        ProposalStageID = c.Guid(nullable: false),
                        LatestOperation = c.String(),
                        ProposalStatusID = c.Guid(nullable: false),
                        IsFinalApprove = c.Boolean(nullable: false),
                        DefenceMeetingTime = c.DateTime(nullable: false),
                        ProposalFile = c.Binary(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("Proposal.ProposalStages", t => t.ProposalStageID)
                .ForeignKey("Proposal.ProposalStatuses", t => t.ProposalStatusID, cascadeDelete: true)
                .ForeignKey("Proposal.ResearchTypes", t => t.ResearchTypeID, cascadeDelete: true)
                .ForeignKey("Proposal.Students", t => t.StudentID, cascadeDelete: true)
                .ForeignKey("Proposal.Professors", t => t.FirstJudgeID)
                .ForeignKey("Proposal.Professors", t => t.SecondJudgeID)
                .Index(t => t.StudentID)
                .Index(t => t.ResearchTypeID)
                .Index(t => t.FirstJudgeID)
                .Index(t => t.SecondJudgeID)
                .Index(t => t.ProposalStageID)
                .Index(t => t.ProposalStatusID);
            
            CreateTable(
                "Proposal.ProposalKeywords",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Title = c.String(),
                        isLatin = c.Boolean(nullable: false),
                        ProposalID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("Proposal.Proposals", t => t.ProposalID, cascadeDelete: true)
                .Index(t => t.ProposalID);
            
            CreateTable(
                "Proposal.ProposalStages",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "Proposal.ProposalStatuses",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "Proposal.ResearchTypes",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "Proposal.ProposalWorkflowHistories",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        ProposalID = c.Guid(nullable: false),
                        ProposalOperationID = c.Guid(nullable: false),
                        OccuranceDate = c.DateTime(nullable: false),
                        OccuredByProfessorID = c.Guid(),
                        OccuredByStudentID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("Proposal.Professors", t => t.OccuredByProfessorID)
                .ForeignKey("Proposal.Students", t => t.OccuredByStudentID)
                .ForeignKey("Proposal.Proposals", t => t.ProposalID, cascadeDelete: true)
                .ForeignKey("Proposal.ProposalOperations", t => t.ProposalOperationID, cascadeDelete: true)
                .Index(t => t.ProposalID)
                .Index(t => t.ProposalOperationID)
                .Index(t => t.OccuredByProfessorID)
                .Index(t => t.OccuredByStudentID);
            
            CreateTable(
                "Proposal.ProposalOperations",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("Proposal.Students", "EducationGradeID", c => c.Guid());
            AddColumn("Proposal.Students", "ResearchType_ID", c => c.Guid());
            CreateIndex("Proposal.Students", "EducationGradeID");
            CreateIndex("Proposal.Students", "ResearchType_ID");
            AddForeignKey("Proposal.Students", "EducationGradeID", "Proposal.EducationGrades", "ID");
            AddForeignKey("Proposal.Students", "ResearchType_ID", "Proposal.ResearchTypes", "ID");
            DropColumn("Proposal.Professors", "EducationGradeID");
        }
        
        public override void Down()
        {
            AddColumn("Proposal.Professors", "EducationGradeID", c => c.Guid());
            DropForeignKey("Proposal.Proposals", "SecondJudgeID", "Proposal.Professors");
            DropForeignKey("Proposal.Proposals", "FirstJudgeID", "Proposal.Professors");
            DropForeignKey("Proposal.ProposalWorkflowHistories", "ProposalOperationID", "Proposal.ProposalOperations");
            DropForeignKey("Proposal.ProposalWorkflowHistories", "ProposalID", "Proposal.Proposals");
            DropForeignKey("Proposal.ProposalWorkflowHistories", "OccuredByStudentID", "Proposal.Students");
            DropForeignKey("Proposal.ProposalWorkflowHistories", "OccuredByProfessorID", "Proposal.Professors");
            DropForeignKey("Proposal.Proposals", "StudentID", "Proposal.Students");
            DropForeignKey("Proposal.Proposals", "ResearchTypeID", "Proposal.ResearchTypes");
            DropForeignKey("Proposal.Students", "ResearchType_ID", "Proposal.ResearchTypes");
            DropForeignKey("Proposal.Proposals", "ProposalStatusID", "Proposal.ProposalStatuses");
            DropForeignKey("Proposal.Proposals", "ProposalStageID", "Proposal.ProposalStages");
            DropForeignKey("Proposal.ProposalComments", "ProposalStageID", "Proposal.ProposalStages");
            DropForeignKey("Proposal.ProposalKeywords", "ProposalID", "Proposal.Proposals");
            DropForeignKey("Proposal.ProposalComments", "ProposalID", "Proposal.Proposals");
            DropForeignKey("Proposal.ProposalComments", "OccuredByStudentID", "Proposal.Students");
            DropForeignKey("Proposal.Students", "EducationGradeID", "Proposal.EducationGrades");
            DropForeignKey("Proposal.ProposalComments", "OccuredByProfessorID", "Proposal.Professors");
            DropIndex("Proposal.ProposalWorkflowHistories", new[] { "OccuredByStudentID" });
            DropIndex("Proposal.ProposalWorkflowHistories", new[] { "OccuredByProfessorID" });
            DropIndex("Proposal.ProposalWorkflowHistories", new[] { "ProposalOperationID" });
            DropIndex("Proposal.ProposalWorkflowHistories", new[] { "ProposalID" });
            DropIndex("Proposal.ProposalKeywords", new[] { "ProposalID" });
            DropIndex("Proposal.Proposals", new[] { "ProposalStatusID" });
            DropIndex("Proposal.Proposals", new[] { "ProposalStageID" });
            DropIndex("Proposal.Proposals", new[] { "SecondJudgeID" });
            DropIndex("Proposal.Proposals", new[] { "FirstJudgeID" });
            DropIndex("Proposal.Proposals", new[] { "ResearchTypeID" });
            DropIndex("Proposal.Proposals", new[] { "StudentID" });
            DropIndex("Proposal.Students", new[] { "ResearchType_ID" });
            DropIndex("Proposal.Students", new[] { "EducationGradeID" });
            DropIndex("Proposal.ProposalComments", new[] { "ProposalStageID" });
            DropIndex("Proposal.ProposalComments", new[] { "OccuredByProfessorID" });
            DropIndex("Proposal.ProposalComments", new[] { "OccuredByStudentID" });
            DropIndex("Proposal.ProposalComments", new[] { "ProposalID" });
            DropColumn("Proposal.Students", "ResearchType_ID");
            DropColumn("Proposal.Students", "EducationGradeID");
            DropTable("Proposal.ProposalOperations");
            DropTable("Proposal.ProposalWorkflowHistories");
            DropTable("Proposal.ResearchTypes");
            DropTable("Proposal.ProposalStatuses");
            DropTable("Proposal.ProposalStages");
            DropTable("Proposal.ProposalKeywords");
            DropTable("Proposal.Proposals");
            DropTable("Proposal.ProposalComments");
            CreateIndex("Proposal.Professors", "EducationGradeID");
            AddForeignKey("Proposal.Professors", "EducationGradeID", "Proposal.EducationGrades", "ID");
        }
    }
}
