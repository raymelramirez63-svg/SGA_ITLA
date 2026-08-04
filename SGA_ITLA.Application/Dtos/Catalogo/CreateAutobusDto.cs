using System.ComponentModel.DataAnnotations;

namespace SGA_ITLA.Application.Dtos.Catalogo
{
    public class CreateAutobusDto
    {
        [Required(ErrorMessage = "La placa es obligatoria.")]
        [StringLength(10, ErrorMessage = "La placa no puede tener más de 10 caracteres.")] 
        public string Placa { get; set; } = string.Empty;

        [Required(ErrorMessage = "La capacidad máxima es obligatoria.")]
        [Range(1, 100, ErrorMessage = "La capacidad debe ser un valor lógico mayor a 0.")]
        public int CapacidadMaxima { get; set; }

        [Required(ErrorMessage = "El estado operativo es obligatorio.")]
        public int EstadoOperativo { get; set; }
    }
}