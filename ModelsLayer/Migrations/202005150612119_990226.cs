namespace ModelsLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _990226 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Proposal.Professors", "Password", c => c.String(nullable: false));
            AddColumn("Proposal.Students", "Password", c => c.String(nullable: false));
            AlterColumn("Proposal.Professors", "SocialSecurityNumber", c => c.String(nullable: false));
            AlterColumn("Proposal.Students", "SocialSecurityNumber", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("Proposal.Students", "SocialSecurityNumber", c => c.String());
            AlterColumn("Proposal.Professors", "SocialSecurityNumber", c => c.String());
            DropColumn("Proposal.Students", "Password");
            DropColumn("Proposal.Professors", "Password");
        }
    }
}
