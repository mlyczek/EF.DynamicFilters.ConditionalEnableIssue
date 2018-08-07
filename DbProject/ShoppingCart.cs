using System;
using System.Collections.Generic;

namespace DbProject
{
    public class ShoppingCart
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<BoughtItem> BoughtItems { get; set; }
    }
}