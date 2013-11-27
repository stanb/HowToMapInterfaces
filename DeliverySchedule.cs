using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample
{
    public class DeliverySchedule : IDeliverySchedule, IIdentified<DeliveryScheduleId>
    {
        protected DeliverySchedule() {}
        public DeliverySchedule(SupplierId supplierId, string code)
        {
            Id = new DeliveryScheduleId((Guid)new NHibernate.Id.GuidCombGenerator().Generate(null, null));
            SupplierId = supplierId;
            Code = code;
            Stores = new HashSet<StoreId>();
            Patterns = new HashSet<IDeliveryPattern>();
        }

        DeliveryScheduleId IIdentified<DeliveryScheduleId>.Id
        {
            get { return Id; }
            set { Id = value; }
        }

        public virtual DeliveryScheduleId Id { get; protected set; }
        public virtual SupplierId SupplierId { get; protected set; }
        public virtual string Code { get; protected set; }
        public virtual ICollection<StoreId> Stores { get; protected set; }
        
        public virtual IEnumerable<IDeliveryPattern> Patterns { get; protected set; }
        public virtual void AddPattern(IDeliveryPattern pattern)
        {
            ((ICollection<IDeliveryPattern>)Patterns).Add(pattern);
        }

        public virtual void RemovePattern(IDeliveryPattern pattern)
        {
            ((ICollection<IDeliveryPattern>)Patterns).Remove(pattern);
        }

        public virtual IEnumerable<DateTime> GetDeliveryDates(DateTime fromDate, DateTime toDate)
        {
            var deliveries = new List<DateTime>();

            var patterns = Patterns.OrderBy(p => p.FirstDate).ToList();
            for (var i = 0; i < patterns.Count; i++)
            {
                var firstDate = patterns[i].FirstDate;
                if (firstDate > toDate)
                    break;

                var lastDate = (i + 1 < patterns.Count) ?
                    patterns[i + 1].FirstDate.Subtract(TimeSpan.FromDays(1)) : toDate;

                if ((firstDate <= fromDate && lastDate >= fromDate) ||
                     (firstDate > fromDate && firstDate <= toDate))
                {
                    var start = firstDate < fromDate ? fromDate : firstDate;
                    var patternDeliveries = patterns[i].GetDeliveryDates(start, lastDate);
                    deliveries.AddRange(patternDeliveries);
                }
            }

            return deliveries;
        }

        public virtual DateTime? GetNextDeliveryDate(DateTime lookupDate)
        {
            var patterns = Patterns.OrderBy(p => p.FirstDate).ToList();
            for (int i = 0; i < patterns.Count; i++)
            {
                var lastDate = (i + 1 < patterns.Count) ?
                    patterns[i + 1].FirstDate.Subtract(TimeSpan.FromDays(1)) : DateTime.MaxValue;
                if (lastDate < lookupDate)
                    continue;

                var nextDelivery = patterns[i].GetNextDelivery(lookupDate);
                if (!nextDelivery.HasValue)
                    continue;
                if (nextDelivery.Value > lastDate)
                    continue;
                return nextDelivery.Value;
            }

            return null;
        }

    }
}
