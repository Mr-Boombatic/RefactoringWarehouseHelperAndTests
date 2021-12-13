using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarehouseHelper.VeiwModel;

namespace WarehouseHelper.Tests
{
    [TestClass]
    public class ProcessingViewModelTests
    {
        SqliteStoneCompanyContextTest context { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            context = new SqliteStoneCompanyContextTest(ViewModel.SlabViewModel);


            // generate data for database by StoneViewModel
            //string relativePath = @"Database\DataForTestingProcessing.db";
            //var parentdir = Directory.GetCurrentDirectory();
            //string myString = parentdir.Replace("WarehouseHelper.Tests\\bin\\Debug\\net6.0-windows", "");
            //string absolutePath = Path.Combine(myString, relativePath);
            //string connectionString = string.Format("Data Source={0}", absolutePath);
            //context = new SqliteStoneCompanyContextTest(connectionString);
            //context.Slabs.Load();
        }

        // Integration tests

        [TestMethod]
        public void ProcessSlabsCommand_AllowedSlabs_ProcessedSlab()
        {
            Thread newWindowThread = new Thread(new ThreadStart(() =>
            {
                // Arrange
                var viewModel = new ProcessingVeiwModel(context);

                // Act
                viewModel.SelectedSlabs.Add("1/1");
                UIElementCollection collection = new UIElementCollection(new Button(), null);
                collection.Add(new RadioButton() { Content = "Термо", IsChecked = true });

                viewModel.ProcessSlabsCommand.Execute(collection);
                Assert.AreEqual("Термо",
                    context.Slabs.SingleOrDefault(b => b.SlabId == "1/1").Processing);
            }));

            newWindowThread.SetApartmentState(ApartmentState.STA);
            newWindowThread.Start();
            newWindowThread.Join();
        }

        // negative integration tests

        [TestMethod]
        public void ProcessSlabsCommand_WrongWorker_ProcessedSlab()
        {
            Thread newWindowThread = new Thread(new ThreadStart(() =>
            {
                var processingViewModel = new ProcessingVeiwModel(context);
                var slabViewModel = new SlabVeiwModel(context);
                slabViewModel.Worker = new Worker()
                { };

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

                processingViewModel.Slabs.Add(context.Slabs.SingleOrDefault(b => b.SlabId == "1/1"));
                UIElementCollection collection = new UIElementCollection(new Button(), null);
                collection.Add(new RadioButton() { Content = "Термо", IsChecked = true });

                processingViewModel.ProcessSlabsCommand.Execute(collection);
                Assert.AreEqual(null,
                    context.Slabs.SingleOrDefault(b => b.SlabId == "1/1").Processing);
            }));

            newWindowThread.SetApartmentState(ApartmentState.STA);
            newWindowThread.Start();
            newWindowThread.Join();
        }

        [TestMethod]
        public void ProcessSlabsCommand_WrongSlab_ProcessedSlab()
        {
            Thread newWindowThread = new Thread(new ThreadStart(() =>
            {
                context = new SqliteStoneCompanyContextTest(ViewModel.StoneViewModel);
                var processingViewModel = new ProcessingVeiwModel(context);
                var slabViewModel = new SlabVeiwModel(context);
                slabViewModel.Worker = new Worker()
                { Date = DateTime.Now, Name = "test", Shift = true };

                var slab = new Slab()
                {
                    Height = 4,
                    Width = 4,
                    Length = 4,
                    Date = null,
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

                processingViewModel.Slabs.Add(context.Slabs.SingleOrDefault(b => b.SlabId == "1/1"));
                UIElementCollection collection = new UIElementCollection(new Button(), null);
                collection.Add(new RadioButton() { Content = "Термо", IsChecked = true });

                    processingViewModel.ProcessSlabsCommand.Execute(collection);
                Assert.AreEqual(null,
                      context.Slabs.SingleOrDefault(b => b.SlabId == "1/1").Processing);
            }));

            newWindowThread.SetApartmentState(ApartmentState.STA);
            newWindowThread.Start();
            newWindowThread.Join();
        }
    }
}
