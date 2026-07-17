using System;
using System.ComponentModel.DataAnnotations;

namespace SGA_ITLA.Application.Dtos.Catalogo
{
    public class CreateHorarioDto
    {
        [Required(ErrorMessage = "El ID de la ruta es obligatorio.")]
        public int RutaId { get; set; }

        [Required(ErrorMessage = "Los días de operación son obligatorios.")]
        public string DiasOperacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La hora de salida es obligatoria.")]
        public TimeSpan HoraSalida { get; set; }
    }
}