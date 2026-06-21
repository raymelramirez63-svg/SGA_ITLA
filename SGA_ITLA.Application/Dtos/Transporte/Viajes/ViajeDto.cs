using System;

namespace SGA_ITLA.Application.Dtos.Transporte.Viajes
{
    public class ViajeDto
    {
        public int ViajeId { get; set; }
        public int RutaId { get; set; }
        public string NombreRuta { get; set; }
        public int AutobusId { get; set; }
        public string FichaAutobus { get; set; }
        public int ConductorId { get; set; }
        public string NombreConductor { get; set; }
        public DateTime HorarioSalida { get; set; }
        public string EstadoViaje { get; set; }
    }
}