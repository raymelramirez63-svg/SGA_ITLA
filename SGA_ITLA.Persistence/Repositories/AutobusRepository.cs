using SGA_ITLA.Domain.Entities.Transporte;
using SGA_ITLA.Persistence.Base;
using SGA_ITLA.Persistence.Context;
using SGA_ITLA.Persistence.Interfaces;
namespace SGA_ITLA.Persistence.Repositories { public class AutobusRepository : BaseRepository<Autobus>, IAutobusRepository { public AutobusRepository(SgaContext context) : base(context) { } } }