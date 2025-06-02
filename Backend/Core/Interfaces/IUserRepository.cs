using FarmSystem.Core.Models;
using System.Threading.Tasks;
namespace FarmSystem.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<int> RegisterAsync(User user);
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllUsersAsync();
    }
}
