using System.Threading.Tasks;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public interface IAuthRepository
    {
         Task<UserInfo> Register(UserInfo user,string password);
         Task<UserInfo> Login(string username,string password);
         Task<bool> UserExists(string username);
    }
}