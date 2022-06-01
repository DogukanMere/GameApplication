namespace GameApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class races : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Races",
                c => new
                    {
                        RaceID = c.Int(nullable: false, identity: true),
                        RaceName = c.String(),
                        RaceOffensive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.RaceID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Races");
        }
    }
}
