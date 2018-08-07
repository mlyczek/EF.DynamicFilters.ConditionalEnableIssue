using System;

namespace DbProject
{
    public class BoughtItem
    {
        public Guid Id { get; set; }

        public string ItemName { get; set; }

        public int Count { get; set; }

        public Guid ShoppingCartId { get; set; }

        public virtual ShoppingCart ShoppingCart { get; set; }
    }
}