using SGA_ITLA.Domain.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGA_ITLA.Domain.Entities.Transporte
{
    [Table("Viajes", Schema = "Transporte")]
    public sealed class Viaje : BaseEntity<int>
    {
        [Key]
        [Column("ViajeId")]
        public override int Id { get; set; }

        [Required]
        public int RutaId { get; set; }

        [Required]
        public int AutobusId { get; set; }

        [Required]
        public int ConductorId { get; set; }

        [Required]
        public DateTime HorarioSalida { get; set; }

        [Required]
        [MaxLength(20)]
        public string EstadoViaje { get; set; }

        [ForeignKey("RutaId")]
        public Ruta Ruta { get; set; }

        [ForeignKey("AutobusId")]
        public Autobus Autobus { get; set; }
    }
}