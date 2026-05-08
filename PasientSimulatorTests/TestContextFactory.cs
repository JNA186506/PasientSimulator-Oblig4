using Microsoft.EntityFrameworkCore;
using PasientSimulator.lib.Models;

namespace PasientSimulatorTests;

/// <summary>
/// Creates a fresh in-memory EF Core Context for each test.
/// Each call with no argument generates a unique database name so tests
/// cannot share or pollute each other's state.
/// </summary>
public static class TestContextFactory
{
    public static Context Create(string? dbName = null)
    {
        var options = new DbContextOptionsBuilder<Context>()
            .UseInMemoryDatabase(dbName ?? Guid.NewGuid().ToString())
            .Options;

        return new Context(options);
    }
}
