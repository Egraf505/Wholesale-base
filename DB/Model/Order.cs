using System;
using System.Collections.Generic;

namespace DB
{
    public partial class Order
    {
        public int Id { get; set; }
        public int IdProducer { get; set; }
        public int IdProduct { get; set; }
        public int CountProduct { get; set; }
        public int? Status { get; set; }
        public DateTime Data { get; set; }
        public string Address { get; set; } = null!;

        public virtual Producer IdProducerNavigation { get; set; } = null!;
        public virtual Product IdProductNavigation { get; set; } = null!;
        public virtual Status? StatusNavigation { get; set; }
    }
}
