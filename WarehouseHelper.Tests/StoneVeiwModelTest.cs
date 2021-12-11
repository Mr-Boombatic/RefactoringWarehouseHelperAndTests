using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarehouseHelper.VeiwModel;
using System.Linq;
using WarehouseHelper;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Microsoft.Data.Sqlite;

namespace WarehouseHelper.Tests
{
    [TestClass]
    public class StoneVeiwModelTest
    {
        SqliteStoneCompanyContextTest context { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            context = new SqliteStoneCompanyContextTest();
        }

        [TestMethod]
        public void RecountCost_1StoneAnd1Car_1100ReturnedCostForEach()
        {
            // Arrange
            var veiwModel = new StoneVeiwModel(context);

            // Act
            veiwModel.Car = new Car() { Cost = 100 };
            var mediator = new ManagerMediator(veiwModel.Car);
            veiwModel.Orders.Add(new Order(mediator)
            {
                Height = 10,
                Width = 10,
                Length = 10,
                PricePerCube = 1
            });
            // Assert

            _ = veiwModel.AddStonesToWarehouseCommand;

            // stone cost value is calculated everytime for veiw
            // when i was making it me seemed logical \_(-__-)_/
            Assert.AreEqual(1100, context.Stones.ToList()[0]);
        }

        [TestMethod]
        public void RecountCost_4StoneAnd1Car_1025ReturnedCostForEach()
        {
            // Arrange
            StoneVeiwModel veiwModel = new StoneVeiwModel(context);

            // Act
            var mediator = new ManagerMediator(new Car() { Cost = 100 });
            var orderTest = new Order(mediator)
            {
                Height = 10,
                Width = 10,
                Length = 10,
                PricePerCube = 1
            };

            for (int i = 0; i < 3; i++)
                veiwModel.Orders.Add((Order)orderTest.Clone());

            // Assert
            veiwModel.Orders.ToList().ForEach(order =>
            {
                if (order.Cost != 1025)
                    Assert.Fail();
            });
        }

        [TestMethod]
        public void RecountCost_4Stone_1000ReturnedCostForEach()
        {
            // Arrange
            var veiwModel = new StoneVeiwModel(context);

            // Act
            var mediator = new ManagerMediator(new Car() { Cost = 0 });
            var orderTest = new Order(mediator)
            {
                Height = 10,
                Width = 10,
                Length = 10,
                PricePerCube = 1
            };

            for (int i = 0; i < 3; i++)
                veiwModel.Orders.Add((Order)orderTest.Clone());

            // Assert
            veiwModel.Orders.ToList().ForEach(order =>
            {
                if (order.Cost != 1000)
                    Assert.Fail();
            });
        }

        [TestMethod]
        public void RecountCost_1Stone_1000ReturnedCostForEachStone()
        {
            // Arrange
            var veiwModel = new StoneVeiwModel(context);

            // Act
            var mediator = new ManagerMediator(new Car() { Cost = 0 });
            veiwModel.Orders.Add(new Order(mediator)
            {
                Height = 10,
                Width = 10,
                Length = 10,
                PricePerCube = 1
            });

            // Assert
            Assert.AreEqual(1000, veiwModel.Orders[0].Cost);
        }

    }

    public class SqliteStoneCompanyContextTest : Stone—ompanyContext, IDisposable
    {
        public SqliteStoneCompanyContextTest()
            : base(
                new DbContextOptionsBuilder<Stone—ompanyContext>()
                    .UseSqlite(CreateInMemoryDatabase())
                    .Options)
        {
            Seed();
        }

        private void Seed()
        {
            using (var context = new Stone—ompanyContext(ContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.SaveChanges();
            }
        }

        private readonly DbConnection _connection;

        private static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");

            connection.Open();

            return connection;
        }

        public void Dispose() => _connection.Dispose();
    }
}
