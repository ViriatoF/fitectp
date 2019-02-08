namespace ContosoUniversity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialTest : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Lesson",
                c => new
                    {
                        LessonId = c.Int(nullable: false, identity: true),
                        InstructorID = c.Int(nullable: false),
                        HourStart = c.Int(nullable: false),
                        HourEnd = c.Int(nullable: false),
                        DateFirstCourse = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.LessonId)
                .ForeignKey("dbo.Person", t => t.InstructorID, cascadeDelete: true)
                .Index(t => t.InstructorID);
            
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
            AddColumn("dbo.Enrollment", "Lesson_LessonId", c => c.Int());
            CreateIndex("dbo.Enrollment", "Lesson_LessonId");
            AddForeignKey("dbo.Enrollment", "Lesson_LessonId", "dbo.Lesson", "LessonId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Lesson", "InstructorID", "dbo.Person");
            DropForeignKey("dbo.Enrollment", "Lesson_LessonId", "dbo.Lesson");
            DropIndex("dbo.Lesson", new[] { "InstructorID" });
            DropIndex("dbo.Enrollment", new[] { "Lesson_LessonId" });
            DropColumn("dbo.Enrollment", "Lesson_LessonId");
            DropColumn("dbo.Person", "Password");
            DropColumn("dbo.Person", "Email");
            DropTable("dbo.PersonRegisterVM");
            DropTable("dbo.Lesson");
        }
    }
}
