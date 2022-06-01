namespace GameApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dungeoncreatures : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Dungeons",
                c => new
                    {
                        DungeonID = c.Int(nullable: false, identity: true),
                        DungeonName = c.String(),
                        DungeonLocation = c.String(),
                    })
                .PrimaryKey(t => t.DungeonID);
            
            CreateTable(
                "dbo.DungeonCreatures",
                c => new
                    {
                        Dungeon_DungeonID = c.Int(nullable: false),
                        Creature_CreatureID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Dungeon_DungeonID, t.Creature_CreatureID })
                .ForeignKey("dbo.Dungeons", t => t.Dungeon_DungeonID, cascadeDelete: true)
                .ForeignKey("dbo.Creatures", t => t.Creature_CreatureID, cascadeDelete: true)
                .Index(t => t.Dungeon_DungeonID)
                .Index(t => t.Creature_CreatureID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DungeonCreatures", "Creature_CreatureID", "dbo.Creatures");
            DropForeignKey("dbo.DungeonCreatures", "Dungeon_DungeonID", "dbo.Dungeons");
            DropIndex("dbo.DungeonCreatures", new[] { "Creature_CreatureID" });
            DropIndex("dbo.DungeonCreatures", new[] { "Dungeon_DungeonID" });
            DropTable("dbo.DungeonCreatures");
            DropTable("dbo.Dungeons");
        }
    }
}
