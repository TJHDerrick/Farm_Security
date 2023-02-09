using EZYSoft_214142Z.Model;
using Fresh_Farm_Market_214142Z.Model;

namespace Fresh_Farm_Market_214142Z.Services
{
    public class UserService
    {
        private readonly AuthDbContext _context;

        public UserService(AuthDbContext context)
        {
            _context = context;
        }


        public ApplicationUser? GetUserByName(string name)
        {
            ApplicationUser? user = _context.Users.FirstOrDefault(
            x => x.UserName.Equals(name));
            return user;
        }

        public ApplicationUser? GetUserByEmail(string name)
        {
            ApplicationUser? user = _context.Users.FirstOrDefault(
            x => x.Email.Equals(name));
            return user;
        }

        public List<ApplicationUser> GetAll()
        {
            return _context.Users.OrderBy(m => m.UserName).ToList();
        }
        public Boolean CheckUser(string username)
        {
            Console.WriteLine(username);
            var applicationUsers = _context.Users.Select(x => x.Email).ToArray();
            Console.WriteLine(applicationUsers);
            if (applicationUsers.Contains(username))
            {
                return true;
            }
            return false;
        }
    }
}
