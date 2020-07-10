namespace ModelsLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _990331 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Proposal.Proposals", "DefenceMeetingHour", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("Proposal.Proposals", "DefenceMeetingHour");
        }
    }
}
