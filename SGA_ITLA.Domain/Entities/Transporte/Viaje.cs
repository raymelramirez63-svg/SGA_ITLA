using System;
using System.Collections.Generic;
using SGA_ITLA.Domain.Base;
using SGA_ITLA.Domain.Enums;
using SGA_ITLA.Domain.Entities.Usuarios;

namespace SGA_ITLA.Domain.Entities.Transporte
{
    public class Viaje : BaseEntity
    {
        public int RutaId { get; set; }
        public Ruta? Ruta { get; set; }

        public int AutobusId { get; set; }
        public Autobus? Autobus { get; set; }

        public int ConductorId { get; set; }
        public Usuario? Conductor { get; set; }

        public DateTime HorarioSalidaPlanificada { get; set; }
        public DateTime? HorarioSalidaReal { get; set; }
        public DateTime? HorarioLlegadaReal { get; set; }

        public EstadoViaje Estado { get; set; } = EstadoViaje.Programado;
        public int CupoDisponibleActual { get; set; }

        public ICollection<Incidencia> Incidencias { get; set; } = new List<Incidencia>();
    }
}