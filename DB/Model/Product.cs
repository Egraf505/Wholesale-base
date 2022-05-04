using System;
using System.Collections.Generic;

namespace DB
{
    public partial class Product
    {
        public Product()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public int IdDeliveries { get; set; }
        public int CountProductOnWarehouse { get; set; }
        public decimal Price { get; set; }
        public int Type { get; set; }

        public virtual Delivery IdDeliveriesNavigation { get; set; } = null!;
        public virtual Type TypeNavigation { get; set; } = null!;
        public virtual ICollection<Order> Orders { get; set; }
    }
}
