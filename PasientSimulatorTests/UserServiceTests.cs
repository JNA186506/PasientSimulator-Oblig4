using PasientSimulator.lib.Models;
using PasientSimulator.lib.Services;

namespace PasientSimulatorTests;

[TestFixture]
public class UserServiceTests
{
    private Context _context;
    private UserService _service;

    [SetUp]
    public void Setup()
    {
        _context = TestContextFactory.Create();
        _service = new UserService(_context);
    }

    [TearDown]
    public void TearDown() => _context.Dispose();

    // ── GetAllStudents ───────────────────────────────────────────────────────

    [Test]
    public async Task GetAllStudents_WhenEmpty_ReturnsEmptyList()
    {
        var result = await _service.GetAllStudents();

        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetAllStudents_ReturnsOnlyUsersWithStudentRole()
    {
        // Role 1 = student, Role 0 = instructor (or admin)
        _context.Users.AddRange(
            new User { Name = "Alice Student", Role = 1, Cases = new List<Case>() },
            new User { Name = "Bob Student",   Role = 1, Cases = new List<Case>() },
            new User { Name = "Carol Teacher", Role = 0, Cases = new List<Case>() }
        );
        await _context.SaveChangesAsync();

        var result = await _service.GetAllStudents();

        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result.All(u => u.Role == 1), Is.True);
    }

    [Test]
    public async Task GetAllStudents_DoesNotReturnNonStudentRoles()
    {
        _context.Users.Add(new User { Name = "Teacher", Role = 0, Cases = new List<Case>() });
        await _context.SaveChangesAsync();

        var result = await _service.GetAllStudents();

        Assert.That(result, Is.Empty);
    }

    // ── FindStudent ──────────────────────────────────────────────────────────

    [Test]
    public async Task FindStudent_ValidId_ReturnsCorrectUser()
    {
        var user = new User { Name = "Test Student", Role = 1, Cases = new List<Case>() };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var result = await _service.FindStudent(user.UserId);

        Assert.That(result.Name, Is.EqualTo("Test Student"));
        Assert.That(result.UserId, Is.EqualTo(user.UserId));
    }

    [Test]
    public void FindStudent_InvalidId_ThrowsKeyNotFoundException()
    {
        Assert.ThrowsAsync<KeyNotFoundException>(
            async () => await _service.FindStudent(99999));
    }

    // ── AddUser ──────────────────────────────────────────────────────────────

    [Test]
    public async Task AddUser_PersistsUserWithCorrectData()
    {
        var result = await _service.AddUser(50, 1, "New User");

        Assert.That(result.UserId, Is.EqualTo(50));
        Assert.That(result.Role,   Is.EqualTo(1));
        Assert.That(result.Name,   Is.EqualTo("New User"));
    }

    // ── RemoveUser ───────────────────────────────────────────────────────────

    [Test]
    public async Task RemoveUser_ExistingUser_ReturnsTrue()
    {
        var user = new User { Name = "To Remove", Role = 1, Cases = new List<Case>() };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var result = await _service.RemoveUser(user.UserId);

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task RemoveUser_ExistingUser_IsRemovedFromDatabase()
    {
        var user = new User { Name = "To Remove", Role = 1, Cases = new List<Case>() };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        await _service.RemoveUser(user.UserId);

        var remaining = await _service.GetAllStudents();
        Assert.That(remaining, Is.Empty);
    }

    [Test]
    public async Task RemoveUser_NonExistentUser_ReturnsFalse()
    {
        var result = await _service.RemoveUser(99999);

        Assert.That(result, Is.False);
    }
}
