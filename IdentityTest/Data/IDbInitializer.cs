using System.Threading.Tasks;

namespace IdentityTest.Data
{
    public interface IDbInitializer
    {
        Task Initialize();
    }
}