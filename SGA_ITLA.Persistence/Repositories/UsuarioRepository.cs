using SGA_ITLA.Domain.Entities.Usuarios;
using SGA_ITLA.Persistence.Base;
using SGA_ITLA.Persistence.Context;
using SGA_ITLA.Persistence.Interfaces;
namespace SGA_ITLA.Persistence.Repositories { public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository { public UsuarioRepository(SgaContext context) : base(context) { } } }