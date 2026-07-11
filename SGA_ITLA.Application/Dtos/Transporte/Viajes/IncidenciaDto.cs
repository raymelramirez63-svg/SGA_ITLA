using System.ComponentModel.DataAnnotations;

namespace SGA_ITLA.Application.Dtos.Transporte.Viajes
{
    public class IncidenciaDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Debe especificar un ID de viaje válido.")]
        public int ViajeId { get; set; }

        [Required(ErrorMessage = "El tipo de incidencia es requerido.")]
        [StringLength(50, ErrorMessage = "El tipo de incidencia no puede exceder los 50 caracteres.")]
        public string? TipoIncidencia { get; set; }

        [Required(ErrorMessage = "Debe proveer una descripción del evento.")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres.")]
        public string? Descripcion { get; set; }
    }
}