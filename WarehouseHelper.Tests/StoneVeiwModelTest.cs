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


        /// <summary>
        /// this method checks the correctness of the recalculation of the cost
        /// when the number of stones and cars is equal to 1.
        /// </summary>
        /// <remarks>
        /// Defined stone parametrs:
        /// Height = 10, Width = 10, Length = 10, PricePerCube = 1.
        /// Defined car parametrs:
        /// Cost = 100
        /// </remarks>
        [TestMethod]
        public void RecountCost_1StoneAnd1Car_1100ReturnedCostForEach()
        {
            // Arrange
            var veiwModel = new StoneVeiwModel(context);

            // Act
            veiwModel.Car = new Car() { Cost = 100 };
            var mediator = new ManagerMediator(veiwModel.Car);
            veiwModel.Order.Add(new OrderedStone(mediator)
            {
                Height = 10,
                Width = 10,
                Length = 10,
                PricePerCube = 1
            });
            // Assert

            // stone cost value is calculated everytime for veiw
            // when i was making it me seemed logical \_(-__-)_/
            Assert.AreEqual(1100, veiwModel.Order[0].Cost);
        }

        [TestMethod]
        public void RecountCost_4StoneAnd1Car_1025ReturnedCostForEach()
        {
            // Arrange
            StoneVeiwModel veiwModel = new StoneVeiwModel(context);

            // Act
            veiwModel.Car.Cost = 100;
            var mediator = new ManagerMediator(veiwModel.Car);
            var orderTest = new OrderedStone(mediator)
            {
                Height = 10,
                Width = 10,
                Length = 10,
                PricePerCube = 1
            };

            for (int i = 0; i < 3; i++)
                veiwModel.Order.Add((OrderedStone)orderTest.Clone());

            // Assert
            veiwModel.Order.ToList().ForEach(order =>
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
            var mediator = new ManagerMediator(veiwModel.Car);
            var orderTest = new OrderedStone(mediator)
            {
                Height = 10,
                Width = 10,
                Length = 10,
                PricePerCube = 1
            };

            for (int i = 0; i < 3; i++)
                veiwModel.Order.Add((OrderedStone)orderTest.Clone());

            // Assert
            veiwModel.Order.ToList().ForEach(order =>
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
            var mediator = new ManagerMediator(veiwModel.Car);
            veiwModel.Order.Add(new OrderedStone(mediator)
            {
                Height = 10,
                Width = 10,
                Length = 10,
                PricePerCube = 1
            });

            // Assert
            Assert.AreEqual(1000, veiwModel.Order[0].Cost);
        }

        // Negative tests

        [TestMethod]
        public void RecalculationCost_NegativeVolume_Exception()
        {
            // Arrange
            var veiwModel = new StoneVeiwModel(context);

            // Act
            var mediator = new ManagerMediator(veiwModel.Car);
            veiwModel.Order.Add(new OrderedStone(mediator)
            {
                Height = 10,
                Width = -10,
                Length = 10,
                PricePerCube = 1
            });

            // Assert
            Assert.AreEqual(0, veiwModel.Order[0].Cost);
            Assert.AreEqual(true, System.Windows.Interop.ComponentDispatcher.IsThreadModal);
        }

        [TestMethod]
        public void RecalculationCost_NegativeWidthAndLength_Exception()
        {
            // Arrange
            var veiwModel = new StoneVeiwModel(context);

            // Act
            var mediator = new ManagerMediator(veiwModel.Car);
            veiwModel.Order.Add(new OrderedStone(mediator)
            {
                Height = 10,
                Width = -10,
                Length = -10,
                PricePerCube = 1
            });

            // Assert
            Assert.AreEqual(0, veiwModel.Order[0].Cost);
            Assert.AreEqual(true, System.Windows.Interop.ComponentDispatcher.IsThreadModal);
        }

        [TestMethod]
        public void RecalculationCost_NegativePricePerCube_Exception()
        {
            // Arrange
            var veiwModel = new StoneVeiwModel(context);

            // Act
            var mediator = new ManagerMediator(veiwModel.Car);
            veiwModel.Order.Add(new OrderedStone(mediator)
            {
                Height = 10,
                Width = 10,
                Length = 10,
                PricePerCube = -1
            });

            // Assert
            Assert.AreEqual(0, veiwModel.Order[0].Cost);
            Assert.AreEqual(true, System.Windows.Interop.ComponentDispatcher.IsThreadModal);
        }

        [TestMethod]
        public void RecalculationCost_NegativePricePerCubeAndParamsOfStone_Exception()
        {
            // Arrange
            var veiwModel = new StoneVeiwModel(context);

            // Act
            var mediator = new ManagerMediator(veiwModel.Car);
            veiwModel.Order.Add(new OrderedStone(mediator)
            {
                Height = 10,
                Width = -10,
                Length = -10,
                PricePerCube = -1
            });

            // Assert
            Assert.AreEqual(0, veiwModel.Order[0].Cost);
            Assert.AreEqual(true, System.Windows.Interop.ComponentDispatcher.IsThreadModal);
        }

        [TestMethod]
        public void RecalculationCost_NegativeCarCost_Exception()
        {
            // Arrange
            var veiwModel = new StoneVeiwModel(context);

            // Act
            veiwModel.Car.Cost = -100;
            var mediator = new ManagerMediator(veiwModel.Car);
            veiwModel.Order.Add(new OrderedStone(mediator)
            {
                Height = 10,
                Width = 10,
                Length = 10,
                PricePerCube = 1
            });

            // Assert
            Assert.AreEqual(0, veiwModel.Order[0].Cost);
            Assert.AreEqual(true, System.Windows.Interop.ComponentDispatcher.IsThreadModal);
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
