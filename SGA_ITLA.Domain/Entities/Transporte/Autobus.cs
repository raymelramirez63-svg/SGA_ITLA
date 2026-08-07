using System.ComponentModel.DataAnnotations;
using SGA_ITLA.Domain.Base;
using SGA_ITLA.Domain.Enums;

namespace SGA_ITLA.Domain.Entities.Transporte
{
    public class Autobus : BaseEntity
    {
        [Required(ErrorMessage = "La placa es obligatoria.")]
        [RegularExpression(@"^[IiLl][0-9]{6}$", ErrorMessage = "Formato de placa inválido. Debe comenzar con 'I' o 'L' seguida de 6 dígitos. Ej: I123456.")]
        public string Placa { get; set; } = string.Empty;
        public int CapacidadMaxima { get; set; }
        public EstadoAutobus EstadoOperativo { get; set; } = EstadoAutobus.Activo;
    }
}