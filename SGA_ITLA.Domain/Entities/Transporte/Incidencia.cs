using System;
using SGA_ITLA.Domain.Base;
using SGA_ITLA.Domain.Enums;
using SGA_ITLA.Domain.Entities.Usuarios;

namespace SGA_ITLA.Domain.Entities.Transporte
{
    public class Incidencia : BaseEntity
    {
        public int ViajeId { get; set; }
        public Viaje? Viaje { get; set; }

        public int ConductorId { get; set; }
        public Usuario? Conductor { get; set; }

        public TipoIncidencia Tipo { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaHoraIncidencia { get; set; } = DateTime.Now;
    }
}