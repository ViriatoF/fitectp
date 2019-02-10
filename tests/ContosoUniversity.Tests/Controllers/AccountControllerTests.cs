using ContosoUniversity.Controllers;
using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using ContosoUniversity.Tests.Tools;
using ContosoUniversity.ViewModels;
using Moq;
using NUnit.Framework;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.IO;

namespace ContosoUniversity.Tests.Controllers
{
    class AccountControllerTests : IntegrationTestsBase
    {
        private MockHttpContextWrapper httpContext;
        private AccountController controllerToTest;
        private SchoolContext dbContext;


        [SetUp]
        public void Initialize()
        {
            httpContext = new MockHttpContextWrapper();
            controllerToTest = new AccountController();
            controllerToTest.ControllerContext = new ControllerContext(httpContext.Context.Object, new RouteData(), controllerToTest);
            dbContext = new DAL.SchoolContext(this.ConnectionString);
            controllerToTest.DbContext = dbContext;
        }


        [Test]
        public void Register_Account_With_Exist_Mail_Fail()
        {

            PersonRegisterVM newAccount = new PersonRegisterVM();
            newAccount.FirstName = "yahya";
            newAccount.LastName = "khoder";
            newAccount.Role = "1"; // to test the registring of student or ="2" for isntructor
            newAccount.Email = "yahya@gmail.com";
            newAccount.Password = "InvalidPassword";
            newAccount.ImagePath = "~/si.png";
            newAccount.ImageType = "image/png";

            BAL.StudentBAL bal = new BAL.StudentBAL(dbContext);
            
            bal.AccountExist(newAccount);

            Student st = this.dbContext.Students.FirstOrDefault(e => e.Email == newAccount.Email);



            Assert.That(st != null);
        }

     

        [Test]
        public void Register_Account_With_Login_And_Good_Password_Success()
        {
            PersonRegisterVM newAccount = new PersonRegisterVM();
            newAccount.FirstName = "test1";
            newAccount.LastName = "test1";
            newAccount.Role = "1";
            newAccount.Email = "test1@yahoo.fr";
            newAccount.Password = "123456";
            newAccount.ImagePath="~/si.png";
            newAccount.ImageType = "image/png";

            BAL.StudentBAL bal = new BAL.StudentBAL(dbContext);

            bal.RegisteringNewAccount(newAccount);

            Student st = this.dbContext.Students.FirstOrDefault(e => e.Email == newAccount.Email);


            Assert.That(st != null);
        }
        [Test]
        public void Register_Account_With_Path_Image_Success()
        {
           //Test with images crieteras in progress

            PersonRegisterVM newAccount = new PersonRegisterVM();
            newAccount.FirstName = "test1";
            newAccount.LastName = "test1";
            newAccount.Role = "1";
            newAccount.Email = "test1@yahoo.fr";
            newAccount.Password = "123456";
            newAccount.ImagePath = "~/si.png";
            newAccount.ImageType = "image/png";

            BAL.StudentBAL bal = new BAL.StudentBAL(dbContext);



            Student st = this.dbContext.Students.FirstOrDefault(e => e.FilePaths.Any(c=>c.FileName ==newAccount.ImagePath));


            Assert.That(st != null);
        }


        

    }
}