using System.ComponentModel.DataAnnotations;

namespace SGA_ITLA.WebMVC.Models
{
    public class AutobusViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La placa es obligatoria")]
        public string Placa { get; set; } = string.Empty;

        [Required(ErrorMessage = "Capacidad obligatoria")]
        [Display(Name = "Capacidad Máxima")]
        public int CapacidadMaxima { get; set; }

        [Display(Name = "Estado Operativo")]
        public int EstadoOperativo { get; set; }
    }
}