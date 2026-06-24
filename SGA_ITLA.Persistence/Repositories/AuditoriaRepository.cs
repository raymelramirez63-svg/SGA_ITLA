using SGA_ITLA.Domain.Entities.Auditoria;
using SGA_ITLA.Persistence.Base;
using SGA_ITLA.Persistence.Context;
using SGA_ITLA.Persistence.Interfaces;
namespace SGA_ITLA.Persistence.Repositories { public class AuditoriaRepository : BaseRepository<RegistroAuditoria>, IAuditoriaRepository { public AuditoriaRepository(SgaContext context) : base(context) { } } }