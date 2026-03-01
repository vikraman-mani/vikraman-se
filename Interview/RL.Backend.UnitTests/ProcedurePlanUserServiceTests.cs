using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RL.Backend.Services;
using RL.Data;
using RL.Data.DataModels;

namespace RL.Backend.UnitTests.Services;

[TestClass]
public class ProcedurePlanUserServiceTests
{
    private RLContext _context = null!;
    private ProcedurePlanUserService _sut = null!;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<RLContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // fresh DB per test
            .Options;

        _context = new RLContext(options);
        _sut = new ProcedurePlanUserService(_context);

        SeedData();
    }

    private void SeedData()
    {
        var user1 = new User { UserId = 1, Name = "Alice" };
        var user2 = new User { UserId = 2, Name = "Bob" };

        _context.Users.AddRange(user1, user2);
        _context.ProcedurePlanUsers.AddRange(
            new ProcedurePlanUser { PlanId = 1, ProcedureId = 10, UserId = 1, User = user1 },
            new ProcedurePlanUser { PlanId = 1, ProcedureId = 10, UserId = 2, User = user2 }
        );
        _context.SaveChanges();
    }

    [TestMethod]
    public async Task GetUsersForProcedureAsync_ReturnsUsers()
    {
        var users = await _sut.GetUsersForProcedureAsync(1, 10);

        users.Should().HaveCount(2);
        users.Select(u => u.Name).Should().Contain(new[] { "Alice", "Bob" });
    }

    [TestMethod]
    public async Task AssignUserToProcedureAsync_AddsNewAssignment()
    {
        var result = await _sut.AssignUserToProcedureAsync(2, 20, 1);

        result.Should().NotBeNull();
        result.PlanId.Should().Be(2);
        result.ProcedureId.Should().Be(20);
        result.UserId.Should().Be(1);

        _context.ProcedurePlanUsers
            .Any(ppu => ppu.PlanId == 2 && ppu.ProcedureId == 20 && ppu.UserId == 1)
            .Should().BeTrue();
    }

    [TestMethod]
    public async Task AssignUserToProcedureAsync_ReturnsExistingAssignment()
    {
        var result = await _sut.AssignUserToProcedureAsync(1, 10, 1);

        result.Should().NotBeNull();
        result.PlanId.Should().Be(1);
        result.ProcedureId.Should().Be(10);
        result.UserId.Should().Be(1);

        _context.ProcedurePlanUsers
            .Count(ppu => ppu.PlanId == 1 && ppu.ProcedureId == 10 && ppu.UserId == 1)
            .Should().Be(1); // no duplicates
    }

    [TestMethod]
    public async Task RemoveUserFromProcedureAsync_RemovesAssignment()
    {
        await _sut.RemoveUserFromProcedureAsync(1, 10, 1);

        _context.ProcedurePlanUsers
            .Any(ppu => ppu.PlanId == 1 && ppu.ProcedureId == 10 && ppu.UserId == 1)
            .Should().BeFalse();
    }

    [TestMethod]
    public async Task RemoveAllUsersFromProcedureAsync_RemovesAllAssignments()
    {
        await _sut.RemoveAllUsersFromProcedureAsync(1, 10);

        _context.ProcedurePlanUsers
            .Any(ppu => ppu.PlanId == 1 && ppu.ProcedureId == 10)
            .Should().BeFalse();
    }
}