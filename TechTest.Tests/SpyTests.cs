using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using TechTest.Controllers;
using TechTest.Core.Interfaces;
using TechTest.Core.Models;
using TechTest.Core.RequestObjects;

namespace Tests
{
    
    public class SpyTests
    {
        Mock<IRepository<Spy>> _repository;
        SpyController _spyController;

        [OneTimeSetUp]
        public void Setup()
        {
            _repository = new Mock<IRepository<Spy>>();
            _spyController = new SpyController(_repository.Object);

            _repository.Setup(r => r.Get()).Returns(new List<Spy>
            {
                new Spy { Id = 1, Name = "James Bond", CodeName = "007"},
                new Spy { Id = 2, Name = "Ethan Hunt", CodeName = "314"},
                new Spy { Id = 3, Name = "Clouseau", CodeName = "Not now Kato" }
            });
        }

        [TestCase(new int[] { 1, 2, 4, 0, 0, 7, 5 }, "James Bond", true)]
        [TestCase(new int[] { 0, 2, 2, 0, 4, 7, 0 }, "James Bond", true)]
        [TestCase(new int[] { 1, 2, 0, 7, 4, 4, 0 }, "James Bond", false)]
        [TestCase(new int[] { 3, 6, 0, 1, 2, 6, 4 }, "Ethan Hunt", true)]
        [TestCase(new int[] { 3, 3, 1, 5, 1, 4, 4 }, "Ethan Hunt", true)]
        [TestCase(new int[] { 4, 1, 3, 8, 4, 3, 1 }, "Ethan Hunt", false)]
        public void SpyMessageWithSpy_ReturnsIsSpy(int[] message, string spy, bool isSpy)
        {
            // Arrange
            // Act
            var sut = _spyController.Post(new SpyRequest { Message = message, Spy = spy }) as OkObjectResult;
            // Assert
            Assert.AreEqual(isSpy, sut.Value);
        }

        [Test]
        public void SpyMessageWithNullSpy_BadRequest()
        {
            // Arrange
            // Act
            var sut = _spyController.Post(new SpyRequest { Message = new int[] { 4, 1, 3, 8, 4, 3, 1 }, Spy = null }) as BadRequestObjectResult;

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(sut);
            Assert.AreEqual("Spy not found", sut.Value);
        }
        [Test]
        public void SpyMessageWithlSpyCodeNameNotIntegers_BadRequest()
        {
            // Arrange
            // Act
            var sut = _spyController.Post(new SpyRequest { Message = new int[] { 4, 1, 3, 8, 4, 3, 1 }, Spy = "Clouseau" }) as BadRequestObjectResult;

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(sut);
            Assert.AreEqual("Invalid code name", sut.Value);
        }
    }
}