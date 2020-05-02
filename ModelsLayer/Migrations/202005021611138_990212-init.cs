namespace ModelsLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _990212init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Proposal.EducationalGroups",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        FacultyID = c.Guid(nullable: false),
                        ManagerID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("Proposal.Faculties", t => t.FacultyID, cascadeDelete: true)
                .ForeignKey("Proposal.Professors", t => t.ManagerID)
                .Index(t => t.FacultyID)
                .Index(t => t.ManagerID);
            
            CreateTable(
                "Proposal.Faculties",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        ManagerID = c.Guid(nullable: false),
                        UniversityID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("Proposal.Universities", t => t.UniversityID, cascadeDelete: true)
                .ForeignKey("Proposal.Professors", t => t.ManagerID)
                .Index(t => t.ManagerID)
                .Index(t => t.UniversityID);
            
            CreateTable(
                "Proposal.Professors",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        SocialSecurityNumber = c.String(),
                        BirthCertificateNumber = c.String(),
                        Email = c.String(),
                        LatestDegree = c.String(),
                        MainSpecialty = c.String(),
                        IsCouncilMember = c.Boolean(nullable: false),
                        GoogleCalendarID = c.String(),
                        UniversityRankID = c.Guid(nullable: false),
                        FacultyID = c.Guid(nullable: false),
                        EducationalGroupID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("Proposal.UniversityRanks", t => t.UniversityRankID, cascadeDelete: true)
                .ForeignKey("Proposal.Faculties", t => t.FacultyID)
                .ForeignKey("Proposal.EducationalGroups", t => t.EducationalGroupID)
                .Index(t => t.UniversityRankID)
                .Index(t => t.FacultyID)
                .Index(t => t.EducationalGroupID);
            
            CreateTable(
                "Proposal.Students",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        StudentNumber = c.String(nullable: false),
                        EnteranceYear = c.Int(nullable: false),
                        Major = c.String(),
                        Orientation = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        SocialSecurityNumber = c.String(),
                        BirthCertificateNumber = c.String(),
                        Email = c.String(),
                        PeriodTypeID = c.Guid(nullable: false),
                        EducationGradeID = c.Guid(nullable: false),
                        FirstGuidingProfessorID = c.Guid(nullable: false),
                        SecondGuidingProfessorID = c.Guid(nullable: false),
                        FacultyID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("Proposal.EducationGrades", t => t.EducationGradeID, cascadeDelete: true)
                .ForeignKey("Proposal.Faculties", t => t.FacultyID, cascadeDelete: true)
                .ForeignKey("Proposal.PeriodTypes", t => t.PeriodTypeID, cascadeDelete: true)
                .ForeignKey("Proposal.Professors", t => t.FirstGuidingProfessorID)
                .ForeignKey("Proposal.Professors", t => t.SecondGuidingProfessorID)
                .Index(t => t.PeriodTypeID)
                .Index(t => t.EducationGradeID)
                .Index(t => t.FirstGuidingProfessorID)
                .Index(t => t.SecondGuidingProfessorID)
                .Index(t => t.FacultyID);
            
            CreateTable(
                "Proposal.EducationGrades",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Title = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "Proposal.PeriodTypes",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Title = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "Proposal.Universities",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        ManagerID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("Proposal.Professors", t => t.ManagerID, cascadeDelete: true)
                .Index(t => t.ManagerID);
            
            CreateTable(
                "Proposal.UniversityRanks",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Title = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Proposal.Professors", "EducationalGroupID", "Proposal.EducationalGroups");
            DropForeignKey("Proposal.EducationalGroups", "ManagerID", "Proposal.Professors");
            DropForeignKey("Proposal.Professors", "FacultyID", "Proposal.Faculties");
            DropForeignKey("Proposal.Faculties", "ManagerID", "Proposal.Professors");
            DropForeignKey("Proposal.Professors", "UniversityRankID", "Proposal.UniversityRanks");
            DropForeignKey("Proposal.Universities", "ManagerID", "Proposal.Professors");
            DropForeignKey("Proposal.Faculties", "UniversityID", "Proposal.Universities");
            DropForeignKey("Proposal.Students", "SecondGuidingProfessorID", "Proposal.Professors");
            DropForeignKey("Proposal.Students", "FirstGuidingProfessorID", "Proposal.Professors");
            DropForeignKey("Proposal.Students", "PeriodTypeID", "Proposal.PeriodTypes");
            DropForeignKey("Proposal.Students", "FacultyID", "Proposal.Faculties");
            DropForeignKey("Proposal.Students", "EducationGradeID", "Proposal.EducationGrades");
            DropForeignKey("Proposal.EducationalGroups", "FacultyID", "Proposal.Faculties");
            DropIndex("Proposal.Universities", new[] { "ManagerID" });
            DropIndex("Proposal.Students", new[] { "FacultyID" });
            DropIndex("Proposal.Students", new[] { "SecondGuidingProfessorID" });
            DropIndex("Proposal.Students", new[] { "FirstGuidingProfessorID" });
            DropIndex("Proposal.Students", new[] { "EducationGradeID" });
            DropIndex("Proposal.Students", new[] { "PeriodTypeID" });
            DropIndex("Proposal.Professors", new[] { "EducationalGroupID" });
            DropIndex("Proposal.Professors", new[] { "FacultyID" });
            DropIndex("Proposal.Professors", new[] { "UniversityRankID" });
            DropIndex("Proposal.Faculties", new[] { "UniversityID" });
            DropIndex("Proposal.Faculties", new[] { "ManagerID" });
            DropIndex("Proposal.EducationalGroups", new[] { "ManagerID" });
            DropIndex("Proposal.EducationalGroups", new[] { "FacultyID" });
            DropTable("Proposal.UniversityRanks");
            DropTable("Proposal.Universities");
            DropTable("Proposal.PeriodTypes");
            DropTable("Proposal.EducationGrades");
            DropTable("Proposal.Students");
            DropTable("Proposal.Professors");
            DropTable("Proposal.Faculties");
            DropTable("Proposal.EducationalGroups");
        }
    }
}
