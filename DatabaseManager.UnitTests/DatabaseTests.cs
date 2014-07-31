using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DatabaseManager.Domain.Abstract;
using DatabaseManager.Domain.Entities;
using DatabaseManager.WebUI.Controllers;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

namespace DatabaseManager.UnitTests
{
    [TestClass]
    public class DatabaseTests
    {
        [TestMethod]
        public void List_Containts_All_Databases()
        {
            // Arrange - create a mock repository
            Mock<ILawsonDatabaseRepository> mock = new Mock<ILawsonDatabaseRepository>();
            mock.Setup(m => m.LawsonDatabases).Returns(new LawsonDatabase[] {
                new LawsonDatabase { LawsonNumber = "1", PIName = "John"},
                new LawsonDatabase { LawsonNumber = "2", PIName = "Chris"},
                new LawsonDatabase { LawsonNumber = "3", PIName = "Steve"}
            });

            // Arrange - create a controller
            DatabaseController target = new DatabaseController(mock.Object);

            // Act
            LawsonDatabase [] result = ((IEnumerable<LawsonDatabase>)target.List().ViewData.Model).ToArray();

            // Assert
            Assert.AreEqual(3, result.Length);
            Assert.AreEqual("1", result[0].LawsonNumber);
            Assert.AreEqual("Chris", result[1].PIName);
            Assert.AreEqual("3", result[2].LawsonNumber);
        }

        [TestMethod]
        public void Can_Edit_Valid_Databases()
        {
            // Arrange - create a mock repo
            Mock<ILawsonDatabaseRepository> mock = new Mock<ILawsonDatabaseRepository>();
            mock.Setup(m => m.LawsonDatabases).Returns(new LawsonDatabase[] {
                new LawsonDatabase { LawsonDatabaseID = 1, PIName = "John"},
                new LawsonDatabase { LawsonDatabaseID = 2, PIName = "Chris"},
                new LawsonDatabase { LawsonDatabaseID = 3, PIName = "Steve"}
            });

            // Arrange - create a controller
            DatabaseController target = new DatabaseController(mock.Object);

            // Act
            LawsonDatabase l1 = target.Edit(1).ViewData.Model as LawsonDatabase;
            LawsonDatabase l2 = (LawsonDatabase)target.Edit(2).ViewData.Model;
            LawsonDatabase l3 = target.Edit(3).ViewData.Model as LawsonDatabase;

            // Assert
            Assert.AreEqual("1", l1.LawsonNumber);
            Assert.AreEqual("2", l2.LawsonNumber);
            Assert.AreEqual("Steve", l3.PIName);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Databases()
        {
            // Arrange - create a mock repo
            Mock<ILawsonDatabaseRepository> mock = new Mock<ILawsonDatabaseRepository>();
            mock.Setup(m => m.LawsonDatabases).Returns(new LawsonDatabase[] {
                new LawsonDatabase { LawsonNumber = "1", PIName = "John"},
                new LawsonDatabase { LawsonNumber = "2", PIName = "Chris"},
                new LawsonDatabase { LawsonNumber = "3", PIName = "Steve"}
            });

            // Arrange - create a controller
            DatabaseController target = new DatabaseController(mock.Object);

            // Act - edit non existent database
            LawsonDatabase db = target.Edit(4).ViewData.Model as LawsonDatabase;

            // Assert
            Assert.IsNull(db);
        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            // Arrange - create a mock repository
            Mock<ILawsonDatabaseRepository> mock = new Mock<ILawsonDatabaseRepository>();
            
            // Arrange - create a controller
            DatabaseController target = new DatabaseController(mock.Object);

            // Arrange - create a db
            LawsonDatabase db = new LawsonDatabase { Name = "Test" };

            // Act - save
            ActionResult result = target.Edit(db);

            // Assert - check the save was called
            mock.Verify(m => m.SaveDatabase(db));

            // Assert - the result type
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public static void Cannot_Save_Invalid_Changes()
        {
            // Arrange - create a mock repository
            Mock<ILawsonDatabaseRepository> mock = new Mock<ILawsonDatabaseRepository>();

            // Arrange - create a controller
            DatabaseController target = new DatabaseController(mock.Object);

            // Arrange - create a db
            LawsonDatabase db = new LawsonDatabase { Name = "Test" };

            // Arrange - add an error to the model state
            target.ModelState.AddModelError("Error", "Error");

            // Act - try to save db
            ActionResult result = target.Edit(db);

            // Assert - check that save wasn't called
            mock.Verify(m => m.SaveDatabase(It.IsAny<LawsonDatabase>()), Times.Never());

            // Assert - check result
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}
