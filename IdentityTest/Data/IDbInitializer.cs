using System.Threading.Tasks;

namespace RdwTechdayRegistration.Data
{
    public interface IDbInitializer
    {
        Task Initialize();
    }
}