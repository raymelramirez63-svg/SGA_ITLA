using SGA_ITLA.Domain.Entities.Autorizaciones;
using SGA_ITLA.Domain.Interfaces;
using SGA_ITLA.Infraestructure.Base;
using SGA_ITLA.Infraestructure.Context;
namespace SGA_ITLA.Infraestructure.Repositories { public class AutorizacionRepository : BaseRepository<Autorizacion>, IAutorizacionRepository { public AutorizacionRepository(SgaContext context) : base(context) { } } }