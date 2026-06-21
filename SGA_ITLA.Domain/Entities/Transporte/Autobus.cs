using SGA_ITLA.Domain.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGA_ITLA.Domain.Entities.Transporte
{
    [Table("Autobuses", Schema = "Transporte")]
    public sealed class Autobus : BaseEntity<int>
    {
        [Key]
        [Column("AutobusId")]
        public override int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string FichaInstitucional { get; set; }

        [Required]
        [MaxLength(20)]
        public string Placa { get; set; }

        [Required]
        public int CapacidadFisica { get; set; }

        public bool EstaOperativo { get; set; }
    }
}