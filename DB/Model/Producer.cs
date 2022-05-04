using System;
using System.Collections.Generic;

namespace DB
{
    public partial class Producer
    {
        public Producer()
        {
            Deliveries = new HashSet<Delivery>();
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Firstname { get; set; } = null!;
        public string Middlename { get; set; } = null!;
        public string? Lastname { get; set; }

        public virtual ICollection<Delivery> Deliveries { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
