using System.ComponentModel.DataAnnotations;

namespace SGA_ITLA.Application.Dtos.Autorizaciones
{
    public class RecargarTarjetaDto
    {
        [Required(ErrorMessage = "El ID del usuario es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "ID de usuario inválido.")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "El monto es obligatorio.")]
        [Range(1.0, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0.")]
        public decimal Monto { get; set; }
    }
}