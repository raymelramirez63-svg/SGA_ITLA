using System.Threading.Tasks;
using SGA_ITLA.Domain.Base;
using SGA_ITLA.Domain.Entities.Transporte;

namespace SGA_ITLA.Application.Interfaces.Catalogo
{
    public interface ICatalogoService
    {
        Task<OperationResult> RegistrarAutobusAsync(Autobus autobus);
        Task<OperationResult> ObtenerRutasAsync();
    }
}