using SGA_ITLA.Domain.Entities.Autorizaciones;
using SGA_ITLA.Persistence.Base;
using SGA_ITLA.Persistence.Context;
using SGA_ITLA.Persistence.Interfaces;
namespace SGA_ITLA.Persistence.Repositories { public class AutorizacionRepository : BaseRepository<Autorizacion>, IAutorizacionRepository { public AutorizacionRepository(SgaContext context) : base(context) { } } }