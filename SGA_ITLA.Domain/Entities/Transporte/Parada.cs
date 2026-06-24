using SGA_ITLA.Domain.Base;

namespace SGA_ITLA.Domain.Entities.Transporte
{
    public class Parada : BaseEntity
    {
        public int RutaId { get; set; }
        public Ruta? Ruta { get; set; }

        public string NombreParada { get; set; } = string.Empty;
        public int Orden { get; set; }
        public int TiempoEstimadoMinutosDesdeAnterior { get; set; }
    }
}