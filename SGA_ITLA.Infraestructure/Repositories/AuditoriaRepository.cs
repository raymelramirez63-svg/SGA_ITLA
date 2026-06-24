using SGA_ITLA.Domain.Entities.Auditoria;
using SGA_ITLA.Domain.Interfaces;
using SGA_ITLA.Infraestructure.Base;
using SGA_ITLA.Infraestructure.Context;
namespace SGA_ITLA.Infraestructure.Repositories { public class AuditoriaRepository : BaseRepository<RegistroAuditoria>, IAuditoriaRepository { public AuditoriaRepository(SgaContext context) : base(context) { } } }