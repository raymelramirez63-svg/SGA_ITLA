using SGA_ITLA.Domain.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGA_ITLA.Domain.Entities.Transporte
{
    [Table("Rutas", Schema = "Transporte")]
    public sealed class Ruta : BaseEntity<int>
    {
        [Key]
        [Column("RutaId")]
        public override int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string NombreRuta { get; set; }

        [Required]
        [MaxLength(150)]
        public string PuntoOrigen { get; set; }

        [Required]
        [MaxLength(150)]
        public string PuntoDestino { get; set; }
    }
}