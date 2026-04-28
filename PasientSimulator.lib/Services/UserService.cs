using PasientSimulator.lib.Models;

namespace PasientSimulator.lib.Services;

public class UserService {
    private Context _context;

    public UserService(Context context) {
        _context = context;
    }

    public User AddUser(int id, int role, string name) {
        User newUser = new User { UserId = id, Role = role, Name = name };
        
        _context.Add(newUser);
        _context.SaveChanges();

        return newUser;
    }

    public bool RemoveUser(int id) {
        User? user = _context.Users.Find(id);

        if (user is not null) {
            user.Cases = null;
            _context.Remove(user);
            return true;
        }

        return false;
    }

    public void AddCase(Case newCase) {
        
    }
}