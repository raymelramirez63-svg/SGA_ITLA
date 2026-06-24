using System;
using SGA_ITLA.Domain.Base;

namespace SGA_ITLA.Domain.Entities.Transporte
{
    public class Horario : BaseEntity
    {
        public int RutaId { get; set; }
        public Ruta? Ruta { get; set; }

        public string DiasOperacion { get; set; } = string.Empty;
        public TimeSpan HoraSalida { get; set; }
    }
}