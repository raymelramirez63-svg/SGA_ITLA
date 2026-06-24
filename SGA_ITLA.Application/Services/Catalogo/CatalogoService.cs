using System;
using System.Threading.Tasks;
using SGA_ITLA.Domain.Base;
using SGA_ITLA.Domain.Entities.Transporte;
using SGA_ITLA.Domain.Interfaces;

namespace SGA_ITLA.Application.Services.Catalogo
{
    public class CatalogoService
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
    }
}