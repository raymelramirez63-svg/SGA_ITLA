using System.Collections.Generic;
using SGA_ITLA.Domain.Base;

namespace SGA_ITLA.Domain.Entities.Transporte
{
    public class Ruta : BaseEntity
    {
        public string NombreRuta { get; set; } = string.Empty;

        public ICollection<Parada> Paradas { get; set; } = new List<Parada>();
        public ICollection<Horario> Horarios { get; set; } = new List<Horario>();
    }
}