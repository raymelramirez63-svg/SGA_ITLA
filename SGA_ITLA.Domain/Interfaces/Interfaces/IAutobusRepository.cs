using System.Threading.Tasks;
using SGA_ITLA.Domain.Entities.Transporte;

namespace SGA_ITLA.Domain.Interfaces
{
    public interface IAutobusRepository : IBaseRepository<Autobus>
    {
        Task<int> ObtenerCapacidadMaximaAsync(int autobusId);
    }
}