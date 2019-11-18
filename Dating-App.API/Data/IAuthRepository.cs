using System.Threading.Tasks;
using Dating_App.API.Models;

namespace Dating_App.API.Data
{
    // Capital I for interfaces
    public interface IAuthRepository
    {
         Task<User> Register(User user, string password);
         Task<User> Login (string username, string password);
         Task<bool> UserExists(string username);
    }
}