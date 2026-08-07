using SGA_ITLA.Domain.Enums;

namespace SGA_ITLA.WebMVC.Models
{
    public class AutobusListItemVM
    {
        public int Id { get; set; }
        public string Placa { get; set; } = string.Empty;
        public int CapacidadMaxima { get; set; }
        public EstadoAutobus EstadoOperativo { get; set; }
        public string? ChoferAsignado { get; set; }
    }
}