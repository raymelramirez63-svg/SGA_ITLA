using SGA_ITLA.Domain.Entities.Transporte;
using SGA_ITLA.Domain.Interfaces;
using SGA_ITLA.Infraestructure.Context;
using SGA_ITLA.Infraestructure.Base; 

namespace SGA_ITLA.Infraestructure.Repositories
{
    public class HorarioRepository : BaseRepository<Horario>, IHorarioRepository
    {
        public HorarioRepository(SgaContext context) : base(context)
        {
        }
    }
}