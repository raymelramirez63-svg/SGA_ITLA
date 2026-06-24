using System;
using SGA_ITLA.Domain.Enums;

namespace SGA_ITLA.Application.Dtos.Transporte.Viajes
{
    public class SaveViajeDto
    {
        public int RutaId { get; set; }
        public int AutobusId { get; set; }
        public int ConductorId { get; set; }

        public DateTime HorarioSalida { get; set; }

        public EstadoViaje EstadoViaje { get; set; }
    }
}