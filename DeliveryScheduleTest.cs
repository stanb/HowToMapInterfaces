using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Tool.hbm2ddl;

namespace Sample
{
    [TestClass]
    public class DeliveryScheduleTest
    {
        private static ISessionFactory factory;

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            var cfg = new Configuration();
            cfg.DataBaseIntegration(db =>
            {
                db.Dialect<MsSql2008Dialect>();
                db.LogFormattedSql = true;
                db.LogSqlInConsole = true;
                db.ConnectionStringName = "Sample";
            });
            cfg.AddAssembly(Assembly.GetExecutingAssembly());
            factory = cfg.BuildSessionFactory();

            var export = new SchemaExport(cfg);
            export.Create(sql => Debug.WriteLine(sql), true);
        }

        [TestInitialize]
        public void TestInit()
        {
            using (var session = factory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                session.CreateQuery("delete from IDeliverySchedule").ExecuteUpdate();
                session.CreateQuery("delete from IDeliveryPattern").ExecuteUpdate();
                tx.Commit();
            }
        }

        [TestMethod]
        public void SaveAndFetch()
        {
            var dsc1 = new DeliverySchedule(new SupplierId("Supplier/1"), "DSC/1");
            dsc1.Stores.Add(new StoreId("Store/1"));
            dsc1.Stores.Add(new StoreId("Store/2"));
            dsc1.Stores.Add(new StoreId("Store/3"));
            var dp1_1 = new WeeklyDeliveryPattern(dsc1, "DP/1");
            var dp1_2 = new MonthlyDeliveryPattern(dsc1, "DP/2") { FirstDate = DateTime.Today.AddMonths(2)};
            dsc1.AddPattern(dp1_1);
            dsc1.AddPattern(dp1_2);

            using (var session = factory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                session.Save(dsc1);
                tx.Commit();
            }

            using (var session = factory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var dsc2 = session.Get<IDeliverySchedule>(dsc1.Id);
                var dp2_1 = dsc2.Patterns.First(dp => dp.Code == "DP/1");
                var dp2_2 = dsc2.Patterns.First(dp => dp.Code == "DP/2");
                Assert.IsInstanceOfType(dp2_1, typeof(WeeklyDeliveryPattern));
                Assert.IsInstanceOfType(dp2_2, typeof(MonthlyDeliveryPattern));
                Assert.AreEqual(((IIdentified<Guid>)dp2_1).Id, ((IIdentified<Guid>)dp1_1).Id);
                Assert.AreEqual(((IIdentified<Guid>)dp2_2).Id, ((IIdentified<Guid>)dp1_2).Id);
                Assert.IsTrue(dsc2.Stores.Any(s => s == "Store/1"));
                Assert.IsTrue(dsc2.Stores.Any(s => s == "Store/2"));
                Assert.IsTrue(dsc2.Stores.Any(s => s == "Store/3"));
                tx.Commit();
            }
        }
    }
}
