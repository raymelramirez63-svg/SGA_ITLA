using System;
using System.Threading.Tasks;
using SGA_ITLA.Application.Interfaces.Catalogo;
using SGA_ITLA.Domain.Base;
using SGA_ITLA.Domain.Entities.Transporte;
using SGA_ITLA.Domain.Entities.Usuarios;
using SGA_ITLA.Domain.Interfaces;

namespace SGA_ITLA.Application.Services.Catalogo
{
    public class CatalogoService : ICatalogoService
    {
        private readonly IAutobusRepository _autobusRepo;
        private readonly IRutaRepository _rutaRepo;

        public CatalogoService(IAutobusRepository autobusRepo, IRutaRepository rutaRepo)
        {
            _autobusRepo = autobusRepo;
            _rutaRepo = rutaRepo;
        }

        public async Task<OperationResult> RegistrarAutobusAsync(Autobus autobus)
        {
            return await _autobusRepo.SaveEntityAsync(autobus);
        }

        public async Task<OperationResult> ObtenerRutasAsync()
        {
            return await _rutaRepo.GetAllAsync();
        }

        public async Task<OperationResult> RegistrarConductorAsync(Usuario conductor)
        {
            return await Task.FromResult(new OperationResult { Success = true, Message = "Conductor registrado." });
        }

        public async Task<OperationResult> RegistrarRutaAsync(Ruta ruta)
        {
            return await Task.FromResult(new OperationResult { Success = true, Message = "Ruta registrada." });
        }

        public async Task<OperationResult> RegistrarHorarioAsync(Horario horario)
        {
            return await Task.FromResult(new OperationResult { Success = true, Message = "Horario registrado." });
        }
    }
}