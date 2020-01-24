using System;
using System.Collections.Generic;
using System.Text;

namespace RegistrationServer
{
    public abstract class Entity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? DeletedDate { get; set; }
    }
}