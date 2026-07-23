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
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly IHorarioRepository _horarioRepo;

        public CatalogoService(
            IAutobusRepository autobusRepo,
            IRutaRepository rutaRepo,
            IUsuarioRepository usuarioRepo,
            IHorarioRepository horarioRepo)
        {
            _autobusRepo = autobusRepo;
            _rutaRepo = rutaRepo;
            _usuarioRepo = usuarioRepo;
            _horarioRepo = horarioRepo;
        }

        public async Task<OperationResult> RegistrarAutobusAsync(Autobus autobus)
        {
            var result = await _autobusRepo.SaveEntityAsync(autobus);
            if (result.Success)
            {
                result.Message = $"Autobús registrado. ID asignado: {autobus.Id}";
            }
            return result;
        }

        public async Task<OperationResult> ObtenerRutasAsync()
        {
            return await _rutaRepo.GetAllAsync();
        }

        public async Task<OperationResult> RegistrarConductorAsync(Usuario conductor)
        {
            var result = await _usuarioRepo.SaveEntityAsync(conductor);
            if (result.Success)
            {
                result.Message = $"Conductor registrado exitosamente. El ID asignado es: {conductor.Id}";
            }
            return result;
        }

        public async Task<OperationResult> RegistrarRutaAsync(Ruta ruta)
        {
            var result = await _rutaRepo.SaveEntityAsync(ruta);
            if (result.Success)
            {
                result.Message = $"Ruta registrada exitosamente. El ID asignado es: {ruta.Id}";
            }
            return result;
        }

        public async Task<OperationResult> RegistrarHorarioAsync(Horario horario)
        {
            var result = await _horarioRepo.SaveEntityAsync(horario);
            if (result.Success)
            {
                result.Message = $"Horario registrado exitosamente. El ID asignado es: {horario.Id}";
            }
            return result;
        }
    }
}