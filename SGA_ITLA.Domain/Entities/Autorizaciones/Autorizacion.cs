using System;
using SGA_ITLA.Domain.Base;
using SGA_ITLA.Domain.Enums;
using SGA_ITLA.Domain.Entities.Usuarios;

namespace SGA_ITLA.Domain.Entities.Autorizaciones
{
    public class Autorizacion : BaseEntity
    {
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public TipoAutorizacion Tipo { get; set; }

        public DateTime? FechaInicioVigencia { get; set; }
        public DateTime? FechaFinVigencia { get; set; }

        public decimal? SaldoDisponible { get; set; }

        public bool IsActive { get; set; } = true;
    }
}