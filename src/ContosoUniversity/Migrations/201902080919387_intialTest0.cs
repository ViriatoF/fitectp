namespace ContosoUniversity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class intialTest0 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FilePath",
                c => new
                    {
                        FilePathId = c.Int(nullable: false, identity: true),
                        FileName = c.String(maxLength: 255),
                        FileType = c.Int(nullable: false),
                        PersonID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FilePathId)
                .ForeignKey("dbo.Person", t => t.PersonID, cascadeDelete: true)
                .Index(t => t.PersonID);
            
            CreateTable(
                "dbo.PersonRegisterVM",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LastName = c.String(nullable: false, maxLength: 50),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        Role = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Person", "Email", c => c.String(nullable: false));
            AddColumn("dbo.Person", "Password", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FilePath", "PersonID", "dbo.Person");
            DropIndex("dbo.FilePath", new[] { "PersonID" });
            DropColumn("dbo.Person", "Password");
            DropColumn("dbo.Person", "Email");
            DropTable("dbo.PersonRegisterVM");
            DropTable("dbo.FilePath");
        }
    }
}
