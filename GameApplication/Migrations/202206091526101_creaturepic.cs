namespace GameApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class creaturepic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Creatures", "CreatureHasPic", c => c.Boolean(nullable: false));
            AddColumn("dbo.Creatures", "PicExtension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Creatures", "PicExtension");
            DropColumn("dbo.Creatures", "CreatureHasPic");
        }
    }
}
