using System;
using System.Collections.Generic;

namespace DB
{
    public partial class Delivery
    {
        public Delivery()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public int IdProducer { get; set; }
        public int IdProduct { get; set; }
        public DateTime Data { get; set; }
        public int Quantity { get; set; }

        public virtual Producer IdProducerNavigation { get; set; } = null!;
        public virtual ICollection<Product> Products { get; set; }
    }
}
