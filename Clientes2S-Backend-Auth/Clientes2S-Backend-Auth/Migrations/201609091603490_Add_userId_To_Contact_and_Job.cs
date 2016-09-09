namespace Clientes2S_Backend_Auth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_userId_To_Contact_and_Job : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contacts", "ApplicationUserId", c => c.String(maxLength: 128));
            AddColumn("dbo.Jobs", "ApplicationUserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Contacts", "ApplicationUserId");
            CreateIndex("dbo.Jobs", "ApplicationUserId");
            AddForeignKey("dbo.Contacts", "ApplicationUserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Jobs", "ApplicationUserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Jobs", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Contacts", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Jobs", new[] { "ApplicationUserId" });
            DropIndex("dbo.Contacts", new[] { "ApplicationUserId" });
            DropColumn("dbo.Jobs", "ApplicationUserId");
            DropColumn("dbo.Contacts", "ApplicationUserId");
        }
    }
}
