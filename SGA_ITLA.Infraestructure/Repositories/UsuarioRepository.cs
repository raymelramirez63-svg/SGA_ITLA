using SGA_ITLA.Domain.Entities.Usuarios;
using SGA_ITLA.Domain.Interfaces;
using SGA_ITLA.Infraestructure.Base;
using SGA_ITLA.Infraestructure.Context;
namespace SGA_ITLA.Infraestructure.Repositories { public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository { public UsuarioRepository(SgaContext context) : base(context) { } } }