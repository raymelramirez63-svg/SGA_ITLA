using System.ComponentModel.DataAnnotations;

namespace SGA_ITLA.Application.Dtos.Catalogo
{
    public class CreateConductorDto
    {
        [Required(ErrorMessage = "La identificación es obligatoria.")]
        public string Identificacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombres { get; set; } = string.Empty;

        [Required(ErrorMessage = "El estado laboral es obligatorio.")]
        public int EstadoLaboral { get; set; } 
    }
}