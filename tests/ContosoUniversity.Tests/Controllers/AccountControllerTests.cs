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
        public void Login_InvalidCredential_Fail()
        {
            Assert.That(false);
        }

        [Test]
        public void Login_ValidLoginInvalidPawword_Fail()
        {
            Assert.That(false);
        }

        [Test]
        public void Register_With_Existing_Login_Fail()
        {
            PersonRegisterVM newAccount = new PersonRegisterVM();
            Assert.That(false);
        }

        [Test]
        public void Register_Student_With_Bad_Password_Fail()
        {

            PersonRegisterVM newAccount = new PersonRegisterVM();
            newAccount.FirstName = "yahya";
            newAccount.LastName = "khoder";
            
            newAccount.Email = "yahya@gmail.com";
            newAccount.Password = "InvalidPassword";

            BAL.StudentBAL bal = new BAL.StudentBAL();
            
            bal.TestRegisteringStudent(newAccount, dbContext);

            Student st = this.dbContext.Students.FirstOrDefault(e => e.Password == newAccount.Password);



            Assert.That(st == null);
        }

        [Test]
        public void Register_Instructor_With_Bad_Password_Fail()
        {

            PersonRegisterVM newAccount = new PersonRegisterVM();

            Assert.That(false);
        }

        [Test]
        public void Register_Student_With_Non_Existing_Login_And_Good_Password_Success()
        {
            PersonRegisterVM newAccount = new PersonRegisterVM();
            newAccount.FirstName = "test";
            newAccount.LastName = "test";
            //newAccount.Role
            newAccount.Email = "test@yahoo.fr";
            newAccount.Password = "123456";

            BAL.StudentBAL bal = new BAL.StudentBAL();

            bal.TestRegisteringStudent(newAccount, dbContext);

            Student stu = this.dbContext.Students.FirstOrDefault(e => e.Email == newAccount.Email);


            Assert.That(stu != null);
        }

        [Test]
        public void Hashing_Same_word_Twice_Result_Two_Identical_Sha()
        {
            Assert.That(false);
        }
    }
}