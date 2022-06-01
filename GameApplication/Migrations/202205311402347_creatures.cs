namespace GameApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class creatures : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Creatures",
                c => new
                    {
                        CreatureID = c.Int(nullable: false, identity: true),
                        CreatureName = c.String(),
                        CreaturePower = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CreatureID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Creatures");
        }
    }
}
