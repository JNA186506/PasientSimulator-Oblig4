using PasientSimulator.lib.Models;

namespace PasientSimulator.lib.Services.Interfaces;

public interface IUserService {
    Task<User> AddUser(int id, int role, string name);
    Task<bool> RemoveUser(int id);
    Task AddCase(User user, Case newCase);
    Task<List<User>> GetAllStudents();
    Task<User> FindStudent(int num);
}
