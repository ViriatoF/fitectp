using ContosoUniversity.Controllers;
using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using ContosoUniversity.Tests.Tools;
using Moq;
using NUnit.Framework;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ContosoUniversity.Tests.Controllers
{
    public class StudentControllerTests : IntegrationTestsBase
    {
        private MockHttpContextWrapper httpContext;
        private StudentController controllerToTest;
        private SchoolContext dbContext;

        [SetUp]
        public void Initialize()
        {
            httpContext = new MockHttpContextWrapper();
            controllerToTest = new StudentController();
            controllerToTest.ControllerContext = new ControllerContext(httpContext.Context.Object, new RouteData(), controllerToTest);
            dbContext = new DAL.SchoolContext(this.ConnectionString);
            controllerToTest.DbContext = dbContext;


        }

        [Test]
        public void GetDetails_ValidStudent_Success()
        {
            string expectedLastName = "Dubois";
            string expectedFirstName = "George";
            string expectedEmail = "George@hotmail.fr";
            string expectedPassowrd = "George";
            DateTime date = DateTime.Now; 

            EntityGenerator generator = new EntityGenerator(dbContext);
            Student student = generator.CreateStudent(expectedLastName, expectedFirstName, expectedEmail, expectedPassowrd, date);


            
            var result = controllerToTest.Details(student.ID) as ViewResult;
            var resultModel = result.Model as Student;

            Assert.That(result, Is.Not.Null);
            Assert.That(resultModel, Is.Not.Null);
            Assert.That(expectedLastName, Is.EqualTo(resultModel.LastName));
            Assert.That(expectedFirstName, Is.EqualTo(resultModel.FirstMidName));
        }

        [Test]
        public void GetDetails_InvalidStudent_Fail404()
        {
            const int expectedStatusCode = 404;
            const int invalidId = 99999999;

            var result = controllerToTest.Details(invalidId) as HttpStatusCodeResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(expectedStatusCode, Is.EqualTo(result.StatusCode));
        }

        [Test]
        public void Edit_ValidStudentData_Success()
        {
            string expectedLastName = "Wood";
            string previousLastName = "Dubois";
            string previousFirstName = "George";
            string previuosEmail = "George@hotmail.fr";
            string previuosPassowrd = "George";
            DateTime date = DateTime.Now;


            EntityGenerator generator = new EntityGenerator(dbContext);
            Student student = generator.CreateStudent(previousLastName, previousFirstName, previuosEmail, previuosPassowrd,date);
            student.LastName = expectedLastName;

            FormDataHelper.PopulateFormData(controllerToTest, student);

            var result = controllerToTest.EditPost(student.ID,null) as ViewResult;

            Student savedStudent = dbContext.Students.Find(student.ID);

            Assert.That(result, Is.Not.Null);
            Assert.That((result.Model as Student).LastName, Is.EqualTo(expectedLastName));
            Assert.That(savedStudent.LastName, Is.EqualTo(expectedLastName));
        }
    }
}

