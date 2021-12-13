using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarehouseHelper.VeiwModel;
using System.Linq;
using WarehouseHelper;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Microsoft.Data.SqlClient;
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
        public void RecalculationCost_1StoneAnd1Car_1100ReturnedCostForEach()
        {
            // Arrange
            var veiwModel = new StoneViewModel(context);

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
        public void RecalculationCost_4StoneAnd1Car_1025ReturnedCostForEach()
        {
            // Arrange
            StoneViewModel viewModel = new StoneViewModel(context);

            // Act
            viewModel.Car.Cost = 100;
            var mediator = new ManagerMediator(viewModel.Car);
            var orderTest = new OrderedStone(mediator)
            {
                Height = 10,
                Width = 10,
                Length = 10,
                PricePerCube = 1
            };

            for (int i = 0; i < 3; i++)
                viewModel.Order.Add((OrderedStone)orderTest.Clone());

            // Assert
            viewModel.Order.ToList().ForEach(order =>
            {
                if (order.Cost != 1025)
                    Assert.Fail();
            });
        }

        [TestMethod]
        public void RecalculationCost_4Stone_1000ReturnedCostForEach()
        {
            // Arrange
            var veiwModel = new StoneViewModel(context);

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
        public void RecalculationCost_1Stone_1000ReturnedCostForEachStone()
        {
            // Arrange
            var veiwModel = new StoneViewModel(context);

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
            var veiwModel = new StoneViewModel(context);

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
            var veiwModel = new StoneViewModel(context);

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
            var veiwModel = new StoneViewModel(context);

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
            var veiwModel = new StoneViewModel(context);

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
            var veiwModel = new StoneViewModel(context);

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

    public enum ViewModel { 
        StoneViewModel,
        SlabViewModel,
        ProcessingViewModel,
        ProductViewModel,
        TradeViewModel,
        None
    }

    public class SqliteStoneCompanyContextTest : Stone—ompanyContext, IDisposable
    {
        public SqliteStoneCompanyContextTest(ViewModel viewModel = ViewModel.None)
            : base(
                new DbContextOptionsBuilder<Stone—ompanyContext>()
                    .UseSqlite(CreateInMemoryDatabase())
                    .Options)
        {
            Seed(viewModel);
        }

        private void Seed(ViewModel viewModel)
        {
            using (var context = new Stone—ompanyContext(ContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                if (viewModel == ViewModel.None)
                    return;

                // generate data for database by StoneViewModel
                var stoneViewModel = new StoneViewModel(context);
                stoneViewModel.Car = new Car { CarId = 0, Date = DateTime.Today, Cost = 100 };
                var mediator = new ManagerMediator(stoneViewModel.Car); // to recalculaction cost for every stone
                var stone = new OrderedStone(mediator)
                {
                    Height = 10,
                    Width = 10,
                    Length = 10,
                    PricePerCube = 1
                };

                for (int i = 0; i < 3; i++)
                    stoneViewModel.Order.Add((OrderedStone)stone.Clone());

                stoneViewModel.AddStonesToWarehouseCommand.Execute(null);
                context.SaveChanges();

                if (viewModel == ViewModel.StoneViewModel)
                    return;

                // generate data for database by SlabViewModel
                var slabViewModel = new SlabVeiwModel(context);
                slabViewModel.Worker = new Worker()
                { Date = DateTime.Now, Name = "test", Shift = true };

                var slab = new Slab()
                {
                    Height = 4,
                    Width = 4,
                    Length = 4,
                    Date = slabViewModel.Worker.Date,
                    Shift = false,
                    Count = 2,
                    Worker = "test"
                };
                for (int i = 0; i < 3; i++)
                {
                    slabViewModel.SelectedStone = context.Stones.ToList()[i];
                    slabViewModel.Slabs.Add((Slab)slab.Clone());
                    slabViewModel.AddSlabsCommand.Execute(null);
                }

                context.SaveChanges();

                if (viewModel == ViewModel.SlabViewModel)
                    return;

                context.Slabs.SingleOrDefault(b => b.SlabId == "1/1").Processing = "“ÂÏÓ";
                context.Slabs.SingleOrDefault(b => b.SlabId == "1/2").Processing = "“ÂÏÓ";
                context.Slabs.SingleOrDefault(b => b.SlabId == "2/1").Processing = "œÓÎË‚ÓÍ‡";
                context.Slabs.SingleOrDefault(b => b.SlabId == "2/2").Processing = "œÓÎË‚ÓÍ‡";
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
