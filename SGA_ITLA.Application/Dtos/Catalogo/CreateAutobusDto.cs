using System.ComponentModel.DataAnnotations;

namespace SGA_ITLA.Application.Dtos.Catalogo
{
    public class CreateAutobusDto
    {
        [Required(ErrorMessage = "La placa es obligatoria.")]
        public string Placa { get; set; } = string.Empty;

        [Required(ErrorMessage = "La capacidad máxima es obligatoria.")]
        [Range(1, 100, ErrorMessage = "La capacidad debe ser un valor lógico mayor a 0.")]
        public int CapacidadMaxima { get; set; }

        [Required(ErrorMessage = "El estado operativo es obligatorio.")]
        public int EstadoOperativo { get; set; }
    }
}