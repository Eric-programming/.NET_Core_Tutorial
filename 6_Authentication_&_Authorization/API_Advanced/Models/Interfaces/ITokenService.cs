using System.Threading.Tasks;

namespace API_Advanced.Models.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}