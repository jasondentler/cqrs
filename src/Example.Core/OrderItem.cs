using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Example
{
    public class OrderItem : IEquatable<OrderItem>
    {
        public Guid MenuItemId { get; private set; }
        public IDictionary<string, string> Options { get; private set; }
        public int Quantity { get; private set; }

        public OrderItem(
            Guid menuItemId,
            IDictionary<string, string> options,
            int quantity)
        {
            MenuItemId = menuItemId;
            Options = options.ToDictionary(i => i.Key, i => i.Value);
            Quantity = quantity;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (OrderItem)) return false;
            return Equals((OrderItem) obj);
        }

        public bool Equals(OrderItem other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.MenuItemId.Equals(MenuItemId) &&
                   other.Quantity == Quantity &&
                   OptionsAreEqual(other.Options, Options);
        }

        public override int GetHashCode()
        {
            int result = MenuItemId.GetHashCode();
            result = (result*397) ^ Quantity;
            return result;
        }

        private static bool OptionsAreEqual(IDictionary<string, string> a, IDictionary<string,string> b)
        {
            if (ReferenceEquals(a,b)) return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;
            return !a.Except(b).Concat(b.Except(a)).Any();
        }

        public override string ToString()
        {
            return string.Format("x{1} {0} {2}",
                                 MenuItemId,
                                 Quantity,
                                 string.Join(", ", Options.Select(o => string.Format("{0}: {1}", o.Key, o.Value))));
        }

    }

}
