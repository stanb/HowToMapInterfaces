using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample
{
    public class SupplierId : IdBase<string>
    {
        protected SupplierId() {}
        public SupplierId(string value) : base(value) {}
    }

    public class StoreId : IdBase<string>
    {
        protected StoreId() {}
        public StoreId(string value) : base(value) { }
    }
}
