using SGA_ITLA.Domain.Entities.Transporte;
using SGA_ITLA.Domain.Interfaces;
using SGA_ITLA.Infraestructure.Base;
using SGA_ITLA.Infraestructure.Context;
namespace SGA_ITLA.Infraestructure.Repositories { public class RutaRepository : BaseRepository<Ruta>, IRutaRepository { public RutaRepository(SgaContext context) : base(context) { } } }