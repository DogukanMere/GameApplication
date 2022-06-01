namespace GameApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class creaturerace : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Creatures", "RaceID", c => c.Int(nullable: false));
            CreateIndex("dbo.Creatures", "RaceID");
            AddForeignKey("dbo.Creatures", "RaceID", "dbo.Races", "RaceID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Creatures", "RaceID", "dbo.Races");
            DropIndex("dbo.Creatures", new[] { "RaceID" });
            DropColumn("dbo.Creatures", "RaceID");
        }
    }
}
