using System.ComponentModel.DataAnnotations;

namespace SGA_ITLA.Application.Dtos.Transporte.Viajes
{
    public class AbordajeDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Debe especificar un viaje válido.")]
        public int ViajeId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Debe especificar la identificación del estudiante.")]
        public int EstudianteId { get; set; }

        [Required(ErrorMessage = "El método de acceso (Ticket/Tarjeta) es obligatorio.")]
        public string? MetodoAcceso { get; set; }
    }
}