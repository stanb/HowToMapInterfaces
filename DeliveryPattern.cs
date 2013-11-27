using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample
{
    /// <summary>
    /// Represents pattern organized by weeks. Every week may contain specific delivery days
    /// </summary>
    /// <example>
    /// Pattern consists of two weeks cicle with delivery on the monday and friday on the first week and delivery on the wednesday on the second week etc.
    /// </example>
    public class WeeklyDeliveryPattern : IDeliveryPattern, IIdentified<Guid>
    {
        protected WeeklyDeliveryPattern() { }

        public WeeklyDeliveryPattern(IDeliverySchedule schedule, string code)
        {
            Schedule = schedule;
            Code = code;
            FirstDate = DateTime.Today;
        }

        Guid IIdentified<Guid>.Id { get; set; }
        public virtual IDeliverySchedule Schedule { get; protected set; }
        public virtual string Code { get; protected set; }
        public virtual DateTime FirstDate { get; set; }
        
        public virtual DateTime? GetNextDelivery(DateTime lookupDate)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<DateTime> GetDeliveryDates(DateTime firstDate, DateTime lastDate)
        {
            throw new NotImplementedException();
        }
        
        //TODO: Add methods and properties for organizing delivery days

    }

    /// <summary>
    /// Represents month based pattern with specific delivery days
    /// </summary>
    /// <example>
    /// Delivery on first monday of the month, second wednesday of the month, last friday of the month etc.
    /// </example>
    public class MonthlyDeliveryPattern : IDeliveryPattern, IIdentified<Guid>
    {
        protected MonthlyDeliveryPattern() {}

        public MonthlyDeliveryPattern(IDeliverySchedule schedule, string code)
        {
            Schedule = schedule;
            Code = code;
            FirstDate = DateTime.Today;
        }

        Guid IIdentified<Guid>.Id { get; set; }

        public virtual IDeliverySchedule Schedule { get; protected set; }
        public virtual string Code { get; protected set; }
        public virtual DateTime FirstDate { get; set; }
        
        public virtual DateTime? GetNextDelivery(DateTime lookupDate)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<DateTime> GetDeliveryDates(DateTime firstDate, DateTime lastDate)
        {
            throw new NotImplementedException();
        }

        //TODO: Add methods and properties for organizing delivery days
    }
}
