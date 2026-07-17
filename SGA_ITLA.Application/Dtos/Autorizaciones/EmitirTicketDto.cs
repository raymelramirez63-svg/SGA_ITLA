using System;
using System.ComponentModel.DataAnnotations;

namespace SGA_ITLA.Application.Dtos.Autorizaciones
{
    public class EmitirTicketDto
    {
        [Required(ErrorMessage = "El ID del usuario es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "ID de usuario inválido.")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "El ID del pago es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe asociar un pago válido.")]
        public int PagoId { get; set; }

        [Required(ErrorMessage = "La fecha de inicio de vigencia es obligatoria.")]
        public DateTime FechaInicio { get; set; }
    }
}