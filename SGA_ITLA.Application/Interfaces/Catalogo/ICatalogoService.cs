using System.Threading.Tasks;
using SGA_ITLA.Domain.Base;
using SGA_ITLA.Domain.Entities.Transporte;
using SGA_ITLA.Domain.Entities.Usuarios; 

namespace SGA_ITLA.Application.Interfaces.Catalogo
{
    public interface ICatalogoService
    {
        Task<OperationResult> RegistrarAutobusAsync(Autobus autobus);
        Task<OperationResult> ObtenerRutasAsync();

        Task<OperationResult> RegistrarConductorAsync(Usuario conductor);
        Task<OperationResult> RegistrarRutaAsync(Ruta ruta);
        Task<OperationResult> RegistrarHorarioAsync(Horario horario);
    }
}