using System;
using System.ComponentModel.DataAnnotations;

namespace SGA_ITLA.Application.Dtos.Transporte.Viajes
{
    public class SaveViajeDto
    {
        public int ViajeId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una ruta.")]
        public int RutaId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un autobús.")]
        public int AutobusId { get; set; }

        [Required(ErrorMessage = "Debe asignar un conductor.")]
        public int ConductorId { get; set; }

        [Required(ErrorMessage = "El horario de salida es obligatorio.")]
        public DateTime HorarioSalida { get; set; }

        public string EstadoViaje { get; set; } = "Planificado";
    }
}