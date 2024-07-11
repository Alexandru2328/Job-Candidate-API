using Job_Candidate_API.Controllers;
using Job_Candidate_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace JobCandidateAPI.Tests
{
    public class CandidatesControllerTests
    {
        private readonly DbContextOptions<AppDbContext> _contextOptions;

        public CandidatesControllerTests()
        {
            _contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new AppDbContext(_contextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }

        private AppDbContext CreateContext() => new AppDbContext(_contextOptions);

        [Fact]
        public async Task UpsertCandidate_ShouldAddNewCandidate()
        {
            // Arrange
            using var context = CreateContext();
            var controller = new CandidatesController(context);
            var candidate = new Candidate
            {
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                Email = "john.doe@example.com",
                PreferredContactTime = "Morning",
                LinkedIn = "http://linkedin.com/johndoe",
                GitHub = "http://github.com/johndoe",
                Comments = "Sample comment"
            };

            // Act
            var result = await controller.UpsertCandidate(candidate);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Candidate>(okResult.Value);
            Assert.Equal(candidate.Email, returnValue.Email);
        }

        [Fact]
        public async Task UpsertCandidate_ShouldUpdateExistingCandidate()
        {
            // Arrange
            using var context = CreateContext();
            var existingCandidate = new Candidate
            {
                FirstName = "Jane",
                LastName = "Smith",
                PhoneNumber = "0987654321",
                Email = "jane.smith@example.com",
                PreferredContactTime = "Afternoon",
                LinkedIn = "http://linkedin.com/janesmith",
                GitHub = "http://github.com/janesmith",
                Comments = "Initial comment"
            };
            context.Candidates.Add(existingCandidate);
            context.SaveChanges();

            var controller = new CandidatesController(context);
            var updatedCandidate = new Candidate
            {
                FirstName = "Jane",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                Email = "jane.smith@example.com",
                PreferredContactTime = "Evening",
                LinkedIn = "http://linkedin.com/janedoe",
                GitHub = "http://github.com/janedoe",
                Comments = "Updated comment"
            };

            var result = await controller.UpsertCandidate(updatedCandidate);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Candidate>(okResult.Value);
            Assert.Equal(updatedCandidate.LastName, returnValue.LastName);
            Assert.Equal(updatedCandidate.PreferredContactTime, returnValue.PreferredContactTime);
            Assert.Equal(updatedCandidate.LinkedIn, returnValue.LinkedIn);
            Assert.Equal(updatedCandidate.GitHub, returnValue.GitHub);
            Assert.Equal(updatedCandidate.Comments, returnValue.Comments);
        }
    }
}
