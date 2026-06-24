using SGA_ITLA.Domain.Entities.Transporte;
using SGA_ITLA.Domain.Interfaces;
using SGA_ITLA.Infraestructure.Base;
using SGA_ITLA.Infraestructure.Context;
namespace SGA_ITLA.Infraestructure.Repositories { public class AutobusRepository : BaseRepository<Autobus>, IAutobusRepository { public AutobusRepository(SgaContext context) : base(context) { } } }