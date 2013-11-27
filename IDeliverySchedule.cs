using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample
{
    public class DeliveryScheduleId : IdBase<Guid>
    {
        protected DeliveryScheduleId() {}
        public DeliveryScheduleId(Guid value) : base(value) {}
    }

    public interface IDeliverySchedule
    {
        DeliveryScheduleId Id { get; }
        SupplierId SupplierId { get; }
        string Code { get; }
        ICollection<StoreId> Stores { get; }

        IEnumerable<IDeliveryPattern> Patterns { get; }
        void AddPattern(IDeliveryPattern pattern);
        void RemovePattern(IDeliveryPattern pattern);

        IEnumerable<DateTime> GetDeliveryDates(DateTime fromDate, DateTime toDate);
        DateTime? GetNextDeliveryDate(DateTime lookupDate);
    }

    public interface IDeliveryPattern
    {
        IDeliverySchedule Schedule { get; }
        string Code { get; }
        DateTime FirstDate { get; set; }
        DateTime? GetNextDelivery(DateTime lookupDate);
        IEnumerable<DateTime> GetDeliveryDates(DateTime firstDate, DateTime lastDate);
    }
}
