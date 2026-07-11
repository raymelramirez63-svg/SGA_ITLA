using System;
using System.ComponentModel.DataAnnotations;

namespace SGA_ITLA.Application.Dtos.Transporte.Viajes
{
    public class SaveViajeDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La ruta es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe especificar un ID de ruta válido.")]
        public int RutaId { get; set; }

        [Required(ErrorMessage = "El autobús es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe especificar un ID de autobús válido.")]
        public int AutobusId { get; set; }

        [Required(ErrorMessage = "El conductor es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe especificar un ID de conductor válido.")]
        public int ConductorId { get; set; }

        [Required(ErrorMessage = "El horario planificado es obligatorio.")]
        public DateTime HorarioSalidaPlanificada { get; set; }
    }
}