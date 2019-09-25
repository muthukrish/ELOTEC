
namespace ELOTEC.Infrastructure.Security
{
    using System.Threading.Tasks;

    public interface IJwtTokenGenerator
    {
        Task<string> CreateToken(string username);
    }
}
