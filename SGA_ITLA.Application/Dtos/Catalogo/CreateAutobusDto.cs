using System.ComponentModel.DataAnnotations;

namespace SGA_ITLA.Application.Dtos.Catalogo
{
    public class CreateAutobusDto
    {
        [Required(ErrorMessage = "La placa es obligatoria.")]
        [StringLength(7, ErrorMessage = "La placa no puede tener más de 7 caracteres.")]
        [RegularExpression(@"^[IiLl][0-9]{6}$", ErrorMessage = "Formato de placa inválido. Debe comenzar con 'I' o 'L' seguida de 6 dígitos. Ej: I123456.")]
        public string Placa { get; set; } = string.Empty;

        [Required(ErrorMessage = "La capacidad máxima es obligatoria.")]
        [Range(1, 100, ErrorMessage = "La capacidad debe ser un valor lógico mayor a 0.")]
        public int CapacidadMaxima { get; set; }

        [Required(ErrorMessage = "El estado operativo es obligatorio.")]
        public int EstadoOperativo { get; set; }
    }
}