using System;
using SGA_ITLA.Domain.Enums;

namespace SGA_ITLA.Application.Dtos.Transporte.Viajes
{
    public class ViajeDto
    {
        public int Id { get; set; }
        public int RutaId { get; set; }

        public string NombreRuta { get; set; } = string.Empty;

        public int AutobusId { get; set; }
        public string FichaAutobus { get; set; } = string.Empty;

        public int ConductorId { get; set; }
        public string NombreConductor { get; set; } = string.Empty;

        public DateTime HorarioSalidaPlanificada { get; set; }
        public EstadoViaje Estado { get; set; }
    }
}