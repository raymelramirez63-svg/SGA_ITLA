using SGA_ITLA.Domain.Entities.Transporte;
using SGA_ITLA.Persistence.Base;
using SGA_ITLA.Persistence.Context;
using SGA_ITLA.Persistence.Interfaces;
namespace SGA_ITLA.Persistence.Repositories { public class ViajeRepository : BaseRepository<Viaje>, IViajeRepository { public ViajeRepository(SgaContext context) : base(context) { } } }