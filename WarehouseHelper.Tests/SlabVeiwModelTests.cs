using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarehouseHelper.VeiwModel;
using System.Linq;
using WarehouseHelper;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Threading;
using System.Windows.Controls;
using Microsoft.Data.Sqlite;

namespace WarehouseHelper.Tests
{
    [TestClass]
    public class SlabVeiwModelTests
    {
        SqliteStoneCompanyContextTest context { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            context = new SqliteStoneCompanyContextTest();

            // generate data for database by StoneViewModel
            var viewModel = new StoneViewModel(context);
            viewModel.Car = new Car { CarId = 0, Date = DateTime.Today, Cost = 100 };
            var mediator = new ManagerMediator(viewModel.Car); // to recalculaction cost for every stone
            var stone = new OrderedStone(mediator)
            {
                Height = 10,
                Width = 10,
                Length = 10,
                PricePerCube = 1
            };

            for (int i = 0; i < 3; i++)
                viewModel.Order.Add((OrderedStone)stone.Clone());

            viewModel.AddStonesToWarehouseCommand.Execute(null);
            context.SaveChanges();
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        // Intagration tests
        SqliteConnection source = new SqliteConnection("Data Source=c:\\test.db");
        [TestMethod]
        public void AddSlabsCommand_SlabForSaving_SavedData()
        {
            Thread newWindowThread = new Thread(new ThreadStart(() =>
            {
                var viewModel = new SlabVeiwModel(context);

                viewModel.SelectedStone = context.Stones.ToList()[0];
                viewModel.Worker = new Worker()
                { Date = DateTime.Now, Name = "test", Shift = new TextBlock() { Text = "День" } };

                var slab = new Slab()
                {
                    Height = 4,
                    Width = 4,
                    Length = 4,
                    Date = viewModel.Worker.Date,
                    Shift = false,
                    Count = 2,
                    Worker = "test"
                };
                for (int i = 0; i < 3; i++)
                    viewModel.Slabs.Add((Slab)slab.Clone());

                viewModel.AddSlabsCommand.Execute(null);
                context.SaveChanges();
                Assert.AreEqual(6, context.Slabs.Count());
                context.Dispose();
            }));

            newWindowThread.SetApartmentState(ApartmentState.STA);
            newWindowThread.Start();
            newWindowThread.Join();
        }

        // Negative unit tests

        [TestMethod]
        public void VerificationOverallVolume_NegativeStoneVolume_ShownModalDialog()
        {
            // Arrange
            var viewModel = new SlabVeiwModel(context);

            // Act
            var textOfModalDialog = (1000.0).IsValidOverallVolume(-1000);
            Assert.AreEqual("Обьем для камня не может быть отрицательным!\nОбьем для слэбов не может быть больше обьема камня!\n", textOfModalDialog);
        }

        [TestMethod]
        public void IsValid_EmptyWorkerDate_ShownModalDialog()
        {
            // Arrange
            var viewModel = new SlabVeiwModel(context);

            // Act
            Thread newWindowThread = new Thread(new ThreadStart(() =>
            {
                var textOfModalDialog = (new Worker()
                { Date = null, Name = "test", Shift = new TextBlock() { Text = "День" } }).IsValid();
                Assert.AreEqual("Не указана дата!\n", textOfModalDialog);
            }));
        }

        [TestMethod]
        public void IsValid_EmptyWorkerName_ShownModalDialog()
        {
            // Arrange
            var viewModel = new SlabVeiwModel(context);

            // Act
            Thread newWindowThread = new Thread(new ThreadStart(() =>
            {
                var textOfModalDialog = (new Worker()
                { Date = DateTime.Now, Name = "", Shift = new TextBlock() { Text = "День" } }).IsValid();

                Assert.AreEqual("Не указано имя рабочего!\n", textOfModalDialog);
            }));
        }

        [TestMethod]
        public void IsValid_EmptyWorkerShift_ShownModalDialog()
        {
            // Arrange
            var viewModel = new SlabVeiwModel(context);

            // Act
            var textOfModalDialog = (new Worker()
            { Date = DateTime.Now, Name = "test", Shift = null }).IsValid();
            Assert.AreEqual("Не указана смена рабочего!\n", textOfModalDialog);
        }

        [TestMethod]
        public void IsValid_FullEmptyParamtertsOfWorker_ShownModalDialog()
        {
            // Arrange
            var viewModel = new SlabVeiwModel(context);

            // Act
            var textOfModalDialog = (new Worker()
            { Date = null, Name = null, Shift = null }).IsValid();
            Assert.AreEqual("Не указана дата!\nНе указано имя рабочего!\nНе указана смена рабочего!\n", textOfModalDialog);
        }
    }
}
