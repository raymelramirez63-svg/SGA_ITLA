using SGA_ITLA.Domain.Base;
using SGA_ITLA.Domain.Enums;

namespace SGA_ITLA.Domain.Entities.Transporte
{
    public class Autobus : BaseEntity
    {
        public string Placa { get; set; } = string.Empty;
        public int CapacidadMaxima { get; set; }
        public EstadoAutobus EstadoOperativo { get; set; } = EstadoAutobus.Activo;
    }
}