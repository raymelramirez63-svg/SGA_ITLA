using System.ComponentModel.DataAnnotations;

namespace SGA_ITLA.Application.Dtos.Autorizaciones
{
    public class RecargarTarjetaDto
    {
        [Required(ErrorMessage = "La identificación institucional es obligatoria.")]
        public string IdentificacionInstitucional { get; set; } = string.Empty;

        [Required(ErrorMessage = "El monto es obligatorio.")]
        [Range(1.0, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0.")]
        public decimal Monto { get; set; }
    }
}