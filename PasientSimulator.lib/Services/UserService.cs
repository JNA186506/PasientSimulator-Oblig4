using Microsoft.EntityFrameworkCore;
using PasientSimulator.lib.Models;
using PasientSimulator.lib.Services.Interfaces;

namespace PasientSimulator.lib.Services;

public class UserService : IUserService {
    private readonly Context _context;

    public UserService(Context context) {
        _context = context;
    }

    public async Task<User> AddUser(int id, int role, string name) {
        User newUser = new User { UserId = id, Role = role, Name = name };
        
        _context.Add(newUser);
        await _context.SaveChangesAsync();

        return newUser;
    }

    public async Task<bool> RemoveUser(int id) {
        User? user = await _context.Users.FindAsync(id);

        if (user is not null) {
            user.Cases = null;
            _context.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task AddCase(User user, Case newCase) {
        user.Cases.Add(newCase);
        _context.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task<List<User>> GetAllStudents() {
        return await _context.Users.Where(s => s.Role == 1).ToListAsync();
    }

    public async Task<User> FindStudent(int id) {
        User? user = await _context.Users.FindAsync(id);

        if (user == null) {
            throw new KeyNotFoundException($"User with id {id} was not found");
        }
        return user;
    }
}