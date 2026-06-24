using System;

namespace SGA_ITLA.Domain.Base
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public int CreationUser { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public int? ModifyUser { get; set; }
        public DateTime? ModifyDate { get; set; }
        public int? DeleteUser { get; set; }
        public DateTime? DeleteDate { get; set; }
        public bool Deleted { get; set; } = false;
    }
}